﻿using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class MarginIssuerEntity : TableEntity, IMarginIssuer
    {
        public string IconUrl { get; set; }

        public string Id => RowKey;

        public string Name { get; set; }
    }
}