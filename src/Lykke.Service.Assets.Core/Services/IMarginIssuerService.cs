using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IMarginIssuerService
    {
        IMarginIssuer CreateDefault();

        //public static MarginIssuer CreateDefault()
        //{
        //    return new MarginIssuer();
        //}
    }
}