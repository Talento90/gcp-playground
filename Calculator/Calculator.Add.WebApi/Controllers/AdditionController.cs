using Calculator.Add.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Calculator.Add.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/add")]
    public class AdditionController : ControllerBase
    {
        private readonly ILogger<AdditionController> _logger;

        public AdditionController(ILogger<AdditionController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Add([FromQuery] InputModel model)
        {
            var result = model.Num1 + model.Num2;
            
            _logger.LogInformation($"Calculating {model.Num1} + {model.Num2} = {result}");
            
            return Ok(new ResultModel(result));
        }
    }
}