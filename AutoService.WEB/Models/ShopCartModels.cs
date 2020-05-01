using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoService.WEB.Models
{
    public class Cart
    {
        private List<Service> shopCart = new List<Service>();

        public void AddItem(Service service)
        {
            var item = shopCart.FirstOrDefault(s => s.ServiceId == service.ServiceId);

            if (item == null)
            {
                shopCart.Add(service);
            }
            else
            {
                throw new Exception();
            }
        }

        public void RemoveFromCart(int? serviceId)
        {
            if (serviceId == null)
            {
                throw new ArgumentNullException();
            }
            var item = shopCart.FirstOrDefault(s => s.ServiceId == serviceId);
            if (item != null)
            {
                shopCart.Remove(item);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public int ComputeTotalValue()
        {
            int result = 0;
            foreach (var service in shopCart)
            {
                if (service.Discount != null)
                {
                    result += service.PriceWithDiscount;
                }
                else
                {
                    result += service.Price;
                }
            }
            return result;
        }

        public void Clear()
        {
            shopCart.Clear();
        }

        public IEnumerable<Service> Lines
        {
            get { return shopCart; }
        }
    }

    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
    }
}