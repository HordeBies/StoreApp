using Microsoft.AspNetCore.Mvc;
using Store.DataAccess.RepositoryContracts;
using Store.Utility;
using System.Security.Claims;

namespace Store.Web.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private IUnitOfWork unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim == null)
            {
                HttpContext.Session.Clear();
                return View(0);
            }
            else
            {
                if(HttpContext.Session.GetInt32(SessionSD.ShoppingCart) == null)
                    HttpContext.Session.SetInt32(SessionSD.ShoppingCart, (await unitOfWork.ShoppingCart.GetAll(r => r.ApplicationUserId == claim.Value)).Count());
                
                return View(HttpContext.Session.GetInt32(SessionSD.ShoppingCart));
            }
        }
    }
}
