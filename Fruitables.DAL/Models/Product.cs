using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruitables.DAL.Models
{
    public enum ProductType
    {
        Fruit,
        Vegetable
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string ImagName { get; set; } = null!;
        public decimal Price { get; set; }
        public ProductType Type { get; set; } 
        public IEnumerable<CartItem> CartItem { get; set; }
        public IEnumerable<OrderItem> OrderItem { get; set; }
        public IEnumerable<Review> Reviews {  get; set; }
    }
}
