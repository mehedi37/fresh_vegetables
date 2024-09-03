using fresh_vegetables.Models;

namespace fresh_vegetables.ViewModel
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public List<CartProducts> CartProducts { get; set; } = new List<CartProducts>();
        public decimal TotalPrice { get; set; }
    }
}
