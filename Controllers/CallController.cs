using Ponisha.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace EmergencyCallQueue.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CallsController : ControllerBase
    {
        // صف تماس‌ها بر اساس اضطرار (بیشینه اولویت)
        private static readonly SortedDictionary<int, Queue<string>> queues = new(Comparer<int>.Create((x, y) => y.CompareTo(x)));

        [HttpPost("call")]
        public IActionResult AddCall([FromBody] CallRequest request)
        {
            if (request.Urgency < 1 || request.Urgency > 5 || string.IsNullOrWhiteSpace(request.Id))
                return BadRequest("Invalid call data.");

            lock (queues)
            {
                if (!queues.ContainsKey(request.Urgency))
                {
                    queues[request.Urgency] = new Queue<string>();
                }
                queues[request.Urgency].Enqueue(request.Id);
            }

            return Ok($"Call {request.Id} with urgency {request.Urgency} added.");
        }

        [HttpGet("dispatch")]
        public IActionResult DispatchCall()
        {
            lock (queues)
            {
                foreach (var q in queues)
                {
                    if (q.Value.Count > 0)
                    {
                        var dispatched = q.Value.Dequeue();
                        return Ok(dispatched);
                    }
                }
            }

            return Ok("No calls to dispatch.");
        }
    }
}
