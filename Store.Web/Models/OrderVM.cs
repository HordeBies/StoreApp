using Store.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Web.Models
{
    public class OrderVM
    {
        public OrderHeader  OrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }

        public int? SelectedOrderDetailID { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }

    }
}
