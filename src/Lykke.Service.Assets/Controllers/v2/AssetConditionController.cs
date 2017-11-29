using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Requests.v2;
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

        public AssetConditionsController(IAssetConditionLayerRepository assetConditionLayerRepository,
            IAssetRepository assetRepository, IAssetConditionLayerLinkClientRepository assetConditionLayerLinkClientRepository)
        {
            _assetConditionLayerRepository = assetConditionLayerRepository;
            _assetRepository = assetRepository;
            _assetConditionLayerLinkClientRepository = assetConditionLayerLinkClientRepository;
        }

        /// <summary>
        /// Get all layers without list of assets.
        /// Return only info about layer
        /// </summary>
        [HttpGet("layer")]
        [SwaggerOperation("AssetConditionLayerGetAll")]
        [ProducesResponseType(typeof(List<AssetConditionLayerDto>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetLayersAsync()
        {
            var layers = await _assetConditionLayerRepository.GetLayerListAsync();
            var result = layers.Select(e => new AssetConditionLayerDto(e.Id, e.Priority, e.Description, e.ClientsCanCashInViaBankCards, e.SwiftDepositEnabled)).ToList();
            return Ok(result);
        }

        /// <summary>
        /// Get layer with list of assets.
        /// </summary>
        [HttpGet("layer/{layerId}")]
        [SwaggerOperation("AssetConditionLayerGetById")]
        [ProducesResponseType(typeof(AssetConditionLayerDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetLayersByIdAsync(string layerId)
        {
            var layer = (await _assetConditionLayerRepository.GetByIdsAsync(new[] {layerId})).FirstOrDefault();
            if (layer == null)
            {
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));
            }

            var result = new AssetConditionLayerDto(layer.Id, layer.Priority, layer.Description, layer.ClientsCanCashInViaBankCards, layer.SwiftDepositEnabled);
            result.AssetConditions.AddRange(layer.AssetConditions.Values.Select(e => new AssetConditionDto(e.Asset, e.AvailableToClient)));

            return Ok(result);
        }

        /// <summary>
        /// Set asset condition in layer
        /// </summary>
        [HttpPut("layer/{layerId}")]
        [SwaggerOperation("PutAssetConditionToLayers")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutAssetConditionToLayersAsync(string layerId,
            [FromBody] AssetConditionDto assetCondition)
        {
            if (assetCondition == null)
            {
                return BadRequest(ErrorResponse.Create("assetCondition id null"));
            }

            var asset = await _assetRepository.GetAsync(assetCondition.Asset);
            if (asset == null)
            {
                return NotFound(ErrorResponse.Create($"asset '{assetCondition.Asset} not found"));
            }

            var layer = (await _assetConditionLayerRepository.GetByIdsAsync(new[] {layerId})).FirstOrDefault();
            if (layer == null)
            {
                return NotFound(ErrorResponse.Create($"Layer with id='{layerId}' not found"));
            }

            await _assetConditionLayerRepository.InsertOrUpdateAssetConditionsToLayer(layer.Id, assetCondition);

            return Ok();
        }

        /// <summary>
        /// Create asset condions Layer
        /// Create only layer without asset conditon list.
        /// After create need fill layers use method PutAssetConditionToLayers
        /// </summary>
        [HttpPost("layer")]
        [SwaggerOperation("CreateAssetConditionLayer")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateLayersAsync([FromBody] AssetConditionLayerRequestDto assetConditionLayer)
        {
            if (assetConditionLayer == null)
            {
                return BadRequest(ErrorResponse.Create("assetConditionLayer id null"));
            }

            if (string.IsNullOrEmpty(assetConditionLayer.Id))
            {
                return BadRequest(ErrorResponse.Create("Cannot add layer with empty id"));
            }

            var layer = (await _assetConditionLayerRepository.GetByIdsAsync(new[] { assetConditionLayer.Id })).FirstOrDefault();
            if (layer != null)
            {
                return BadRequest(ErrorResponse.Create($"Layer with id='{assetConditionLayer.Id}' already exists"));
            }

            await _assetConditionLayerRepository.InsetLayerAsync(assetConditionLayer);

            return Ok();
        }

        /// <summary>
        /// Update asset condions Layer 
        /// Update only layer without asset conditon list
        /// </summary>
        [HttpPut("layer")]
        [SwaggerOperation("UpdateAssetConditionLayer")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateLayersAsync([FromBody] AssetConditionLayerRequestDto assetConditionLayer)
        {
            if (assetConditionLayer == null)
            {
                return BadRequest(ErrorResponse.Create("assetConditionLayer id null"));
            }

            if (string.IsNullOrEmpty(assetConditionLayer.Id))
            {
                return BadRequest(ErrorResponse.Create("Cannot add layer with empty id"));
            }

            var layer = (await _assetConditionLayerRepository.GetByIdsAsync(new[] { assetConditionLayer.Id })).FirstOrDefault();
            if (layer == null)
            {
                return BadRequest(ErrorResponse.Create($"Layer with id='{assetConditionLayer.Id}' not found"));
            }

            await _assetConditionLayerRepository.UpdateLayerAsync(assetConditionLayer);

            return Ok();
        }


        /// <summary>
        /// Delete asset condition Layer.
        /// With layer we delete all link from clients to this layer
        /// </summary>
        [HttpDelete("layer/{layerId}")]
        [SwaggerOperation("DeleteAssetConditionToLayers")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAssetConditionToLayersAsync(string layerId)
        {
            if (string.IsNullOrEmpty(layerId))
            {
                return BadRequest(ErrorResponse.Create("layerId is null"));
            }

            await _assetConditionLayerLinkClientRepository.RemoveLayerFromClientsAsync(layerId);

            await _assetConditionLayerRepository.DeleteLayerAsync(layerId);
            
            return Ok();
        }


        /// <summary>
        /// Set asset condition Layer to client
        /// </summary>
        [HttpPost("client/{clientId}/{layerId}")]
        [SwaggerOperation("SetAssetConditionLayerToClient")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SetAssetConditionLayerToClientAsync(string clientId, string layerId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return BadRequest(ErrorResponse.Create("ClientId is null"));
            }

            var layer = (await _assetConditionLayerRepository.GetByIdsAsync(new[] { layerId })).FirstOrDefault();
            if (layer == null)
            {
                return BadRequest(ErrorResponse.Create($"Layer with id='{layerId}' not found"));
            }

            await _assetConditionLayerLinkClientRepository.AddAsync(clientId, layer.Id);

            return Ok();
        }

        /// <summary>
        /// Remove asset condition Layer from client
        /// </summary>
        [HttpDelete("client/{clientId}/{layerId}")]
        [SwaggerOperation("RemoveAssetConditionLayerFrom")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveAssetConditionLayerFromClientAsync(string clientId, string layerId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return BadRequest(ErrorResponse.Create("ClientId is null"));
            }

            if (string.IsNullOrEmpty(layerId))
            {
                return BadRequest(ErrorResponse.Create("layerId is null"));
            }

            await _assetConditionLayerLinkClientRepository.RemoveAsync(clientId, layerId);

            return Ok();
        }

        /// <summary>
        /// Get asset condition Layers by Client
        /// </summary>
        [HttpGet("client/{clientId}")]
        [SwaggerOperation("RemoveAssetConditionLayerFrom")]
        [ProducesResponseType(typeof(List<AssetConditionLayerDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAssetConditionLayersByClientAsync(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return BadRequest(ErrorResponse.Create("ClientId is null"));
            }
            var layerIds = await _assetConditionLayerLinkClientRepository.GetAllLayersByClientAsync(clientId);
            var layers = await _assetConditionLayerRepository.GetByIdsAsync(layerIds);
            var result = layers.Select(e => new AssetConditionLayerDto(e.Id, e.Priority, e.Description, e.ClientsCanCashInViaBankCards, e.SwiftDepositEnabled)).ToList();
            return Ok(result);
        }
    }

}
