using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    [Route("/api/v2/margin-asset-pairs")]
    public class MarginAssetPairController : Controller
    {
        private readonly IMarginAssetPairService _marginAssetPairService;


        public MarginAssetPairController(
            IMarginAssetPairService marginAssetPairService)
        {
            _marginAssetPairService = marginAssetPairService;
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("MarginAssetPairRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            await _marginAssetPairService.RemoveAsync(id);

            return NoContent();
        }

        [HttpGet]
        [SwaggerOperation("MarginAssetPairGetAll")]
        [ProducesResponseType(typeof(IEnumerable<MarginAssetPair>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var assetCategories = (await _marginAssetPairService.GetAllAsync())
                .Select(Mapper.Map<MarginAssetPair>);

            return Ok(assetCategories);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("MarginAssetPairGet")]
        [ProducesResponseType(typeof(MarginAssetPair), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var marginAssetPair = await _marginAssetPairService.GetAsync(id);

            if (marginAssetPair != null)
            {
                return Ok(Mapper.Map<MarginAssetPair>(marginAssetPair));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("default")]
        [SwaggerOperation("MarginAssetPairGetDefault")]
        [ProducesResponseType(typeof(MarginAssetPair), (int)HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var marginAssetPair = _marginAssetPairService.CreateDefault();

            return Ok(Mapper.Map<MarginAssetPair>(marginAssetPair));
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("MarginAssetPairExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExists(string id)
        {
            var marginAssetPairExists = await _marginAssetPairService.GetAsync(id) != null;

            return Ok(marginAssetPairExists);
        }

        [HttpPost]
        [SwaggerOperation("MarginAssetPairAdd")]
        [ProducesResponseType(typeof(MarginAssetPair), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] MarginAssetPair marginAssetPair)
        {
            marginAssetPair = Mapper.Map<MarginAssetPair>(await _marginAssetPairService.AddAsync(marginAssetPair));

            return Created
            (
                uri:   $"/api/v2/margin-asset-pairs/{marginAssetPair.Id}",
                value: marginAssetPair
            );
        }

        [HttpPut]
        [SwaggerOperation("MarginAssetPairUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Put([FromBody] MarginAssetPair assetCategory)
        {
            await _marginAssetPairService.UpdateAsync(assetCategory);

            return NoContent();
        }
    }
}