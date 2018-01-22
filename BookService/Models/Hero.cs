using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookService.Models
{
    public class Hero
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
    }
}