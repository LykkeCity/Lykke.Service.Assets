using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System.Collections.Generic;

namespace Lykke.Service.Assets.Controllers
{
    /// <summary>
    /// Controller for assets
    /// </summary>
    [Route("api/[controller]")]
    public class AssetsController : Controller
    {
        private readonly IDictionaryManager<IAsset> _manager;
        private readonly IDictionaryManager<IAssetAttributes> _assetAttributesManager;

        public AssetsController(IDictionaryManager<IAsset> manager, IDictionaryManager<IAssetAttributes> assetAttributesManager)
        {
            _manager = manager;
            _assetAttributesManager = assetAttributesManager;
        }

        /// <summary>
        /// Forcibly updates assets cache
        /// </summary>
        /// <returns></returns>
        [HttpPost("updateCache")]
        [SwaggerOperation("UpdateAssetsCache")]
        public async Task UpdateCache()
        {
            await _manager.UpdateCacheAsync();
        }

        /// <summary>
        /// Returns asset by ID
        /// </summary>
        /// <param name="assetId">Asset ID</param>
        [HttpGet("{assetId}")]
        [SwaggerOperation("GetAsset")]
        [ProducesResponseType(typeof(AssetResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string assetId)
        {
            var asset = await _manager.TryGetAsync(assetId);

            if (asset == null)
            {
                return NotFound(ErrorResponse.Create(nameof(assetId), "Asset not found"));
            }

            return Ok(AssetResponseModel.Create(asset));
        }

        /// <summary>
        /// Returns all assets
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(AssetResponseModel[]), (int)HttpStatusCode.OK)]
        [SwaggerOperation("GetAssets")]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _manager.GetAllAsync();

            return Ok(assets.Select(AssetResponseModel.Create));
        }

        /// <summary>
        /// Returns asset attributes by ID
        /// </summary>
        /// <param name="assetId">Asset ID</param>
        [HttpGet("{assetId}/attributes")]
        [Produces("application/json", Type = typeof(AssetAttributesResponseModel))]
        [ProducesResponseType(typeof(AssetAttributesResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(AssetAttributesResponseModel), (int)HttpStatusCode.NotFound)]
        [SwaggerOperation("GetAssetAttributes")]
        public async Task<IActionResult> AssetAttributes(string assetId)
        {
            var asset = await _manager.TryGetAsync(assetId);

            if (asset == null)
            {
                return NotFound(AssetAttributesResponseModel.Create(ErrorResponse.Create(nameof(assetId), "Asset not found")));
            }

            var assetAttributes = await _assetAttributesManager.TryGetAsync(assetId);

            return Ok(AssetAttributesResponseModel.Create(assetAttributes));
        }


        /// <summary>
        /// Returns asset attribute by ID and key
        /// </summary>
        /// <param name="assetId">Asset ID</param>
        /// <param name="key">Attribute key</param>
        [HttpGet("{assetId}/attributes/{key}")]
        [Produces("application/json", Type = typeof(AssetAttributesResponseModel))]
        [ProducesResponseType(typeof(AssetAttributesResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(AssetAttributesResponseModel), (int)HttpStatusCode.NotFound)]
        [SwaggerOperation("GetAssetAttributeByKey")]
        public async Task<IActionResult> AssetAttributeByKey(string assetId, string key)
        {
            var asset = await _manager.TryGetAsync(assetId);

            if (asset == null)
            {
                return NotFound(AssetAttributesResponseModel.Create(ErrorResponse.Create(nameof(assetId), "Asset not found")));
            }

            var assetAttributes = await _assetAttributesManager.TryGetAsync(assetId);
            assetAttributes.Attributes = assetAttributes.Attributes.Where(a => a.Key == key);

            return Ok(AssetAttributesResponseModel.Create(assetAttributes));
        }
    }
}