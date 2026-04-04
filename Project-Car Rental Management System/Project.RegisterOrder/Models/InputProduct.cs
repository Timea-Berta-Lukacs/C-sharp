using System.ComponentModel.DataAnnotations;

namespace Project.RegisterOrder.Models
{
    public class InputProduct
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int Quantity { get; set; }
    }
}
