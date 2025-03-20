using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderingSystem.API.Errors;

namespace OrderingSystem.API.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            if (code == 401)
            {
                return Unauthorized(new ApiResponse(code)); 
            }
            else if (code == 404)
            {
                return NotFound(new ApiResponse(code)); 
            }
            else
            {
                return StatusCode(code, new ApiResponse(code)); 
            }
        }
    }
}
