using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Utility
{
    public static class OrderStatus
    {
        public const string Pending = "Pending";
        public const string Approved = "Approved";
        public const string InProcess = "Processing";
        public const string Shipped= "Shipped";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
        public const string Refunded = "Refunded";

    }
}
