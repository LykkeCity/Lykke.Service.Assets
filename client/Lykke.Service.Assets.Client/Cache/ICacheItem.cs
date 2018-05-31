namespace Lykke.Service.Assets.Client.Cache
{
    /// <summary>
    /// Cache item {T} in a <see cref="IDictionaryCache{T}"/>.
    /// </summary>
    internal interface ICacheItem
    {
        /// <summary>
        /// The id of the entry
        /// </summary>
        string Id { get; }
    }
}
