using System;
using System.Formats.Asn1;
using System.Data;
using System.Globalization;
using System.Net;
using CsvHelper;
using Dapper;
using MySqlConnector;
using Sylvia_API.Models;
using Sylvia_API.Repository;

namespace SYLVIA
{
    static public class Program
    {
        static void Main(string[] args)
        {
            string function = "TIME_SERIES_DAILY", symbol, outputsize = "compact", datatype = "csv", apiKey = "";

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
            if(!File.Exists($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv"))
            {
                string data = webClient.DownloadString(queryURL);
                File.WriteAllText($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv", data);
            }
            
            List<RawStockData> stockList = new List<RawStockData>();

            // Read in CSV File to StreamReader
            using(StreamReader reader = new StreamReader($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv"))
            using(CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                

                csv.Read();
                csv.ReadHeader();

                while(csv.Read())
                {
                    string ts = DateTime.Parse(csv.GetField<string>("timestamp")).ToString("MM-dd-yyyy");

                    RawStockData stockData = new RawStockData
                    {
                        Symbol = symbol,
                        TimeStamp = Convert.ToDateTime(ts),
                        Open = csv.GetField<double>("open"),
                        High = csv.GetField<double>("high"),
                        Low = csv.GetField<double>("low"),
                        Close = csv.GetField<double>("close"),
                        Volume = csv.GetField<int>("volume")
                    };

                    stockList.Add(stockData);
                }
            }

            string table = "rawstockdata";

            foreach(var stock in stockList)
            {
                InsertRawData(table, stock);
            }

            // Delete csv files
            if(File.Exists($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv"))
            {
                File.Delete($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv");
            }

            Console.Clear();
        }

        static private IDbConnection CreateConn()
        {
            return new MySqlConnection("Server=xxxx;Database=xxxx;Uid=xxxx;Pwd=xxxx");
        }

        static public void InsertRawData(string table, RawStockData data)
        {
            using var conn = CreateConn();

            try
            {
                var query = @$"INSERT INTO {table} (symbol, timestamp, open, high, low, close, volume)
                               VALUES (@Symbol, @TimeStamp, @Open, @High, @Low, @Close, @Volume)";

                var parameters = new DynamicParameters();

                parameters.Add("Symbol", data.Symbol, DbType.String);
                parameters.Add("TimeStamp", data.TimeStamp, DbType.String);
                parameters.Add("Open", data.Open, DbType.String);
                parameters.Add("High", data.High, DbType.String);
                parameters.Add("Low", data.Low, DbType.String);
                parameters.Add("Close", data.Close, DbType.String);
                parameters.Add("Volume", data.Volume, DbType.String);

                conn.Open();

                var results = conn.Execute(query, parameters);
            }
            catch
            {

            }
        }
    }
}