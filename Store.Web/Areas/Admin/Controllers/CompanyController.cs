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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            Company model = await unitOfWork.Company.GetFirstOrDefault(r => r.Id == id) ?? new Company();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(Company model)
        {
            if (ModelState.IsValid)
            {
                if(model.Id == 0)
                    unitOfWork.Company.Add(model);
                else
                    unitOfWork.Company.Update(model);
                await unitOfWork.SaveAsync();
                TempData["success"] = $"Company {(model.Id == 0 ? "Created" : "Updated")} successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var model = await unitOfWork.Company.GetFirstOrDefault(r => r.Id == id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Company model)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Company.Update(model);
                await unitOfWork.SaveAsync();
                TempData["success"] = "Company updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var product = await unitOfWork.Company.GetFirstOrDefault(r => r.Id == id);
        //    return View(product);
        //}
        //[HttpPost, ActionName("Delete")]
        //public async Task<IActionResult> DeletePOST(int? id)
        //{
        //    var product = await unitOfWork.Company.GetFirstOrDefault(r => r.Id == id);
        //    if (product == null)
        //        return NotFound();
        //    unitOfWork.Company.Remove(product);
        //    await unitOfWork.SaveAsync();
        //    TempData["success"] = "Company deleted successfully";
        //    return RedirectToAction(nameof(Index));
        //}

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await unitOfWork.Company.GetAll()});
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var companyToBeDeleted = await unitOfWork.Company.GetFirstOrDefault(r => r.Id == id);
            if(companyToBeDeleted == null)
                return Json(new {success= false, message = "Error while deleting" });
            
            unitOfWork.Company.Remove(companyToBeDeleted);
            await unitOfWork.SaveAsync();
            return Json(new {success= true, message="Delete Successful"});
        }
        #endregion
    }
}
