using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using Lykke.Service.Assets.Services;

namespace Lykke.Service.Assets.Controllers
{
    /// <summary>
    /// Controller for asset pairs
    /// </summary>
    [Route("api/[controller]")]
    public class AssetPairsController : Controller
    {
        private readonly IDictionaryManager<IAssetPair> _manager;
        private readonly IAssetsServiceHelper _assetsServiceHelper;

        public AssetPairsController(IDictionaryManager<IAssetPair> manager, IAssetsServiceHelper assetsServiceHelper)
        {
            _manager = manager;
            _assetsServiceHelper = assetsServiceHelper;
        }

        /// <summary>
        /// Forcibly updates asset pairs cache
        /// </summary>
        /// <returns></returns>
        [HttpPost("updateCache")]
        [SwaggerOperation("UpdateAssetPairsCache")]
        public async Task UpdateCache()
        {
            await _manager.UpdateCacheAsync();
        }

        /// <summary>
        /// Returns asset pair by ID
        /// </summary>
        /// <param name="assetPairId">Asset pair ID</param>
        [HttpGet("{assetPairId}")]
        [SwaggerOperation("GetAssetPair")]
        [ProducesResponseType(typeof(AssetPairResponseModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string assetPairId)
        {
            var assetPair = await _manager.TryGetAsync(assetPairId);

            if (assetPair == null)
            {
                return NotFound(ErrorResponse.Create(nameof(assetPairId), "Asset pair not found"));
            }

            return Ok(AssetPairResponseModel.Create(assetPair));
        }

        /// <summary>
        /// Returns all asset pairs
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(AssetPairResponseModel[]), (int)HttpStatusCode.OK)]
        [SwaggerOperation("GetAssetPairs")]
        public async Task<IActionResult> GetAll()
        {
            var assetPairs = await _manager.GetAllAsync();

            return Ok(assetPairs.Select(AssetPairResponseModel.Create));
        }

        [HttpPost("client")]
        [ProducesResponseType(typeof(AssetPairResponseModel[]), (int)HttpStatusCode.OK)]
        [SwaggerOperation("GetAssetsPairsForClient")]
        public async Task<IActionResult> GetAssetsPairsForClient([FromBody]GetAssetPairsForClientRequestModel request)
        {
            var assetsForClient = await _assetsServiceHelper.GetAssetsForClient(request.ClientId, request.IsIosDevice, request.PartnerId); 

            var result = (await _manager.GetAllAsync()).Where(x => !x.IsDisabled);

            //if (!ignoreBase)
            //    result = result.WhichHaveAssets((await _assetsServiceHelper.GetBaseAssetForClient(clientId, isIosDevice, partnerId)).Id); 

            return Ok(result.WhichConsistsOfAssets(assetsForClient.Select(x => x.Id).ToArray()).Select(AssetPairResponseModel.Create));
        }
    }
}