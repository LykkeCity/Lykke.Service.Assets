using System.Collections.Generic;
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
    [Route("api/v2/asset-categories")]
    public class AssetCategoryController : Controller
    {
        private readonly IAssetCategoryService _assetCategoryService;


        public AssetCategoryController(IAssetCategoryService assetCategoryService)
        {
            _assetCategoryService = assetCategoryService;
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("AssetCategoryRemove")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            await _assetCategoryService.RemoveAsync(id);

            return NoContent();
        }

        [HttpGet]
        [SwaggerOperation("AssetCategoryGetAll")]
        [ProducesResponseType(typeof(IEnumerable<AssetCategory>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var assetCategories = (await _assetCategoryService.GetAllAsync())
                .Select(Mapper.Map<AssetCategory>);
            
            return Ok(assetCategories);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("AssetCategoryGet")]
        [ProducesResponseType(typeof(AssetCategory), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var assetCategory = await _assetCategoryService.GetAsync(id);

            if (assetCategory != null)
            {
                return Ok(Mapper.Map<AssetCategory>(assetCategory));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [SwaggerOperation("AssetCategoryAdd")]
        [ProducesResponseType(typeof(AssetCategory), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] AssetCategory assetCategory)
        {
            assetCategory = Mapper.Map<AssetCategory>(await _assetCategoryService.AddAsync(assetCategory));

            return Created
            (
                uri:   $"api/v2/asset-categories/{assetCategory.Id}",
                value: assetCategory
            );
        }

        [HttpPut]
        [SwaggerOperation("AssetCategoryUpdate")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Put([FromBody] AssetCategory assetCategory)
        {
            await _assetCategoryService.UpdateAsync(assetCategory);

            return NoContent();
        }
    }
}