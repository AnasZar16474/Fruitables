using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fruitables.DAL.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string FirstName {  get; set; }
        public string? LastName { get; set; }
        public string? CompanyName {  get; set; }
        public string Address {  get; set; }
        public string City { get; set; }
        public string Country {  get; set; }
        public string? Postcode {  get; set; }   
        public string mobile {  get; set; }
        [DataType(DataType.EmailAddress)]
        public string EmailAddress {  get; set; }
        public string? Notes {  get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public IList<OrderItem> OrderItem { get; set; }
    }
}
