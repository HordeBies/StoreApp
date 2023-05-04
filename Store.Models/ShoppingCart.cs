using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public class ShoppingCart
    {
        [Key]
        [BindNever]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [ValidateNever]
        public Product Product { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public Company Company { get; set; }
        [Range(1,1000, ErrorMessage ="Please enter a value between 1 and 1000")]
        public int Count { get; set; }
        [BindNever]
        public string ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey($"{nameof(CompanyId)},{nameof(ProductId)}")]
        [ValidateNever]
        public CompanyProduct CompanyProduct { get; set; }
        [NotMapped]
        public double Price{ get; set; }
    }
}
