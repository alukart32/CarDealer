using CarDealer.Models.Domain;
using CarDealer.Models.Purchase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CarDealer.Models.Stock
{
    public class Stock
    {
        public CarContext db;
        public Stock()
        {
            db = new CarContext();
        }
        // Найти заказчика
        public Customer FindCustomer(string fn, string ln, string em)
        {
            IQueryable<Customer> c = db.Customers.Where(p =>
                    p.firstName == fn &&
                    p.lastName == ln &&
                    p.email == em 
                );
            return c.FirstOrDefault();
        }
        // Добавление заказчика
        public Customer AddCustomer(string fn, string ln, string mail)
        {
            Customer c = new Customer
            {
                firstName = fn,
                lastName = ln,
                email = mail,
               // customer_id = db.Customers.Count()
            };
            db.Customers.Add(c);
            db.SaveChanges();
            return c;
        }

        public Order AddOrder(int CustID, ShopBasket myCart)
        {
            // Создаем и инициализируем новый заказ
            Order o = new Order
            {
                date = DateTime.Today, // Текущая дата
                customer = CustID // Ид-р заказчика
            };
            
            db.Orders.Add(o); // Добавляем заказ в сущность
                              // Проходим по строкам корзины и добавляем их в детали заказа

            foreach (var line in myCart.GetLines())
            {
                OrderDetail ordItems = new OrderDetail
                {                  
                    car_id = line.ProdID,
                    amount = line.Quantity,
                   // order_id = o.order_id
            };
                
                // Через навигационное свойство добавляем позицию в заказ
                o.OrderDetails.Add(ordItems);
            };
            db.SaveChanges();
            // Возвращаем новый вставленный заказ
            return o;
        }

        public Order CommitTrans(int custID, ShopBasket myCart, out string message)
        {
            message = "Транзакция прошла успешно";
            // Запускаем транзакцию
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (myCart.GetTotal() == 0)
                    {
                        throw new ApplicationException("Вы забили заполнить корзину!");
                    }
                    // Сохраняем изменения во всех таблицах
                    Order o = AddOrder(custID, myCart);
                    // Фиксируем транзакцию
                    trans.Commit();
                    return o;
                }
                catch (Exception ex)
                {
                    // Откатывем транзакцию
                    message = "Транзакция откатилась со следующей ошибкой: " +
                    ex.Message;
                    trans.Rollback();
                    return null;
                }
                finally
                {
                    // Чтобы контекст увидел результаты
                    // работы транзакции, надо его пересоздать заново !!!
                    // Либо запускать транзакцию внутри блока
                    // using (db = new SkladContext()) {... }
                    // При пересоздании контекст получает все новые данные из базы
                    db.Dispose();
                    db = new CarContext();
                }
            }
        }
    }
}