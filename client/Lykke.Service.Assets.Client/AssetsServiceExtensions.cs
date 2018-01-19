using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Models;

namespace Lykke.Service.Assets.Client
{
    /// <summary>
    /// Extension for backward compatibility 
    /// </summary>
    public static partial class AssetsServiceExtensions
    {
        /// <summary>
        /// Returns all assets including nontradable.
        /// </summary>
        /// <param name='operations'>The operations group for this extension method</param>
        /// <returns></returns>
        public static IList<Asset> AssetGetAll(this IAssetsService operations)
        {
            return operations.AssetGetAllAsync(false).GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Returns all assets including nontradable.
        /// </summary>
        /// <param name='operations'>The operations group for this extension method.</param>
        /// <param name='cancellationToken'>The cancellation token.</param>
        /// <returns></returns>
        public static async Task<IList<Asset>> AssetGetAllAsync(this IAssetsService operations, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var result = await operations.AssetGetAllWithHttpMessagesAsync(false, null, cancellationToken).ConfigureAwait(false))
            {
                return result.Body;
            }
        }
    }
}
