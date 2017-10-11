﻿using System.Collections.Generic;
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
    [Route("api/v2/asset-pairs")]
    public class AssetPairController : Controller
    {
        private readonly IAssetPairService _assetPairService;


        public AssetPairController(
            IAssetPairService assetPairService)
        {
            _assetPairService = assetPairService;
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("AssetPairRemove")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            await _assetPairService.RemoveAsync(id);

            return NoContent();
        }

        [HttpGet]
        [SwaggerOperation("AssetPairGetAll")]
        [ProducesResponseType(typeof(IEnumerable<AssetPair>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var assets = (await _assetPairService.GetAllAsync())
                .Select(Mapper.Map<AssetPair>);

            return Ok(assets);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("AssetPairGet")]
        [ProducesResponseType(typeof(AssetPair), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var asset = await _assetPairService.GetAsync(id);

            if (asset != null)
            {
                return Ok(Mapper.Map<AssetPair>(asset));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("default")]
        [SwaggerOperation("AssetPairGetDefault")]
        [ProducesResponseType(typeof(AssetPair), (int) HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var assetPair = _assetPairService.CreateDefault();

            return Ok(Mapper.Map<AssetPair>(assetPair));
        }

        [HttpPost]
        [SwaggerOperation("AssetPairAdd")]
        [ProducesResponseType(typeof(AssetPair), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] AssetPair assetPair)
        {
            assetPair = Mapper.Map<AssetPair>(await _assetPairService.AddAsync(assetPair));

            return Created
            (
                uri:   $"api/v2/asset-pairs/{assetPair.Id}",
                value: assetPair
            );
        }

        [HttpPut]
        [SwaggerOperation("AssetPairUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Put([FromBody] AssetPair assetPair)
        {
            await _assetPairService.UpdateAsync(assetPair);

            return NoContent();
        }
    }
}