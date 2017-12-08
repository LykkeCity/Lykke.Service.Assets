using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;
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

        [HttpGet("{clientId}/asset-conditions")]
        [SwaggerOperation("ClientGetAssetConditions")]
        [ProducesResponseType(typeof(IEnumerable<AssetConditionDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAssetConditions(string clientId)
        {
            IReadOnlyDictionary<string, IAssetCondition> result =
                await _assetConditionService.GetAssetConditionsByClient(clientId);

            return Ok(result.Values);
        }
    }
}
