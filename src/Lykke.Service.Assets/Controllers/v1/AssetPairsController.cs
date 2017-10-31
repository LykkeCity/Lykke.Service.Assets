using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses;
using Lykke.Service.Assets.Responses.v1;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V1
{
    /// <inheritdoc />
    /// <summary>
    /// Controller for asset pairs
    /// </summary>
    [Obsolete]
    [Route("api/[controller]")]
    public class AssetPairsController : Controller
    {
        private readonly IAssetPairService  _assetPairService;
        private readonly ICache<IAssetPair> _cache;

        public AssetPairsController(
            IAssetPairService assetPairService,
            ICache<IAssetPair> cache)
        {
            _assetPairService = assetPairService;
            _cache            = cache;
        }

        /// <summary>
        /// Forcibly updates asset pairs cache
        /// </summary>
        /// <returns></returns>
        [HttpPost("updateCache"), Obsolete]
        [SwaggerOperation("UpdateAssetPairsCache")]
        public async Task UpdateCache()
        {
            await _cache.InvalidateAsync();
        }

        /// <summary>
        /// Returns asset pair by ID
        /// </summary>
        /// <param name="assetPairId">Asset pair ID</param>
        [HttpGet("{assetPairId}"), Obsolete]
        [SwaggerOperation("GetAssetPair")]
        [ProducesResponseType(typeof(AssetPairResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string assetPairId)
        {
            var assetPair = await _cache.GetAsync(assetPairId, () => _assetPairService.GetAsync(assetPairId));

            if (assetPair == null)
            {
                return NotFound(ErrorResponse.Create(nameof(assetPairId), "Asset pair not found"));
            }

            return Ok(AssetPairResponseModel.Create(assetPair));
        }

        /// <summary>
        /// Returns all asset pairs
        /// </summary>
        [HttpGet, Obsolete]
        [ProducesResponseType(typeof(AssetPairResponseModel[]), (int)HttpStatusCode.OK)]
        [SwaggerOperation("GetAssetPairs")]
        public async Task<IActionResult> GetAll()
        {
            var assetPairs = await _cache.GetListAsync("All", () => _assetPairService.GetAllAsync());

            return Ok(assetPairs.Select(AssetPairResponseModel.Create));
        }
    }
}
