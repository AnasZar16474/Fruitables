using Fruitables.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fruitables.PL.Areas.DashBoard.ViewModel.OrderVM
{
    public class OrderDisplayVM
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string mobile { get; set; }
    }
}
