using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers
{
    [Route("/api/v2/margin-assets")]
    public class MarginAssetController : Controller
    {
        private readonly IMarginAssetService _marginAssetService;


        public MarginAssetController(
            IMarginAssetService marginAssetService)
        {
            _marginAssetService = marginAssetService;
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("MarginAssetRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            await _marginAssetService.RemoveAsync(id);

            return NoContent();
        }

        [HttpGet]
        [SwaggerOperation("MarginAssetGetAll")]
        [ProducesResponseType(typeof(IEnumerable<MarginAsset>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var marginAssets = (await _marginAssetService.GetAllAsync())
                .Select(Mapper.Map<MarginAsset>);

            return Ok(marginAssets);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("MarginAssetGet")]
        [ProducesResponseType(typeof(MarginAsset), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var marginAsset = await _marginAssetService.GetAsync(id);

            if (marginAsset != null)
            {
                return Ok(Mapper.Map<MarginAsset>(marginAsset));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("default")]
        [SwaggerOperation("MarginAssetGetDefault")]
        [ProducesResponseType(typeof(MarginAsset), (int)HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var marginAssetPair = _marginAssetService.CreateDefault();

            return Ok(Mapper.Map<MarginAsset>(marginAssetPair));
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("MarginAssetExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetExists(string id)
        {
            var marginAssetPairExists = await _marginAssetService.GetAsync(id) != null;

            return Ok(marginAssetPairExists);
        }

        [HttpPost]
        [SwaggerOperation("MarginAssetAdd")]
        [ProducesResponseType(typeof(MarginAsset), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] MarginAsset marginAsset)
        {
            marginAsset = Mapper.Map<MarginAsset>(await _marginAssetService.AddAsync(marginAsset));

            return Created
            (
                uri:   $"/api/v2/margin-assets/{marginAsset.Id}",
                value: marginAsset
            );
        }

        [HttpPut]
        [SwaggerOperation("MarginAssetUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Put([FromBody] MarginAsset assetCategory)
        {
            await _marginAssetService.UpdateAsync(assetCategory);

            return NoContent();
        }
    }
}