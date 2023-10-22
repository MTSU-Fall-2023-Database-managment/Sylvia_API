using System.Data;
using System.Globalization;
using System.Net;
using CsvHelper;
using Dapper;
using MySqlConnector;
using Sylvia_API.Models;

namespace SYLVIA
{
    static public class Program
    {
        static void Main(string[] args)
        {
            int result;
            bool run = true;

            StartText();

            while (run)
            {
                Console.WriteLine("Please select one of the following options:\n" +
                                  "-------------------------------------------\n" +
                                  "1: Pull data from Alpha Vantage\n" +
                                  "2: Analyze stock data\n" +
                                  "~: Close application\n\n");
                
                Console.Write("Selection: ");
                var todo = Console.ReadLine();
                
                switch(todo)
                {
                    case "1":
                        Console.Clear();
                        result = AlphaVantage();

                        if(result == 1)
                            return;
                        break;

                    case "2":
                        Console.Clear();
                        result = Analysis();

                        if (result == 1)
                            return;
                        break;

                    default:
                        return;
                }
            }

            return;
        }

        static private void StartText()
        {
            Console.WriteLine("================================================================================================\n" + 
                              "                                 Welcome to SYLVIA\n" +
                              "================================================================================================");
            Thread.Sleep(2000);
            Console.Clear();
            Console.WriteLine("================================================================================================");
            Console.Write("                                 S");
            Thread.Sleep(500);
            Console.WriteLine("tock");
            Console.Write("                                 Y");
            Thread.Sleep(500);
            Console.WriteLine("ield");
            Console.Write("                                 L");
            Thread.Sleep(500);
            Console.WriteLine("iquidity");
            Console.Write("                                 V");
            Thread.Sleep(500);
            Console.WriteLine("alue");
            Console.Write("                                 I");
            Thread.Sleep(500);
            Console.WriteLine("nvestment");
            Console.Write("                                 A");
            Thread.Sleep(500);
            Console.WriteLine("nalyzer");
            Console.WriteLine("================================================================================================");
            Thread.Sleep(3000);
            Console.Clear();
        }

        static private int AlphaVantage()
        {
            int result;
            bool run = true;

            while (run)
            {
                Console.WriteLine("Pulling data from AlphaVantage:\n" +
                                  "-------------------------------\n" +
                                  "1: Call AlphaVantage API\n" +
                                  "2: Information on API\n" +
                                  "3: Main menu\n" +
                                  "~: Close application\n\n");

                Console.Write("Selection: ");
                var todo = Console.ReadLine();

                switch(todo)
                {
                    case "1":
                        Console.Clear();
                        result = CallAVAPI();

                        if(result == 1)
                            return 1;
                        else
                            return 0;

                    case "2":
                        Console.Clear();
                        HelpAlphaVantage();
                        break;

                    case "3":
                        Console.Clear();
                        return 0;

                    default:
                        return 1;
                }
            }
            
            return 1;
        }

        static private void HelpAlphaVantage()
        {
            Console.WriteLine("================================================================================================\n" +
                              "                           MORE INFORMATION ABOUT ALPHA VANTAGE API\n" +
                              "================================================================================================\n\n" +
                              "API functions:\n" +
                              "     TIME_SERIES_INTRADAY: Returns latest 100 data points based on the time interval provided\n" +
                              "                           (1min, 5min, 15min, 30min, 60min)\n" +
                              "        TIME_SERIES_DAILY: Returns raw daily data\n" +
                              "                           (date, daily open, daily high, daily low, daily close, daily volume)\n" +
                              "       TIME_SERIES_WEEKLY: Returns raw weekly data \n" +
                              "                           (last trading day of each week, weekly open, weekly high, weekly low, weekly close, weekly volume) \n" +
                              "      TIME_SERIES_MONTHLY: Returns raw daily data \n" +
                              "                           (last trading day of each month, monthly open, monthly high, monthly low, monthly close, monthly volume)\n\n" +
                              "Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static private int CallAVAPI()
        {
            int result;
            bool run = true;

            while(run)
            {
                Console.WriteLine("Pick a function:\n" +
                                  "-------------------------------\n" +
                                  "1: TIME_SERIES_INTRADAY\n" +
                                  "2: TIME_SERIES_DAILY\n" +
                                  "3: TIME_SERIES_WEEKLY\n" +
                                  "4: TIME_SERIES_MONTHLY\n" +
                                  "5: Information on API\n" +
                                  "6: Main menu\n" +
                                  "~: Close application\n\n");

                Console.Write("Selection: ");
                var todo = Console.ReadLine();

                switch(todo)
                {
                    case "1":
                        Console.Clear();
                        result = AVFunctionCall("TIME_SERIES_INTRADAY", "rawintraday");

                        if(result == 0)
                            Console.WriteLine("Successfully pulled INTRADAY data from AplhaVantage!\n");
                        else
                            Console.WriteLine("Unable to pull INTRADAY data from AlphaVantage\n");
                        break;

                    case "2":
                        Console.Clear();
                        result = AVFunctionCall("TIME_SERIES_DAILY", "rawdaily");

                        if(result == 0)
                            Console.WriteLine("Successfully pulled DAILY data from AplhaVantage!\n");
                        else
                            Console.WriteLine("Unable to pull DAILY data from AlphaVantage\n");
                        break;

                    case "3":
                        Console.Clear();
                        result = AVFunctionCall("TIME_SERIES_WEEKLY", "rawweekly");

                        if(result == 0)
                            Console.WriteLine("Successfully pulled WEEKLY data from AplhaVantage!\n");
                        else
                            Console.WriteLine("Unable to pull WEEKLY data from AlphaVantage\n");
                        break;

                    case "4":
                        Console.Clear();
                        result = AVFunctionCall("TIME_SERIES_MONTHLY", "rawmonthly");

                        if(result == 0)
                            Console.WriteLine("Successfully pulled MONTHLY data from AplhaVantage!\n");
                        else
                            Console.WriteLine("Unable to pull MONTHLY data from AlphaVantage\n");
                        break;

                    case "5":
                        Console.Clear();
                        HelpCallAVAPI();
                        break;

                    case "6":
                        Console.Clear();
                        return 0;

                    default:
                        return 1;
                }
            }

            return 1;
        }

        static private void HelpCallAVAPI()
        {
            Console.Clear();
            Console.WriteLine("================================================================================================\n" +
                              "                           MORE INFORMATION ABOUT API CALL\n" +
                              "================================================================================================\n\n" +
                              "API parameters:\n" +
                              "     function: type of data requested from API\n" +
                              "     symbol: company requested\n" +
                              "     outputsize: [compact | full] compact pulls past 100 days; full pulls 20+ years of histocial data\n" +
                              "     datatype: [json | csv] pulls data as either json or csv\n" +
                              "     apikey: needed to access api\n\n" +
                              "Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static private void PressToCont()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static private int AVFunctionCall(string function, string table)
        {
            Console.WriteLine("Please Enter Symbol below, if enter more than please separate by a comma");
            Console.Write("Symbol(s):");

            var symbolString = Console.ReadLine();

            List<string> symbols = symbolString.Split(',').ToList();
            symbols = symbols.Select(x => x.Trim()).ToList();

            try
            {
                foreach(string symbol in symbols)
                {
                    string URL = CreateURL(function, symbol);

                    string file = CreateCSV(URL, symbol);

                    InsertData(file, symbol, table);

                    DeleteCSV(file);

                    PressToCont();
                }

                return 0;
            }
            catch
            {
                Console.Clear();
                return 1;
            }
        }

        static private string CreateURL(string function, string symbol)
        {
            string URL, interval = "60min", datatype = "csv", apiKey = "KOR2N2FPTN8LVEV4";

            if (function == "TIME_SERIES_INTRADAY")
            {
                URL = $"https://www.alphavantage.co/query?function={function}&symbol={symbol}&interval={interval}&datatype={datatype}&apikey={apiKey}";
            }
            else
            {
                URL = $"https://www.alphavantage.co/query?function={function}&symbol={symbol}&datatype={datatype}&apikey={apiKey}";
            }

            return URL;
        }

        static private string CreateCSV(string URL, string symbol)
        {
            string fileName = symbol + "-" + DateTime.Now.ToString("MM-dd-yyyy");

            WebClient webClient = new();

            string data = webClient.DownloadString(URL);

            File.WriteAllText($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv", data);
            Console.WriteLine($"{fileName}.csv created\n");

            return fileName;
        }

        static private void InsertData(string fileName, string symbol, string table)
        {
            List<RawStockData> stockList = new List<RawStockData>();

            using(StreamReader reader = new StreamReader($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv"))

            using(CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                while(csv.Read())
                {
                    RawStockData stockData = new RawStockData
                    {
                        Symbol = symbol,
                        TimeStamp = Convert.ToDateTime(DateTime.Parse(csv.GetField<string>("timestamp")).ToString("MM-dd-yyyy")),
                        Open = csv.GetField<double>("open"),
                        High = csv.GetField<double>("high"),
                        Low = csv.GetField<double>("low"),
                        Close = csv.GetField<double>("close"),
                        Volume = csv.GetField<int>("volume")
                    };

                    stockList.Add(stockData);
                }
            }

            int count = 0;

            Console.WriteLine($"Begining {symbol} insert\n");

            foreach(var stock in stockList)
            {
                InsertRawData(table, stock);
                count++;
            }

            Console.WriteLine($"Finished {symbol} insert: {count} rows inserted\n");
        }

        static private void DeleteCSV(string fileName)
        {
            if (File.Exists($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv"))
            {
                File.Delete($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv");
                Console.WriteLine($"{fileName}.csv deleted\n");
            }
        }

        //static private void Test()
        //{
        //    string queryURL = $"https://www.alphavantage.co/query?function={function}&symbol={symbol}&outputsize={outputsize}&datatype={datatype}&apikey={apiKey}";
        //    string fileName = symbol + "-" + DateTime.Now.ToString("MM-dd-yyyy");

        //    WebClient webClient = new();

        //    string data = webClient.DownloadString(queryURL);

        //    File.WriteAllText($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv", data);
        //    Console.WriteLine($"{fileName}.csv created\n");

        //    List<RawStockData> stockList = new List<RawStockData>();

        //    // Read in CSV File
        //    using(StreamReader reader = new StreamReader($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv"))

        //    using(CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //    {
        //        csv.Read();
        //        csv.ReadHeader();

        //        while(csv.Read())
        //        {
        //            RawStockData stockData = new RawStockData
        //            {
        //                Symbol = symbol,
        //                TimeStamp = Convert.ToDateTime(DateTime.Parse(csv.GetField<string>("timestamp")).ToString("MM-dd-yyyy")),
        //                Open = csv.GetField<double>("open"),
        //                High = csv.GetField<double>("high"),
        //                Low = csv.GetField<double>("low"),
        //                Close = csv.GetField<double>("close"),
        //                Volume = csv.GetField<int>("volume")
        //            };

        //            stockList.Add(stockData);
        //        }
        //    }

        //    int count = 0;

        //    Console.WriteLine($"Begining {symbol} insert\n");

        //    foreach(var stock in stockList)
        //    {
        //        InsertRawData(table, stock);
        //        count++;
        //    }

        //    Console.WriteLine($"Finished {symbol} insert: {count} rows inserted\n");

        //    if(File.Exists($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv"))
        //    {
        //        File.Delete($@"C:\Users\{Environment.UserName}\5560\{fileName}.csv");
        //        Console.WriteLine($"{fileName}.csv deleted\n");
        //    }
        //}

        static private IDbConnection CreateConn()
        {
            return new MySqlConnection("Server=127.0.0.1;Database=sylvia;Uid=root;Pwd=myRootToSQL");
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

        static private int Analysis()
        {
            return 1;
        }
    }
}