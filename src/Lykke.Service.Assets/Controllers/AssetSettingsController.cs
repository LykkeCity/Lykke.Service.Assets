using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Models;
using Lykke.Service.Assets.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Controllers
{
    /// <inheritdoc />
    /// <summary>
    ///     Controller for asset settings
    /// </summary>
    [Route("api/[controller]")]
    public class AssetSettingsController : Controller
    {
        private readonly IAssetSettingsService _assetSettingsService;

        public AssetSettingsController(IAssetSettingsService assetSettingsService)
        {
            _assetSettingsService = assetSettingsService;
        }

        [HttpGet]
        [SwaggerOperation("GetAllAsync")]
        [ProducesResponseType(typeof(ListResponse<AssetSettings>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllAsync()
        {
            var allSettings = await _assetSettingsService.GetAllAsync();
            var responseList = allSettings?.Select(x => Mapper.Map<AssetSettings>(x));

            return Ok(new ListResponse<AssetSettings>()
            {
                List = responseList
            });
        }

        [HttpPost]
        [SwaggerOperation("AddAsync")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddAsync([FromBody] AssetSettings settings)
        {
            await _assetSettingsService.AddAsync(settings);

            return Ok();
        }

        [HttpPut]
        [SwaggerOperation("UpdateAsync")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateAsync([FromBody] AssetSettings settings)
        {
            await _assetSettingsService.UpdateAsync(settings);

            return Ok();
        }

        [HttpGet("{assetId}")]
        [SwaggerOperation("GetAsync")]
        [ProducesResponseType(typeof(AssetSettings), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync([FromRoute] string assetId)
        {
            var setting = await _assetSettingsService.GetAsync(assetId);
            var response = Mapper.Map<AssetSettings>(setting);

            return Ok(response);
        }

        [HttpDelete("{assetId}")]
        [SwaggerOperation("GetAsync")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAsync([FromRoute] string assetId)
        {
            await _assetSettingsService.RemoveAsync(assetId);

            return Ok();
        }
    }
}