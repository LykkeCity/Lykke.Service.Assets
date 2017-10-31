using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    [Route("api/v2/asset-pairs")]
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

        [HttpPost]
        [SwaggerOperation("AssetPairAdd")]
        [ProducesResponseType(typeof(AssetPair), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody] AssetPair assetPair)
        {
            await _cache.InvalidateAsync();

            assetPair = Mapper.Map<AssetPair>(await _assetPairService.AddAsync(assetPair));

            return Created
            (
                uri:   $"api/v2/asset-pairs/{assetPair.Id}",
                value: assetPair
            );
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("AssetPairExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Exists(string id)
        {
            var assetPairExists = await _cache.GetAsync(id, () => _assetPairService.GetAsync(id));
            
            return Ok(assetPairExists != null);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("AssetPairGet")]
        [ProducesResponseType(typeof(AssetPair), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var assetPair = await _cache.GetAsync(id, () => _assetPairService.GetAsync(id));
            
            if (assetPair != null)
            {
                return Ok(Mapper.Map<AssetPair>(assetPair));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [SwaggerOperation("AssetPairGetAll")]
        [ProducesResponseType(typeof(IEnumerable<AssetPair>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var assetPairs = (await _cache.GetListAsync("All", () => _assetPairService.GetAllAsync()))
                .Select(Mapper.Map<AssetPair>);

            return Ok(assetPairs);
        }

        [HttpGet("__default")]
        [SwaggerOperation("AssetPairGetDefault")]
        [ProducesResponseType(typeof(AssetPair), (int) HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var assetPair = _assetPairService.CreateDefault();

            return Ok(Mapper.Map<AssetPair>(assetPair));
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("AssetPairRemove")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string id)
        {
            await _cache.InvalidateAsync();

            await _assetPairService.RemoveAsync(id);

            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("AssetPairUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] AssetPair assetPair)
        {
            await _cache.InvalidateAsync();

            await _assetPairService.UpdateAsync(assetPair);

            return NoContent();
        }
    }
}
