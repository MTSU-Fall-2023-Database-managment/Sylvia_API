using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sylvia_API.Models
{
    public class RawStockData
    {
        public string? Symbol { get; set; }
        public DateTime? TimeStamp { get; set; }
        public decimal? Open { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public ulong? Volume { get; set; }
    }
    
    public class OrderedStockData
    {
        public int? Id { get; set; }
        public string? Symbol { get; set; }
        public DateTime? TimeStamp { get; set; }
        public decimal? Open { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public ulong? Volume { get; set; }
    }
}
