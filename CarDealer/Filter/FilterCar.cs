using CarDealer.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealer.Filter
{
    public class FilterCar
    {
        public List<Car> manufacturerCars;
        public List<Car> modelCars;
        public List<Car> typeCars;
    }
}