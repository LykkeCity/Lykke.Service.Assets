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
    [Route("api/v2/asset-categories")]
    public class AssetCategoriesController : Controller
    {
        private readonly IAssetCategoryManager _assetCategoryManager;


        public AssetCategoriesController(
            IAssetCategoryManager assetCategoryManager)
        {
            _assetCategoryManager = assetCategoryManager;
        }

        [HttpPost]
        [SwaggerOperation("AssetCategoryAdd")]
        [ProducesResponseType(typeof(AssetCategory), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody] AssetCategory assetCategory)
        {
            assetCategory = Mapper.Map<AssetCategory>(await _assetCategoryManager.AddAsync(assetCategory));

            return Created
            (
                uri:   $"api/v2/asset-categories/{assetCategory.Id}",
                value: assetCategory
            );
        }

        [HttpGet("{id}")]
        [SwaggerOperation("AssetCategoryGet")]
        [ProducesResponseType(typeof(AssetCategory), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var assetCategory = await _assetCategoryManager.GetAsync(id);

            if (assetCategory != null)
            {
                return Ok(Mapper.Map<AssetCategory>(assetCategory));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [SwaggerOperation("AssetCategoryGetAll")]
        [ProducesResponseType(typeof(IEnumerable<AssetCategory>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var assetCategories = (await _assetCategoryManager.GetAllAsync())
                .Select(Mapper.Map<AssetCategory>);
            
            return Ok(assetCategories);
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("AssetCategoryRemove")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string id)
        {
            await _assetCategoryManager.RemoveAsync(id);

            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("AssetCategoryUpdate")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] AssetCategory assetCategory)
        {
            await _assetCategoryManager.UpdateAsync(assetCategory);

            return NoContent();
        }
    }
}
