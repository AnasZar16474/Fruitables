using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruitables.DAL.Models
{
    public class Review
    {
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }   
        public int Rating { get; set; } 
        public string? Text { get; set; } 
        public ApplicationUser ApplicationUser {  get; set; }
        public Product Product {  get; set; }
    }
}
