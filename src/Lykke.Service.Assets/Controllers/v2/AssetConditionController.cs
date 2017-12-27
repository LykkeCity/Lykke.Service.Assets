using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses;
using Lykke.Service.Assets.Responses.v2;
using Lykke.Service.Assets.Services.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    [Route("api/v2/asset-conditions")]
    public class AssetConditionsController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly IAssetConditionService _assetConditionService;
        private readonly IAssetConditionSettingsService _assetConditionSettingsService;

        public AssetConditionsController(
            IAssetService assetService,
            IAssetConditionService assetConditionService,
            IAssetConditionSettingsService assetConditionSettingsService)
        {
            _assetService = assetService;
            _assetConditionService = assetConditionService;
            _assetConditionSettingsService = assetConditionSettingsService;
        }

        /// <summary>
        /// Gets all layers without assets conditions (only info about layers).
        /// </summary>
        /// <response code="200">The collection of layers.</response>
        [HttpGet("layer")]
        [SwaggerOperation("AssetConditionGetLayers")]
        [ProducesResponseType(typeof(List<AssetConditionLayerDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLayersAsync()
        {
            IEnumerable<IAssetConditionLayer> layers = await _assetConditionService.GetLayersAsync();

            var model = Mapper.Map<List<AssetConditionLayerDto>>(layers);

            return Ok(model);
        }

        /// <summary>
        /// Gets layer with assets conditions by specified id.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <response code="200">The layer with assets conditions.</response>
        /// <response code="404">Layer not found.</response>
        [HttpGet("layer/{layerId}")]
        [SwaggerOperation("AssetConditionGetLayerById")]
        [ProducesResponseType(typeof(AssetConditionLayerDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetLayerByIdAsync(string layerId)
        {
            IAssetConditionLayer layer = await _assetConditionService.GetLayerAsync(layerId);

            if (layer == null)
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));

            var model = Mapper.Map<AssetConditionLayerDto>(layer);

            return Ok(model);
        }

        /// <summary>
        /// Adds asset condition to layer.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="model">The model what describes asset condition.</param>
        /// <response code="204">Asset condition successfully added to layer.</response>
        /// <response code="400">Invalid model.</response>
        /// <response code="404">Layer or asset not found.</response>
        [HttpPut("layer/{layerId}")]
        [SwaggerOperation("AssetConditionAddAssetConditionToLayer")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAssetConditionLayerAsync(string layerId, [FromBody] AssetConditionDto model)
        {
            if (model == null)
                return BadRequest(ErrorResponse.Create("Asset condition required"));

            if (string.IsNullOrEmpty(model.Asset))
                return BadRequest(ErrorResponse.Create("Asset required"));

            IAsset asset = await _assetService.GetAsync(model.Asset);

            if (asset == null)
                return NotFound(ErrorResponse.Create($"asset '{model.Asset} not found"));

            IAssetConditionLayer layer = await _assetConditionService.GetLayerAsync(layerId);

            if (layer == null)
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));

            var condition = Mapper.Map<AssetCondition>(model);

            await _assetConditionService.AddAssetConditionAsync(layer.Id, condition);

            return NoContent();
        }

        /// <summary>
        /// Updates asset condition.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="model">The model that describes asset condition.</param>
        /// <response code="204">Asset condition successfully updated.</response>
        /// <response code="400">Invalid model.</response>
        /// <response code="404">Layer or asset not found.</response>
        [HttpPut("layer/{layerId}/asset/conditions")]
        [SwaggerOperation("AssetConditionUpdateAssetCondition")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAssetConditionAsync(string layerId, [FromBody] AssetConditionDto model)
        {
            if (model == null)
                return BadRequest(ErrorResponse.Create("Asset condition required"));

            if (string.IsNullOrEmpty(model.Asset))
                return BadRequest(ErrorResponse.Create("Asset required"));

            IAsset asset = await _assetService.GetAsync(model.Asset);

            if (asset == null)
                return NotFound(ErrorResponse.Create($"asset '{model.Asset} not found"));

            IAssetConditionLayer layer = await _assetConditionService.GetLayerAsync(layerId);

            if (layer == null)
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));

            var condition = Mapper.Map<AssetCondition>(model);

            await _assetConditionService.UpdateAssetConditionAsync(layer.Id, condition);

            return NoContent();
        }

        /// <summary>
        /// Deletes asset condition.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="asset">The asset.</param>
        /// <response code="204">Asset condition successfully updated.</response>
        [HttpDelete("layer/{layerId}/asset/{asset}")]
        [SwaggerOperation("AssetConditionDeleteAssetCondition")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAssetConditionAsync(string layerId, string asset)
        {
            await _assetConditionService.DeleteAssetConditionAsync(layerId, asset);

            return NoContent();
        }

        /// <summary>
        /// Adds new layer without assets.
        /// </summary>
        /// <remarks>
        /// Creates only layer without asset conditon list.
        /// After create need fill layers use method PutAssetConditionToLayers.
        /// </remarks>
        /// <param name="model">The model what describes layer.</param>
        /// <response code="204">Layer successfully added.</response>
        /// <response code="400">Invalid model.</response>
        [HttpPost("layer")]
        [SwaggerOperation("AssetConditionAddLayer")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddLayerAsync([FromBody] AssetConditionLayerDto model)
        {
            if (model == null)
            {
                return BadRequest(ErrorResponse.Create("Asset condition layer required"));
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                return BadRequest(ErrorResponse.Create("Asset condition layer id required"));
            }

            if (!this.ValidateKey(model.Id))
            {
                return BadRequest(ErrorResponse.Create($"Incorect layers name(id): {model.Id}"));
            }

            IAssetConditionLayer layer = await _assetConditionService.GetLayerAsync(model.Id);

            if (layer != null)
            {
                return BadRequest(ErrorResponse.Create($"Layer with id='{model.Id}' already exists"));
            }

            layer = Mapper.Map<AssetConditionLayer>(model);

            await _assetConditionService.AddLayerAsync(layer);

            return NoContent();
        }

        /// <summary>
        /// Updates layer without assets.
        /// </summary>
        /// <param name="model">The model what describes layer.</param>
        /// <response code="204">Layer successfully updated.</response>
        /// <response code="400">Invalid model.</response>
        /// <response code="404">Layer not found.</response>
        [HttpPut("layer")]
        [SwaggerOperation("AssetConditionUpdateLayer")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateLayerAsync([FromBody] AssetConditionLayerDto model)
        {
            if (model == null)
            {
                return BadRequest(ErrorResponse.Create("Asset condition layer required"));
            }

            if (string.IsNullOrEmpty(model.Id))
            {
                return BadRequest(ErrorResponse.Create("Asset condition layer id required"));
            }

            IAssetConditionLayer layer = await _assetConditionService.GetLayerAsync(model.Id);
            
            if (layer == null)
            {
                return NotFound(ErrorResponse.Create($"Layer with id='{model.Id}' not found"));
            }

            layer = Mapper.Map<AssetConditionLayer>(model);

            await _assetConditionService.UpdateLayerAsync(layer);

            return NoContent();
        }

        /// <summary>
        /// Deletes layer and links to clients.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <response code="204">Layer successfully deleted.</response>
        /// <response code="400">Invalid model.</response>
        [HttpDelete("layer/{layerId}")]
        [SwaggerOperation("AssetConditionDeleteLayer")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteLayerAsync(string layerId)
        {
            if (string.IsNullOrEmpty(layerId))
            {
                return BadRequest(ErrorResponse.Create("Layer id required"));
            }

            await _assetConditionService.DeleteLayerAsync(layerId);

            return NoContent();
        }

        /// <summary>
        /// Adds layer to client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="layerId">The layer id.</param>
        /// <response code="204">Layer successfully added to client.</response>
        /// <response code="400">Invalid model.</response>
        /// <response code="404">Layer not found.</response>
        [HttpPost("client/{clientId}/{layerId}")]
        [SwaggerOperation("AssetConditionAddLayerToClient")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddLayerToClientAsync(string clientId, string layerId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return BadRequest(ErrorResponse.Create("Client id required"));
            }

            if (!this.ValidateKey(layerId))
            {
                return BadRequest(ErrorResponse.Create($"Incorect layers name(id): {layerId}"));
            }

            IAssetConditionLayer layer = await _assetConditionService.GetLayerAsync(layerId);
            
            if (layer == null)
            {
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));
            }

            await _assetConditionService.AddClientLayerAsync(clientId, layer.Id);

            return NoContent();
        }

        /// <summary>
        /// Removes layer from client.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <param name="layerId">The layer id.</param>
        /// <response code="204">Layer successfully removed from client.</response>
        /// <response code="400">Invalid model.</response>
        [HttpDelete("client/{clientId}/{layerId}")]
        [SwaggerOperation("AssetConditionRemoveLayerFromClient")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveLayerFromClientAsync(string clientId, string layerId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return BadRequest(ErrorResponse.Create("Client id required"));
            }

            if (string.IsNullOrEmpty(layerId))
            {
                return BadRequest(ErrorResponse.Create("Layer id required"));
            }

            await _assetConditionService.RemoveClientLayerAsync(clientId, layerId);

            return NoContent();
        }

        /// <summary>
        /// Gets all layers by client id with assets conditions.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <response code="200">The collection of layers with assets conditions.</response>
        /// <response code="400">Invalid model.</response>
        [HttpGet("client/{clientId}")]
        [SwaggerOperation("AssetConditionGetLayersByClientId")]
        [ProducesResponseType(typeof(List<AssetConditionLayerDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetLayersByClientIdAsync(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return BadRequest(ErrorResponse.Create("Client id required"));
            }

            IEnumerable<IAssetConditionLayer> layers = await _assetConditionService.GetClientLayers(clientId);

            var model = Mapper.Map<List<AssetConditionLayerDto>>(layers);

            return Ok(model);
        }

        /// <summary>
        /// Returns default asset conditions layer.
        /// </summary>
        /// <response code="200">The model that describes default layer.</response>
        /// <response code="400">Invalid model.</response>
        [HttpGet("layer/default")]
        [SwaggerOperation("AssetConditionGetDefaultLayer")]
        [ProducesResponseType(typeof(Responses.v2.AssetConditionLayerSettings), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDefaultLayerAsync()
        {
            Task<IAssetConditionSettings> assetSettingsTask =
                _assetConditionSettingsService.GetConditionSettingsAsync();

            Task<IAssetConditionLayerSettings> layerSettingsTask =
                _assetConditionSettingsService.GetConditionLayerSettingsAsync();

            await Task.WhenAll(assetSettingsTask, layerSettingsTask);

            var model = new AssetConditionDefaultSettings
            {
                AssetSettings = Mapper.Map<Responses.v2.AssetConditionSettings>(assetSettingsTask.Result),
                LayerSettings = Mapper.Map<Responses.v2.AssetConditionLayerSettings>(layerSettingsTask.Result)
            };

            return Ok(model);
        }

        /// <summary>
        /// Updates default asset conditions layer.
        /// </summary>
        /// <param name="model">The model that describes default layer.</param>
        /// <response code="204">Default layer successfully updated.</response>
        /// <response code="400">Invalid model.</response>
        [HttpPost("layer/default")]
        [SwaggerOperation("AssetConditionUpdateDefaultLayer")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateDefaultLayerAsync([FromBody] AssetConditionDefaultSettings model)
        {
            if (model == null)
            {
                return BadRequest(ErrorResponse.Create("Asset condition layer required"));
            }

            var assetSettings = Mapper.Map<Services.Domain.AssetConditionSettings>(model.AssetSettings);
            var layerSettings = Mapper.Map<Services.Domain.AssetConditionLayerSettings>(model.LayerSettings);

            await _assetConditionSettingsService.UpdateAsync(layerSettings, assetSettings);

            return NoContent();
        }
    }
}
