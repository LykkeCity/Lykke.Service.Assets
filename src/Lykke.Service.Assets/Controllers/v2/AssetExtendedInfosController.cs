using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    [Route("api/v2/asset-extended-infos")]
    public class AssetExtendedInfosController : Controller
    {
        private readonly IAssetExtendedInfoService _assetExtendedInfoService;

        public AssetExtendedInfosController(
            IAssetExtendedInfoService assetExtendedInfoService)
        {
            _assetExtendedInfoService = assetExtendedInfoService;
        }

        [HttpPost]
        [SwaggerOperation("AssetExtendedInfoAdd")]
        [ProducesResponseType(typeof(AssetExtendedInfo), (int) HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody] AssetExtendedInfo assetInfo)
        {
            assetInfo = Mapper.Map<AssetExtendedInfo>(await _assetExtendedInfoService.AddAsync(assetInfo));

            return Created
            (
                uri:   $"api/v2/asset-extended-infos/{assetInfo.Id}",
                value: assetInfo
            );
        }

        [HttpGet("{id}/exists")]
        [SwaggerOperation("AssetExtendedInfoExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Exists(string id)
        {
            var extendedInfoExists = await _assetExtendedInfoService.GetAsync(id) != null;

            return Ok(extendedInfoExists);
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

        [HttpGet]
        [SwaggerOperation("AssetExtendedInfoGetAll")]
        [ProducesResponseType(typeof(IEnumerable<AssetExtendedInfo>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var assets = (await _assetExtendedInfoService.GetAllAsync())
                .Select(Mapper.Map<AssetExtendedInfo>);

            return Ok(assets);
        }

        [HttpGet("__default")]
        [SwaggerOperation("AssetExtendedInfoGetDefault")]
        [ProducesResponseType(typeof(AssetExtendedInfo), (int) HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var assetExtendedInfo = _assetExtendedInfoService.CreateDefault();

            return Ok(Mapper.Map<AssetExtendedInfo>(assetExtendedInfo));
        }


        [HttpDelete("{id}")]
        [SwaggerOperation("AssetExtendedInfoRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string id)
        {
            await _assetExtendedInfoService.RemoveAsync(id);

            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("AssetExtendedInfoUpdate")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] AssetExtendedInfo assetInfo)
        {
            await _assetExtendedInfoService.UpdateAsync(assetInfo);

            return NoContent();
        }
    }
}