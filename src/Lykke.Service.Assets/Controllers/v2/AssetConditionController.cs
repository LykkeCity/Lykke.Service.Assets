using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Requests.V2;
using Lykke.Service.Assets.Responses;
using Lykke.Service.Assets.Responses.v2;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    [Route("api/v2/asset-conditions")]
    public class AssetConditionsController : Controller
    {
        private readonly IAssetService _assetService;
        private readonly IAssetConditionService _assetConditionService;
        private readonly IAssetConditionDefaultLayerService _assetConditionDefaultLayerService;

        public AssetConditionsController(
            IAssetService assetService,
            IAssetConditionService assetConditionService,
            IAssetConditionDefaultLayerService assetConditionDefaultLayerService)
        {
            _assetService = assetService;
            _assetConditionService = assetConditionService;
            _assetConditionDefaultLayerService = assetConditionDefaultLayerService;
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
            IReadOnlyList<IAssetConditionLayer> layers = await _assetConditionService.GetLayersAsync();

            List<AssetConditionLayerDto> result = layers
                .Select(e => new AssetConditionLayerDto(e.Id, e.Priority, e.Description, e.ClientsCanCashInViaBankCards, e.SwiftDepositEnabled, e.AssetConditions))
                .ToList();

            return Ok(result);
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
            {
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));
            }

            var result = new AssetConditionLayerDto(layer.Id, layer.Priority, layer.Description, layer.ClientsCanCashInViaBankCards, layer.SwiftDepositEnabled, layer.AssetConditions);

            return Ok(result);
        }

        /// <summary>
        /// Adds asset condition to layer.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="assetCondition">The model what describes asset condition.</param>
        /// <response code="204">Asset condition successfully added to layer.</response>
        /// <response code="400">Invalid model.</response>
        /// <response code="404">Layer or asset not found.</response>
        [HttpPut("layer/{layerId}")]
        [SwaggerOperation("AssetConditionAddAssetConditionToLayer")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAssetConditionToLayerAsync(string layerId, [FromBody] AssetConditionDto assetCondition)
        {
            if (assetCondition == null)
            {
                return BadRequest(ErrorResponse.Create("Asset condition required"));
            }

            if (string.IsNullOrEmpty(assetCondition.Asset))
            {
                return BadRequest(ErrorResponse.Create("Asset required"));
            }

            IAsset asset = await _assetService.GetAsync(assetCondition.Asset);

            if (asset == null)
            {
                return NotFound(ErrorResponse.Create($"asset '{assetCondition.Asset} not found"));
            }

            IAssetConditionLayer layer = await _assetConditionService.GetLayerAsync(layerId);

            if (layer == null)
            {
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));
            }

            await _assetConditionService.AddAssetConditionAsync(layer.Id, assetCondition);
            
            return NoContent();
        }

        /// <summary>
        /// Updates asset condition.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="assetCondition">The model that describes asset condition.</param>
        /// <response code="204">Asset condition successfully updated.</response>
        /// <response code="400">Invalid model.</response>
        /// <response code="404">Layer or asset not found.</response>
        [HttpPut("layer/{layerId}/asset/conditions")]
        [SwaggerOperation("AssetConditionUpdateAssetCondition")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAssetConditionAsync(string layerId, [FromBody] AssetConditionDto assetCondition)
        {
            if (assetCondition == null)
            {
                return BadRequest(ErrorResponse.Create("Asset condition required"));
            }

            if (string.IsNullOrEmpty(assetCondition.Asset))
            {
                return BadRequest(ErrorResponse.Create("Asset required"));
            }

            IAsset asset = await _assetService.GetAsync(assetCondition.Asset);

            if (asset == null)
            {
                return NotFound(ErrorResponse.Create($"asset '{assetCondition.Asset} not found"));
            }

            IAssetConditionLayer layer = await _assetConditionService.GetLayerAsync(layerId);

            if (layer == null)
            {
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));
            }

            await _assetConditionService.AddAssetConditionAsync(layer.Id, assetCondition);

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
        /// <param name="assetConditionLayer">The model what describes layer.</param>
        /// <response code="204">Layer successfully added.</response>
        /// <response code="400">Invalid model.</response>
        [HttpPost("layer")]
        [SwaggerOperation("AssetConditionAddLayer")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddLayerAsync([FromBody] AssetConditionLayerRequestDto assetConditionLayer)
        {
            if (assetConditionLayer == null)
            {
                return BadRequest(ErrorResponse.Create("Asset condition layer required"));
            }

            if (string.IsNullOrEmpty(assetConditionLayer.Id))
            {
                return BadRequest(ErrorResponse.Create("Asset condition layer id required"));
            }

            if (!this.ValidateKey(assetConditionLayer.Id))
            {
                return BadRequest(ErrorResponse.Create($"Incorect layers name(id): {assetConditionLayer.Id}"));
            }

            IAssetConditionLayer layer = await _assetConditionService.GetLayerAsync(assetConditionLayer.Id);

            if (layer != null)
            {
                return BadRequest(ErrorResponse.Create($"Layer with id='{assetConditionLayer.Id}' already exists"));
            }

            await _assetConditionService.AddLayerAsync(assetConditionLayer);

            return NoContent();
        }

        /// <summary>
        /// Updates layer without assets.
        /// </summary>
        /// <param name="assetConditionLayer">The model what describes layer.</param>
        /// <response code="204">Layer successfully updated.</response>
        /// <response code="400">Invalid model.</response>
        /// <response code="404">Layer not found.</response>
        [HttpPut("layer")]
        [SwaggerOperation("AssetConditionUpdateLayer")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateLayerAsync([FromBody] AssetConditionLayerRequestDto assetConditionLayer)
        {
            if (assetConditionLayer == null)
            {
                return BadRequest(ErrorResponse.Create("Asset condition layer required"));
            }

            if (string.IsNullOrEmpty(assetConditionLayer.Id))
            {
                return BadRequest(ErrorResponse.Create("Asset condition layer id required"));
            }

            IAssetConditionLayer layer = await _assetConditionService.GetLayerAsync(assetConditionLayer.Id);
            
            if (layer == null)
            {
                return NotFound(ErrorResponse.Create($"Layer with id='{assetConditionLayer.Id}' not found"));
            }

            await _assetConditionService.UpdateLayerAsync(assetConditionLayer);

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

            IReadOnlyList<IAssetConditionLayer> layers = await _assetConditionService.GetClientLayers(clientId);

            IEnumerable<AssetConditionLayerDto> result = layers
                .Select(e => new AssetConditionLayerDto(e.Id, e.Priority, e.Description, e.ClientsCanCashInViaBankCards, e.SwiftDepositEnabled, e.AssetConditions))
                .ToList();

            return Ok(result);
        }

        /// <summary>
        /// Returns default asset conditions layer.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">The model that describes default layer.</response>
        /// <response code="400">Invalid model.</response>
        [HttpGet("layer/default")]
        [SwaggerOperation("AssetConditionGetDefaultLayer")]
        [ProducesResponseType(typeof(AssetConditionDefaultLayerDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDefaultLayerAsync()
        {
            IAssetConditionDefaultLayer defaultLayer = await _assetConditionDefaultLayerService.GetAsync();

            var model = new AssetConditionDefaultLayerDto
            {
                SwiftDepositEnabled = defaultLayer?.SwiftDepositEnabled,
                ClientsCanCashInViaBankCards = defaultLayer?.ClientsCanCashInViaBankCards,
                Regulation = defaultLayer?.Regulation,
                AvailableToClient = defaultLayer?.AvailableToClient
            };

            return Ok(model);
        }

        /// <summary>
        /// Updates default asset conditions layer.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="model">The model that describes default layer.</param>
        /// <response code="204">Default layer successfully updated.</response>
        /// <response code="400">Invalid model.</response>
        [HttpPost("layer/default")]
        [SwaggerOperation("AssetConditionUpdateDefaultLayer")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateDefaultLayerAsync([FromBody] AssetConditionDefaultLayerDto model)
        {
            if (model == null)
            {
                return BadRequest(ErrorResponse.Create("Asset condition layer required"));
            }

            await _assetConditionDefaultLayerService.InsertOrUpdateAsync(model);

            return NoContent();
        }
    }
}
