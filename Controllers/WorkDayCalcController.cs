using Microsoft.AspNetCore.Mvc;
using Ponisha.Models;

namespace Ponisha.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkDayCalcController : ControllerBase
    {
        [HttpPost("max-consecutive")]
        public IActionResult GetMaxConsecutiveDays([FromBody] WorkDaysRequest request)
        {
            int max = 0, current = 0;

            foreach (var c in request.WorkDaysBinary)
            {
                if (c == '1')
                {
                    current++;
                    if (current > max)
                        max = current;
                }
                else
                {
                    current = 0;
                }
            }

            return Ok(new { maxConsecutive = max });
        }
    }

}
