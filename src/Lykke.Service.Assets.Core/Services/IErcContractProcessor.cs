using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IErcContractProcessor
    {
        Task ProcessErc20ContractAsync(IErc20Asset message);
    }
}
