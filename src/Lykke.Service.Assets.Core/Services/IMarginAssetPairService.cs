using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IMarginAssetPairService
    {
        IMarginAssetPair CreateDefault();

        //public static MarginAssetPair CreateDefault()
        //{
        //    return new MarginAssetPair
        //    {
        //        Accuracy = 5,
        //    };
        //}
    }
}