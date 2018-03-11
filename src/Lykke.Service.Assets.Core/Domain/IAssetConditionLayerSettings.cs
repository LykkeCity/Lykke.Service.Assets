using System;

namespace Lykke.Service.Assets.Core.Domain
{
    /// <summary>
    /// Settings to all asset in layer
    /// </summary>
    public interface IAssetConditionLayerSettings
    {
        /// <summary>
        /// Enabled or Disables cashin via back card for all asserst
        /// </summary>
        [Obsolete("This settings moved to the asset condition")]
        bool? ClientsCanCashInViaBankCards { get; }

        /// <summary>
        /// Enabled or Disables cashin swift for all asserst
        /// </summary>
        [Obsolete("This settings moved to the asset condition")]
        bool? SwiftDepositEnabled { get; }
    }
}
