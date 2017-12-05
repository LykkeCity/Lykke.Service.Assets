using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Requests.V2;
using Lykke.Service.Assets.Responses;
using Lykke.Service.Assets.Responses.V2;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    [Route("api/v2/asset-conditions")]
    public class AssetConditionsController : Controller
    {
        private readonly IAssetConditionLayerRepository _assetConditionLayerRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetConditionLayerLinkClientRepository _assetConditionLayerLinkClientRepository;

        public AssetConditionsController(
            IAssetConditionLayerRepository assetConditionLayerRepository,
            IAssetRepository assetRepository,
            IAssetConditionLayerLinkClientRepository assetConditionLayerLinkClientRepository)
        {
            _assetConditionLayerRepository = assetConditionLayerRepository;
            _assetRepository = assetRepository;
            _assetConditionLayerLinkClientRepository = assetConditionLayerLinkClientRepository;
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
            IReadOnlyList<IAssetConditionLayer> layers = await _assetConditionLayerRepository.GetLayerListAsync();

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
            IAssetConditionLayer layer = (await _assetConditionLayerRepository.GetByIdsAsync(new[] { layerId }))
                .FirstOrDefault();

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

            IAsset asset = await _assetRepository.GetAsync(assetCondition.Asset);

            if (asset == null)
            {
                return NotFound(ErrorResponse.Create($"asset '{assetCondition.Asset} not found"));
            }

            IAssetConditionLayer layer = (await _assetConditionLayerRepository.GetByIdsAsync(new[] { layerId })).FirstOrDefault();

            if (layer == null)
            {
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));
            }

            await _assetConditionLayerRepository.InsertOrUpdateAssetConditionsToLayer(layer.Id, assetCondition);

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

            IAssetConditionLayer layer = (await _assetConditionLayerRepository.GetByIdsAsync(new[] { assetConditionLayer.Id })).FirstOrDefault();

            if (layer != null)
            {
                return BadRequest(ErrorResponse.Create($"Layer with id='{assetConditionLayer.Id}' already exists"));
            }

            await _assetConditionLayerRepository.InsetLayerAsync(assetConditionLayer);

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

            IAssetConditionLayer layer = (await _assetConditionLayerRepository.GetByIdsAsync(new[] { assetConditionLayer.Id })).FirstOrDefault();

            if (layer == null)
            {
                return NotFound(ErrorResponse.Create($"Layer with id='{assetConditionLayer.Id}' not found"));
            }

            await _assetConditionLayerRepository.UpdateLayerAsync(assetConditionLayer);

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

            await _assetConditionLayerLinkClientRepository.RemoveLayerFromClientsAsync(layerId);

            await _assetConditionLayerRepository.DeleteLayerAsync(layerId);

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

            IAssetConditionLayer layer = (await _assetConditionLayerRepository.GetByIdsAsync(new[] { layerId }))
                .FirstOrDefault();

            if (layer == null)
            {
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));
            }

            await _assetConditionLayerLinkClientRepository.AddAsync(clientId, layer.Id);

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

            await _assetConditionLayerLinkClientRepository.RemoveAsync(clientId, layerId);

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

            IReadOnlyList<string> layerIds = await _assetConditionLayerLinkClientRepository.GetAllLayersByClientAsync(clientId);

            IReadOnlyList<IAssetConditionLayer> layers = await _assetConditionLayerRepository.GetByIdsAsync(layerIds);

            IEnumerable<AssetConditionLayerDto> result = layers
                .Select(e => new AssetConditionLayerDto(e.Id, e.Priority, e.Description, e.ClientsCanCashInViaBankCards, e.SwiftDepositEnabled, e.AssetConditions))
                .ToList();

            return Ok(result);
        }
    }
}
