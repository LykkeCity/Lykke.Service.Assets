using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Requests.V2;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    /// <inheritdoc />
    /// <summary>
    ///     Controller for erc 20 tokens
    /// </summary>
    [Route("api/v2/erc20-tokens")]
    public class Erc20TokensController : Controller
    {
        private readonly IErc20TokenService _erc20TokenService;

        public Erc20TokensController(IErc20TokenService erc20TokenService)
        {
            _erc20TokenService = erc20TokenService;
        }

        [HttpPost]
        [SwaggerOperation("Erc20TokenAdd")]
        [ProducesResponseType(typeof(Erc20Token), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody]Erc20Token token)
        {
            await _erc20TokenService.AddAsync(token);

            return Ok();
        }

        [HttpGet]
        [SwaggerOperation("Erc20TokenGetAll")]
        [ProducesResponseType(typeof(ListOf<Erc20Token>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var allTokens = await _erc20TokenService.GetAllAsync();
            var responseList = allTokens?.Select(Mapper.Map<Erc20Token>);

            return Ok(new ListOf<Erc20Token>()
            {
                Items = responseList
            });
        }

        [HttpGet("assets")]
        [SwaggerOperation("Erc20TokenGetAllWithAssets")]
        [ProducesResponseType(typeof(ListOf<Erc20Token>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Erc20TokenGetAllWithAssets()
        {
            var allTokens = await _erc20TokenService.GetAllWithAssetsAsync();
            var responseList = allTokens?.Select(Mapper.Map<Erc20Token>);

            return Ok(new ListOf<Erc20Token>()
            {
                Items = responseList
            });
        }

        [HttpGet("{address}")]
        [SwaggerOperation("Erc20TokenGetByAddress")]
        [ProducesResponseType(typeof(Erc20Token), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByAddress(string address)
        {
            var token = await _erc20TokenService.GetByTokenAddressAsync(address);

            if (token != null)
            {
                var response = Mapper.Map<Erc20Token>(token);

                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("specification")]
        [SwaggerOperation("Erc20TokenGetBySpecification")]
        [ProducesResponseType(typeof(ListOf<Erc20Token>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBySpecification([FromBody]Erc20TokenSpecification specification)
        {
            var ids          = specification.Ids;
            var allTokens    = await _erc20TokenService.GetAsync(ids?.ToArray());
            var responseList = allTokens?.Select(Mapper.Map<Erc20Token>);

            return Ok(new ListOf<Erc20Token>
            {
                Items = responseList
            });
        }

        [HttpPut]
        [SwaggerOperation("Erc20TokenUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody]Erc20Token token)
        {
            await _erc20TokenService.UpdateAsync(token);

            return NoContent();
        }

        //[HttpPut("{address}/verify")]
        //[SwaggerOperation("Erc20TokenVerify")]
        //[ProducesResponseType((int)HttpStatusCode.NoContent)]
        //public async Task<IActionResult> Put(string address)
        //{
        //    await _erc20TokenService.VerifyAsync(address);
        //
        //    return NoContent();
        //}
    }
}