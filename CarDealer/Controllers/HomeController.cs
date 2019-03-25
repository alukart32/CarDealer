using CarDealer.Filter;
using CarDealer.Models.Domain;
using CarDealer.Models.Paging;
using CarDealer.Models.Purchase;
using CarDealer.Models.Stock;
using CarDealer.Models.Users;
using CarDealer.Models.Users.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarDealer.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
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

        public ActionResult Catalog(String manufacturerList, String relationList, CarFilter carFilter = null, bool restore = false, int page = 1)
        {
            CarContext db = new CarContext();
            IQueryable<Car> cars = db.Cars;

            if (carFilter == null)
                carFilter = new CarFilter();
            
            if (restore) // если необходимо восстановить данные, то восстанавливаем из сессии
                carFilter = CarFilter.GetCarFilter(Session["CarFilter"]);
            

            if (manufacturerList != "" && manufacturerList != null)
                cars = cars.Where(e => e.manufacturer.Equals(manufacturerList));
            if (carFilter.model != "" && carFilter.model != null)
                cars = cars.Where(e => e.model.Equals(carFilter.model));
            if (carFilter.type != "" && carFilter.type != null)
                cars = cars.Where(e => e.type.Equals(carFilter.type));

            if (carFilter.price > 0)
            {
                
                if (relationList != "")
                {
                    decimal d = carFilter.price;

                    switch (relationList)
                    {
                        case "<":
                            cars = cars.Where(e => e.price < d);
                            break;
                        case "<=":
                            cars = cars.Where(e => e.price <= d);
                            break;
                        case ">":
                            cars = cars.Where(e => e.price > d);
                            break;
                        case ">=":
                            cars = cars.Where(e => e.price >= d);
                            break;
                        case "=":
                            cars = cars.Where(e => e.price == d);
                            break;
                    }
                }
            }

            int pageSize = 5;
                
            IEnumerable<Car> carsPerPages = cars.ToList().Skip((page - 1) * pageSize).Take(pageSize);
            CarPageInfo pageInfo = new CarPageInfo { PageNumber = page, PageSize = pageSize, TotalItems = cars.ToList().Count };
            CarIndexView ivm = new CarIndexView { PageInfo = pageInfo, Cars = carsPerPages };

            // сейвим текущий фильтр в сессию
            Session["CarFilter"] = carFilter; 
            ivm.carFilter = carFilter;
            
            return View(ivm);
        }

        public ActionResult OrderStatus(int ID, string msg, string mail)
        {
            ViewBag.Message = "Статус заказа: " + msg;
            ViewBag.OrdID = "Номер заказа: " + ID.ToString();
            ViewBag.Mail = "E-mal: " + mail;

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
                 myCart.AddItem((int)ID, car.manufacturer, car.price, 1);
                // Запись корзины в сессию
                Session["MyCart"] = myCart;
            }
            return Redirect("/Home/Catalog/?restoreFilter=true");
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
        public ActionResult Buy(string FirstName, string LastName, string Email)
        {

            Stock sdb = new Stock();
            // Ищем существующего заказчика
            Customer cust = sdb.FindCustomer(FirstName, LastName, Email);
            if (cust == null) // либо создаем нового
                cust = sdb.AddCustomer(FirstName, LastName, Email);
 
            // Читаем корзину из сессии
            ShopBasket myCart = ShopBasket.GetCart(Session["MyCart"]);
            // Пытаемся оформить заказ
            string message;
            Order o = sdb.CommitTrans(cust.customer_id, myCart, out message);
            // В случае неудачи обнуляем номер
            int id = (o != null) ? o.order_id : 0;
            // В случае успеха очищаем сессию с корзиной
            Session["MyCart"] = (o != null) ? null : Session["MyCart"];
            // переходим на страницу со статусом
            return RedirectToAction("OrderStatus", "Home",
            new { ID = id, msg = message, mail = cust.email });
        }
        public ActionResult DeleteFromCart(int prodID)
        {
            ShopBasket basket = ShopBasket.GetCart(Session["MyCart"]);
            basket.RemoveLine(prodID);
            Session["MyCart"] = basket;

            return Redirect("/Home/CartBrowse");
        }

        public ActionResult ChangeQuantity(int prodID, bool add)
        {
            ShopBasket basket = ShopBasket.GetCart(Session["MyCart"]);
            basket.changeQuantity(prodID,add);
            Session["MyCart"] = basket;

            return Redirect("/Home/CartBrowse");
        }
    }
}