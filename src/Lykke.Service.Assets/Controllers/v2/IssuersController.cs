using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{

    /// <inheritdoc />
    /// <summary>
    ///     Controller for issuers
    /// </summary>
    [Route("api/v2/issuers")]
    public class IssuersController : Controller
    {
        private readonly IIssuerService _issuerService;


        public IssuersController(
            IIssuerService issuerService)
        {
            _issuerService = issuerService;
        }

        [HttpPost]
        [SwaggerOperation("IssuerAdd")]
        [ProducesResponseType(typeof(Issuer), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody] Issuer issuer)
        {
            issuer = Mapper.Map<Issuer>(await _issuerService.AddAsync(issuer));

            return Created
            (
                uri:   $"api/v2/issuers/{issuer.Id}",
                value: issuer
            );
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("IssuerExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Exists(string id)
        {
            var issuerExists = await _issuerService.GetAsync(id) != null;

            return Ok(issuerExists);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("IssuerGet")]
        [ProducesResponseType(typeof(Issuer), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var asset = await _issuerService.GetAsync(id);

            if (asset != null)
            {
                return Ok(Mapper.Map<Issuer>(asset));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [SwaggerOperation("IssuerGetAll")]
        [ProducesResponseType(typeof(IEnumerable<Issuer>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var assets = (await _issuerService.GetAllAsync())
                .Select(Mapper.Map<Issuer>);

            return Ok(assets);
        }

        [HttpGet("__default")]
        [SwaggerOperation("IssuerGetDefault")]
        [ProducesResponseType(typeof(Issuer), (int)HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var issuer = _issuerService.CreateDefault();

            return Ok(Mapper.Map<Issuer>(issuer));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("IssuerRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string id)
        {
            await _issuerService.RemoveAsync(id);

            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("IssuerUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] Issuer issuer)
        {
            await _issuerService.UpdateAsync(issuer);

            return NoContent();
        }
    }
}