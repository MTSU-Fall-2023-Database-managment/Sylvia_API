using System.Data;
using System.Globalization;
using System.Linq;
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
                var todo = Console.ReadKey();
                
                switch(todo.Key)
                {
                    case ConsoleKey.NumPad1 or ConsoleKey.D1:
                        Console.Clear();
                        result = AlphaVantage();

                        if(result == 1)
                            return;
                        break;

                    case ConsoleKey.NumPad2 or ConsoleKey.D2:
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
                var todo = Console.ReadKey();

                switch(todo.Key)
                {
                    case ConsoleKey.NumPad1 or ConsoleKey.D1:
                        Console.Clear();
                        result = CallAVAPI();

                        if(result == 1)
                            return 1;
                        else
                            return 0;

                    case ConsoleKey.NumPad2 or ConsoleKey.D2:
                        Console.Clear();
                        HelpAlphaVantage();
                        break;

                    case ConsoleKey.NumPad3 or ConsoleKey.D3:
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
                var todo = Console.ReadKey();

                switch(todo.Key)
                {
                    case ConsoleKey.NumPad1 or ConsoleKey.D1:
                        Console.Clear();
                        result = AVFunctionCall("TIME_SERIES_INTRADAY", "rawintraday");

                        if(result == 0)
                            Console.WriteLine("Successfully pulled INTRADAY data from AplhaVantage!\n");
                        else
                            Console.WriteLine("Unable to pull INTRADAY data from AlphaVantage\n");
                        break;

                    case ConsoleKey.NumPad2 or ConsoleKey.D2:
                        Console.Clear();
                        result = AVFunctionCall("TIME_SERIES_DAILY", "rawdaily");

                        if(result == 0)
                            Console.WriteLine("Successfully pulled DAILY data from AplhaVantage!\n");
                        else
                            Console.WriteLine("Unable to pull DAILY data from AlphaVantage\n");
                        break;

                    case ConsoleKey.NumPad3 or ConsoleKey.D3:
                        Console.Clear();
                        result = AVFunctionCall("TIME_SERIES_WEEKLY", "rawweekly");

                        if(result == 0)
                            Console.WriteLine("Successfully pulled WEEKLY data from AplhaVantage!\n");
                        else
                            Console.WriteLine("Unable to pull WEEKLY data from AlphaVantage\n");
                        break;

                    case ConsoleKey.NumPad4 or ConsoleKey.D4:
                        Console.Clear();
                        result = AVFunctionCall("TIME_SERIES_MONTHLY", "rawmonthly");

                        if(result == 0)
                            Console.WriteLine("Successfully pulled MONTHLY data from AplhaVantage!\n");
                        else
                            Console.WriteLine("Unable to pull MONTHLY data from AlphaVantage\n");
                        break;

                    case ConsoleKey.NumPad5 or ConsoleKey.D5:
                        Console.Clear();
                        HelpCallAVAPI();
                        break;

                    case ConsoleKey.NumPad6 or ConsoleKey.D6:
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

            string symbolString;

            do
            {
                symbolString = Console.ReadLine();

                if (symbolString == null)
                {
                    Console.WriteLine("Please enter one or more Symbols before continuing\n");
                }
            } while(symbolString == null);

            List<string> symbols = symbolString.Split(',').ToList();
            symbols = symbols.Select(x => x.Trim()).ToList();

            try
            {
                foreach(string symbol in symbols)
                {
                    string URL = CreateURL(function, symbol);

                    string data = GetCSV(URL, symbol);

                    if (data.Contains("Thank you for using Alpha Vantage! Our standard API rate limit is 25 requests per day."))
                    {
                        Console.WriteLine("Thank you for using Alpha Vantage! Our standard API rate limit is 25 requests per day.");
                        return 1;
                    }

                    InsertData(data, symbol, table);

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

        static private string GetCSV(string URL, string symbol)
        {
            WebClient webClient = new();

            string data = webClient.DownloadString(URL);

            return data;
        }

        static private void InsertData(string data, string symbol, string table)
        {
            List<RawStockData> stockList = new();

            using(StringReader reader = new StringReader(data))

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
                        Open = csv.GetField<decimal>("open"),
                        High = csv.GetField<decimal>("high"),
                        Low = csv.GetField<decimal>("low"),
                        Close = csv.GetField<decimal>("close"),
                        Volume = csv.GetField<ulong>("volume")
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

        static private IDbConnection CreateConn()
        {
            return new MySqlConnection("Server=xxx;Database=xxx;Uid=xxx;Pwd=xxx");
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
            int result;
            bool run = true;

            while(run)
            {
                Console.WriteLine("Analyze collected data:\n" +
                                  "-----------------------\n" +
                                  "1: List of Companies\n" +
                                  "2: Highest stock price\n" +
                                  "3: Lowest stock price\n" +
                                  "4: Information on Analysis\n" +
                                  "5: Main menu\n" +
                                  "~: Close application\n\n");

                Console.Write("Selection: ");
                var todo = Console.ReadKey();

                switch(todo.Key)
                {
                    case ConsoleKey.NumPad1 or ConsoleKey.D1:
                        Console.Clear();
                        result = SelectTable(1);

                        if(result == 1)
                            return 1;
                        else
                            return 0;
                    
                    case ConsoleKey.NumPad2 or ConsoleKey.D2:
                        Console.Clear();
                        result = SelectTable(2);

                        if(result == 1)
                            return 1;
                        else
                            return 0;
                    
                    case ConsoleKey.NumPad3 or ConsoleKey.D3:
                        Console.Clear();
                        result = SelectTable(3);

                        if(result == 1)
                            return 1;
                        else
                            return 0;
                    
                    case ConsoleKey.NumPad4 or ConsoleKey.D4:
                        Console.Clear();
                        HelpAnalysis();
                        break;

                    case ConsoleKey.NumPad5 or ConsoleKey.D5:
                        Console.Clear();
                        return 0;

                    default:
                        return 1;
                }
            }

            return 1;
        }

        static private void HelpAnalysis()
        {
            Console.WriteLine("================================================================================================\n" +
                              "                           MORE INFORMATION ABOUT ANALYSIS\n" +
                              "================================================================================================\n\n" +
                              "Functions:\n" +
                              "        List of Companies: Returns a list of distinct company symbols in the given table\n" +
                              "      Highest stock price: Returns a list of companies with the highest stock price in the given table\n" +
                              "       Lowest stock price: Returns a list of caompanies with the lowest stock price in the given table\n" +
                              "Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        static private int SelectTable(int selection)
        {
            int result;
            bool run = true;

            while(run)
            {
                Console.WriteLine("Select table for analysis:\n" +
                                  "--------------------------\n" +
                                  "1: INTRADAY\n" +
                                  "2: DAILY\n" +
                                  "3: WEEKLY\n" +
                                  "4: MONTHLY\n" +
                                  "5: Main menu\n" +
                                  "~: Close application\n\n");

                Console.Write("Selection: ");
                var todo = Console.ReadKey();

                switch(todo.Key)
                {
                    case ConsoleKey.NumPad1 or ConsoleKey.D1:
                        Console.Clear();

                        if (selection == 1)
                        {
                            result = GetCompanies("rawintraday");
                        }
                        else if (selection == 2)
                        {
                            result = GetHighest("rawintraday");
                        }
                        else if (selection == 3)
                        {
                            result = GetLowest("rawintraday");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong please try again");
                            return 0;
                        }
                        
                        if(result == 0)
                            Console.WriteLine("Successfully pulled INTRADAY data from AplhaVantage!\n");
                        else
                            Console.WriteLine("Unable to pull INTRADAY data from AlphaVantage\n");
                        break;

                    case ConsoleKey.NumPad2 or ConsoleKey.D2:
                        Console.Clear();

                        if(selection == 1)
                        {
                            result = GetCompanies("rawdaily");
                        }
                        else if(selection == 2)
                        {
                            result = GetHighest("rawdaily");
                        }
                        else if(selection == 3)
                        {
                            result = GetLowest("rawdaily");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong please try again");
                            return 0;
                        }

                        if(result == 0)
                            Console.WriteLine("Successfully pulled DAILY data from AplhaVantage!\n");
                        else
                            Console.WriteLine("Unable to pull DAILY data from AlphaVantage\n");
                        break;

                    case ConsoleKey.NumPad3 or ConsoleKey.D3:
                        Console.Clear();

                        if(selection == 1)
                        {
                            result = GetCompanies("rawweekly");
                        }
                        else if(selection == 2)
                        {
                            result = GetHighest("rawweekly");
                        }
                        else if(selection == 3)
                        {
                            result = GetLowest("rawweekly");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong please try again");
                            return 0;
                        }

                        if(result == 0)
                            Console.WriteLine("Successfully pulled WEEKLY data from AplhaVantage!\n");
                        else
                            Console.WriteLine("Unable to pull WEEKLY data from AlphaVantage\n");
                        break;

                    case ConsoleKey.NumPad4 or ConsoleKey.D4:
                        Console.Clear();

                        if(selection == 1)
                        {
                            result = GetCompanies("rawmonthly");
                        }
                        else if(selection == 2)
                        {
                            result = GetHighest("rawmonthly");
                        }
                        else if(selection == 3)
                        {
                            result = GetLowest("rawmonthly");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong please try again");
                            return 0;
                        }

                        if(result == 0)
                            Console.WriteLine("Successfully pulled MONTHLY data from AplhaVantage!\n");
                        else
                            Console.WriteLine("Unable to pull MONTHLY data from AlphaVantage\n");
                        break;

                    case ConsoleKey.NumPad5 or ConsoleKey.D5:
                        Console.Clear();
                        return 0;

                    default:
                        return 1;
                }
            }

            return 1;
        }

        static private int GetCompanies(string table)
        {
            using var conn = CreateConn();

            try
            {
                var query = @$"SELECT DISTINCT Symbol FROM {table} ORDER BY Symbol";

                conn.Open();

                var results = conn.ExecuteReader(query);

                var resultList = new List<OrderedStockData>();

                while (results.Read())
                {
                    resultList.Add(
                        new OrderedStockData
                        {
                            Symbol = (string)results["symbol"]
                        });
                }

                PrintResults(resultList, "Companies", table);

                return 0;
            }
            catch
            {
                return 1;
            }
        }

        static private int GetHighest(string table)
        {
            using var conn = CreateConn();

            try
            {
                var query = @$"SELECT * FROM {table} WHERE high = (SELECT max(high) FROM {table})";

                conn.Open();

                var results = conn.ExecuteReader(query);

                var resultList = new List<OrderedStockData>();

                while(results.Read())
                {
                    resultList.Add(
                        new OrderedStockData
                        {
                            Id = (int)results["id"],
                            Symbol = (string)results["symbol"],
                            TimeStamp = (DateTime)results["timestamp"],
                            Open = (decimal)results["open"],
                            High = (decimal)results["high"],
                            Low = (decimal)results["low"],
                            Close = (decimal)results["close"],
                            Volume = (ulong)results["volume"]
                        });
                }

                PrintResults(resultList, "Highest Stock Price", table);

                return 0;
            }
            catch
            {
                return 1;
            }
        }

        static private int GetLowest(string table)
        {
            using var conn = CreateConn();

            try
            {
                var query = @$"SELECT * FROM {table} WHERE low = (SELECT min(low) FROM {table})";

                conn.Open();

                var results = conn.ExecuteReader(query);

                var resultList = new List<OrderedStockData>();

                while(results.Read())
                {
                    resultList.Add(
                        new OrderedStockData
                        {
                            Id = (int)results["id"],
                            Symbol = (string)results["symbol"],
                            TimeStamp = (DateTime)results["timestamp"],
                            Open = (decimal)results["open"],
                            High = (decimal)results["high"],
                            Low = (decimal)results["low"],
                            Close = (decimal)results["close"],
                            Volume = (ulong)results["volume"]
                        });
                }

                PrintResults(resultList, "Lowest Stock Price", table);

                return 0;
            }
            catch
            {
                return 1;
            }
        }

        static private void PrintResults(List<OrderedStockData> results, string type, string table)
        {
            string header = "================================================================================================\n" +
                           $"                   LIST OF RESULTS FOR {type.ToUpper()} IN {table.ToUpper()}\n" +
                            "================================================================================================\n\n";
            
            string body = $"{type} in {table}\n";
            int count = 1;
            
            if (type == "Companies")
            {
                foreach (var res in results)
                {
                    body += $"{count}: {res.Symbol}\n";
                    count++;
                }
            }
            else
            {
                foreach(var res in results)
                {
                    body += $"ID: {res.Id}\n\t   Symbol: {res.Symbol}\n\tTimeStamp: {res.TimeStamp}\n\t     Open: {res.Open}\n\t     High: {res.High}\n\t      Low: {res.Low}\n\t    Close: {res.Close}\n\t   Volume: {res.Volume}\n\n";
                }
            }

            Console.WriteLine(header + body);
            PressToCont();
        }
    }
}