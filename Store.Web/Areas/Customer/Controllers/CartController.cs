using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Store.DataAccess.RepositoryContracts;
using Store.Models;
using Store.Utility;
using Store.Web.Areas.Customer.Models;
using Stripe.Checkout;
using System.Security.Claims;

namespace Store.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = Role.Customer)]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEmailSender emailSender;
        [BindProperty]
        public ShoppingCartVM viewModel { get; set; }
        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            this.unitOfWork = unitOfWork;
            this.emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var shoppingCartList = await unitOfWork.ShoppingCart.GetAll(r => r.ApplicationUserId == userId, includeProperties: "Product,Company,CompanyProduct");
            foreach (var item in shoppingCartList)
            {
                item.Price = item.CompanyProduct.Price;
            }
            viewModel = new ShoppingCartVM()
            {
                ShoppingCartList = shoppingCartList,
                OrderHeader = new()
                {
                    OrderTotal = shoppingCartList.Sum(r => r.Price * r.Count)
                }
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var shoppingCartList = await unitOfWork.ShoppingCart.GetAll(r => r.ApplicationUserId == userId, includeProperties: "Product,Company,CompanyProduct");
            foreach (var item in shoppingCartList)
            {
                item.Price = item.CompanyProduct.Price;
            }
            viewModel = new ShoppingCartVM()
            {
                ShoppingCartList = shoppingCartList,
                OrderHeader = new()
                {
                    OrderTotal = shoppingCartList.Sum(r => r.Price * r.Count)
                }
            };
            viewModel.OrderHeader.ApplicationUser = await unitOfWork.ApplicationUser.GetFirstOrDefault(r => r.Id == userId);

            viewModel.OrderHeader.FullName = viewModel.OrderHeader.ApplicationUser.Name;
            viewModel.OrderHeader.PhoneNumber = viewModel.OrderHeader.ApplicationUser.PhoneNumber;
            viewModel.OrderHeader.City = viewModel.OrderHeader.ApplicationUser.City;
            viewModel.OrderHeader.StreetAddress = viewModel.OrderHeader.ApplicationUser.StreetAddress;
            viewModel.OrderHeader.PostalCode = viewModel.OrderHeader.ApplicationUser.PostalCode;

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var shoppingCartList = await unitOfWork.ShoppingCart.GetAll(r => r.ApplicationUserId == userId, includeProperties: "CompanyProduct,Product");
            foreach (var item in shoppingCartList)
            {
                item.Price = item.CompanyProduct.Price;
            }
            viewModel.ShoppingCartList = shoppingCartList;

            viewModel.OrderHeader.OrderDate = DateTime.Now;
            viewModel.OrderHeader.ApplicationUserId = userId;

            //viewModel.OrderHeader.ApplicationUser = await unitOfWork.ApplicationUser.GetFirstOrDefault(r => r.Id == userId);

            viewModel.OrderHeader.OrderTotal = shoppingCartList.Sum(r => r.Price * r.Count);

            viewModel.OrderHeader.OrderStatus = OrderStatus.Pending;
            viewModel.OrderHeader.PaymentStatus = PaymentStatus.Pending;

            unitOfWork.OrderHeader.Add(viewModel.OrderHeader);
            await unitOfWork.SaveAsync();

            foreach (var cart in viewModel.ShoppingCartList)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderHeaderId = viewModel.OrderHeader.Id,
                    ProductId = cart.ProductId,
                    CompanyId = cart.CompanyId,
                    Count = cart.Count,
                    Price = cart.Price,
                };
                unitOfWork.OrderDetail.Add(orderDetail);
                await unitOfWork.SaveAsync();
            }

            // TODO: Handle dynamic url creation
            var domain = "https://localhost:7140";
            var options = new SessionCreateOptions
            {
                SuccessUrl = $"{domain}/customer/cart/OrderConfirmation?id={viewModel.OrderHeader.Id}",
                CancelUrl = $"{domain}/customer/cart/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in viewModel.ShoppingCartList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            unitOfWork.OrderHeader.UpdateStripePaymentId(viewModel.OrderHeader.Id, session.Id, session.PaymentIntentId);
            await unitOfWork.SaveAsync();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

            //return RedirectToAction(nameof(OrderConfirmation), new { id = viewModel.OrderHeader.Id });
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            OrderHeader orderHeader = await unitOfWork.OrderHeader.GetFirstOrDefault(r => r.Id == id, includeProperties:"ApplicationUser");
            if (orderHeader.PaymentStatus == PaymentStatus.Approved)
            {
                return View(id);
            }
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if(session.PaymentStatus.ToLower() == "paid")
            {
                unitOfWork.OrderHeader.UpdateStripePaymentId(orderHeader.Id,session.Id,session.PaymentIntentId);
                unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, OrderStatus.Approved, PaymentStatus.Approved);
                await unitOfWork.SaveAsync();

                var shoppingCarts = await unitOfWork.ShoppingCart.GetAll(r => r.ApplicationUserId == orderHeader.ApplicationUserId);
                unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
                HttpContext.Session.Remove(SessionSD.ShoppingCart);

                await unitOfWork.SaveAsync();

                await emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email,"New Order - Bies Book", $"<p>New Order Created - {orderHeader.Id}</p>");

                return View(id);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cartFromDb = await unitOfWork.ShoppingCart.GetFirstOrDefault(r => r.Id == cartId);
            unitOfWork.ShoppingCart.Remove(cartFromDb);
            await unitOfWork.SaveAsync();
            HttpContext.Session.SetInt32(SessionSD.ShoppingCart, (await unitOfWork.ShoppingCart.GetAll(r => r.ApplicationUserId == cartFromDb.ApplicationUserId)).Count());
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
            HttpContext.Session.SetInt32(SessionSD.ShoppingCart, (await unitOfWork.ShoppingCart.GetAll(r => r.ApplicationUserId == cartFromDb.ApplicationUserId)).Count());
            return RedirectToAction(nameof(Index));
        }
    }
}
