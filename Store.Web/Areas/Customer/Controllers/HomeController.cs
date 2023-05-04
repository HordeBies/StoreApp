using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.DataAccess.RepositoryContracts;
using Store.Models;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Security.Claims;

namespace Store.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var productList = await unitOfWork.Product.GetAll(includeProperties: "CompanyProducts");
            return View(productList);
        }
        public async Task<IActionResult> Details(int? id, int? companyId)
        {
            var product = await unitOfWork.Product.GetFirstOrDefault(r => r.Id == id, includeProperties: "Category,CompanyProducts,Companies");
            if (product == null)
                return RedirectToAction("Index");

            var selectedCompany = product.CompanyProducts.FirstOrDefault(r => r.CompanyId == companyId) ?? product.CompanyProducts.MinBy(r => r.Price);

            ShoppingCart model = new ShoppingCart
            {
                ProductId = product.Id,
                Product = product,
                CompanyId = selectedCompany?.CompanyId ?? 0,
                Company = selectedCompany?.Company,
                Count = 1
            };

            ViewBag.SelectedCompanyProduct = selectedCompany;
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            var cartFromDb = await unitOfWork.ShoppingCart.GetFirstOrDefault(r => r.ApplicationUserId == shoppingCart.ApplicationUserId && r.ProductId == shoppingCart.ProductId && r.CompanyId == shoppingCart.CompanyId);
            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
                unitOfWork.ShoppingCart.Add(shoppingCart);
            }
            await unitOfWork.SaveAsync();
            TempData["success"] = "Cart updated successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}