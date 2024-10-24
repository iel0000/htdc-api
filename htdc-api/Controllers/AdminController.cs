using AutoMapper;
using htdc_api.Authentication;
using htdc_api.Models;
using htdc_api.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace htdc_api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AdminController: BaseController
{
    public AdminController(
        ApplicationDbContext context, 
        IMapper mapper, 
        UserManager<IdentityUser> userManager) :
        base(context, mapper, userManager)
    {
    }

    [HttpGet]
    [Route("GetServices")]
    public async Task<IActionResult> GetServices()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }
    
    [HttpGet]
    [Route("GetServicesById/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetServicesById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        return Ok(product);
    }

    [HttpPost]
    [Route("CreateService")]
    public async Task<IActionResult> CreateService(CreateProductsViewModel model)
    {
        var product = _mapper.Map<Products>(model);
        var products = await _context.Products.Where(x => x.Name.ToLower() == product.Name.ToLower()).ToListAsync();
        if (products.Any())
        {
            return BadRequest("Product already exists");
        }
        
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return Ok("Product created");
    }
}