using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        public const string PartiallyShipped= "Partially Shipped";
        public const string Shipped= "Shipped";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
        public static readonly string[] Collection = 
        {
            Pending,
            Approved,
            InProcess,
            PartiallyShipped,
            Shipped,
            Completed,
            Cancelled,
        };
        public static bool CanBeCancelled(this string status) =>  status == Pending || status == Approved || status == InProcess;
    }
}
