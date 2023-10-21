using Dapper;
using MySqlConnector;
using Sylvia_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sylvia_API.Repository
{
    public class AlphaVantage
    {
        private IDbConnection CreateConn()
        {
            return new MySqlConnection("Server=xxxx;Database=xxxx;Uid=xxxx;Pwd=xxxx");
        }

        public void InsertRawData(string table, RawStockData data)
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
