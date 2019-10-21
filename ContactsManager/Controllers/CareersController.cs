using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactsManager.Extentions;
using ContactsManager.Interfaces;
using ContactsManager.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactsManager.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CareersController : ControllerBase
    {
        # region Fields
        //
        private readonly IAsyncRepository<Career> _repo;
        //
        private readonly ILoggerManager _logger;
        # endregion

        public CareersController(ILoggerManager logger, IAsyncRepository<Career> cCtx)
        {
            _logger = logger;
            _repo = cCtx;
        }

        // GET api/v1/careers
        [HttpGet]
        public async Task<ActionResult<List<Contact>>> Get(int page = 0, int pageSize = 10)
        {
            try
            {
                var jobs = await _repo.GetPaged(page, pageSize);
                return Ok(jobs.Select(e => e.Name).ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Some error in the GetPaged method: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
