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
    [Route("api/[controller]")]
    public class MarginIssuerController : Controller
    {
        private readonly IMarginIssuerService _marginIssuerService;

        public MarginIssuerController(IMarginIssuerService marginIssuerService)
        {
            _marginIssuerService = marginIssuerService;
        }

        [HttpGet]
        [SwaggerOperation("GetAllAsync")]
        [ProducesResponseType(typeof(ListResponse<MarginIssuer>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllAsync()
        {
            var issuers = await _marginIssuerService.GetAllAsync();
            var responseList = issuers?.Select(x => Mapper.Map<MarginIssuer>(x));

            return Ok(new ListResponse<MarginIssuer>()
            {
                List = responseList
            });
        }

        [HttpGet("{id}")]
        [SwaggerOperation("GetAsync")]
        [ProducesResponseType(typeof(MarginIssuer), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] string id)
        {
            var issuer = await _marginIssuerService.GetAsync(id);
            var response = Mapper.Map<MarginIssuer>(issuer);

            return Ok(response);
        }

        [HttpPost("createDefault")]
        [SwaggerOperation("CreateDefaultAsync")]
        [ProducesResponseType(typeof(MarginIssuer), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreateDefaultAsync()
        {
            var issuer = _marginIssuerService.CreateDefault();
            var response = Mapper.Map<MarginIssuer>(issuer);

            return Ok(response);
        }

        [HttpPost]
        [SwaggerOperation("AddAsync")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddAsync([FromBody] MarginIssuer marginIssuer)
        {
            await _marginIssuerService.AddAsync(marginIssuer);

            return Ok();
        }

        [HttpPut]
        [SwaggerOperation("UpdateAsync")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateAsync([FromBody] MarginIssuer marginIssuer)
        {
            await _marginIssuerService.UpdateAsync(marginIssuer);

            return Ok();
        }
    }
}