using LocationContextDb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocationService.Controllers
{
    [Authorize(AuthenticationSchemes = "Service", Policy = "ServicePolicy")]
    [ApiController]
    [Route("api/[controller]")]
    public class CheckController: ControllerBase
    {
        private readonly ILogger<CheckController> _logger;
        private readonly ILocationContext _context;
        public CheckController(ILogger<CheckController> logger, ILocationContext context)
        {
            _logger = logger;
            _context = context;
            _logger.LogDebug($".ctor {nameof(CheckController)}");
        }

        [HttpGet("{countryId}/{provinceId}")]
        public async Task<IActionResult> Check(int countryId, int provinceId)
        {
            try
            {
                if (await _context.Provinces.FirstOrDefaultAsync(_=>_.CountryId==countryId && _.Id == provinceId)!=null)
                    return Ok();

                return BadRequest("Province not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Error in check province");
            }
            
        }
    }
}
