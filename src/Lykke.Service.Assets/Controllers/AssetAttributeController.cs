using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers
{
    [Route("api/asset-attributes")]
    public class AssetAttributeController : Controller
    {
        private readonly IAssetAttributeService _assetAttributeService;


        public AssetAttributeController(
            IAssetAttributeService assetAttributeService)
        {
            _assetAttributeService = assetAttributeService;
        }

        [HttpPost("{assetId}")]
        [SwaggerOperation("AssetAttributeAdd")]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<IActionResult> Post(string assetId, [FromBody] AssetAttribute attribute)
        {
            return Created
            (
                uri:   $"api/assets/{assetId}/attribute/{attribute.Key}",
                value: await _assetAttributeService.AddAsync(assetId, attribute)
            );
        }

        [HttpGet]
        [SwaggerOperation("AssetAttributeGetAll")]
        [ProducesResponseType(typeof(AssetAttributes), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var assetAttributes = (await _assetAttributeService.GetAllAsync())
                .Select(Mapper.Map<AssetAttributes>);

            return Ok(assetAttributes);
        }

        [HttpGet("{assetId}")]
        [SwaggerOperation("AssetAttributeGetAllForAsset")]
        [ProducesResponseType(typeof(AssetAttributes), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string assetId)
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
        
        [HttpDelete("{assetId}/{key}")]
        [SwaggerOperation("AssetAttributeRemove")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(string assetId, string key)
        {
            await _assetAttributeService.RemoveAsync(assetId, key);

            return NoContent();
        }

        [HttpPut("{assetId}")]
        [SwaggerOperation("AssetAttributeUpdate")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Put(string assetId, [FromBody] AssetAttribute attribute)
        {
            await _assetAttributeService.UpdateAsync(assetId, attribute);

            return NoContent();
        }
    }
}