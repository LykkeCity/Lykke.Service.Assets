namespace Lykke.Service.Assets.Core.Domain
{
    /// <summary>
    /// Set of settings for the asset, rules\conditions of use of the asset.
    /// These conditions can be redefined at different levels, so everything is not mandatory.
    /// </summary>
    public interface IAssetCondition : IAssetConditionSettings
    {
        /// <summary>
        /// Asset id for this settings, conditions
        /// </summary>
        string Asset { get; }
    }
}
