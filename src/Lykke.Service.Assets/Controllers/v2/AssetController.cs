using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Requests;
using Lykke.Service.Assets.Requests.V2;
using Lykke.Service.Assets.Responses;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
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
        [ProducesResponseType(typeof(IEnumerable<Asset>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var assets = (await _assetService.GetAllAsync())
                .Select(Mapper.Map<Asset>);

            return Ok(assets);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("AssetGet")]
        [ProducesResponseType(typeof(Asset), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var asset = await _assetService.GetAsync(id);

            if (asset != null)
            {
                return Ok(Mapper.Map<Asset>(asset));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("specification")]
        [SwaggerOperation("AssetGetBySpecification")]
        [ProducesResponseType(typeof(ListOf<Asset>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromBody] AssetSpecification specification)
        {
            var ids          = specification.Ids;
            var allTokens    = await _assetService.GetAsync(ids?.ToArray());
            var responseList = allTokens?.Select(Mapper.Map<Asset>);

            return Ok(new ListOf<Asset>
            {
                Items = responseList
            });
        }

        [HttpGet("default")]
        [SwaggerOperation("AssetGetDefault")]
        [ProducesResponseType(typeof(Asset), (int)HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var asset = _assetService.CreateDefault();

            return Ok(Mapper.Map<Asset>(asset));
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("AssetExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public IActionResult GetExists(string id)
        {
            var assetExists = _assetService.GetAsync(id) != null;

            return Ok(assetExists);
        }

        [HttpPost]
        [SwaggerOperation("AssetAdd")]
        [ProducesResponseType(typeof(Asset), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] Asset asset)
        {
            asset = Mapper.Map<Asset>(await _assetService.AddAsync(asset));

            return Created
            (
                uri:   $"api/v2/assets/{asset.Id}",
                value: asset
            );
        }

        [HttpPut]
        [SwaggerOperation("AssetUpdate")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Put([FromBody] Asset asset)
        {
            await _assetService.UpdateAsync(asset);

            return NoContent();
        }
    }
}