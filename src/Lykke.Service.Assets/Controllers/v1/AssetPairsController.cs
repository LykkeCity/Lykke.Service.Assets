using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Managers;
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
        private readonly IAssetPairManager  _assetPairManager;


        public AssetPairsController(
            IAssetPairManager assetPairManager)
        {
            _assetPairManager = assetPairManager;
        }

        /// <summary>
        /// Forcibly updates asset pairs cache
        /// </summary>
        /// <returns></returns>
        [HttpPost("updateCache"), Obsolete]
        [SwaggerOperation("UpdateAssetPairsCache")]
        public async Task UpdateCache()
        {
            await _assetPairManager.InvalidateCache();
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
            var assetPair = await _assetPairManager.GetAsync(assetPairId);

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
            var assetPairs = await _assetPairManager.GetAllAsync();

            return Ok(assetPairs.Select(AssetPairResponseModel.Create));
        }
    }
}
