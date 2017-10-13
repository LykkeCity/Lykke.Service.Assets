using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.Assets.Controllers.V2
{
    [Route("/api/v2/clients")]
    public class ClientController : Controller
    {
        private readonly IAssetGroupService _assetGroupService;


        public ClientController(
            IAssetGroupService assetGroupService)
        {
            _assetGroupService = assetGroupService;
        }

        [HttpGet("{clientId}/asset-ids")]
        [SwaggerOperation("ClientGetAssetIds")]
        [ProducesResponseType(typeof(IEnumerable<string>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAssetIds(string clientId, [FromQuery] bool isIosDevice)
        {
            var assetIds = await _assetGroupService.GetAssetIdsForClient(clientId, isIosDevice);

            return Ok(assetIds);
        }

        [HttpGet("{clientId}/cash-in-via-bank-card-enabled")]
        [SwaggerOperation("ClientIsAllowedToCashInViaBankCard")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetCashInViaBankCardEnabled(string clientId, [FromQuery] bool isIosDevice)
        {
            var result = await _assetGroupService.CashInViaBankCardEnabledAsync(clientId, isIosDevice);

            return Ok(result);
        }

        [HttpGet("{clientId}/swift-deposit-enabled")]
        [SwaggerOperation("ClientIsAllowedMakeSwiftDeposit")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetSwiftDepositEnabled(string clientId, [FromQuery] bool isIosDevice)
        {
            var result = await _assetGroupService.SwiftDepositEnabledAsync(clientId, isIosDevice);

            return Ok(result);
        }
    }
}