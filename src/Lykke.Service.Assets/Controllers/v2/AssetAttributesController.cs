using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Assets.Controllers.V2
{
    [ApiController]
    [Route("api/v2/asset-attributes")]
    public class AssetAttributesController : Controller
    {
        private readonly IAssetAttributeService _assetAttributeService;


        public AssetAttributesController(
            IAssetAttributeService assetAttributeService)
        {
            _assetAttributeService = assetAttributeService;
        }

        [HttpPost("{assetId}")]
        [SwaggerOperation("AssetAttributeAdd")]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<IActionResult> Add(string assetId, [FromBody] AssetAttribute attribute)
        {
            attribute = Mapper.Map<AssetAttribute>(await _assetAttributeService.AddAsync(assetId, attribute));

            return Created
            (
                uri:   $"api/v2/asset-attributes/{assetId}/{attribute.Key}",
                value: attribute
            );
        }

        [HttpGet("{assetId}/{key}")]
        [SwaggerOperation("AssetAttributeGet")]
        [ProducesResponseType(typeof(AssetAttribute), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string assetId, string key)
        {
            var assetAttribute = await _assetAttributeService.GetAsync(assetId, key);
            if (assetAttribute != null)
            {
                return Ok(Mapper.Map<AssetAttribute>(assetAttribute));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [SwaggerOperation("AssetAttributeGetAll")]
        [ProducesResponseType(typeof(IEnumerable<AssetAttributes>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var assetAttributes = (await _assetAttributeService.GetAllAsync())
                .Select(Mapper.Map<AssetAttributes>);

            return Ok(assetAttributes);
        }

        [HttpGet("{assetId}")]
        [SwaggerOperation("AssetAttributeGetAllForAsset")]
        [ProducesResponseType(typeof(AssetAttributes), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllForAsset(string assetId)
        {
            var assetAttributes = (await _assetAttributeService.GetAllAsync(assetId)).FirstOrDefault();
            if (assetAttributes != null)
            {
                return Ok(Mapper.Map<AssetAttributes>(assetAttributes));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{assetId}/{key}")]
        [SwaggerOperation("AssetAttributeRemove")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string assetId, string key)
        {
            await _assetAttributeService.RemoveAsync(assetId, key);

            return NoContent();
        }

        [HttpPut("{assetId}")]
        [SwaggerOperation("AssetAttributeUpdate")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update(string assetId, [FromBody] AssetAttribute attribute)
        {
            await _assetAttributeService.UpdateAsync(assetId, attribute);

            return NoContent();
        }
    }
}
