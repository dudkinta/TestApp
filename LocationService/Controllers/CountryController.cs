using LocationContextDb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> _logger;
        private readonly ILocationContext _context;

        public CountryController(ILogger<CountryController> logger, ILocationContext context)
        {
            _logger = logger;
            _context = context;
            _logger.LogDebug($".ctor {nameof(CountryController)}");
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"enter to method GetCountries");
            try
            {
                var countries = await _context.Countries.ToListAsync(cancellationToken);
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
