using ApiHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IEmailValidator _emailValidator;
        private readonly IInnerApiClient _innerApiClient;

        public UserController(ILogger<UserController> logger, IUserContext context, IConfiguration configuration,
            IPasswordService passwordService, IEmailValidator emailValidator, IInnerApiClient innerApiClient)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _passwordService = passwordService;
            _emailValidator = emailValidator;
            _innerApiClient = innerApiClient;
            _logger.LogDebug($".ctor {nameof(UserController)}");
        }

        [HttpPost]
        public async Task<IActionResult> Post(RegistrationUserModel regUser)
        {
            _logger.LogInformation($"attemp save user to db");
            if (string.IsNullOrEmpty(regUser.Email) ||
                !_emailValidator.IsEmailValid(regUser.Email) ||
                string.IsNullOrEmpty(regUser.Password) || 
                !_passwordService.IsPasswordValid(regUser.Password))
                return BadRequest("user fields is incorrect");

            var checkEndpoint = _configuration.GetSection("AppSettings:CheckProvincesEndpoint").Value ?? string.Empty;
            var url = $"{checkEndpoint}{regUser.CountryId}/{regUser.ProvinceId}";
            var checkResp = await _innerApiClient.GetAsync<bool>(url);
            if (!checkResp.IsSuccessStatusCode)
                return BadRequest("Check province failure");

            try
            {
                if (await _context.Users.FirstOrDefaultAsync(_=>_.Email == regUser.Email)==null)
                {
                    var user = new UserModel()
                    {
                        Country = regUser.CountryId,
                        Email = regUser.Email,
                        Provinces = regUser.ProvinceId,
                        HashPassword = _passwordService.HashPassword(regUser.Email, regUser.Password)
                    };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
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
