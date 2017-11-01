using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Managers;
using Lykke.Service.Assets.Requests.V2;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    [Route("api/v2/assets")]
    public class AssetsController : Controller
    {
        private readonly IAssetManager         _assetManager;
        private readonly IAssetCategoryManager _assetCategoryManager;

        public AssetsController(
            IAssetManager assetManager,
            IAssetCategoryManager assetCategoryManager)
        {
            _assetManager         = assetManager;
            _assetCategoryManager = assetCategoryManager;
        }

        [HttpPost]
        [SwaggerOperation("AssetAdd")]
        [ProducesResponseType(typeof(Asset), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody] Asset asset)
        {
            asset = Mapper.Map<Asset>(await _assetManager.AddAsync(asset));
            
            return Created
            (
                uri:   $"api/v2/assets/{asset.Id}",
                value: asset
            );
        }

        [HttpPost("{id}/disable")]
        [SwaggerOperation("AssetDisable")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Disable(string id)
        {
            await _assetManager.DisableAsync(id);
            
            return NoContent();
        }

        [HttpPost("{id}/enable")]
        [SwaggerOperation("AssetEnable")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Enable(string id)
        {
            await _assetManager.EnableAsync(id);
            
            return NoContent();
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("AssetExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Exists(string id)
        {
            var asset = await _assetManager.GetAsync(id);

            return Ok(asset != null);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("AssetGet")]
        [ProducesResponseType(typeof(Asset), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var asset = await _assetManager.GetAsync(id);

            if (asset != null)
            {
                return Ok(Mapper.Map<Asset>(asset));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [SwaggerOperation("AssetGetAll")]
        [ProducesResponseType(typeof(IEnumerable<Asset>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var assets = (await _assetManager.GetAllAsync())
                .Select(Mapper.Map<Asset>);

            return Ok(assets);
        }

        [HttpPost("__specification")]
        [SwaggerOperation("AssetGetBySpecification")]
        [ProducesResponseType(typeof(ListOf<Asset>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBySpecification([FromBody] AssetSpecification specification)
        {
            var ids          = specification.Ids;
            var allTokens    = await _assetManager.GetAsync(ids?.ToArray());
            var responseList = allTokens?.Select(Mapper.Map<Asset>);

            return Ok(new ListOf<Asset>
            {
                Items = responseList
            });
        }

        [HttpGet("{id}/category")]
        [SwaggerOperation("AssetGet")]
        [ProducesResponseType(typeof(AssetCategory), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetCategory(string id)
        {
            var asset = await _assetManager.GetAsync(id);

            if (asset == null)
            {
                return NotFound("Asset not found");
            }

            var assetCategory = await _assetCategoryManager.GetAsync(asset.CategoryId ?? "");

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
            var asset = _assetManager.CreateDefault();

            return Ok(Mapper.Map<Asset>(asset));
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("AssetRemove")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string id)
        {
            await _assetManager.RemoveAsync(id);
            
            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("AssetUpdate")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] Asset asset)
        {
            await _assetManager.UpdateAsync(asset);
            
            return NoContent();
        }
    }
}
