using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers
{
    //TODO: Test carefully

    [Route("api/v2/assets")]
    public class AssetController : Controller
    {
        private readonly IAssetService _assetService;


        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("AssetRemove")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            await _assetService.RemoveAsync(id);

            return NoContent();
        }

        [HttpPost("{id}/disable")]
        [SwaggerOperation("AssetDisable")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Disable(string id)
        {
            await _assetService.DisableAsync(id);

            return NoContent();
        }

        [HttpPost("{id}/enable")]
        [SwaggerOperation("AssetEnable")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Enable(string id)
        {
            await _assetService.EnableAsync(id);

            return NoContent();
        }

        [HttpGet]
        [SwaggerOperation("AssetGetAll")]
        [ProducesResponseType(typeof(IEnumerable<Models.Asset>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var assets = (await _assetService.GetAllAsync())
                .Select(Mapper.Map<Models.Asset>);

            return Ok(assets);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("AssetGet")]
        [ProducesResponseType(typeof(Models.Asset), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var asset = await _assetService.GetAsync(id);

            if (asset != null)
            {
                return Ok(Mapper.Map<Models.Asset>(asset));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [SwaggerOperation("AssetAdd")]
        [ProducesResponseType(typeof(Models.Asset), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] Models.Asset asset)
        {
            asset = Mapper.Map<Models.Asset>(await _assetService.AddAsync(asset));

            return Created
            (
                uri:   $"api/v2/assets/{asset.Id}",
                value: asset
            );
        }

        [HttpPut]
        [SwaggerOperation("AssetUpdate")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Put([FromBody] Models.Asset asset)
        {
            await _assetService.UpdateAsync(asset);

            return NoContent();
        }
    }
}