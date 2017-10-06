using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System.Collections.Generic;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Services;
using AssetAttributes = Lykke.Service.Assets.Models.AssetAttributes;
using AssetCategory = Lykke.Service.Assets.Models.AssetCategory;
using AssetDescription = Lykke.Service.Assets.Models.AssetDescription;

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
        private readonly IAssetsServiceHelper _assetsServiceHelper;


        public AssetsController(IDictionaryManager<IAsset> manager, 
            IDictionaryManager<IAssetAttributes> assetAttributesManager, 
            IDictionaryManager<IAssetDescription> assetExtendedInfoManager, 
            IDictionaryManager<IIssuer> assetIssuerManager,
            IDictionaryManager<IAssetCategory> assetCategoryManager,
            IAssetGroupRepository assetGroupRepo,
            IAssetsServiceHelper assetsServiceHelper)
        {
            _manager = manager;
            _assetAttributesManager = assetAttributesManager;
            _assetExtendedInfoManager = assetExtendedInfoManager;
            _assetIssuerManager = assetIssuerManager;
            _assetCategoryManager = assetCategoryManager;
            _assetsServiceHelper = assetsServiceHelper;
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
        [ProducesResponseType(typeof(Asset), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string assetId)
        {
            var asset = await _manager.TryGetAsync(assetId);

            if (asset == null)
            {
                return NotFound(ErrorResponse.Create(nameof(assetId), "Asset not found"));
            }

            return Ok(Asset.Create(asset));
        }

        /// <summary>
        /// Returns all assets
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Asset[]), (int)HttpStatusCode.OK)]
        [SwaggerOperation("GetAssets")]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _manager.GetAllAsync();

            return Ok(assets.Select(Asset.Create));
        }

        /// <summary>
        /// Returns asset attributes by ID
        /// </summary>
        /// <param name="assetId">Asset ID</param>
        [HttpGet("{assetId}/attributes")]
        [Produces("application/json", Type = typeof(AssetAttributes))]
        [ProducesResponseType(typeof(AssetAttributes), (int)HttpStatusCode.OK)]
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

            return Ok(Models.AssetAttributes.Create(assetId, assetAttributes != null ? assetAttributes.Attributes.ToArray() : new KeyValue[0]));
        }


        /// <summary>
        /// Returns asset attribute by asset ID and attribute key
        /// </summary>
        /// <param name="assetId">Asset ID</param>
        /// <param name="key">Attribute key</param>
        [HttpGet("{assetId}/attributes/{key}")]
        [Produces("application/json", Type = typeof(AssetAttributes))]
        [ProducesResponseType(typeof(AssetAttributes), (int)HttpStatusCode.OK)]
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
            return Ok(Models.AssetAttributes.Create(assetId, assetAttributes != null ? assetAttributes.Attributes.Where(a => a.Key == key).ToArray() : new KeyValue[0]));
        }

        /// <summary>
        /// Returns asset descriptions for array of assets
        /// </summary>
        /// <param name="request.Ids">Array asset ids</param>
        [HttpPost("description")]
        [Produces("application/json", Type = typeof(AssetDescription[]))]
        [ProducesResponseType(typeof(AssetDescription[]), (int)HttpStatusCode.OK)]
        [SwaggerOperation("GetAssetDescriptions")]
        public async Task<IActionResult> GetAssetDescriptions([FromBody]AssetDescriptionListRequest request)
        {
            var assets = ((await _manager.GetAllAsync()).Where(x => request == null || request.Ids == null || request.Ids.Contains(x.Id))).ToArray();
            List<Core.Domain.AssetDescription> res = new List<Core.Domain.AssetDescription>();

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

            return Ok(res.Select(AssetDescription.Create));
        }

        private async Task<Core.Domain.AssetDescription> GetAssetDescriptionAsync(IAsset asset)
        {
            var extendedInfo = await _assetExtendedInfoManager.TryGetAsync(asset.Id);
            if (extendedInfo == null) return null;

            var issuer = asset?.IdIssuer == null ? null : await _assetIssuerManager.TryGetAsync(asset.IdIssuer);
            var assetInfo = Core.Domain.AssetDescription.Create(extendedInfo, issuer);
            return assetInfo;
        }

        /// <summary>
        /// Returns all asset categories
        /// </summary>
        [HttpGet("categories")]
        [Produces("application/json", Type = typeof(AssetCategory[]))]
        [ProducesResponseType(typeof(AssetCategory[]), (int)HttpStatusCode.OK)]
        [SwaggerOperation("GetAssetCategories")]
        public async Task<IActionResult> GetAssetCategories()
        {         
            var assetCategories = await _assetCategoryManager.GetAllAsync();
            return Ok(assetCategories.Select(AssetCategory.Create));  // Ok(AssetCategories.Create(assetCategories));
        }


        /// <summary>
        /// Returns asset category for asset id
        /// </summary>
        /// <param name="assetId">Asset ID</param>
        [HttpGet("{assetId}/categories")]
        [Produces("application/json", Type = typeof(AssetCategory))]
        [ProducesResponseType(typeof(AssetCategory), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)] 
        [SwaggerOperation("GetAssetCategory")]
        public async Task<IActionResult> GetAssetCategories(string assetId)
        {
            var asset = await _manager.TryGetAsync(assetId);

            if (asset == null)
            {
                return NotFound(ErrorResponse.Create(nameof(assetId), "Asset not found")); //return NotFound(AssetCategories.Create(ErrorResponse.Create(nameof(assetId), "Asset not found")));
            }

            var assetCategory = await _assetCategoryManager.TryGetAsync(asset.CategoryId ?? "");

            if(assetCategory == null)
            {
                return NotFound(ErrorResponse.Create(nameof(assetId), "No asset category found")); //return NotFound(AssetCategories.Create(ErrorResponse.Create(nameof(assetId), "No asset category found")));
            }

            return Ok(AssetCategory.Create(assetCategory)); //return Ok(AssetCategories.Create(new List<IAssetCategory> { assetCategories }));
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

        private async Task<ExtendedAsset> GetAssetExtendedInfo(IAsset asset)
        {
            var assetDescription = await GetAssetDescriptionAsync(asset);
            var assetCategory = await _assetCategoryManager.TryGetAsync(asset.CategoryId ?? "") ?? new Core.Domain.AssetCategory();
            var assetAttributes = await _assetAttributesManager.TryGetAsync(asset.Id) ?? new Core.Domain.AssetAttributes { AssetId = asset.Id, Attributes = new List<KeyValue>() };

            return ExtendedAsset.Create(Asset.Create(asset), AssetDescription.Create(assetDescription ?? new Core.Domain.AssetDescription()), AssetCategory.Create(assetCategory ?? new Core.Domain.AssetCategory()), Models.AssetAttributes.Create(asset.Id, assetAttributes?.Attributes.ToArray() ?? new KeyValue[0]));
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

            return Ok(AssetExtendedResponseModel.Create(new List<ExtendedAsset> { assetExtended }));
        }

        [HttpGet("client")]
        [ProducesResponseType(typeof(Asset[]), (int)HttpStatusCode.OK)]
        [SwaggerOperation("GetAssetsForClient")]
        public async Task<IActionResult> GetAssetsForClient(string clientId, bool isIosDevice, string partnerId = null)
        {
            var result = await _assetsServiceHelper.GetAssetsForClient(clientId, isIosDevice, partnerId);
            return Ok(result.Select(Asset.Create));            
        }  
    }
}