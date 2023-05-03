using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.DataAccess.RepositoryContracts;
using Store.Models;
using Store.Utility;
using Store.Web.Areas.Admin.Models;

namespace Store.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductVM model = new() { 
                Product = (id == null ? new() : await unitOfWork.Product.GetFirstOrDefault(r => r.Id == id))!, 
                CategoryList = (await unitOfWork.Category.GetAll()).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }) 
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(ProductVM model, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                string rootPath = webHostEnvironment.WebRootPath;
                if(imageFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    string filePath = Path.Combine(rootPath, @"images\products", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    if(!string.IsNullOrEmpty(model.Product.ImageURL))
                    {
                        string oldFilePath = Path.Combine(rootPath, model.Product.ImageURL.TrimStart('\\'));
                        if(System.IO.File.Exists(oldFilePath))
                            System.IO.File.Delete(oldFilePath);
                    }
                    model.Product.ImageURL = @"\images\products\" + fileName;
                }
                if(model.Product.Id == 0)
                    unitOfWork.Product.Add(model.Product);
                else
                    unitOfWork.Product.Update(model.Product);
                await unitOfWork.SaveAsync();
                TempData["success"] = $"Product {(model.Product.Id == 0 ? "Created" : "Updated")} successfully";
                return RedirectToAction(nameof(Index));
            }
            model.CategoryList = (await unitOfWork.Category.GetAll()).Select(c => new SelectListItem{Text = c.Name, Value = c.Id.ToString()});
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var product = await unitOfWork.Product.GetFirstOrDefault(r => r.Id == id);
            IEnumerable<SelectListItem> categoryList = (await unitOfWork.Category.GetAll()).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            ViewBag.CategoryList = categoryList;
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Product.Update(product);
                await unitOfWork.SaveAsync();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<SelectListItem> categoryList = (await unitOfWork.Category.GetAll()).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            ViewBag.CategoryList = categoryList;
            return View(product);
        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var product = await unitOfWork.Product.GetFirstOrDefault(r => r.Id == id);
        //    return View(product);
        //}
        //[HttpPost, ActionName("Delete")]
        //public async Task<IActionResult> DeletePOST(int? id)
        //{
        //    var product = await unitOfWork.Product.GetFirstOrDefault(r => r.Id == id);
        //    if (product == null)
        //        return NotFound();
        //    unitOfWork.Product.Remove(product);
        //    await unitOfWork.SaveAsync();
        //    TempData["success"] = "Product deleted successfully";
        //    return RedirectToAction(nameof(Index));
        //}

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await unitOfWork.Product.GetAll(includeProperties: "Category")});
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var productToBeDeleted = await unitOfWork.Product.GetFirstOrDefault(r => r.Id == id);
            if(productToBeDeleted == null)
                return Json(new {success= false, message = "Error while deleting" });
            if(productToBeDeleted.ImageURL != null)
            {
                var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, productToBeDeleted.ImageURL.TrimStart('\\'));
                if(System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }
            unitOfWork.Product.Remove(productToBeDeleted);
            await unitOfWork.SaveAsync();
            return Json(new {success= true, message="Delete Successful"});
        }
        #endregion
    }
}
