using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealer.Filter
{
    public class PageFilter
    {
        public int amount { get; set; }
        public PageFilter()
        {
            amount = 5;
        }

        public static PageFilter GetPageFilter(object s)
        {
            PageFilter filter = (PageFilter)s;
            if (filter == null) // если фильтр пуст, то создаем новый объект
            {
                filter = new PageFilter();
                s = filter;
            }
            return filter;
        }
    }
}