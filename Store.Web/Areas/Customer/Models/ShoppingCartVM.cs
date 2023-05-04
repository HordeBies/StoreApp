using Store.Models;

namespace Store.Web.Areas.Customer.Models
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        //public IDictionary<ShoppingCart,CompanyProduct?> CompanyProductDict { get; set; }
        public double OrderTotal { get; set; }
    }
}
