using Microsoft.EntityFrameworkCore;
using Store.DataAccess.Data;
using Store.DataAccess.RepositoryContracts;
using Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.DataAccess.Repositories
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
        }

        public void Update(OrderHeader obj)
        {
            db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderHeader = db.OrderHeaders.FirstOrDefault(r => r.Id == id);
            if (orderHeader != null)
            {
                orderHeader.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    orderHeader.PaymentStatus = paymentStatus;
                }
            }
            else
            {
                // Log here, this should not thrown
                throw new NullReferenceException("Order Header is null");
            }
        }
        public void UpdateStripePaymentId(int id, string sessionId, string paymentIntentId)
        {
            var orderHeader = db.OrderHeaders.FirstOrDefault(r => r.Id == id);
            if (orderHeader != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    orderHeader.SessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    orderHeader.PaymentIntentId = paymentIntentId;
                    orderHeader.PaymentDate = DateTime.Now;
                }
            }
            else
            {
                // Log here, this should not thrown
                throw new NullReferenceException("Order Header is null");
            }
        }
    }
}
