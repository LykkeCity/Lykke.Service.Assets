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
    [Route("/api/v2/margin-assets")]
    public class MarginAssetsController : Controller
    {
        private readonly IMarginAssetService _marginAssetService;

        public MarginAssetsController(
            IMarginAssetService marginAssetService)
        {
            _marginAssetService = marginAssetService;
        }

        [HttpPost]
        [SwaggerOperation("MarginAssetAdd")]
        [ProducesResponseType(typeof(MarginAsset), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody] MarginAsset marginAsset)
        {
            marginAsset = Mapper.Map<MarginAsset>(await _marginAssetService.AddAsync(marginAsset));

            return Created
            (
                uri: $"/api/v2/margin-assets/{marginAsset.Id}",
                value: marginAsset
            );
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("MarginAssetExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Exists(string id)
        {
            var marginAssetPairExists = await _marginAssetService.GetAsync(id) != null;

            return Ok(marginAssetPairExists);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("MarginAssetGet")]
        [ProducesResponseType(typeof(MarginAsset), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var marginAsset = await _marginAssetService.GetAsync(id);
            if (marginAsset == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<MarginAsset>(marginAsset));
        }

        [HttpGet]
        [SwaggerOperation("MarginAssetGetAll")]
        [ProducesResponseType(typeof(IEnumerable<MarginAsset>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var marginAssets = (await _marginAssetService.GetAllAsync())
                .Select(Mapper.Map<MarginAsset>);

            return Ok(marginAssets);
        }

        [HttpGet("__default")]
        [SwaggerOperation("MarginAssetGetDefault")]
        [ProducesResponseType(typeof(MarginAsset), (int)HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var marginAssetPair = _marginAssetService.CreateDefault();

            return Ok(Mapper.Map<MarginAsset>(marginAssetPair));
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("MarginAssetRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string id)
        {
            await _marginAssetService.RemoveAsync(id);

            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("MarginAssetUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] MarginAsset assetCategory)
        {
            await _marginAssetService.UpdateAsync(assetCategory);

            return NoContent();
        }
    }
}
