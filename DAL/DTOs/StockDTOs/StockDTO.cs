using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs.StockDTOs
{
    public class StockDTO
    {
        public int StockId { get; set; }
        public string? StockName { get; set; }
        public string? StockAddress { get; set; }
    }
}