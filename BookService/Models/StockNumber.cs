using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookService.Models
{
    public class StockNumber
    {
        public bool IsStockAvailableCheckRequired { get; set; }
        
        public string Name { get; set; }

        public int Key { get; set; }
        
    }
}