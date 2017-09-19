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
        private readonly IDictionaryManager<IAssetDescription> _assetExtendedInfoManager;
        private readonly IDictionaryManager<IIssuer> _assetIssuerManager;
        private readonly IDictionaryManager<IAssetCategory> _assetCategoryManager;

        public AssetsController(IDictionaryManager<IAsset> manager, 
            IDictionaryManager<IAssetAttributes> assetAttributesManager, 
            IDictionaryManager<IAssetDescription> assetExtendedInfoManager, 
            IDictionaryManager<IIssuer> assetIssuerManager,
            IDictionaryManager<IAssetCategory> assetCategoryManager)
        {
            _manager = manager;
            _assetAttributesManager = assetAttributesManager;
            _assetExtendedInfoManager = assetExtendedInfoManager;
            _assetIssuerManager = assetIssuerManager;
            _assetCategoryManager = assetCategoryManager;
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
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [SwaggerOperation("GetAssetAttributes")]
        public async Task<IActionResult> AssetAttributes(string assetId)
        {
            var asset = await _manager.TryGetAsync(assetId);

            if (asset == null)
            {
                return NotFound((ErrorResponse.Create(nameof(assetId), "Asset not found")));
            }

            var assetAttributes = await _assetAttributesManager.TryGetAsync(assetId);

            return Ok(AssetAttributesResponseModel.Create(assetId, assetAttributes != null ? assetAttributes.Attributes.ToArray() : new KeyValue[0]));
        }


        /// <summary>
        /// Returns asset attribute by asset ID and attribute key
        /// </summary>
        /// <param name="assetId">Asset ID</param>
        /// <param name="key">Attribute key</param>
        [HttpGet("{assetId}/attributes/{key}")]
        [Produces("application/json", Type = typeof(AssetAttributesResponseModel))]
        [ProducesResponseType(typeof(AssetAttributesResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [SwaggerOperation("GetAssetAttributeByKey")]
        public async Task<IActionResult> AssetAttributeByKey(string assetId, string key)
        {
            var asset = await _manager.TryGetAsync(assetId);

            if (asset == null)
            {
                return NotFound((ErrorResponse.Create(nameof(assetId), "Asset not found")));
            }

            var assetAttributes = await _assetAttributesManager.TryGetAsync(assetId);
            return Ok(AssetAttributesResponseModel.Create(assetId, assetAttributes != null ? assetAttributes.Attributes.Where(a => a.Key == key).ToArray() : new KeyValue[0]));
        }

        /// <summary>
        /// Returns asset descriptions for array of assets
        /// </summary>
        /// <param name="request.Ids">Array asset ids</param>
        [HttpPost("description")]
        [Produces("application/json", Type = typeof(AssetDescriptionsResponseModel[]))]
        [ProducesResponseType(typeof(AssetDescriptionsResponseModel[]), (int)HttpStatusCode.OK)]
        [SwaggerOperation("GetAssetDescriptions")]
        public async Task<IActionResult> GetAssetDescriptions([FromBody]GetAssetDescriptionsRequestModel request)
        {
            var assets = ((await _manager.GetAllAsync()).Where(x => request == null || request.Ids == null || request.Ids.Contains(x.Id))).ToArray();
            List<AssetDescription> res = new List<AssetDescription>();

            if (assets.Any())
            {
                foreach (var asset in assets)
                {
                    var assetInfo = await GetAssetDescriptionAsync(asset);
                    if(assetInfo!=null)
                    {
                        res.Add(assetInfo);
                    }                    
                }
            }

            return Ok(res.Select(AssetDescriptionsResponseModel.Create));
        }

        private async Task<AssetDescription> GetAssetDescriptionAsync(IAsset asset)
        {
            var extendedInfo = await _assetExtendedInfoManager.TryGetAsync(asset.Id);
            if (extendedInfo == null) return null;

            var issuer = asset?.IdIssuer == null ? null : await _assetIssuerManager.TryGetAsync(asset.IdIssuer);
            var assetInfo = AssetDescription.Create(extendedInfo, issuer);
            return assetInfo;
        }

        /// <summary>
        /// Returns all asset categories
        /// </summary>
        [HttpGet("categories")]
        [Produces("application/json", Type = typeof(AssetCategoriesResponseModel[]))]
        [ProducesResponseType(typeof(AssetCategoriesResponseModel[]), (int)HttpStatusCode.OK)]
        [SwaggerOperation("GetAssetCategories")]
        public async Task<IActionResult> GetAssetCategories()
        {         
            var assetCategories = await _assetCategoryManager.GetAllAsync();
            return Ok(assetCategories.Select(AssetCategoriesResponseModel.Create));  // Ok(AssetCategoriesResponseModel.Create(assetCategories));
        }


        /// <summary>
        /// Returns asset category for asset id
        /// </summary>
        /// <param name="assetId">Asset ID</param>
        [HttpGet("{assetId}/categories")]
        [Produces("application/json", Type = typeof(AssetCategoriesResponseModel))]
        [ProducesResponseType(typeof(AssetCategoriesResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)] 
        [SwaggerOperation("GetAssetCategory")]
        public async Task<IActionResult> GetAssetCategories(string assetId)
        {
            var asset = await _manager.TryGetAsync(assetId);

            if (asset == null)
            {
                return NotFound(ErrorResponse.Create(nameof(assetId), "Asset not found")); //return NotFound(AssetCategoriesResponseModel.Create(ErrorResponse.Create(nameof(assetId), "Asset not found")));
            }

            var assetCategory = await _assetCategoryManager.TryGetAsync(asset.CategoryId ?? "");

            if(assetCategory == null)
            {
                return NotFound(ErrorResponse.Create(nameof(assetId), "No asset category found")); //return NotFound(AssetCategoriesResponseModel.Create(ErrorResponse.Create(nameof(assetId), "No asset category found")));
            }

            return Ok(AssetCategoriesResponseModel.Create(assetCategory)); //return Ok(AssetCategoriesResponseModel.Create(new List<IAssetCategory> { assetCategories }));
        }


        /// <summary>
        /// Returns all assets extended
        /// </summary>
        [HttpGet("extended")]
        [SwaggerOperation("GetAssetsExtended")]
        [ProducesResponseType(typeof(AssetExtendedResponseModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAssetsExtended()
        {
            var assets = await _manager.GetAllAsync();

            var result =  assets.Select(async asset => 
            {
                return await GetAssetExtendedInfo(asset);

            }).Select(r => r.Result).ToList();

            return Ok(AssetExtendedResponseModel.Create(result));          
        }

        private async Task<AssetExtended> GetAssetExtendedInfo(IAsset asset)
        {
            var assetDescription = await GetAssetDescriptionAsync(asset);
            var assetCategory = await _assetCategoryManager.TryGetAsync(asset.CategoryId ?? "") ?? new AssetCategory();
            var assetAttributes = await _assetAttributesManager.TryGetAsync(asset.Id) ?? new AssetAttributes { AssetId = asset.Id, Attributes = new List<KeyValue>() };

            return AssetExtended.Create(AssetResponseModel.Create(asset), AssetDescriptionsResponseModel.Create(assetDescription ?? new AssetDescription()), AssetCategoriesResponseModel.Create(assetCategory ?? new AssetCategory()), AssetAttributesResponseModel.Create(asset.Id, assetAttributes?.Attributes.ToArray() ?? new KeyValue[0]));
        }

        /// <summary>
        /// Returns all assets extended
        /// </summary>
        [HttpGet("{assetId}/extended")]
        [SwaggerOperation("GetAssetExtended")]
        [ProducesResponseType(typeof(AssetExtendedResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAssetExtended(string assetId)
        {
            var asset = await _manager.TryGetAsync(assetId);

            if (asset == null)
            {
                return NotFound(ErrorResponse.Create(nameof(assetId), "Asset not found"));
            }

            var assetExtended = await GetAssetExtendedInfo(asset);

            return Ok(AssetExtendedResponseModel.Create(new List<AssetExtended> { assetExtended }));
        }
    }
}