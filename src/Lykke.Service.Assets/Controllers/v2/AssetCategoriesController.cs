using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lykke.Service.Assets.Controllers.V2
{
    [ApiController]
    [Route("api/v2/asset-categories")]
    public class AssetCategoriesController : Controller
    {
        private readonly ICachedAssetCategoryService _assetCategoryService;


        public AssetCategoriesController(
            ICachedAssetCategoryService assetCategoryService)
        {
            _assetCategoryService = assetCategoryService;
        }

        [HttpPost]
        [SwaggerOperation("AssetCategoryAdd")]
        [ProducesResponseType(typeof(AssetCategory), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody] AssetCategory assetCategory)
        {
            assetCategory = await _assetCategoryService.AddAsync(assetCategory);

            return Created
            (
                uri: $"api/v2/asset-categories/{assetCategory.Id}",
                value: assetCategory
            );
        }

        [HttpGet("{id}")]
        [SwaggerOperation("AssetCategoryGet")]
        [ProducesResponseType(typeof(AssetCategory), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var assetCategory = await _assetCategoryService.GetAsync(id);

            if (assetCategory != null)
            {
                return Ok(assetCategory);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [SwaggerOperation("AssetCategoryGetAll")]
        [ProducesResponseType(typeof(IEnumerable<AssetCategory>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var assetCategories = await _assetCategoryService.GetAllAsync();

            return Ok(assetCategories);
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("AssetCategoryRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string id)
        {
            await _assetCategoryService.RemoveAsync(id);

            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("AssetCategoryUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] AssetCategory assetCategory)
        {
            await _assetCategoryService.UpdateAsync(assetCategory);

            return NoContent();
        }
    }
}
