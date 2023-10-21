using System;
using System.Formats.Asn1;
using System.Data;
using System.Globalization;
using System.Net;
using CsvHelper;
using Dapper;
using MySqlConnector;
using Sylvia_API.Models;

namespace SYLVIA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string function = "TIME_SERIES_DAILY_ADJUSTED", symbol, outputsize = "compact", datatype = "csv", apiKey = "KOR2N2FPTN8LVEV4";
            double DayTotal100 = 0, DayTotal50 = 0, DayTotal20 = 0;
            int dayCounter = 0;
            string printOutSpacer = "  ";

            Console.WriteLine("Welcome to SYLVIA\n\n");
            Console.WriteLine("Please enter the following information to pull in new data.");
            Console.WriteLine("Symbol:");
            symbol = Console.ReadLine();


            /* 
             * Alpha Vantage API call
             *      function: type of api call needed
             *      symbol: company requested
             *      outputsize: [compact | full] compact pulls past 100 days; full pulls 20+ years of histocial data
             *      datatype: [json | csv] pulls data as either json or csv
             *      apikey: needed to access api
             */
            string queryURL = $"https://www.alphavantage.co/query?function={function}&symbol={symbol}&outputsize={outputsize}&datatype={datatype}&apikey={apiKey}";

            // Create a new Web Client
            WebClient webClient = new();

            // Create file name to be used
            string fileName = symbol + "-" + DateTime.Now.ToString("MM-dd-yyyy");

            // Check if file exists
            if(!File.Exists(@"C:\Users\" + Environment.UserName + @"\5560\" + fileName + ".csv"))
            {
                string data = webClient.DownloadString(queryURL);
                File.WriteAllText(@"C:\Users\" + Environment.UserName + @"\Desktop\" + fileName + ".csv", data);
            }
            
            List<RawStockData> stockList = new List<RawStockData>();

            // Read in CSV File to StreamReader
            using(StreamReader reader = new StreamReader(@"C:\Users\" + Environment.UserName + @"\5560\" + fileName + ".csv"))
            using(CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                

                csv.Read();
                csv.ReadHeader();

                while(csv.Read())
                {
                    RawStockData stockData = new RawStockData
                    {
                        Symbol = symbol,
                        TimeStamp = DateTime.Parse(csv.GetField<string>("timestamp")).ToString("MM-dd-yyyy"),
                        Open = csv.GetField<double>("open"),
                        High = csv.GetField<double>("high"),
                        Low = csv.GetField<double>("low"),
                        Close = csv.GetField<double>("close"),
                        AdjustedClose = csv.GetField<double>("adjusted_close"),
                        Volume = csv.GetField<int>("volume"),
                        DividendAmount = csv.GetField<double>("dividend_amount"),
                        SplitCoefficient = csv.GetField<double>("split_coefficient")
                    };

                    stockList.Add(stockData);
                }

                
            }
            
            foreach(var stock in stockList)
            {
                MySqlData.InsertRawData("test", stock);
            }

            // Delete csv files

            // Keep Console from Closing automatically
            Console.Read();

        }
    }

    class MySqlData
    {
        private IDbConnection CreateConn()
        {
            return new MySqlConnection("");
        }

        public void InsertRawData(string table, RawStockData data)
        {
            using var conn = CreateConn();

            try
            {
                var query = @$"INSERT INTO {table} (symbol, timestamp, open, high, low, close, adjustedclose, volume, dividendamount, splitcoefficient)
                               VALUES (@Symbol, @TimeStamp, @Open, @High, @Low, @Close, @AdjustedClose, @Volume, @DividendAmount, @SplitCoefficient)";

                var parameters = new DynamicParameters();

                parameters.Add("Symbol", data.Symbol, DbType.String);
                parameters.Add("TimeStamp", data.TimeStamp, DbType.String);
                parameters.Add("Open", data.Open, DbType.String);
                parameters.Add("High", data.High, DbType.String);
                parameters.Add("Low", data.Low, DbType.String);
                parameters.Add("Close", data.Close, DbType.String);
                parameters.Add("AdjustedClose", data.AdjustedClose, DbType.String);
                parameters.Add("Volume", data.Volume, DbType.String);
                parameters.Add("DividendAmount", data.DividendAmount, DbType.String);
                parameters.Add("SplitCoefficient", data.SplitCoefficient, DbType.String);

                conn.Open();

                var results = conn.Execute(query, parameters);
            }
            catch
            {

            }
        }
    }
}