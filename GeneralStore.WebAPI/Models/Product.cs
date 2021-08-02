using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStore.WebAPI.Models
{
    public class Product
    {
        [Key]
        public string SKU { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public int NumberInInventory { get; set; }
        public bool IsInStock
        {
            get
            {
                return NumberInInventory > 0;
            }
        }
    }
}