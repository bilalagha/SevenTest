using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SevenTest.Business;
using SevenTest.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SevenTest.WebApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private readonly IPersonService _personService;


        public PersonController(ILogger<PersonController> logger, IPersonService personService)
        {
            _logger = logger;
            _personService = personService;
        }


        [HttpGet("GetFullNameById")]
        public async Task<IActionResult> GetFullNameById([FromQuery] int id)
        {
            _logger.LogInformation($"GetFullNameById Called for id {id}");
            try
            {
                return Ok(await _personService.GetFullNameById(id));

            }
            catch
            {
                _logger.LogError($"Person for id{id} not found");
                return NotFound();
            }
        }

        [HttpGet("GetFirstNamesByAge")]

        public async Task<ActionResult<List<string>>> GetFirstNamesByAge([FromQuery] int age)
        {
            _logger.LogInformation($"GetFirstNamesByAge Called for age {age}");
            var result = await _personService.GetFirstNamesByAge(age);
            return Ok(result);
        }

        [HttpGet("GetGenersPerAge")]
        public async Task<IActionResult> GetGenersPerAge()
        {
            _logger.LogInformation($"GetGenersPerAge Called");
            return Ok(await _personService.GetGendersPerAge());
        }

    }
}
