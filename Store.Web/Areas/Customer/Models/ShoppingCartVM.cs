using Store.Models;

namespace Store.Web.Areas.Customer.Models
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        //public IDictionary<ShoppingCart,CompanyProduct?> CompanyProductDict { get; set; }
    }
}
