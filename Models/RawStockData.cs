using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sylvia_API.Models
{
    public class RawStockData
    {
        public string Symbol { get; set; }
        public string TimeStamp { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double AdjustedClose { get; set; }
        public int Volume { get; set; }
        public double DividendAmount { get; set; }
        public double SplitCoefficient { get; set; }
    }
}
