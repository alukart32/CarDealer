using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealer.Models.Purchase
{
    public class ShopBasket
    {
        // Конструктор
        public ShopBasket() { }
        // Содержание корзины - коллекция строк
        private List<ShopBasketPos> lineCollection = new List<ShopBasketPos>();
        // Добавление элемента в корзину
        public void AddItem(int prodID, string prodName, decimal price, int quantity)
        {
            // Ищем в корзине товар с данным идентификатором
            ShopBasketPos line = lineCollection.Where
            (p => p.ProdID == prodID).FirstOrDefault();
            // Если его нет, то добавляем в корзину
            if (line == null)
            {
                lineCollection.Add(new ShopBasketPos
                {
                    ProdID = prodID,
                    ProdName = prodName,
                    Price = price,
                    Quantity = quantity
                });
            }
            else
            // иначе увеличиваем количество
            {
                line.Quantity += quantity;
            }
        }
        // Удаление из корзины
        public void RemoveLine(int prodID)
        {
            lineCollection.RemoveAll(l => l.ProdID == prodID);
        }
        //Сумма к оплате
        public decimal GetTotal()
        {
            return lineCollection.Sum(e => e.Price * e.Quantity);
        }
        // Очистка корзины
        public void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<ShopBasketPos> Lines
        {
            get { return lineCollection; }
        }
        // Чтение содержимого корзины
        public List<ShopBasketPos> GetLines()
        {
            return lineCollection;
        }
        // Инициализация корзины из сеансовой переменной s
        public static ShopBasket GetCart(object s)
        {
            ShopBasket myCart = (ShopBasket)s;
            if (myCart == null)
            {
                myCart = new ShopBasket();
                s = myCart;
            }
            return myCart;
        }
    }
}