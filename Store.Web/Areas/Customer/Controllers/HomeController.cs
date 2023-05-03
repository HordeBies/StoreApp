using Microsoft.AspNetCore.Mvc;
using Store.DataAccess.RepositoryContracts;
using Store.Models;
using System.Diagnostics;

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
            var product = await unitOfWork.Product.GetFirstOrDefault(r => r.Id == id,includeProperties: "Category,CompanyProducts,Companies");
            if (product == null)
                return RedirectToAction("Index");
            
            ViewBag.SelectedCompanyProduct = product.CompanyProducts.FirstOrDefault(r => r.CompanyId == companyId);
            if(ViewBag.SelectedCompanyProduct == null)
                product.CompanyProducts.MinBy(r => r.Price);

            return View(product);
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