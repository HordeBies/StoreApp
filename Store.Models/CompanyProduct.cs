using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models
{
    public class CompanyProduct
    {

        public int CompanyId { get; set; }
        public int ProductId{ get; set; }
        [Required]
        [Range(0, 1000000)]
        public double Price { get; set; }
        [Required]
        [Range(0, 1000000)]
        public double ListPrice { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        [ForeignKey(nameof (ProductId))]
        public Product Product { get; set; }
    }
}
