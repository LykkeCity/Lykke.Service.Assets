using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers
{

    /// <inheritdoc />
    /// <summary>
    ///     Controller for issuers
    /// </summary>
    [Route("api/v2/issuers")]
    public class IssuerController : Controller
    {
        private readonly IIssuerService _issuerService;


        public IssuerController(
            IIssuerService issuerService)
        {
            _issuerService = issuerService;
        }

        [HttpDelete("{id}")]
        [SwaggerOperation("IssuerRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            await _issuerService.RemoveAsync(id);

            return NoContent();
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("IssuerExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetExists(string id)
        {
            var issuerExists = await _issuerService.GetAsync(id) != null;

            return Ok(issuerExists);
        }

        [HttpGet]
        [SwaggerOperation("IssuerGetAll")]
        [ProducesResponseType(typeof(IEnumerable<Issuer>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var assets = (await _issuerService.GetAllAsync())
                .Select(Mapper.Map<Issuer>);

            return Ok(assets);
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

        [HttpGet("default")]
        [SwaggerOperation("IssuerGetDefault")]
        [ProducesResponseType(typeof(Issuer), (int)HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var issuer = _issuerService.CreateDefault();

            return Ok(Mapper.Map<Issuer>(issuer));
        }

        [HttpPost]
        [SwaggerOperation("IssuerAdd")]
        [ProducesResponseType(typeof(Issuer), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] Issuer issuer)
        {
            issuer = Mapper.Map<Issuer>(await _issuerService.AddAsync(issuer));

            return Created
            (
                uri:   $"api/v2/issuers/{issuer.Id}",
                value: issuer
            );
        }

        [HttpPut]
        [SwaggerOperation("IssuerUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Put([FromBody] Issuer issuer)
        {
            await _issuerService.UpdateAsync(issuer);

            return NoContent();
        }
    }
}