using System.ComponentModel.DataAnnotations;

namespace Database.NET.Models
{
    // MODEL: one row of the Product table
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name can be at most 100 characters.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Category is required.")]
        [StringLength(100, ErrorMessage = "Category can be at most 100 characters.")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, 99999999.99, ErrorMessage = "Enter a valid price.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Enter a valid quantity.")]
        public int Quantity { get; set; }
    }
}
