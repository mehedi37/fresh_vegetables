using fresh_vegetables.Areas.Identity.Data;
using fresh_vegetables.Data;
using fresh_vegetables.Models;
using fresh_vegetables.Services;
using fresh_vegetables.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace fresh_vegetables.Controllers
{
    [Authorize]
    public class SellController : Controller
    {
        private readonly ILogger<SellController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;

        public SellController(ILogger<SellController> logger, AppDbContext context, UserManager<AppUser> userManager, ProductService productService, CustomerService customerService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _productService = productService;
            _customerService = customerService;
        }

        public async Task<IActionResult> SellVegetables()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(); // or handle the null case appropriately
            }

            var itemsForSale = await _productService.ItemsForSaleByUserIdAsync(userId);
            var customers = await _customerService.CustomersBySellerIdAsync(userId);

            var model = new SellerViewModel
            {
                ItemsForSale = itemsForSale,
                Customers = customers
            };

            return View(model);
        }

        public IActionResult AddItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(new Products
            {
                ProductId = new int(),
                ProductName = string.Empty,
                ProductDescription = string.Empty,
                ProductImage = string.Empty,
                ProductPrice = 0.0M,
                UserId = userId,
                Stock = 0
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddItems(Products product)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                product.UserId = userId ?? throw new InvalidOperationException("User ID cannot be null");
                await _productService.AddItemAsync(product);
                return RedirectToAction("SellVegetables");
            }
            return View(product);
        }

        public async Task<IActionResult> EditItems(int id)
        {
            var product = await _productService.ItemByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditItems(Products product)
        {
            if (ModelState.IsValid)
            {
                await _productService.UpdateItemAsync(product);
                return RedirectToAction("SellVegetables");
            }
            return View(product);
        }

        public async Task<IActionResult> DeleteItem(int id)
        {
            var product = await _productService.ItemByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (product.UserId != userId)
            {
                return Unauthorized();
            }

            await _productService.DeleteItemAsync(id);
            return RedirectToAction("SellVegetables");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
