using System.Collections.Generic;

namespace Lykke.Service.Assets.Client.Custom
{
    public interface IErrorResponse
    {
        /// <summary>
        /// </summary>
        IDictionary<string, IList<string>> ErrorMessages { get; }
    }
}