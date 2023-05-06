using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.DataAccess.RepositoryContracts;
using Store.Models;
using Store.Utility;

namespace Store.Web.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = Role.Company)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<IdentityUser> userManager;
        public ProductController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            var user = await userManager.GetUserAsync(User) as ApplicationUser;
            if (user == null)
                return NotFound();

            CompanyProduct model = await unitOfWork.CompanyProduct.GetFirstOrDefault(cp => cp.ProductId == id && cp.CompanyId == user.CompanyId,includeProperties:"Product") ?? new();
            model.CompanyId = user.CompanyId ?? 0;
            
            ViewBag.isNew = model.ProductId == 0;
            
            if(id != null)
            {
                model.Product = await unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
                model.ProductId = id ?? 0;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(CompanyProduct model)
        {
            if ((await unitOfWork.Product.GetFirstOrDefault(r => r.Id == model.ProductId) == null))
                ModelState.AddModelError("ProductId", "Not a valid product");
            if (ModelState.IsValid)
            {
                var cp = await unitOfWork.CompanyProduct.GetFirstOrDefault(r => r.CompanyId == model.CompanyId && r.ProductId == model.ProductId);
                bool isNew = cp == null;
                if (isNew)
                    unitOfWork.CompanyProduct.Add(model);
                else
                    unitOfWork.CompanyProduct.Update(model);
                await unitOfWork.SaveAsync();
                TempData["success"] = $"Product {(isNew ? "Created" : "Updated")} successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAllCompanyProducts()
        {
            var user = await userManager.GetUserAsync(User) as ApplicationUser;
            if (user == null || user.CompanyId == null)
                return Json(new { success = false, message = "Error while authenticating company user" });
            return Json(new { data = (await unitOfWork.CompanyProduct.GetAll(includeProperties: "Product")).Where(cp => cp.CompanyId == user.CompanyId)});
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var user = await userManager.GetUserAsync(User) as ApplicationUser;
            if (user == null || user.CompanyId == null)
                return Json(new { success = false, message = "Error while authenticating company user" });

            var products = await unitOfWork.Product.GetAll(includeProperties:"Category");
            var companyProducts = (await unitOfWork.CompanyProduct.GetAll()).Where(cp => cp.CompanyId == user.CompanyId);
            var exclusiveProducts = products.Where(r => !companyProducts.Any(t => t.ProductId == r.Id));
            return Json(new { data = exclusiveProducts });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await userManager.GetUserAsync(User) as ApplicationUser;
            if(user == null)
                return Json(new {success= false, message = "Error while authenticating company user" });

            var companyProductToBeDeleted = await unitOfWork.CompanyProduct.GetFirstOrDefault(r => r.ProductId == id && r.CompanyId == user.CompanyId);
            if(companyProductToBeDeleted == null)
                return Json(new {success= false, message = "Error while deleting" });

            unitOfWork.CompanyProduct.Remove(companyProductToBeDeleted);
            await unitOfWork.SaveAsync();
            return Json(new {success= true, message="Delete Successful"});
        }
        #endregion
    }
}