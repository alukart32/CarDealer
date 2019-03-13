using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealer.Filter
{
    public class CarFilter
    {
        public String model { get; set; }
        public String type { get; set; }
        public int rest { get; set; }

        public CarFilter()
        {
            model = null;
            type = null;
            rest = -1;
        }

        public static CarFilter GetCarFilter(object s)
        {
            CarFilter filter = (CarFilter)s;
            if (filter == null) // если фильтр пуст, то создаем новый объект
            {
                filter = new CarFilter();
                s = filter;
            }
            return filter;
        }
    }
}