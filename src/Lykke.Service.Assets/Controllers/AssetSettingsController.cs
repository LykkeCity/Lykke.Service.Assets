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
    /// <inheritdoc />
    /// <summary>
    ///     Controller for asset settings
    /// </summary>
    [Route("api/v2/asset-settings")]
    public class AssetSettingsController : Controller
    {
        private readonly IAssetSettingsService _assetSettingsService;

        public AssetSettingsController(IAssetSettingsService assetSettingsService)
        {
            _assetSettingsService = assetSettingsService;
        }

        [HttpDelete("{assetId}")]
        [SwaggerOperation("AssetSettingsDelete")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsync(string assetId)
        {
            await _assetSettingsService.RemoveAsync(assetId);

            return NoContent();
        }

        [HttpGet]
        [SwaggerOperation("AssetSettingsGetAll")]
        [ProducesResponseType(typeof(ListResponse<AssetSettings>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get()
        {
            var allSettings = await _assetSettingsService.GetAllAsync();
            var responseList = allSettings?.Select(Mapper.Map<AssetSettings>);

            return Ok(new ListResponse<AssetSettings>
            {
                List = responseList
            });
        }

        [HttpGet("{assetId}")]
        [SwaggerOperation("AssetSettingsGet")]
        [ProducesResponseType(typeof(AssetSettings), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string assetId)
        {
            var settings = await _assetSettingsService.GetAsync(assetId);

            if (settings != null)
            {
                return Ok(Mapper.Map<AssetSettings>(settings));
            }
            else
            {

                return NotFound();
            }
        }

        [HttpPost]
        [SwaggerOperation("AssetSettingsAdd")]
        [ProducesResponseType(typeof(AssetSettings), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Post([FromBody] AssetSettings settings)
        {
            settings = Mapper.Map<AssetSettings>(await _assetSettingsService.AddAsync(settings));

            return Created
            (
                uri: $"api/v2/asset-settings/{settings.Asset}",
                value: settings
            );
        }

        [HttpPut]
        [SwaggerOperation("AssetSettingsUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Put([FromBody] AssetSettings settings)
        {
            await _assetSettingsService.UpdateAsync(settings);

            return NoContent();
        }
    }
}