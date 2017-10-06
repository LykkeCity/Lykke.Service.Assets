using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Models;
using Lykke.Service.Assets.Repositories.Extensions;
using Lykke.Service.Assets.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;


namespace Lykke.Service.Assets.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Controller for asset pairs
    /// </summary>
    [Route("api/[controller]")]
    public class AssetPairsController : Controller
    {
        private readonly IAssetsServiceHelper           _assetsServiceHelper;
        private readonly IDictionaryManager<IAssetPair> _manager;


        public AssetPairsController(
            IAssetsServiceHelper assetsServiceHelper,
            IDictionaryManager<IAssetPair> manager)
        {
            _assetsServiceHelper = assetsServiceHelper;
            _manager             = manager;
        }


        /// <summary>
        ///     Forcibly updates asset pairs cache
        /// </summary>
        [HttpPost("updateCache")]
        [SwaggerOperation("UpdateAssetPairsCache")]
        public async Task UpdateCache()
        {
            await _manager.UpdateCacheAsync();
        }

        /// <summary>
        ///     Returns asset pair by id
        /// </summary>
        /// <param name="assetPairId">
        ///    Asset pair id
        /// </param>
        [HttpGet("{assetPairId}")]
        [SwaggerOperation("GetAssetPair")]
        [ProducesResponseType(typeof(AssetPair),     (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAssetPair(string assetPairId)
        {
            var assetPair = await _manager.TryGetAsync(assetPairId);
            if (assetPair == null)
            {
                return NotFound(ErrorResponse.Create(nameof(assetPairId), "Asset pair not found"));
            }

            return Ok(AssetPair.Create(assetPair));
        }

        /// <summary>
        ///     Returns all asset pairs
        /// </summary>
        [HttpGet]
        [SwaggerOperation("GetAssetPairs")]
        [ProducesResponseType(typeof(IEnumerable<AssetPair>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAssetPairs()
        {
            var assetPairs = await _manager.GetAllAsync();
            var result     = assetPairs.Select(AssetPair.Create);

            return Ok(result);
        }

        [HttpPost("client")]
        [SwaggerOperation("GetAssetsPairsForClient")]
        [ProducesResponseType(typeof(AssetPair[]), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAssetsPairsForClient([FromBody] AssetPairListForClientRequest request)
        {
            var assets   = await _assetsServiceHelper.GetAssetsForClient(request.ClientId, request.IsIosDevice, request.PartnerId);
            var assetIds = assets.Select(x => x.Id).ToArray();

            var result = (await _manager.GetAllAsync())
                .Where(x => !x.IsDisabled)
                .WhichConsistsOfAssets(assetIds)
                .Select(AssetPair.Create);

            return Ok(result);
        }
    }
}