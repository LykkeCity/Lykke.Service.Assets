using AutoMapper;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;
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
    [Route("/api/v2/asset-groups")]
    public class AssetGroupsController : Controller
    {
        private readonly IAssetGroupService _assetGroupService;

        public AssetGroupsController(IAssetGroupService assetGroupService)
        {
            _assetGroupService = assetGroupService;
        }

        [HttpPost]
        [SwaggerOperation("AssetGroupAdd")]
        [ProducesResponseType(typeof(AssetGroup), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> Add([FromBody] AssetGroup group)
        {
            group = Mapper.Map<AssetGroup>(await _assetGroupService.AddGroupAsync(group));

            return Created
            (
                uri: $"api/v2/asset-groups/{group.Name}",
                value: group
            );
        }

        [HttpPost("{groupName}/assets/{assetId}")]
        [SwaggerOperation("AssetGroupAddAsset")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> AddAsset(string assetId, string groupName)
        {
            await _assetGroupService.AddAssetToGroupAsync(assetId, groupName);

            return NoContent();
        }

        [HttpPost("{groupName}/clients/{clientId}")]
        [SwaggerOperation("AssetGroupAddClient")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddClient(string clientId, string groupName)
        {
            var group = await _assetGroupService.GetGroupAsync(groupName);
            if (group == null)
            {
                return NotFound();
            }

            await _assetGroupService.AddClientToGroupAsync(clientId, group);

            return NoContent();
        }

        [HttpPost("{groupName}/clients/{clientId}/add-or-replace")]
        [SwaggerOperation("AssetGroupAddOrReplaceClient")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AddOrReplaceClient(string clientId, string groupName)
        {
            var group = await _assetGroupService.GetGroupAsync(groupName);
            if (group == null)
            {
                return NotFound();
            }

            await _assetGroupService.AddClientToGroupOrReplaceAsync(clientId, group);

            return NoContent();
        }

        [HttpGet("{groupName}")]
        [SwaggerOperation("AssetGroupGet")]
        [ProducesResponseType(typeof(AssetGroup), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(string groupName)
        {
            var group = await _assetGroupService.GetGroupAsync(groupName);
            if (group == null)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<AssetGroup>(group));
        }

        [HttpGet]
        [SwaggerOperation("AssetGroupGetAll")]
        [ProducesResponseType(typeof(IEnumerable<AssetGroup>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var assetGroups = (await _assetGroupService.GetAllGroupsAsync())
                .Select(Mapper.Map<AssetGroup>);

            return Ok(assetGroups);
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

        [HttpDelete("{groupName}")]
        [SwaggerOperation("AssetGroupRemove")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Remove(string groupName)
        {
            await _assetGroupService.RemoveGroupAsync(groupName);

            return NoContent();
        }

        [HttpDelete("{groupName}/assets/{assetId}")]
        [SwaggerOperation("AssetGroupRemoveAsset")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveAsset(string assetId, string groupName)
        {
            await _assetGroupService.RemoveAssetFromGroupAsync(assetId, groupName);

            return NoContent();
        }

        [HttpDelete("{groupName}/clients/{clientId}")]
        [SwaggerOperation("AssetGroupRemoveClient")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> RemoveClient(string clientId, string groupName)
        {
            await _assetGroupService.RemoveClientFromGroupAsync(clientId, groupName);

            return NoContent();
        }

        [HttpPut]
        [SwaggerOperation("AssetGroupUpdate")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Update([FromBody] AssetGroup group)
        {
            await _assetGroupService.UpdateGroupAsync(group);

            return NoContent();
        }
    }
}
