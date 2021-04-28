using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers
{
   
    public class ProductoController : BaseApiController
    {

        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IMapper _mapper;
        public ProductoController(IGenericRepository<Producto> productoRepository, IMapper mapper) {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        //http://localhost:38109/api/Producto
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductoDto>>> GetProductos([FromQuery]ProductoSpecificationParams productoParams) {
           
            var spec = new ProductoWithCategoriaAndMarcaSpecification(productoParams);
            var productos = await _productoRepository.GetAllWithSpec(spec);

            var specCount = new ProductoForCountingSpecification(productoParams);
            var totalProductos = await _productoRepository.CountAsync(specCount);

            var rounded = Math.Ceiling(  Convert.ToDecimal( totalProductos / productoParams.PageSize ));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoDto>>(productos);

            return Ok(
                new Pagination<ProductoDto> { 
                    Count = totalProductos,
                    Data = data,
                    PageCount = totalPages,
                    PageIndex = productoParams.PageIndex,
                    PageSize = productoParams.PageSize
                }
            );
        }

        //http://localhost:38109/api/Producto/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id) {
            
            var spec = new ProductoWithCategoriaAndMarcaSpecification(id);
            var producto =  await _productoRepository.GetByIdWithSpec(spec);

            if (producto == null) {
                return NotFound(new CodeErrorResponse(404, "El producto no existe"));
            }

            return _mapper.Map<Producto, ProductoDto>(producto);
        }

    }
}
