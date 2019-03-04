using CarDealer.Filter;
using CarDealer.Models.Domain;
using CarDealer.Models.Purchase;
using CarDealer.Models.Stock;
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

        public ActionResult Catalog(String manufacturer, String model, String type)
        {
            CarContext db = new CarContext();
            //List<Car> products = db.Cars.Where(e => e.manufacturer.Equals("BMW")).ToList();
            //List<Car> cars = db.Cars.ToList();
            IQueryable<Car> cars = db.Cars;

            FilterCar filterCar = new FilterCar();

            if (manufacturer != null || model != null || type != null) {
                
                if (manufacturer != "")
                {
                    foreach (var item in cars)
                    {
                        if (item.manufacturer.Equals(manufacturer))
                        {
                            filterCar.manufacturerCars = cars.Where(e => e.manufacturer.Equals(manufacturer)).ToList();
                            break;
                        }
                    }
                }

                if (model != "")
                {
                    foreach (var item in cars)
                    {
                        if (item.model.Equals(model))
                        {
                            filterCar.modelCars = cars.Where(e => e.model.Equals(model)).ToList();
                            break;
                        }
                    }
                }

                if (type != "")
                {
                    foreach (var item in cars)
                    {
                        if (item.type.Equals(type))
                        {
                           filterCar.typeCars = cars.Where(e => e.type.Equals(type)).ToList();
                           break;
                        }
                    }
                }

                List<Car> filteredCars = null;
                if (filterCar.manufacturerCars != null)
                    filteredCars = filterCar.manufacturerCars;

                if (filterCar.modelCars != null && filteredCars != null)
                    filteredCars = filteredCars.Intersect(filterCar.modelCars.Where(e => e.manufacturer.Equals(manufacturer))).ToList();
                else
                    if(filterCar.modelCars != null)
                    filteredCars = filterCar.modelCars;

                if(filteredCars != null)
                {
                    if (filterCar.typeCars != null)
                    {
                        if (filterCar.modelCars != null)
                            filteredCars = filteredCars.Intersect(filterCar.typeCars.Where(e => e.model.Equals(model))).ToList();
                        else
                            filteredCars = filterCar.typeCars;
                    }
                }
                else
                    return View(cars);

                return View(filteredCars);
            }
            return View(cars);
        }

        public ActionResult OrderStatus(int ID, string msg, string mail)
        {
            ViewBag.Message = "Статус заказа: " + msg;
            ViewBag.OrdID = "Номер заказа: " + ID.ToString();
            ViewBag.Address = "Адрес доставки: " + mail;

            return View();
        }

        public ActionResult AddToCart(int? ID = 1)
        {
            CarContext db = new CarContext();
            Car car = db.Cars.Find(ID);
            // Чтение корзины из сессии
            ShopBasket myCart = ShopBasket.GetCart(Session["MyCart"]);
            if (ID != null)
            {
               // myCart.AddItem((int)ID, car.ProdName, car.price, 1);
                // Запись корзины в сессию
                Session["MyCart"] = myCart;
            }
            return Redirect("/Home/Catalog");
        }
        
        public ActionResult CartBrowse()
        {
            // Чтение корзины из сессии
            ShopBasket myCart = ShopBasket.GetCart(Session["MyCart"]);
            List<ShopBasketPos> lines = (List<ShopBasketPos>)myCart.Lines;
            return View(lines);
        }
        [HttpGet]
        public ActionResult Buy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Buy(string FirstName, string LastName, string Address)
        {
            Stock sdb = new Stock();
            // Добавляем заказчика
            Customer c = sdb.AddCustomer(FirstName, LastName, Address);
            // Читаем корзину из сессии
            ShopBasket myCart = ShopBasket.GetCart(Session["MyCart"]);
            // Пытаемся оформить заказ
            string message;
            Order o = sdb.CommitTrans(c.customer_id, myCart, out message);
            // В случае неудачи обнуляем номер
            int id = (o != null) ? o.order_id : 0;
            // В случае успеха очищаем сессию с корзиной
            Session["MyCart"] = (o != null) ? null : Session["MyCart"];
            // переходим на страницу со статусом
            return RedirectToAction("OrderStatus", "Home",
            new { ID = id, msg = message, mail = c.email });
        }
    }
}