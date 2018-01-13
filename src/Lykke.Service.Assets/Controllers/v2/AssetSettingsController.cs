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

        [HttpPost]
        [SwaggerOperation("AssetSettingsAdd")]
        [ProducesResponseType(typeof(AssetSettings), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody] AssetSettings settings)
        {
            settings = Mapper.Map<AssetSettings>(await _assetSettingsService.AddAsync(settings));

            return Created
            (
                uri:   $"api/v2/asset-settings/{settings.Asset}",
                value: settings
            );
        }

        [HttpGet("{assetId}/exists")]
        [SwaggerOperation("AssetSettingsExists")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Exists(string assetId)
        {
            var assetSettingsExists = await _assetSettingsService.GetAsync(assetId) != null;

            return Ok(assetSettingsExists);
        }

        [HttpGet("{assetId}")]
        [SwaggerOperation("AssetSettingsGet")]
        [ProducesResponseType(typeof(AssetSettings), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
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

        [HttpGet]
        [SwaggerOperation("AssetSettingsGetAll")]
        [ProducesResponseType(typeof(ListOf<AssetSettings>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var allSettings = await _assetSettingsService.GetAllAsync();
            var responseList = allSettings?.Select(Mapper.Map<AssetSettings>);

            return Ok(new ListOf<AssetSettings>
            {
                Items = responseList
            });
        }

        [HttpGet("__default")]
        [SwaggerOperation("AssetSettingsGetDefault")]
        [ProducesResponseType(typeof(AssetSettings), (int)HttpStatusCode.OK)]
        public IActionResult GetDefault()
        {
            var assetSettings = _assetSettingsService.CreateDefault();
            var response      = Mapper.Map<AssetSettings>(assetSettings);

            return Ok(response);
        }

        [HttpDelete("{assetId}")]
        [SwaggerOperation("AssetSettingsRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string assetId)
        {
            await _assetSettingsService.RemoveAsync(assetId);

            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("AssetSettingsUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] AssetSettings settings)
        {
            await _assetSettingsService.UpdateAsync(settings);

            return NoContent();
        }
    }
}
