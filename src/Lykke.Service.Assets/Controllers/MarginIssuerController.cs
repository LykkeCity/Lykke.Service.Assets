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
        public async Task<IActionResult> GetAll()
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
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var issuer   = await _marginIssuerService.GetAsync(id);

            if (issuer != null)
            {
                return Ok(Mapper.Map<MarginIssuer>(issuer));
            }
            else
            {
                return NotFound();
            }

            
        }

        [HttpGet("default")]
        [SwaggerOperation("MarginIssuerGetDefault")]
        [ProducesResponseType(typeof(MarginIssuer), (int)HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var issuer = _marginIssuerService.CreateDefault();
            
            return Ok(Mapper.Map<MarginIssuer>(issuer));
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("MarginIssuerExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExists([FromRoute] string id)
        {
            var issuerExists = await _marginIssuerService.GetAsync(id) != null;

            return Ok(issuerExists);
        }

        [HttpPost]
        [SwaggerOperation("MarginIssuerAdd")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] MarginIssuer marginIssuer)
        {
            await _marginIssuerService.AddAsync(marginIssuer);

            return Ok();
        }

        [HttpPut]
        [SwaggerOperation("MarginIssuerUpdate")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Put([FromBody] MarginIssuer marginIssuer)
        {
            await _marginIssuerService.UpdateAsync(marginIssuer);

            return Ok();
        }
    }
}