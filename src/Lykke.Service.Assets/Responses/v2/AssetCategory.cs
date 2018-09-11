using Lykke.Service.Assets.Core.Domain;
using ProtoBuf;

namespace Lykke.Service.Assets.Responses.V2
{
    [ProtoContract]
    public class AssetCategory : IAssetCategory
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public string AndroidIconUrl { get; set; }

        [ProtoMember(4)]
        public string IosIconUrl { get; set; }

        [ProtoMember(5)]
        public int SortOrder { get; set; }
    }
}
