using fresh_vegetables.Areas.Identity.Data;
using fresh_vegetables.Data;
using fresh_vegetables.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace fresh_vegetables.Services
{
    public class CartService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CartService(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<CartProducts>> CartDetailsAsync(AppUser user)
        {
            var cart = await _context.Cart
                .Include(c => c.CartProducts)
                .ThenInclude(cd => cd.Products)
                .FirstOrDefaultAsync(c => c.UserId == user.Id && !c.IsPurchased);

            if (cart == null || cart.CartProducts == null)
            {
                return new List<CartProducts>();
            }

            return cart.CartProducts.ToList();
        }

        public async Task<Cart?> CartByUserIdAsync(string userId)
        {
            var cart = await _context.Cart
                .Include(c => c.CartProducts)
                .ThenInclude(cd => cd.Products)
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsPurchased);

            if (cart == null || cart.CartProducts == null)
            {
                return null;
            }
            return cart;
        }

        public async Task AddToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await CartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    CartId = new int(),
                    UserId = userId,
                    IsPurchased = false
                };
                _context.Cart.Add(cart);
                await _context.SaveChangesAsync();
            }

            var CartProducts = await _context.CartProducts
                .FirstOrDefaultAsync(cd => cd.CartId == cart.CartId && cd.ProductId == productId);

            if (CartProducts == null)
            {
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                CartProducts = new CartProducts
                {
                    CartProductsId = new int(),
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.ProductPrice
                };
                _context.CartProducts.Add(CartProducts);
            }
            else
            {
                CartProducts.Quantity += quantity;
                _context.CartProducts.Update(CartProducts);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(int cartDetailsId, int quantity)
        {
            var CartProducts = await _context.CartProducts.FindAsync(cartDetailsId);
            if (CartProducts != null)
            {
                CartProducts.Quantity = quantity;
                _context.CartProducts.Update(CartProducts);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFromCartAsync(int cartDetailsId)
        {
            var CartProducts = await _context.CartProducts.FindAsync(cartDetailsId);
            if (CartProducts != null)
            {
                _context.CartProducts.Remove(CartProducts);
                await _context.SaveChangesAsync();

                var cart = await _context.Cart
                    .Include(c => c.CartProducts)
                    .FirstOrDefaultAsync(c => c.CartId == CartProducts.CartId);

                if (cart != null && !cart.CartProducts.Any())
                {
                    _context.Cart.Remove(cart);
                    await _context.SaveChangesAsync();
                }
            }
        }
        public async Task<int> CartDetailsCountAsync(string userId)
        {
            var cart = await CartByUserIdAsync(userId);
            return cart?.CartProducts.Count ?? 0;
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await CartByUserIdAsync(userId);
            if (cart != null)
            {
                _context.CartProducts.RemoveRange(cart.CartProducts);
                _context.Cart.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }
        public async Task PurchaseCartAsync(string userId)
        {
            var cart = await CartByUserIdAsync(userId);
            if (cart != null)
            {
                cart.IsPurchased = true;
                _context.Cart.Update(cart);
                await _context.SaveChangesAsync();
            }
        }

    }
}
