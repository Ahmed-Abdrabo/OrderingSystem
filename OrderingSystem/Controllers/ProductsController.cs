using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.API.Dtos;
using OrderingSystem.API.Errors;
using OrderingSystem.Core.Services.Contract;


namespace OrderingSystem.API.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;

        private readonly IMapper _mapper;

        public ProductsController(IProductService productService,
            IMapper mapper)
        {
           
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {

            var products = await _productService.GetAllProductsAsync();

            var productsToReturn = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(products);



            return Ok(productsToReturn);
        }

        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

      
    }
}
