using InnerTokenService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InnerTokenService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly ITokenHelper _tokenHelper;
        public TokenController(ILogger<TokenController> logger, ITokenHelper tokenHelper)
        {
            _logger = logger;
            _tokenHelper = tokenHelper;
            _logger.LogDebug($".ctor {nameof(TokenController)}");
        }

        [HttpGet]
        public IActionResult Login()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            _logger.LogInformation($"Get inner token from {ipAddress}");
            var jwt = _tokenHelper.GenerateServiceToken(new TimeSpan(24, 0, 0));
            return Ok(new { token = jwt });
        }
    }
}
