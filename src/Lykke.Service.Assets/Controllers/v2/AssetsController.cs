﻿using AutoMapper;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Requests.V2;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Controllers.V2
{
    [ApiController]
    [Route("api/v2/assets")]
    public class AssetsController : Controller
    {
        private readonly ICachedAssetService _assetService;
        private readonly ICachedAssetCategoryService _assetCategoryService;

        public AssetsController(
            ICachedAssetService assetService,
            ICachedAssetCategoryService assetCategoryService)
        {
            _assetService = assetService;
            _assetCategoryService = assetCategoryService;
        }

        [HttpPost]
        [SwaggerOperation("AssetAdd")]
        [ProducesResponseType(typeof(Asset), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody] Asset asset)
        {
            asset = Mapper.Map<Asset>(await _assetService.AddAsync(asset));

            return Created
            (
                uri: $"api/v2/assets/{asset.Id}",
                value: asset
            );
        }

        [HttpPost("{id}/disable")]
        [SwaggerOperation("AssetDisable")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Disable(string id)
        {
            await _assetService.DisableAsync(id);

            return NoContent();
        }

        [HttpPost("{id}/enable")]
        [SwaggerOperation("AssetEnable")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Enable(string id)
        {
            await _assetService.EnableAsync(id);

            return NoContent();
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("AssetExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Exists(string id)
        {
            var asset = await _assetService.GetAsync(id);

            return Ok(asset != null);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("AssetGet")]
        [ProducesResponseType(typeof(Asset), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var asset = await _assetService.GetAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Asset>(asset));
        }

        [HttpGet]
        [SwaggerOperation("AssetGetAll")]
        [ProducesResponseType(typeof(IEnumerable<Asset>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] bool includeNonTradable = false)
        {
            var assets = (await _assetService.GetAllAsync(includeNonTradable))
                .Select(Mapper.Map<Asset>);

            return Ok(assets);
        }

        [HttpPost("__specification")]
        [SwaggerOperation("AssetGetBySpecification")]
        [ProducesResponseType(typeof(ListOf<Asset>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBySpecification([FromBody] AssetSpecification specification)
        {
            var ids = specification?.Ids?.ToArray() ?? new string[0];
            var isTradable = specification?.IsTradable;
            var allTokens = await _assetService.GetAsync(ids.ToArray(), isTradable);
            var responseList = allTokens?.Select(Mapper.Map<Asset>);

            return Ok(new ListOf<Asset>
            {
                Items = responseList
            });
        }

        [HttpGet("{id}/category")]
        [SwaggerOperation("AssetGetCategory")]
        [ProducesResponseType(typeof(AssetCategory), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCategory(string id)
        {
            var asset = await _assetService.GetAsync(id);
            if (asset == null)
            {
                return NotFound("Asset not found");
            }

            var assetCategory = await _assetCategoryService.GetAsync(asset.CategoryId ?? "");
            if (assetCategory == null)
            {
                return NotFound("Asset category not found");
            }

            return Ok(Mapper.Map<AssetCategory>(assetCategory));
        }

        [HttpGet("default")]
        [SwaggerOperation("AssetGetDefault")]
        [ProducesResponseType(typeof(Asset), (int)HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var asset = _assetService.CreateDefault();

            return Ok(Mapper.Map<Asset>(asset));
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("AssetRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string id)
        {
            await _assetService.RemoveAsync(id);

            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("AssetUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] Asset asset)
        {
            await _assetService.UpdateAsync(asset);

            return NoContent();
        }
    }
}
