using fresh_vegetables.Models;

namespace fresh_vegetables.ViewModel
{
    public class ProductDetailsViewModel
    {
        public required Products Product { get; set; }
        public required List<Products> OtherProducts { get; set; }
    }
}
