using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IGenericRepository<Product> productRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts([FromQuery]ProductSpecParams specParams)
    {
        var spec = new ProductSpecification(specParams);
        return await CreatePagedResult(productRepository, spec, specParams.PageIndex, specParams.PageSize);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        productRepository.Add(product);
        if (await productRepository.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        return BadRequest("Problem creating the product");   
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !ProductExists(id))
        {
            return BadRequest();
        }
        productRepository.Update(product);
        if (await productRepository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem updating the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        productRepository.Remove(product);
        if (await productRepository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem to delete the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();
        return Ok(await productRepository.ListAsync(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
       var spec = new TypeListSpecification();
        return Ok(await productRepository.ListAsync(spec));
    }

    private bool ProductExists(int id)
    {
        return productRepository.Exists(id);
    }
}
