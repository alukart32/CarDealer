using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealer.Models.Purchase
{
    public class ShopBasketPos
    {
        public int ProdID { get; set; }
        public string ProdName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}