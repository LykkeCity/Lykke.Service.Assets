using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    [Route("/api/v2/clients")]
    public class ClientsController : Controller
    {
        private readonly IAssetGroupService _assetGroupService;
        private readonly IAssetConditionService _assetConditionService;


        public ClientsController(
            IAssetGroupService assetGroupService,
            IAssetConditionService assetConditionService)
        {
            _assetGroupService = assetGroupService;
            _assetConditionService = assetConditionService;
        }

        [HttpGet("{clientId}/asset-ids")]
        [SwaggerOperation("ClientGetAssetIds")]
        [ProducesResponseType(typeof(IEnumerable<string>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAssetIds(string clientId, [FromQuery] bool isIosDevice)
        {
            var assetIds = await _assetGroupService.GetAssetIdsForClient(clientId, isIosDevice);

            var conditions = await _assetConditionService.GetAssetConditionsByClient(clientId);

            assetIds = assetIds.Where(e => !conditions.ContainsKey(e) || (conditions[e].AvailableToClient ?? true));

            return Ok(assetIds);
        }

        [HttpGet("{clientId}/swift-deposit-enabled")]
        [SwaggerOperation("ClientIsAllowedMakeSwiftDeposit")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> IsAllowedMakeSwiftDeposit(string clientId, [FromQuery] bool isIosDevice)
        {
            var result = await _assetGroupService.SwiftDepositEnabledAsync(clientId, isIosDevice);

            return Ok(result);
        }

        [HttpGet("{clientId}/cash-in-via-bank-card-enabled")]
        [SwaggerOperation("ClientIsAllowedToCashInViaBankCard")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> IsAllowedToCashInViaBankCard(string clientId, [FromQuery] bool isIosDevice)
        {
            var result = await _assetGroupService.CashInViaBankCardEnabledAsync(clientId, isIosDevice);

            return Ok(result);
        }
    }
}
