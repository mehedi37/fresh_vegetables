using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fresh_vegetables.Models
{
    public class CartProducts
    {
        [Key]
        public required int CartProductsId { get; set; }
        [ForeignKey("Products")]
        public required int ProductId { get; set; }
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Products? Products { get; set; }

        [Precision(10, 2)]
        public required decimal Price { get; set; }

        [ForeignKey("Cart")]
        public required int CartId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Cart? Cart { get; set; }

        public required int Quantity { get; set; }
    }
}
