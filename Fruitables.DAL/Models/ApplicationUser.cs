using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruitables.DAL.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string? Gender { get; set; }
        [Required]
        public string Address { get; set; } = null!;
        public Cart cart { get; set; }
        public IEnumerable<Order> Order { get; set; }
        public IEnumerable<Review> Reviews {  get; set; }
    }
}
