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

        public UserController(ILogger<UserController> logger, IUserContext context, IPasswordService passwordService)
        {
            _logger = logger;
            _context = context;
            _passwordService = passwordService;
            _logger.LogInformation($".ctor {nameof(UserController)}");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Test");
        }

        [HttpPost]
        public async Task<IActionResult> Post(RegistrationUserModel regUser)
        {
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
