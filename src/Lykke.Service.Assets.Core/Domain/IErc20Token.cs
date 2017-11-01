namespace Lykke.Service.Assets.Core.Domain
{
    public interface IErc20Token
    {
        string AssetId { get; }

        string Address { get; }

        string BlockHash { get; }

        int BlockTimestamp { get; }

        string DeployerAddress { get; }

        int? TokenDecimals { get; }

        string TokenName { get; }

        string TokenSymbol { get; }

        string TokenTotalSupply { get; }

        string TransactionHash { get; }
    }
}
