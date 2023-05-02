using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        [Required, StringLength(100)]
        public string ISBN { get; set; }
        [Required, StringLength(100)]
        public string Author { get; set; }
        [Required]
        [Range(1,1000)]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 1000)]
        public double Price { get; set; }
        public int CategoryID { get; set; }
        [ForeignKey(nameof(CategoryID))]
        [ValidateNever]
        public Category Category { get; set; }
        [StringLength(256)]
        public string? ImageURL { get; set; }
    }
}
