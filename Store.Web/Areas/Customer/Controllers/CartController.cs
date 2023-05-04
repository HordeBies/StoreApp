using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.DataAccess.RepositoryContracts;
using Store.Web.Areas.Customer.Models;
using System.Security.Claims;

namespace Store.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var shoppingCartList = await unitOfWork.ShoppingCart.GetAll(r => r.ApplicationUserId == userId, includeProperties: "Product,Company");
            //var dict = shoppingCartList.ToDictionary(t => t, t => unitOfWork.CompanyProduct.GetFirstOrDefault(i => i.CompanyId == t.CompanyId && i.ProductId == t.ProductId).GetAwaiter().GetResult());
            //var groupedShoppingCartList = shoppingCartList.GroupBy(t => t.Company.Name).ToList();
            foreach (var item in shoppingCartList)
            {
                var companyProduct = await unitOfWork.CompanyProduct.GetFirstOrDefault(i => i.CompanyId == item.CompanyId && i.ProductId == item.ProductId);
                item.UnitPrice = companyProduct.Price;
                item.TotalPrice = item.UnitPrice * item.Count;
            }
            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = shoppingCartList,
                OrderTotal = shoppingCartList.Sum(r => r.TotalPrice)
            };
            return View(ShoppingCartVM);
        }

        public async Task<IActionResult> Summary()
        {
            return View();
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cartFromDb = await unitOfWork.ShoppingCart.GetFirstOrDefault(r => r.Id == cartId);
            unitOfWork.ShoppingCart.Remove(cartFromDb);
            await unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Plus(int cartId)
        {
            var cartFromDb = await unitOfWork.ShoppingCart.GetFirstOrDefault(r => r.Id == cartId);
            cartFromDb.Count += 1;
            unitOfWork.ShoppingCart.Update(cartFromDb);
            await unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Minus(int cartId)
        {
            var cartFromDb = await unitOfWork.ShoppingCart.GetFirstOrDefault(r => r.Id == cartId);
            cartFromDb.Count -= 1;
            if (cartFromDb.Count <= 0)
            {
                unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            await unitOfWork.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
