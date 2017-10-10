using System;
using Lykke.Service.Assets.Models;
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
        /// <summary>
        ///    Returns service alive status
        /// </summary>
        [HttpGet]
        [SwaggerOperation("IsAlive")]
        public IsAlive IsAlive()
        {
            return new IsAlive
            {
                Env     = Environment.GetEnvironmentVariable("ENV_INFO"),
                Version = PlatformServices.Default.Application.ApplicationVersion
            };
        }
    }
}