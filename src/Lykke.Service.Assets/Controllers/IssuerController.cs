using Lykke.Service.Assets.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lykke.Service.Assets.Controllers
{

    /// <inheritdoc />
    /// <summary>
    ///     Controller for erc 20 tokens
    /// </summary>
    [Route("api/issuers")]
    public class IssuerController : Controller
    {
        private readonly IIssuerService _issuerService;


        public IssuerController(
            IIssuerService issuerService)
        {
            _issuerService = issuerService;
        }


    }
}