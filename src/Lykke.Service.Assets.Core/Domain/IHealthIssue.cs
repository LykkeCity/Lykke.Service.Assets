namespace Lykke.Service.Assets.Core.Domain
{
    public interface IHealthIssue
    {
        string Type { get; }

        string Value { get; }
    }
}
