using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Store.DataAccess.Repositories;
using Store.DataAccess.RepositoryContracts;
using Store.Models;
using Store.Utility;
using Store.Web.Models;
using Stripe;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Store.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<IdentityUser> userManager;
        public OrderController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int id, int orderDetailId) // order id
        {
            var model = new OrderVM()
            {
                OrderHeader = await unitOfWork.OrderHeader.GetFirstOrDefault(r => r.Id == id, includeProperties: "ApplicationUser"),
                SelectedOrderDetailID = orderDetailId,
            };
            var allOrderDetails = await unitOfWork.OrderDetail.GetAll(r => r.OrderHeaderId == id, includeProperties: "Product,Company");
            if (User.IsInRole(Role.Company))
            {
                var user = await userManager.GetUserAsync(User) as ApplicationUser;
                model.OrderDetails = allOrderDetails.Where(r => r.CompanyId == user.CompanyId);
            }
            else
                model.OrderDetails = allOrderDetails;

            var selectedOrder = model.OrderDetails.FirstOrDefault(r => r.Id == orderDetailId);
            if (selectedOrder != null)
            {
                model.ShippingDate = selectedOrder.ShippingDate;
                model.TrackingNumber = selectedOrder.TrackingNumber;
                model.Carrier = selectedOrder.Carrier;
            }

            if (model == null)
                return NotFound();
            return View("Order/Details", model);
        }
        [HttpPost]
        [Authorize(Roles = Role.Admin + "," + Role.Company)]
        public async Task<IActionResult> UpdateOrderDetails(OrderVM order)
        {
            if (User.IsInRole(Role.Admin))
            {
                // Update OrderHeader
                var orderHeader = await unitOfWork.OrderHeader.GetFirstOrDefault(r => r.Id == order.OrderHeader.Id);
                orderHeader.FullName = order.OrderHeader.FullName;
                orderHeader.PhoneNumber = order.OrderHeader.PhoneNumber;
                orderHeader.StreetAddress = order.OrderHeader.StreetAddress;
                orderHeader.City = order.OrderHeader.City;
                orderHeader.PostalCode = order.OrderHeader.PostalCode;
                unitOfWork.OrderHeader.Update(orderHeader);
            }

            // Update Tracking Info
            var orderDetails = await unitOfWork.OrderDetail.GetAll(r => r.OrderHeaderId == order.OrderHeader.Id);
            int? companyId = null;
            if (User.IsInRole(Role.Company))
            {
                var user = await userManager.GetUserAsync(User) as ApplicationUser;
                companyId = user.CompanyId;
            }
            else
            {
                var selectedOrder = orderDetails.FirstOrDefault(r => r.Id == order.SelectedOrderDetailID);
                companyId = selectedOrder?.CompanyId;
            }
            if (companyId != null)
            {
                foreach (var orderDetail in orderDetails.Where(r => r.CompanyId == companyId))
                {
                    orderDetail.Carrier = order.Carrier;
                    orderDetail.TrackingNumber = order.TrackingNumber;
                    unitOfWork.OrderDetail.Update(orderDetail);
                }
            }
            await unitOfWork.SaveAsync();

            TempData["success"] = "Order Details Updated Successfully";
            return RedirectToAction(nameof(Details), new { id = order.OrderHeader.Id, orderDetailId = order.SelectedOrderDetailID });
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> StartProcessing(OrderVM order)
        {
            order.OrderHeader.OrderStatus = OrderStatus.InProcess;
            unitOfWork.OrderHeader.UpdateStatus(order.OrderHeader.Id, OrderStatus.InProcess);
            await unitOfWork.SaveAsync();
            TempData["success"] = "Order Status Updated Successfully";
            return RedirectToAction(nameof(Details), new { id = order.OrderHeader.Id, orderDetailId = order.SelectedOrderDetailID });
        }

        [HttpPost]
        [Authorize(Roles = Role.Company)]
        public async Task<IActionResult> ShipPartialOrder(OrderVM order)
        {
            var user = await userManager.GetUserAsync(User) as ApplicationUser;
            var companyId = user.CompanyId;
            var orderDetails = await unitOfWork.OrderDetail.GetAll(r => r.OrderHeaderId == order.OrderHeader.Id);
            foreach (var orderDetail in orderDetails.Where(r => r.CompanyId == companyId))
            {
                orderDetail.ShippingDate = DateTime.Now;
                orderDetail.Carrier = order.Carrier;
                orderDetail.TrackingNumber = order.TrackingNumber;
                unitOfWork.OrderDetail.Update(orderDetail);
            }
            unitOfWork.OrderHeader.UpdateStatus(order.OrderHeader.Id, orderDetails.Any(r => r.ShippingDate == null) ? OrderStatus.PartiallyShipped : OrderStatus.Shipped);
            await unitOfWork.SaveAsync();

            TempData["success"] = "Order Shipment Placed Successfully";
            return RedirectToAction(nameof(Details), new { id = order.OrderHeader.Id, orderDetailId = order.SelectedOrderDetailID });
        }

        [HttpPost]
        [Authorize(Roles = Role.Customer)]
        public async Task<IActionResult> CompleteOrder(OrderVM order)
        {
            unitOfWork.OrderHeader.UpdateStatus(order.OrderHeader.Id, OrderStatus.Completed);
            await unitOfWork.SaveAsync();

            TempData["success"] = "Order Completed Successfully";
            return RedirectToAction(nameof(Details), new { id = order.OrderHeader.Id, orderDetailId = order.SelectedOrderDetailID });
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> CancelOrder(OrderVM order)
        {
            var orderHeader = await unitOfWork.OrderHeader.GetFirstOrDefault(r => r.Id == order.OrderHeader.Id, includeProperties: "ApplicationUser");
            if(!orderHeader.OrderStatus.CanBeCancelled())
            {
                TempData["error"] = "Order can not be cancelled";
                return RedirectToAction(nameof(Details), new { id = order.OrderHeader.Id, orderDetailId = order.SelectedOrderDetailID });
            }
            if(orderHeader.PaymentStatus == PaymentStatus.Approved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };
                var service = new RefundService();
                Refund refund = service.Create(options);
                unitOfWork.OrderHeader.UpdateStatus(order.OrderHeader.Id, OrderStatus.Cancelled,PaymentStatus.Refunded);

            }
            else
            {
                unitOfWork.OrderHeader.UpdateStatus(order.OrderHeader.Id, OrderStatus.Cancelled, PaymentStatus.Cancelled);
            }
            await unitOfWork.SaveAsync();
            TempData["success"] = "Order Cancelled Successfully";
            return RedirectToAction(nameof(Details), new { id = order.OrderHeader.Id, orderDetailId = order.SelectedOrderDetailID });
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll(string? status)
        {
            IEnumerable<OrderHeader> data;

            // Default role filter is empty
            Func<OrderHeader, bool> roleFilter = r => false;

            if (string.IsNullOrEmpty(status) || status == "All" || status == "null")
                data = await unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser,OrderDetails");
            else
                data = await unitOfWork.OrderHeader.GetAll(r => r.OrderStatus == status, includeProperties: "ApplicationUser,OrderDetails");


            if (User.IsInRole(Role.Admin))
            {
                // Accept all
                roleFilter = r => true;
            }
            else if (User.IsInRole(Role.Company))
            {
                // Accept only orders that have at least one order detail with the same company id
                var user = await userManager.GetUserAsync(User) as ApplicationUser;
                roleFilter = r => r.OrderDetails.Any(t => t.CompanyId == user.CompanyId);
            }
            else if (User.IsInRole(Role.Customer))
            {
                // Accept only orders that belong to the current user
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                roleFilter = r => r.ApplicationUserId == userId;
            }

            data = data.Where(roleFilter);
            return Json(new { data });
        }

        #endregion
    }
}
