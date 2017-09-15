using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Client.Custom
{
    public interface IAssetCategory
    {
        string Id { get; }
        string Name { get; }
        string IosIconUrl { get; set; }
        string AndroidIconUrl { get; set; }
        int? SortOrder { get; set; }

    }
}
