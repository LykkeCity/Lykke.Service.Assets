using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Requests;

namespace Lykke.Service.Assets.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Controller for erc 20 tokens
    /// </summary>
    [Route("api/v2/erc20-tokens")]
    public class Erc20TokenController : Controller
    {
        private readonly IErc20TokenService _erc20TokenService;

        public Erc20TokenController(IErc20TokenService erc20TokenService)
        {
            _erc20TokenService = erc20TokenService;
        }

        [HttpGet("{address}")]
        [SwaggerOperation("Erc20TokenGetByAddressAsync")]
        [ProducesResponseType(typeof(Erc20Token), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Erc20TokenGetByAddressAsync(string address)
        {
            var token = await _erc20TokenService.GetByTokenAddressAsync(address);
            var response = Mapper.Map<Erc20Token>(token);

            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation("Erc20TokenGetAll")]
        [ProducesResponseType(typeof(ListOf<Erc20Token>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllAsync()
        {
            var allTokens    = await _erc20TokenService.GetAllAsync();
            var responseList = allTokens?.Select(Mapper.Map<Erc20Token>);

            return Ok(new ListOf<Erc20Token>()
            {
                Items = responseList
            });
        }

        [HttpPost("specification")]
        [SwaggerOperation("Erc20TokenGetBySpecification")]
        [ProducesResponseType(typeof(ListOf<Erc20Token>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromBody]Erc20TokenSpecification specification)
        {
            var ids          = specification.Ids;
            var allTokens    = await _erc20TokenService.GetAsync(ids?.ToArray());
            var responseList = allTokens?.Select(Mapper.Map<Erc20Token>);

            return Ok(new ListOf<Erc20Token>()
            {
                Items = responseList
            });
        }

        [HttpPost]
        [SwaggerOperation("Erc20TokenAdd")]
        [ProducesResponseType(typeof(Erc20Token), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> CreateTokenAsync([FromBody]Erc20Token token)
        {
            await _erc20TokenService.AddAsync(token);

            return Ok();
        }

        [HttpPut]
        [SwaggerOperation("Erc20TokenUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateTokenAsync([FromBody]Erc20Token token)
        {
            await _erc20TokenService.UpdateAsync(token);

            return NoContent();
        }
    }
}