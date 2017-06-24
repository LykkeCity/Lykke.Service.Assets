using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers
{
    /// <summary>
    /// Controller for asset pairs
    /// </summary>
    [Route("api/[controller]")]
    public class AssetPairsController : Controller
    {
        private readonly IAssetPairsManager _manager;

        public AssetPairsController(IAssetPairsManager manager)
        {
            _manager = manager;
        }

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

        [HttpGet]
        [ProducesResponseType(typeof(AssetPairResponseModel[]), (int)HttpStatusCode.OK)]
        [SwaggerOperation("GetAssetPairs")]
        public async Task<IActionResult> GetAll()
        {
            var assetPairs = await _manager.GetAllAsync();

            return Ok(assetPairs.Select(AssetPairResponseModel.Create));
        }
    }
}