using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Controller for Margin Issuers
    /// </summary>
    [Route("api/v2/margin-issuers")]
    public class MarginIssuerController : Controller
    {
        private readonly IMarginIssuerService _marginIssuerService;

        public MarginIssuerController(IMarginIssuerService marginIssuerService)
        {
            _marginIssuerService = marginIssuerService;
        }

        [HttpGet]
        [SwaggerOperation("MarginIssuerGetAll")]
        [ProducesResponseType(typeof(ListOf<MarginIssuer>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllAsync()
        {
            var issuers = await _marginIssuerService.GetAllAsync();
            var responseList = issuers?.Select(Mapper.Map<MarginIssuer>);

            return Ok(new ListOf<MarginIssuer>()
            {
                Items = responseList
            });
        }

        [HttpGet("{id}")]
        [SwaggerOperation("MarginIssuerGet")]
        [ProducesResponseType(typeof(MarginIssuer), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var issuer   = await _marginIssuerService.GetAsync(id);
            var response = Mapper.Map<MarginIssuer>(issuer);

            return Ok(response);
        }

        [HttpGet("default")]
        [SwaggerOperation("MarginIssuerGetDefault")]
        [ProducesResponseType(typeof(MarginIssuer), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public IActionResult GetDefault()
        {
            var issuer   = _marginIssuerService.CreateDefault();
            var response = Mapper.Map<MarginIssuer>(issuer);

            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation("MarginIssuerAdd")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Post([FromBody] MarginIssuer marginIssuer)
        {
            await _marginIssuerService.AddAsync(marginIssuer);

            return Ok();
        }

        [HttpPut]
        [SwaggerOperation("MarginIssuerUpdate")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Put([FromBody] MarginIssuer marginIssuer)
        {
            await _marginIssuerService.UpdateAsync(marginIssuer);

            return Ok();
        }
    }
}