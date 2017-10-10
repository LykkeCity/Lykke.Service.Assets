namespace Lykke.Service.Assets.Core.Domain
{
    public interface IErc20Asset
    {
        string AssetId { get; }

        string Address { get; }

        string BlockHash { get; }

        int BlockTimestamp { get; }

        string DeployerAddress { get; }

        uint? TokenDecimals { get; }

        string TokenName { get; }

        string TokenSymbol { get; }

        string TokenTotalSupply { get; }

        string TransactionHash { get; }
    }
}
