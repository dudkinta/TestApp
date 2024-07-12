using Microsoft.AspNetCore.Mvc;
using UserContextDb;

namespace RegistrationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController: ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserContext _context;

        public UserController(ILogger<UserController> logger, IUserContext context)
        {
            _logger = logger;
            _context = context;
            _logger.LogInformation($".ctor {nameof(UserController)}");
        }
    }
}
