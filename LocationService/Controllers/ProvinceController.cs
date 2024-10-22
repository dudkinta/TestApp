﻿using LocationContextDb;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvinceController : ControllerBase
    {
        private readonly ILogger<ProvinceController> _logger;
        private readonly ILocationContext _context;

        public ProvinceController(ILogger<ProvinceController> logger, ILocationContext context)
        {
            _logger = logger;
            _context = context;
            _logger.LogDebug($".ctor {nameof(ProvinceController)}");
        }

        [HttpGet("{countryId}")]
        public async Task<IActionResult> Get(int countryId, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get provinces with country id {countryId}");
            try
            {
                var provinces = await _context.Provinces.Where(_ => _.CountryId == countryId).ToListAsync(cancellationToken);
                return Ok(provinces);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
