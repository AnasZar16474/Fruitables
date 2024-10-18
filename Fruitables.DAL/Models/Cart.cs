using Fruitables.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruitables.DAL.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationDbContext Context { get; set; }
        public IList<CartItem> CartItem { get; set; }




    }
}
