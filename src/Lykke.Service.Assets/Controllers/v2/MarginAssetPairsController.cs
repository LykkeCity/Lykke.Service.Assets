using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    [ApiController]
    [Route("/api/v2/margin-asset-pairs")]
    public class MarginAssetPairsController : Controller
    {
        private readonly IMarginAssetPairService _marginAssetPairService;

        public MarginAssetPairsController(
            IMarginAssetPairService marginAssetPairService)
        {
            _marginAssetPairService = marginAssetPairService;
        }

        [HttpPost]
        [SwaggerOperation("MarginAssetPairAdd")]
        [ProducesResponseType(typeof(MarginAssetPair), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody] MarginAssetPair marginAssetPair)
        {
            marginAssetPair = Mapper.Map<MarginAssetPair>(await _marginAssetPairService.AddAsync(marginAssetPair));

            return Created
            (
                uri: $"/api/v2/margin-asset-pairs/{marginAssetPair.Id}",
                value: marginAssetPair
            );
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("MarginAssetPairExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Exists(string id)
        {
            var marginAssetPairExists = await _marginAssetPairService.GetAsync(id) != null;

            return Ok(marginAssetPairExists);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("MarginAssetPairGet")]
        [ProducesResponseType(typeof(MarginAssetPair), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var marginAssetPair = await _marginAssetPairService.GetAsync(id);
            if (marginAssetPair == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<MarginAssetPair>(marginAssetPair));
        }

        [HttpGet]
        [SwaggerOperation("MarginAssetPairGetAll")]
        [ProducesResponseType(typeof(IEnumerable<MarginAssetPair>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var assetCategories = (await _marginAssetPairService.GetAllAsync())
                .Select(Mapper.Map<MarginAssetPair>);

            return Ok(assetCategories);
        }

        [HttpGet("__default")]
        [SwaggerOperation("MarginAssetPairGetDefault")]
        [ProducesResponseType(typeof(MarginAssetPair), (int)HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var marginAssetPair = _marginAssetPairService.CreateDefault();

            return Ok(Mapper.Map<MarginAssetPair>(marginAssetPair));
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("MarginAssetPairRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string id)
        {
            await _marginAssetPairService.RemoveAsync(id);

            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("MarginAssetPairUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] MarginAssetPair assetCategory)
        {
            await _marginAssetPairService.UpdateAsync(assetCategory);

            return NoContent();
        }
    }
}
