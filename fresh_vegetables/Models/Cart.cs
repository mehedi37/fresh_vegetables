using fresh_vegetables.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fresh_vegetables.Models
{
    public class Cart
    {
        [Key]
        public required int CartId { get; set; }
        [ForeignKey("AppUser")]
        public required string UserId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public AppUser? AppUser { get; set; }
        public bool IsPurchased { get; set; } = false;
        public ICollection<CartProducts> CartProducts { get; set; } = new List<CartProducts>();

    }
}
