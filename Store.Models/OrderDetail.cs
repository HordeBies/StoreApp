using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrderHeaderId { get; set; }

        [Required]
        public int ProductId { get; set; }
        [Required]
        public int CompanyId { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }
        #region Navigation Properties
        [ForeignKey(nameof(OrderHeaderId))]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        [ForeignKey(nameof(ProductId))]
        [ValidateNever]
        public Product Product { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public Company Company { get; set; }
        #endregion
    }
}
