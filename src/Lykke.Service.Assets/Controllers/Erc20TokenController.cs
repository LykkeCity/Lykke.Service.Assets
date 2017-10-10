using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Models;
using Lykke.Service.Assets.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Controller for erc 20 tokens
    /// </summary>
    [Route("api/[controller]")]
    public class Erc20TokenController : Controller
    {
        private readonly IErc20AssetService _erc20AssetService;

        public Erc20TokenController(IErc20AssetService erc20AssetService)
        {
            _erc20AssetService = erc20AssetService;
        }

        [HttpGet]
        [SwaggerOperation("GetAllAsync")]
        [ProducesResponseType(typeof(ListResponse<Erc20TokenModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllAsync()
        {
            var allTokens    = await _erc20AssetService.GetAllAsync();
            var responseList = allTokens?.Select(x => Mapper.Map<Erc20TokenModel>(x));

            return Ok(new ListResponse<Erc20TokenModel>()
            {
                List = responseList
            });
        }

        [HttpPost("getByAssetIds")]
        [SwaggerOperation("GetByAssetIdsAsync")]
        [ProducesResponseType(typeof(ListResponse<Erc20TokenModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByIdsAsync([FromBody]GetByIdsRequest assetIds)
        {
            var ids          = assetIds.Ids;
            var allTokens    = await _erc20AssetService.GetAsync(ids?.ToArray());
            var responseList = allTokens?.Select(x => Mapper.Map<Erc20TokenModel>(x));

            return Ok(new ListResponse<Erc20TokenModel>()
            {
                List = responseList
            });
        }

        [HttpPost]
        [SwaggerOperation("CreateTokenAsync")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> CreateTokenAsync([FromBody]Erc20TokenModel token)
        {
            await _erc20AssetService.AddAsync(token);

            return Ok();
        }

        [HttpPut]
        [SwaggerOperation("UpdateTokenAsync")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateTokenAsync([FromBody]Erc20TokenModel token)
        {
            await _erc20AssetService.UpdateAsync(token);

            return Ok();
        }
    }
}