namespace Lykke.Service.Assets.Client.Models.v3
{
    /// <summary>
    /// The main model for the Asset.
    /// </summary>
    public class Asset
    {
        public string Id { get; set; }
        public string DisplayId { get; set; }
        public int Accuracy { get; set; }
        public bool IsDisabled { get; set; }
    }
}
