using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarDealer.Models.Users.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using CarDealer.Models.Users.Models;
using System.Threading.Tasks;
using CarDealer.Models.Domain;
using CarDealer.Models.Paging;
using CarDealer.Filter;

namespace CarDealer.Models.Users.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Name, Email = model.Email };
                IdentityResult result =
                    await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Пользователь не найден" });
            }
        }

        public async Task<ActionResult> Edit(string id)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, string password)
        {
            AppUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail
                    = await UserManager.UserValidator.ValidateAsync(user);

                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }

                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass
                        = await UserManager.PasswordValidator.ValidateAsync(password);

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash =
                            UserManager.PasswordHasher.HashPassword(password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                if ((validEmail.Succeeded && validPass == null) ||
                        (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }
            return View(user);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }


        [Authorize(Roles = "Administrators")]
        public ActionResult EditEntryCarInDB(int prodID)
        {
            CarContext db = new CarContext();
            Car c = db.Cars.Find(prodID);

            if (c != null)
            {
                IQueryable<OrderDetail> orderDetails = db.OrderDetails;

                foreach (OrderDetail o in orderDetails)
                {
                    // нашли машину в заказе -> ! не удаляем
                    if (o.car_id == prodID)
                    {
                        return Redirect("AdminCatalog");
                    }
                }

                db.Cars.Remove(c);
                db.SaveChanges();
            }

            return Redirect("AdminCatalog");
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult DeleteFromDB(int prodID)
        {
            CarContext db = new CarContext();
            Car c = db.Cars.Find(prodID);

            if (c != null)
            {
                IQueryable<OrderDetail> orderDetails = db.OrderDetails;

                foreach (OrderDetail o in orderDetails)
                {
                    if (o.car_id == prodID)
                    {
                        return Redirect("AdminCatalog");
                    }
                }

                db.Cars.Remove(c);
                db.SaveChanges();
            }

            return Redirect("AdminCatalog");
        }
        
        [Authorize(Roles = "Administrators")]
        public ActionResult AddInDB(String country, String manufacturer, String model, String type, decimal price = 0)
        {
            if(country == null & manufacturer == null & model == null & type == null && price == 0)
            {
               
            }
            else
            {             
                CarContext db = new CarContext();

                int nextId = db.Cars.Count();

                Car car = db.Cars.Find(nextId);
                while(car != null)
                {
                    nextId++;
                    car = db.Cars.Find(nextId);
                }

                Car c = new Car
                {
                    manufacturer = manufacturer,
                    model = model,
                    type = type,
                    price = price,
                    country = country,
                    car_id = nextId
                };

                db.Cars.Add(c);
                db.SaveChanges();

            }

            return View();
        }

        [Authorize(Roles = "Administrators")]
        public ActionResult AdminCatalog(String manufacturerList, String relationList, CarFilter carFilter = null, bool restore = false, int page = 1)
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

        [Authorize(Roles = "Administrators")]
        public ActionResult AdminIndex()
        {
            return View("~/Views/Admin/AdminIndex.cshtml");
        }
    }
}
