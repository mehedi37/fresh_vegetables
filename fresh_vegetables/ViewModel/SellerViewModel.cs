using fresh_vegetables.Models;

namespace fresh_vegetables.ViewModel
{
    public class SellerViewModel
    {
        public List<Products> ItemsForSale { get; set; } = new List<Products>();
        public List<CustomerViewModel> Customers { get; set; } = new List<CustomerViewModel>();
    }

    public class CustomerViewModel
    {
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
    }
}
