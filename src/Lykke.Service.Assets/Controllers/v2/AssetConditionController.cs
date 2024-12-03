using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Requests.v2.AssetConditions;
using Lykke.Service.Assets.Responses;
using Lykke.Service.Assets.Responses.v2.AssetConditions;
using Lykke.Service.Assets.Services.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    [ApiController]
    [Route("api/v2/asset-conditions")]
    public class AssetConditionsController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly IAssetConditionService _assetConditionService;

        public AssetConditionsController(
            IAssetService assetService,
            IAssetConditionService assetConditionService)
        {
            _assetService = assetService;
            _assetConditionService = assetConditionService;
        }

        /// <summary>
        /// Gets all layers without assets conditions.
        /// </summary>
        /// <response code="200">The collection of layers.</response>
        [HttpGet("layer")]
        [SwaggerOperation("AssetConditionGetLayers")]
        [ProducesResponseType(typeof(List<AssetConditionLayerModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetLayersAsync()
        {
            var layers = await _assetConditionService.GetLayersAsync();

            var model = Mapper.Map<List<AssetConditionLayerModel>>(layers);

            return Ok(model);
        }

        /// <summary>
        /// Gets layer with assets conditions.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <response code="200">The layer with assets conditions.</response>
        /// <response code="404">Layer not found.</response>
        [HttpGet("layer/{layerId}")]
        [SwaggerOperation("AssetConditionGetLayerById")]
        [ProducesResponseType(typeof(AssetConditionLayerModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetLayerByIdAsync(string layerId)
        {
            var layer = await _assetConditionService.GetLayerAsync(layerId);

            if (layer == null)
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));

            var model = Mapper.Map<AssetConditionLayerModel>(layer);

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
        [HttpPost("layer/{layerId}/condition")]
        [SwaggerOperation("AssetConditionAddAssetCondition")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAssetConditionAsync(string layerId, [FromBody] EditAssetConditionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            var asset = await _assetService.GetAsync(model.Asset);

            if (asset == null)
                return NotFound(ErrorResponse.Create($"asset '{model.Asset} not found"));

            var layer = await _assetConditionService.GetLayerAsync(layerId);
            var defaultLayer = await _assetConditionService.GetDefaultLayerAsync();

            if (layer == null && defaultLayer.Id != layerId)
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));

            if (layer != null && layer.AssetConditions.Any(o => o.Asset == model.Asset) ||
                layer == null && defaultLayer.AssetConditions.Any(o => o.Asset == model.Asset))
                return NotFound(ErrorResponse.Create("Asset condition already exists"));

            var condition = Mapper.Map<AssetCondition>(model);

            await _assetConditionService.AddAssetConditionAsync(layerId, condition);

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
        [HttpPut("layer/{layerId}/condition")]
        [SwaggerOperation("AssetConditionUpdateAssetCondition")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAssetConditionAsync(string layerId, [FromBody] EditAssetConditionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            var asset = await _assetService.GetAsync(model.Asset);

            if (asset == null)
                return NotFound(ErrorResponse.Create($"asset '{model.Asset} not found"));

            var layer = await _assetConditionService.GetLayerAsync(layerId);
            var defaultLayer = await _assetConditionService.GetDefaultLayerAsync();

            if (layer == null && defaultLayer.Id != layerId)
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));

            if (layer != null && layer.AssetConditions.All(o => o.Asset != model.Asset) ||
                layer == null && defaultLayer.AssetConditions.All(o => o.Asset != model.Asset))
                return NotFound(ErrorResponse.Create("Asset condition does not exists"));

            var condition = Mapper.Map<AssetCondition>(model);

            await _assetConditionService.UpdateAssetConditionAsync(layerId, condition);

            return NoContent();
        }

        /// <summary>
        /// Deletes asset condition.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="asset">The asset.</param>
        /// <response code="204">Asset condition successfully updated.</response>
        [HttpDelete("layer/{layerId}/condition/{asset}")]
        [SwaggerOperation("AssetConditionDeleteAssetCondition")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAssetConditionAsync(string layerId, string asset)
        {
            await _assetConditionService.DeleteAssetConditionAsync(layerId, asset);

            return NoContent();
        }

        /// <summary>
        /// Adds default asset condition to layer.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="model">The model what describes default asset condition.</param>
        /// <response code="204">Default asset condition successfully added to layer.</response>
        /// <response code="400">Invalid model.</response>
        /// <response code="404">Layer not found.</response>
        [HttpPost("layer/{layerId}/condition/default")]
        [SwaggerOperation("AssetConditionAddDefaultAssetCondition")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddDefaultAssetConditionAsync(string layerId, [FromBody] EditAssetDefaultConditionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            var layer = await _assetConditionService.GetLayerAsync(layerId);

            if (layer == null)
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));

            if (layer.AssetDefaultCondition != null)
                return BadRequest(ErrorResponse.Create("Default asset conditions already exists."));

            var condition = Mapper.Map<AssetDefaultCondition>(model);

            await _assetConditionService.AddDefaultAssetConditionAsync(layer.Id, condition);

            return NoContent();
        }

        /// <summary>
        /// Updates layer default asset condition.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="model">The model that describes default asset condition.</param>
        /// <response code="204">Default asset condition successfully updated.</response>
        /// <response code="400">Invalid model.</response>
        /// <response code="404">Layer not found.</response>
        [HttpPut("layer/{layerId}/condition/default")]
        [SwaggerOperation("AssetConditionUpdateDefaultAssetCondition")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateDefaultAssetConditionAsync(string layerId, [FromBody] EditAssetDefaultConditionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            var layer = await _assetConditionService.GetLayerAsync(layerId);

            if (layer == null)
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));

            if (layer.AssetDefaultCondition == null)
                return BadRequest(ErrorResponse.Create("Default asset conditions does not exists."));

            var condition = Mapper.Map<AssetDefaultCondition>(model);

            await _assetConditionService.UpdateDefaultAssetConditionAsync(layer.Id, condition);

            return NoContent();
        }

        /// <summary>
        /// Deletes layer default asset condition.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <response code="204">Default asset condition successfully deleted.</response>
        [HttpDelete("layer/{layerId}/condition/default")]
        [SwaggerOperation("AssetConditionDeleteDefaultAssetCondition")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteDefaultAssetConditionAsync(string layerId)
        {
            await _assetConditionService.DeleteDefaultAssetConditionAsync(layerId);

            return NoContent();
        }

        /// <summary>
        /// Adds new conditons layer.
        /// </summary>
        /// <param name="model">The model what describes layer.</param>
        /// <response code="204">Layer successfully added.</response>
        /// <response code="400">Invalid model.</response>
        [HttpPost("layer")]
        [SwaggerOperation("AssetConditionAddLayer")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddLayerAsync([FromBody] EditAssetConditionLayerModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            var layer = await _assetConditionService.GetLayerAsync(model.Id);

            if (layer != null)
            {
                return BadRequest(ErrorResponse.Create($"Layer with id='{model.Id}' already exists"));
            }

            layer = Mapper.Map<AssetConditionLayer>(model);

            await _assetConditionService.AddLayerAsync(layer);

            return NoContent();
        }

        /// <summary>
        /// Updates conditons layer.
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
        public async Task<IActionResult> UpdateLayerAsync([FromBody] EditAssetConditionLayerModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            var layer = await _assetConditionService.GetLayerAsync(model.Id);

            if (layer == null)
            {
                return NotFound(ErrorResponse.Create($"Layer with id='{model.Id}' not found"));
            }

            layer = Mapper.Map<AssetConditionLayer>(model);

            await _assetConditionService.UpdateLayerAsync(layer);

            return NoContent();
        }

        /// <summary>
        /// Deletes conditons layer.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <response code="204">Layer successfully deleted.</response>
        [HttpDelete("layer/{layerId}")]
        [SwaggerOperation("AssetConditionDeleteLayer")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteLayerAsync(string layerId)
        {
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
            if (!this.ValidateKey(layerId))
            {
                return BadRequest(ErrorResponse.Create($"Incorect layers name(id): {layerId}"));
            }

            var layer = await _assetConditionService.GetLayerAsync(layerId);

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
        [ProducesResponseType(typeof(List<AssetConditionLayerModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetLayersByClientIdAsync(string clientId)
        {
            var layers = await _assetConditionService.GetClientLayers(clientId);

            var model = Mapper.Map<List<AssetConditionLayerModel>>(layers);

            return Ok(model);
        }

        /// <summary>
        /// Returns default conditions layer with asset conditions.
        /// </summary>
        /// <response code="200">The default conditions layer with asset conditions.</response>
        [HttpGet("layer/default")]
        [SwaggerOperation("AssetConditionGetDefaultLayer")]
        [ProducesResponseType(typeof(AssetDefaultConditionLayerModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDefaultLayerAsync()
        {
            var defaultLayer = await _assetConditionService.GetDefaultLayerAsync();

            var model = Mapper.Map<AssetDefaultConditionLayerModel>(defaultLayer);

            return Ok(model);
        }

        /// <summary>
        /// Updates default asset conditions layer.
        /// </summary>
        /// <param name="model">The model that describes default layer.</param>
        /// <response code="204">Default layer successfully updated.</response>
        /// <response code="400">Invalid model.</response>
        [HttpPut("layer/default")]
        [SwaggerOperation("AssetConditionUpdateDefaultLayer")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateDefaultLayerAsync([FromBody] EditAssetDefaultConditionLayerModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorResponse.Create(ModelState));
            }

            var settings = Mapper.Map<AssetDefaultConditionLayer>(model);

            await _assetConditionService.UpdateDefaultLayerAsync(settings);

            return NoContent();
        }
    }
}
