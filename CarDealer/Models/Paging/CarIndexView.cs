using CarDealer.Filter;
using CarDealer.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealer.Models.Paging
{
    public class CarIndexView
    {
        public IEnumerable<Car> Cars { get; set; }
        public CarPageInfo PageInfo { get; set; }
        public CarFilter carFilter { get; set; }
        public PriceFilter priceFilter { get; set; }
        public PageFilter pageFilter { get; set; }
    }
}