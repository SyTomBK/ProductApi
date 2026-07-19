using Microsoft.AspNetCore.Mvc;
using Product.Api.Contracts.Products;
using Product.Api.Data;
using ProductEntity = Product.Api.Entities.Product;

namespace Product.Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = new ProductEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            CreatedDate = DateTime.UtcNow
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync(cancellationToken);

        return Ok (new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CreatedDate = product.CreatedDate
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FindAsync([id], cancellationToken);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            CreatedDate = product.CreatedDate
        });
    }
}
