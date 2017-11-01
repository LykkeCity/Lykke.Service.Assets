namespace Lykke.Service.Assets.Core.Domain
{
    public interface IIssuer
    {
        string IconUrl { get; }

        string Id { get; }

        string Name { get; }
    }
}