using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Calculator.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Calculator.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/calculator")]
    public class CalculatorController : ControllerBase
    {
        private static JsonSerializerOptions JsonOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private readonly ILogger<CalculatorController> _logger;
        private readonly HttpClient _httpClient;

        public CalculatorController(ILogger<CalculatorController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> Calculate([FromQuery] InputModel model, CancellationToken ct)
        {
            var url = model.Operation switch
            {
                Operation.Addition => "http://addition-api/api/v1/add",
                Operation.Subtraction => "http://subtraction-api/api/v1/sub",
                _ => throw new ArgumentOutOfRangeException()
            };
            
            _logger.LogInformation($"Calling API: {url}");
            _logger.LogInformation($"Calculating {model.Num1} {model.Operation} {model.Num2}");
            
            var response = await _httpClient.GetAsync($"{url}?Num1={model.Num1}&Num2={model.Num2}", ct);

            var json = await response.Content.ReadAsStringAsync(ct);

            var result = JsonSerializer.Deserialize<ResultModel>(json, JsonOptions);
            var operationResult = result == null ? 0 : result.Result;
            
            _logger.LogInformation($"Result of {model.Num1} {model.Operation} {model.Num2} = {operationResult}");

            return Ok(new ResultModel(operationResult));
        }
    }
}