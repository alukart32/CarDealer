using CarDealer.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarDealer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Catalog()
        {
            CarContext db = new CarContext();
            //List<Car> products = db.Cars.Where(e => e.manufacturer.Equals("BMW")).ToList();
            List<Car> cars = db.Cars.ToList();
            return View(cars);
        }

        public ActionResult OrderStatus(int ID, string msg, string mail)
        {
            ViewBag.Message = "Статус заказа: " + msg;
            ViewBag.OrdID = "Номер заказа: " + ID.ToString();
            ViewBag.Address = "Адрес доставки: " + mail;

            return View();
        }

    }
}