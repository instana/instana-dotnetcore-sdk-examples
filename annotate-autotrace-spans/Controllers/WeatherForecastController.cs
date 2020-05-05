using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Instana.Tracing.Sdk.Spans;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace annotate_autotrace_spans.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            // when we enter this method, there is already a span
            // on the stack, which has not yet been closed, because it will only be closed
            // when the request has been handled completely.
            // so now is the time to annotate this span a bit.

            using (CustomSpan autospan = CustomSpan.FromCurrentContext())
            {
                var rng = new Random();
                var range = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                });
                autospan.SetTag("SUMMARIES", string.Join(",", range.Select((fc) => fc.Summary).ToArray()));
                return range.ToArray();
            }
        }
    }
}
