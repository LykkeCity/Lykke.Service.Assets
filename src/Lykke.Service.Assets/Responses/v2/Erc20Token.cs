using Lykke.Service.Assets.Core.Domain;
using ProtoBuf;

namespace Lykke.Service.Assets.Responses.V2
{
    [ProtoContract]
    public class Erc20Token : IErc20Token
    {
        [ProtoMember(1)]
        public string AssetId { get; set; }

        [ProtoMember(2)]
        public string Address { get; set; }

        [ProtoMember(3)]
        public string BlockHash { get; set; }

        [ProtoMember(4)]
        public int BlockTimestamp { get; set; }

        [ProtoMember(5)]
        public string DeployerAddress { get; set; }

        [ProtoMember(6)]
        public int? TokenDecimals { get; set; }

        [ProtoMember(7)]
        public string TokenName { get; set; }

        [ProtoMember(8)]
        public string TokenSymbol { get; set; }

        [ProtoMember(9)]
        public string TokenTotalSupply { get; set; }

        [ProtoMember(10)]
        public string TransactionHash { get; set; }
    }
}
