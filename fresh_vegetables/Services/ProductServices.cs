using fresh_vegetables.Data;
using fresh_vegetables.Models;
using Microsoft.EntityFrameworkCore;

namespace fresh_vegetables.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Products>> ItemsForSaleByUserIdAsync(string userId)
        {
            return await _context.Products.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<Products?> ItemByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddItemAsync(Products product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(Products product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
