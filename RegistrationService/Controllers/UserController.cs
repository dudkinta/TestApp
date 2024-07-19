using ApiHelper;
using CommonLibrary;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationService.Models;
using RegistrationService.Services;
using UserContextDb;
using UserContextDb.Models;

namespace RegistrationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordService _passwordService;
        private readonly IInnerApiClient _innerApiClient;
        private readonly IValidator<RegistrationUserModel> _userValidator;
        private readonly ConsulServiceDiscovery _serviceDiscovery;

        public UserController(ILogger<UserController> logger, IUserContext context, IConfiguration configuration,
            IPasswordService passwordService, IInnerApiClient innerApiClient,
            IValidator<RegistrationUserModel> userValidator, ConsulServiceDiscovery consulServiceDiscovery)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _passwordService = passwordService;
            _innerApiClient = innerApiClient;
            _userValidator = userValidator;
            _serviceDiscovery = consulServiceDiscovery;
            _logger.LogDebug($".ctor {nameof(UserController)}");
        }

        [HttpPost]
        public async Task<IActionResult> Post(RegistrationUserModel regUser, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Attemp save user to db");
            var validationResult = await _userValidator.ValidateAsync(regUser, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var checkEndpoint = await _serviceDiscovery.GetServiceAddress("LocationService");
            if (string.IsNullOrEmpty(checkEndpoint))
                return NotFound("Service Location not found");

            var url = $"http://{checkEndpoint}/api/check/{regUser.CountryId}/{regUser.ProvinceId}";
            var checkResp = await _innerApiClient.GetAsync<bool>(url, cancellationToken);
            if (!checkResp.IsSuccessStatusCode)
                return BadRequest("Check province failure");

            try
            {
                if (await _context.Users.FirstOrDefaultAsync(_ => _.Email == regUser.Email, cancellationToken) == null)
                {
                    var user = new UserModel()
                    {
                        Country = regUser.CountryId,
                        Email = regUser.Email,
                        Provinces = regUser.ProvinceId,
                        HashPassword = _passwordService.HashPassword(regUser.Email, regUser.Password)
                    };
                    _context.Users.Add(user);
                    await _context.SaveAsync(cancellationToken);
                    return Ok();
                }
                return BadRequest("User alredy exist");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
