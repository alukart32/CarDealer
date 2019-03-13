using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealer.Filter
{
    public class PriceFilter
    {
        public decimal price { get; set; }
        public PriceFilter()
        {
            price = 0;
        }

        public static PriceFilter GetPriceFilter(object s)
        {
            PriceFilter filter = (PriceFilter)s;
            if (filter == null) // если фильтр пуст, то создаем новый объект
            {
                filter = new PriceFilter();
                s = filter;
            }
            return filter;
        }
    }
}