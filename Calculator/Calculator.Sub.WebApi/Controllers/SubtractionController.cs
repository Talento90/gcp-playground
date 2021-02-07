using Calculator.Sub.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Calculator.Sub.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/sub")]
    public class SubtractionController : ControllerBase
    {
        private readonly ILogger<SubtractionController> _logger;

        public SubtractionController(ILogger<SubtractionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Sub([FromQuery]InputModel model)
        {
            var result = model.Num1 - model.Num2;
            
            _logger.LogInformation($"Calculating {model.Num1} - {model.Num2} = {result}");
            
            return Ok(new ResultModel(result));
        }
    }
}