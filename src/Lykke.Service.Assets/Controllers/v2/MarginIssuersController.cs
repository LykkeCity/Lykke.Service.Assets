using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Controllers.V2
{
    [ApiController]
    [Route("api/v2/margin-issuers")]
    public class MarginIssuersController : Controller
    {
        private readonly IMarginIssuerService _marginIssuerService;

        public MarginIssuersController(IMarginIssuerService marginIssuerService)
        {
            _marginIssuerService = marginIssuerService;
        }

        [HttpPost]
        [SwaggerOperation("MarginIssuerAdd")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add([FromBody] MarginIssuer marginIssuer)
        {
            await _marginIssuerService.AddAsync(marginIssuer);

            return Ok();
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("MarginIssuerExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Exists([FromRoute] string id)
        {
            var issuerExists = await _marginIssuerService.GetAsync(id) != null;

            return Ok(issuerExists);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("MarginIssuerGet")]
        [ProducesResponseType(typeof(MarginIssuer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var issuer = await _marginIssuerService.GetAsync(id);
            if (issuer == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<MarginIssuer>(issuer));
        }

        [HttpGet]
        [SwaggerOperation("MarginIssuerGetAll")]
        [ProducesResponseType(typeof(ListOf<MarginIssuer>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var issuers = await _marginIssuerService.GetAllAsync();
            var responseList = issuers?.Select(Mapper.Map<MarginIssuer>);

            return Ok(new ListOf<MarginIssuer>
            {
                Items = responseList
            });
        }

        [HttpGet("__default")]
        [SwaggerOperation("MarginIssuerGetDefault")]
        [ProducesResponseType(typeof(MarginIssuer), (int)HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var issuer = _marginIssuerService.CreateDefault();

            return Ok(Mapper.Map<MarginIssuer>(issuer));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("MarginIssuerRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string id)
        {
            await _marginIssuerService.RemoveAsync(id);

            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("MarginIssuerUpdate")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update([FromBody] MarginIssuer marginIssuer)
        {
            await _marginIssuerService.UpdateAsync(marginIssuer);

            return Ok();
        }
    }
}
