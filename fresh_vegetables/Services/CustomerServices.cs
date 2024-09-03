using fresh_vegetables.Data;
using fresh_vegetables.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace fresh_vegetables.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerViewModel>> CustomersBySellerIdAsync(string sellerId)
        {
            // Fetch the necessary data without performing the aggregation
            var cartData = await _context.Cart
                .Where(c => c.IsPurchased && c.CartProducts.Any(cd => cd.Products.UserId == sellerId))
                .Include(c => c.CartProducts)
                .ThenInclude(cd => cd.Products)
                .Include(c => c.AppUser)
                .ToListAsync();

            // Perform the aggregation in memory
            var customers = cartData
                .GroupBy(c => c.UserId)
                .Select(g => new CustomerViewModel
                {
                    CustomerName = g.First().AppUser.Name,
                    TotalSpent = g.Sum(c => c.CartProducts.Sum(cd => cd.Price * cd.Quantity))
                })
                .ToList();

            return customers;
        }
    }
}
