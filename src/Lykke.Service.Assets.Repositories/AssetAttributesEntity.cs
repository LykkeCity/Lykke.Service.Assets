using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetAttributesEntity : TableEntity, IAssetAttributes
    {
        public string Id => PartitionKey;
        public string AssetId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }
        public string Key
        {
            get { return RowKey; }
            set { RowKey = value; }
        }
        public string Value { get; set; }
        public IEnumerable<IAssetAttributesKeyValue> Attributes { get; set; }

        public static AssetAttributesEntity Create(string assetId, IAssetAttributesKeyValue keyValue)
        {
            return new AssetAttributesEntity
            {
                RowKey = keyValue.Key,
                PartitionKey = assetId,
                Value = keyValue.Value
            };
        }
    }
}
