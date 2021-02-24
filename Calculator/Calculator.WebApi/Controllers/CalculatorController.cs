using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Calculator.WebApi.Models;
using Calculator.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Calculator.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/calculator")]
    public class CalculatorController : ControllerBase
    {
        private static JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        private readonly ILogger<CalculatorController> _logger;
        private readonly HttpClient _httpClient;
        private readonly GoogleAuthToken _tokenService;

        public CalculatorController(ILogger<CalculatorController> logger, HttpClient httpClient, GoogleAuthToken tokenService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> Calculate([FromQuery] InputModel model, CancellationToken ct)
        {
            var url = model.Operation switch
            { 
                Operation.Addition => Environment.GetEnvironmentVariable("ADD_API"),
                Operation.Subtraction => Environment.GetEnvironmentVariable("SUB_API"),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            var operation = model.Operation switch
            { 
                Operation.Addition => "/api/v1/add",
                Operation.Subtraction => "/api/v1/sub",
                _ => throw new ArgumentOutOfRangeException()
            };
            
            _logger.LogInformation($"Calling API: {url}{operation}");
            _logger.LogInformation($"Calculating {model.Num1} {model.Operation} {model.Num2}");
            
            var token = await _tokenService.GetAuthTokenAsync(url, ct);
            
            _logger.LogInformation("Google Auth Token: {Token}", token);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await _httpClient.GetAsync($"{url}{operation}?Num1={model.Num1}&Num2={model.Num2}", ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Response error StatusCode: {StatusCode} Body: {Body}", response.StatusCode, await response.Content.ReadAsStringAsync(ct));
                return Unauthorized();
            }
            
            var json = await response.Content.ReadAsStringAsync(ct);
            
            var result = JsonSerializer.Deserialize<ResultModel>(json, JsonOptions);
            var operationResult = result == null ? 0 : result.Result;
            
            _logger.LogInformation($"Result of {model.Num1} {model.Operation} {model.Num2} = {operationResult}");

            return Ok(new ResultModel(operationResult));
        }
    }
}