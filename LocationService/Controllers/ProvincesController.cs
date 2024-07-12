using LocationContextDb;
using Microsoft.AspNetCore.Mvc;

namespace LocationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProvincesController : ControllerBase
    {
        private readonly ILogger<ProvincesController> _logger;
        private readonly ILocationContext _context;

        public ProvincesController(ILogger<ProvincesController> logger, ILocationContext context)
        {
            _logger = logger;
            _context = context;
            _logger.LogInformation($".ctor {nameof(ProvincesController)}");
        }
    }
}
