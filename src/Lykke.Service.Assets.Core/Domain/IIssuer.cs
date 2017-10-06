namespace Lykke.Service.Assets.Core.Domain
{
    public interface IIssuer
    {
        string IconUrl { get; }

        string Id { get; set; }

        string Name { get; }
    }
}