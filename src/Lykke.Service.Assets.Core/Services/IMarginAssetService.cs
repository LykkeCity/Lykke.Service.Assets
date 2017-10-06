using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IMarginAssetService
    {
        IMarginAsset CreateDefault();

        //public static MarginAsset CreateDefault()
        //{
        //    return new MarginAsset();
        //}
    }
}