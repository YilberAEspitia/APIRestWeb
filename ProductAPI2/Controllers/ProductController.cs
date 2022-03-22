using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductAPI2.Models;
using ProductAPI2.Models.DTO;
using ProductAPI2.Repository.Repository;
using System.Collections.Generic;

namespace ProductAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllProduct()
        {
            var product = productRepository.GetAllProducts();
            return Ok(mapper.Map<List<ProductDTO>>(product));
        }

        [HttpGet("{id}", Name ="GetProduct")]
        public IActionResult GetProduct(int id)
        {
            var product = productRepository.GetProduct(id);
            if(product == null) return NotFound();
            return Ok(mapper.Map<ProductDTO>(product));
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDTO productDTO)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);
            if(productRepository.ProductExists(productDTO.Name))
            {
                ModelState.AddModelError(string.Empty, $"Ya existe un producto con el nombre {productDTO.Name}");
                return StatusCode(404, ModelState);
            }

            var product = mapper.Map<Product>(productDTO);
            if (!productRepository.CreateProduct(product))
            {
                ModelState.AddModelError(string.Empty, $"Ha ocurrido un error al intentar guardar el producto {productDTO.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetProduct", new {id = product.Id}, product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] ProductDTO productDTO)
        {
            if(id != productDTO.Id) return BadRequest(ModelState);
            var product = mapper.Map<Product>(productDTO);
            if (!productRepository.UpdateProduct(product))
            {
                ModelState.AddModelError(string.Empty, $"Ha ocurrido un error al intentar actualizar el producto {productDTO.Name}");
                return StatusCode(500,ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (!productRepository.ProductExists(id)) return NotFound();

            var product = productRepository.GetProduct(id);

            if(!productRepository.DeleteProduct(product))
            {
                ModelState.AddModelError(string.Empty, $"Ha ocurrido un error al intentar eliminar el producto {product.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
