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
        private readonly IPasswordService _passwordService;
        private readonly IEmailValidator _emailValidator;

        public UserController(ILogger<UserController> logger, IUserContext context, IPasswordService passwordService, IEmailValidator emailValidator)
        {
            _logger = logger;
            _context = context;
            _passwordService = passwordService;
            _emailValidator = emailValidator;
            _logger.LogInformation($".ctor {nameof(UserController)}");
        }

        [HttpPost]
        public async Task<IActionResult> Post(RegistrationUserModel regUser)
        {
            _logger.LogInformation($"attemp save user to db");
            if (string.IsNullOrEmpty(regUser.Email) ||
                !_emailValidator.IsEmailValid(regUser.Email) ||
                string.IsNullOrEmpty(regUser.Password) || 
                !_passwordService.IsPasswordValid(regUser.Password) ||
                regUser.CountryId == 0 || regUser.ProvincesId == 0)
                return BadRequest("user fields is incorrect");

            try
            {
                if (await _context.Users.FirstOrDefaultAsync(_=>_.Email == regUser.Email)==null)
                {
                    var user = new UserModel()
                    {
                        Country = regUser.CountryId,
                        Email = regUser.Email,
                        Provinces = regUser.ProvincesId,
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
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }


        }
    }
}
