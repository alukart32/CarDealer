using CarDealer.Filter;
using CarDealer.Models.Domain;
using CarDealer.Models.Paging;
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

        public List<Car> page_cars;
        public ActionResult Catalog(String manufacturerList, String model, String type, String price, String relationList, int page = 1)
        {
            CarContext db = new CarContext();
            IQueryable<Car> cars = db.Cars;

            FilterCar filterCar = new FilterCar();

            if (manufacturerList != "" && manufacturerList != null)
                cars = cars.Where(e => e.manufacturer.Equals(manufacturerList));
            if (model != "" && model != null)
                cars = cars.Where(e => e.model.Equals(model));
            if (type != "" && type != null)
                cars = cars.Where(e => e.type.Equals(type));

            if(price != "" && price != null)
            {
                decimal d = decimal.Parse(price);

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

            /*
            if ((manufacturer != "" && manufacturer != null)
                ||( model != "" && model != null)
                || (type != "" && type != null)) {
                
                filterCar.manufacturerCars = cars.Where(e => e.manufacturer.Equals(manufacturer)).ToList();
                 
                filterCar.modelCars = cars.Where(e => e.model.Equals(model)).ToList();

                filterCar.typeCars = cars.Where(e => e.type.Equals(type)).ToList();

                List<Car> filteredCars = null;

                // есть 3 критерия
                if (filterCar.manufacturerCars.Capacity != 0 && filterCar.modelCars.Capacity != 0 && filterCar.typeCars.Capacity != 0)
                {
                    filteredCars = filterCar.manufacturerCars;
                    filteredCars = filteredCars.Intersect(filterCar.modelCars).ToList();
                    filteredCars = filteredCars.Intersect(filterCar.typeCars).ToList();
                }
                else
                {
                    // каких-то 2 критерия
                    if(filterCar.manufacturerCars.Capacity != 0 && filterCar.modelCars.Capacity != 0)
                    {
                        filteredCars = filterCar.manufacturerCars;
                        filteredCars = filteredCars.Intersect(filterCar.modelCars).ToList();
                    }
                    else
                    {
                        if(filterCar.manufacturerCars.Capacity != 0 && filterCar.typeCars.Capacity != 0)
                        {
                            filteredCars = filterCar.manufacturerCars;
                            filteredCars = filteredCars.Intersect(filterCar.typeCars).ToList();
                        }
                        else
                        {
                            if(filterCar.modelCars.Capacity != 0 && filterCar.typeCars.Capacity != 0)
                            {
                                filteredCars = filterCar.modelCars;
                                filteredCars = filteredCars.Intersect(filterCar.typeCars).ToList();
                            }
                            else
                            {
                                // один критерий
                                if (filterCar.manufacturerCars.Capacity != 0)
                                    filteredCars = filterCar.manufacturerCars;
                                if(filterCar.modelCars.Capacity != 0)
                                    filteredCars = filterCar.modelCars;
                                if (filterCar.typeCars.Capacity != 0)
                                    filteredCars = filterCar.typeCars;
                            }
                        }
                    }
                }
                
               page_cars = filteredCars;

               if(page_cars == null )
                    page_cars = cars.ToList();
            }
            else*/

            page_cars = cars.ToList();

            int pageSize = 5;
          
                //{
            //    if (!changePages)
            //    {
            //        pageSize = pageSizes;
            //        prevPages = pageSizes;
            //        changePages = true;
            //    }
            //    else
            //    {
            //        if(prevPages != pageSizes)
            //            if(pageSizes == 50)
            //            {
            //                pageSize = 5;
            //                changePages = false;
            //            }
                        
            //    }

            //}

            IEnumerable<Car> carsPerPages = page_cars.Skip((page - 1) * pageSize).Take(pageSize);
            CarPageInfo pageInfo = new CarPageInfo { PageNumber = page, PageSize = pageSize, TotalItems = page_cars.Count };
            CarIndexView ivm = new CarIndexView { PageInfo = pageInfo, Cars = carsPerPages };
            return View(ivm);
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