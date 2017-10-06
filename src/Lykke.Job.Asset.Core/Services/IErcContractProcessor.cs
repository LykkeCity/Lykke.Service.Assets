using Lykke.Job.Asset.Core.Domain;
using Lykke.Job.Asset.Core.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Job.Asset.Core.Services
{
    public interface IErcContractProcessor
    {
        Task ProcessErc20ContractAsync(IErc20Asset message);
    }
}
