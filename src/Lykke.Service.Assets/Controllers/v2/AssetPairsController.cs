using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Managers;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Assets.Controllers.V2
{
    [Route("api/v2/asset-pairs")]
    public class AssetPairsController : Controller
    {
        private readonly IAssetPairManager  _assetPairManager;


        public AssetPairsController(
            IAssetPairManager assetPairManager)
        {
            _assetPairManager = assetPairManager;
        }

        [HttpPost]
        [SwaggerOperation("AssetPairAdd")]
        [ProducesResponseType(typeof(AssetPair), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody] AssetPair assetPair)
        {
            assetPair = Mapper.Map<AssetPair>(await _assetPairManager.AddAsync(assetPair));

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
            var assetPairExists = await _assetPairManager.GetAsync(id);
            
            return Ok(assetPairExists != null);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("AssetPairGet")]
        [ProducesResponseType(typeof(AssetPair), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var assetPair = await _assetPairManager.GetAsync(id);
            
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
            var assetPairs = (await _assetPairManager.GetAllAsync())
                .Select(Mapper.Map<AssetPair>);

            return Ok(assetPairs);
        }

        [HttpGet("__default")]
        [SwaggerOperation("AssetPairGetDefault")]
        [ProducesResponseType(typeof(AssetPair), (int) HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var assetPair = _assetPairManager.CreateDefault();

            return Ok(Mapper.Map<AssetPair>(assetPair));
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("AssetPairRemove")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string id)
        {
            await _assetPairManager.RemoveAsync(id);

            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("AssetPairUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] AssetPair assetPair)
        {
            await _assetPairManager.UpdateAsync(assetPair);

            return NoContent();
        }
    }
}
