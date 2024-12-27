using System.Security.Claims;
using management.Data;
using management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace management.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetMyModels()
        {
            return await _context.Products.ToListAsync();
        }
      
        [Authorize]
        [HttpGet("current-user-id")]
        public ActionResult<string> GetCurrentUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }
            return Ok(userId);
        }

        // Get all products for the current user
        [Authorize]
        [HttpGet("current-user-products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetCurrentUserProducts()
        {
            // Get the current user's email from the claims
            //var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var userEmail = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userEmail == null)
            {
                return Unauthorized("User not authenticated.");
            }

            // Get the products associated with this email (assuming the 'User' entity has an 'Email' field)
            var userProducts = await _context.Products
                .Where(p => p.User.Email == userEmail)
                .ToListAsync();

            if (!userProducts.Any())
            {
                return NotFound("No products found for this user.");
            }

            return Ok(userProducts);
        }
    }

}
