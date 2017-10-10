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
    [Route("api/asset-categories")]
    public class AssetCategoryController : Controller
    {
        private readonly IAssetCategoryService _assetCategoryService;


        public AssetCategoryController(IAssetCategoryService assetCategoryService)
        {
            _assetCategoryService = assetCategoryService;
        }


        [HttpGet]
        [SwaggerOperation("AssetCategoryGetAll")]
        [ProducesResponseType(typeof(IEnumerable<AssetCategory>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var assetCategories = (await _assetCategoryService.GetAllAsync())
                .Select(Mapper.Map<AssetCategory>);
            
            return Ok(assetCategories);
        }
    }
}