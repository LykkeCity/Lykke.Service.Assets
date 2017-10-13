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
    [Route("api/v2/asset-extended-infos")]
    public class AssetExtendedInfoController : Controller
    {
        private readonly IAssetExtendedInfoService _assetExtendedInfoService;

        public AssetExtendedInfoController(
            IAssetExtendedInfoService assetExtendedInfoService)
        {
            _assetExtendedInfoService = assetExtendedInfoService;
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("AssetExtendedInfoRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            await _assetExtendedInfoService.RemoveAsync(id);

            return NoContent();
        }

        [HttpGet]
        [SwaggerOperation("AssetExtendedInfoGetAll")]
        [ProducesResponseType(typeof(IEnumerable<AssetExtendedInfo>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var assets = (await _assetExtendedInfoService.GetAllAsync())
                .Select(Mapper.Map<AssetExtendedInfo>);

            return Ok(assets);
        }

        [HttpGet("{id}")]
        [SwaggerOperation("AssetExtendedInfoGet")]
        [ProducesResponseType(typeof(AssetExtendedInfo), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string id)
        {
            var asset = await _assetExtendedInfoService.GetAsync(id);

            if (asset != null)
            {
                return Ok(Mapper.Map<AssetExtendedInfo>(asset));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/default")]
        [SwaggerOperation("AssetExtendedInfoGetDefault")]
        [ProducesResponseType(typeof(AssetExtendedInfo), (int) HttpStatusCode.OK)]
        public IActionResult GetDefault(string id)
        {
            var assetExtendedInfo = _assetExtendedInfoService.CreateDefault(id);

            return Ok(Mapper.Map<AssetExtendedInfo>(assetExtendedInfo));
        }

        [HttpGet("{id}/or-default")]
        [SwaggerOperation("AssetExtendedInfoGetOrGetDefault")]
        [ProducesResponseType(typeof(AssetExtendedInfo), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrGetDefault(string id)
        {
            var asset = await _assetExtendedInfoService.GetAsync(id);

            return asset != null 
                 ? Ok(Mapper.Map<AssetExtendedInfo>(asset))
                 : GetDefault(id);
        }

        [HttpPost]
        [SwaggerOperation("AssetExtendedInfoAdd")]
        [ProducesResponseType(typeof(AssetExtendedInfo), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] AssetExtendedInfo assetInfo)
        {
            assetInfo = Mapper.Map<AssetExtendedInfo>(await _assetExtendedInfoService.AddAsync(assetInfo));

            return Created
            (
                uri:   $"api/v2/asset-extended-infos/{assetInfo.Id}",
                value: assetInfo
            );
        }

        [HttpPut]
        [SwaggerOperation("AssetExtendedInfoUpdate")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Put([FromBody] AssetExtendedInfo assetInfo)
        {
            await _assetExtendedInfoService.UpdateAsync(assetInfo);

            return NoContent();
        }
    }
}