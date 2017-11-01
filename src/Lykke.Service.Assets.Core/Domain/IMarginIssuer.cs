namespace Lykke.Service.Assets.Core.Domain
{
    public interface IMarginIssuer
    {
        string IconUrl { get; }

        string Id { get; }

        string Name { get; }
    }
}