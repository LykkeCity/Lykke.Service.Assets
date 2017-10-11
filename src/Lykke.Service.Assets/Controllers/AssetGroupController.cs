using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers
{
    [Route("/api/v2/asset-groups")]
    public class AssetGroupController : Controller
    {
        private readonly IAssetGroupService _assetGroupService;


        public AssetGroupController(
            IAssetGroupService assetGroupService)
        {
            _assetGroupService = assetGroupService;
        }

        [HttpPost("{groupName}/assets/{assetId}")]
        [SwaggerOperation("AssetGroupAddAsset")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> AddAsset(string assetId, string groupName)
        {
            await _assetGroupService.AddAssetToGroupAsync(assetId, groupName);

            return NoContent();
        }

        [HttpPost("{groupName}/clients/{clientId}")]
        [SwaggerOperation("AssetGroupAddClient")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> AddClient(string clientId, string groupName)
        {
            await _assetGroupService.AddClientToGroupAsync(clientId, groupName);
        
            return NoContent();
        }

        [HttpDelete("{groupName}/assets/{assetId}")]
        [SwaggerOperation("AssetGroupRemoveAsset")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteAsset(string assetId, string groupName)
        {
            await _assetGroupService.RemoveAssetFromGroupAsync(assetId, groupName);

            return NoContent();
        }

        [HttpDelete("{groupName}/clients/{clientId}")]
        [SwaggerOperation("AssetGroupRemoveClient")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteClient(string clientId, string groupName)
        {
            await _assetGroupService.RemoveClientFromGroupAsync(clientId, groupName);
        
            return NoContent();
        }

        [HttpDelete("{groupName}")]
        [SwaggerOperation("AssetGroupRemove")]
        [ProducesResponseType((int) HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteGroup(string groupName)
        {
            await _assetGroupService.RemoveGroupAsync(groupName);

            return NoContent();
        }

        [HttpGet("{groupName}/asset-ids")]
        [SwaggerOperation("AssetGroupGetAssetIds")]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAssetIds(string groupName)
        {
            var assetIds = await _assetGroupService.GetAssetIdsForGroupAsync(groupName);

            return Ok(assetIds);
        }

        [HttpGet("{groupName}/client-ids")]
        [SwaggerOperation("AssetGroupGetClientIds")]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetClientIds(string groupName)
        {
            var assetIds = await _assetGroupService.GetClientIdsForGroupAsync(groupName);

            return Ok(assetIds);
        }

        [HttpGet("{groupName}")]
        [SwaggerOperation("AssetGroupGet")]
        [ProducesResponseType(typeof(IEnumerable<AssetGroup>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetGroup(string groupName)
        {
            var assetGroup = await _assetGroupService.GetGroupAsync(groupName);

            if (assetGroup != null)
            {
                return Ok(Mapper.Map<AssetGroup>(assetGroup));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [SwaggerOperation("AssetGroupGetAll")]
        [ProducesResponseType(typeof(IEnumerable<AssetGroup>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetGroups()
        {
            var assetGroups = (await _assetGroupService.GetAllGroupsAsync())
                .Select(Mapper.Map<AssetGroup>);

            return Ok(assetGroups);
        }

        [HttpPost]
        [SwaggerOperation("AssetGroupAdd")]
        [ProducesResponseType(typeof(AssetGroup), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> PostGroup([FromBody] AssetGroup group)
        {
            group = Mapper.Map<AssetGroup>(await _assetGroupService.AddGroupAsync(group));

            return Created
            (
                uri:   $"api/v2/asset-groups/{group.Name}",
                value: group
            );
        }

        [HttpPut]
        [SwaggerOperation("AssetGroupUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> PutGroup([FromBody] AssetGroup group)
        {
            await _assetGroupService.UpdateGroupAsync(group);

            return NoContent();
        }
    }
}