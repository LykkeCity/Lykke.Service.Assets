using System;
using System.Linq;
using System.Net;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///    Allows to test if service is alive
    /// </summary>
    [Route("api/[controller]")]
    public class IsAliveController : Controller
    {
        private readonly IHealthService _healthService;

        public IsAliveController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        /// <summary>
        ///    Checks service is alive
        /// </summary>
        [HttpGet]
        [SwaggerOperation("IsAlive")]
        [ProducesResponseType(typeof(IsAliveResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse),   (int)HttpStatusCode.InternalServerError)]
        public IActionResult Get()
        {
            var healthViloationMessage = _healthService.GetHealthViolationMessage();
            if (healthViloationMessage != null)
            {
                return StatusCode
                (
                    (int) HttpStatusCode.InternalServerError,
                    ErrorResponse.Create($"Service is unhealthy: {healthViloationMessage}")
                );
            }

            var issueIndicators = _healthService.GetHealthIssues()
                .Select(i => new IsAliveResponse.IssueIndicator
                {
                    Type  = i.Type,
                    Value = i.Value
                });
            
            return Ok(new IsAliveResponse
            {
                Name    = PlatformServices.Default.Application.ApplicationName,
                Version = PlatformServices.Default.Application.ApplicationVersion,
                Env     = Environment.GetEnvironmentVariable("ENV_INFO"),
#if DEBUG
                IsDebug = true,
#else
                IsDebug = false,
#endif
                IssueIndicators = issueIndicators
            });
        }
    }
}
