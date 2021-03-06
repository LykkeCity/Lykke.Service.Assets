﻿using AutoMapper;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Requests.V2;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Controllers.V2
{
    [ApiController]
    [Route("api/v2/erc20-tokens")]
    public class Erc20TokensController : Controller
    {
        private readonly ICachedErc20TokenAssetService _erc20TokenAssetService;
        private readonly ICachedErc20TokenService _erc20TokenService;

        public Erc20TokensController(
            ICachedErc20TokenAssetService erc20TokenAssetService,
            ICachedErc20TokenService erc20TokenService)
        {
            _erc20TokenAssetService = erc20TokenAssetService;
            _erc20TokenService = erc20TokenService;
        }

        [HttpPost]
        [SwaggerOperation("Erc20TokenAdd")]
        [ProducesResponseType(typeof(Erc20Token), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody]Erc20Token token)
        {
            await _erc20TokenService.AddAsync(token);

            return Ok();
        }

        [HttpPut("{address}/create-asset")]
        [SwaggerOperation("Erc20TokenCreateAsset")]
        [ProducesResponseType(typeof(Asset), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateAsset(string address)
        {
            var asset = Mapper.Map<Asset>(await _erc20TokenAssetService.CreateAssetForErc20TokenAsync(address));

            return Created
            (
                uri: $"api/v2/assets/{asset.Id}",
                value: asset
            );
        }

        [HttpGet("with-assets")]
        [SwaggerOperation("Erc20TokenGetAllWithAssets")]
        [ProducesResponseType(typeof(ListOf<Erc20Token>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Erc20TokenGetAllWithAssets()
        {
            var allTokens = await _erc20TokenService.GetAllWithAssetsAsync();

            return Ok(new ListOf<Erc20Token>
            {
                Items = allTokens
            });
        }

        [HttpGet("{address}")]
        [SwaggerOperation("Erc20TokenGetByAddress")]
        [ProducesResponseType(typeof(Erc20Token), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByAddress(string address)
        {
            var token = await _erc20TokenService.GetByTokenAddressAsync(address);
            if (token == null)
            {
                return NotFound();
            }

            return Ok(token);
        }

        [HttpPost("__specification")]
        [SwaggerOperation("Erc20TokenGetBySpecification")]
        [ProducesResponseType(typeof(ListOf<Erc20Token>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBySpecification([FromBody]Erc20TokenSpecification specification)
        {
            var ids = specification.Ids;
            var allTokens = await _erc20TokenService.GetByAssetIdsAsync(ids?.ToArray());

            return Ok(new ListOf<Erc20Token>
            {
                Items = allTokens
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
    }
}
