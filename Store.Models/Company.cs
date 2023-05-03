using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Store.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }

        [ValidateNever]
        public List<Product> Products{ get; set; }
        [ValidateNever]
        public List<CompanyProduct> CompanyProducts { get; set; }
    }
}
