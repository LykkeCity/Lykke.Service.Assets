using System;
using Autofac;
using AzureStorage;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common.Log;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;
using Lykke.SettingsReader;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories
{
    public class RepositoriesModule : Module
    {
        private readonly ILog                                   _log;
        private readonly IReloadingManager<ApplicationSettings> _settings;

        public RepositoriesModule(IReloadingManager<ApplicationSettings> settings, ILog log)
        {
            _log      = log;
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            const string assetIssuerTableName = "AssetIssuers";
            const string dictionaryTableName  = "Dictionaries";
            const string erc20TokenTableName  = "Erc20Tokens";
            const string watchListTableName   = "WatchLists";
            const string assetConditionTableName = "AssetCondition";
            const string assetConditionLayerTableName = "AssetConditionLayer";

            string ClientPersonalInfoConnectionString(ApplicationSettings x) => x.AssetsService.Db.ClientPersonalInfoConnString;
            string DictionariesConnectionString(ApplicationSettings x)       => x.AssetsService.Dictionaries.DbConnectionString;
            

            var assetAttributeTable      = CreateTable<AssetAttributeEntity>(DictionariesConnectionString, "AssetAttributes");
            var assetCategoryTable       = CreateTable<AssetCategoryEntity>(DictionariesConnectionString, "AssetCategories");
            var assetExtendedInfoTable   = CreateTable<AssetExtendedInfoEntity>(DictionariesConnectionString, dictionaryTableName);
            var assetGroupTable          = CreateTable<AssetGroupEntity>(ClientPersonalInfoConnectionString, "AssetGroups");
            var assetPairTable           = CreateTable<AssetPairEntity>(DictionariesConnectionString, dictionaryTableName);
            var assetSettingsTable       = CreateTable<AssetSettingsEntity>(DictionariesConnectionString, "AssetSettings");
            var assetTable               = CreateTable<AssetEntity>(DictionariesConnectionString, dictionaryTableName);
            var customWatchListTable     = CreateTable<CustomWatchListEntity>(DictionariesConnectionString, watchListTableName);
            var erc20TokenTable          = CreateTable<Erc20TokenEntity>(DictionariesConnectionString, erc20TokenTableName);
            var erc20IndexTable          = CreateTable<AzureIndex>(DictionariesConnectionString, erc20TokenTableName);
            var issuerTable              = CreateTable<IssuerEntity>(DictionariesConnectionString, assetIssuerTableName);
            var marginAssetPairTable     = CreateTable<MarginAssetPairEntity>(DictionariesConnectionString, dictionaryTableName);
            var marginAssetTable         = CreateTable<MarginAssetEntity>(DictionariesConnectionString, dictionaryTableName);
            var marginIssuerTable        = CreateTable<MarginIssuerEntity>(DictionariesConnectionString, assetIssuerTableName);
            var predefinedWatchListTable = CreateTable<PredefinedWatchListEntity>(DictionariesConnectionString, watchListTableName);

            var assetConditionTable = CreateTable<AssetConditionEntity>(DictionariesConnectionString, assetConditionLayerTableName);
            var assetConditionLayerTable = CreateTable<AssetConditionLayerEntity>(DictionariesConnectionString, assetConditionLayerTableName);
            var assetDefaultConditionTable = CreateTable<AssetDefaultConditionEntity>(DictionariesConnectionString, assetConditionLayerTableName);
            var assetDefaultConditionLayerTable = CreateTable<AssetDefaultConditionLayerEntity>(DictionariesConnectionString, assetConditionLayerTableName);
            var assetConditionLayerLinkClientTable = CreateTable<AssetConditionLayerLinkClientEntity>(ClientPersonalInfoConnectionString, assetConditionTableName);
            
            builder.RegisterInstance<IAssetAttributeRepository>
                (new AssetAttributeRepository(assetAttributeTable));

            builder.RegisterInstance<IAssetCategoryRepository>
                (new AssetCategoryRepository(assetCategoryTable));

            builder.RegisterInstance<IAssetExtendedInfoRepository>
                (new AssetExtendedInfoRepository(assetExtendedInfoTable));

            builder.RegisterInstance<IAssetGroupAssetLinkRepository>
                (new AssetGroupAssetLinkRepository(assetGroupTable));

            builder.RegisterInstance<IAssetGroupClientLinkRepository>
                (new AssetGroupClientLinkRepository(assetGroupTable));

            builder.RegisterInstance<IAssetGroupRepository>
                (new AssetGroupRepository(assetGroupTable));

            builder.RegisterInstance<IAssetPairRepository>
                (new AssetPairRepository(assetPairTable));

            builder.RegisterInstance<IAssetRepository>
                (new AssetRepository(assetTable));

            builder.RegisterInstance<IAssetSettingsRepository>
                (new AssetSettingsRepository(assetSettingsTable));

            builder.RegisterInstance<IClientAssetGroupLinkRepository>
                (new ClientAssetGroupLinkRepository(assetGroupTable));

            builder.RegisterInstance<ICustomWatchListRepository>
                (new CustomWatchListRepository(customWatchListTable));

            builder.RegisterInstance<IErc20TokenRepository>
                (new Erc20TokenRepository(erc20TokenTable, erc20IndexTable));

            builder.RegisterInstance<IIssuerRepository>
                (new IssuerRepository(issuerTable));

            builder.RegisterInstance<IMarginAssetPairRepository>
                (new MarginAssetPairRepository(marginAssetPairTable));

            builder.RegisterInstance<IMarginAssetRepository>
                (new MarginAssetRepository(marginAssetTable));

            builder.RegisterInstance<IMarginIssuerRepository>
                (new MarginIssuerRepository(marginIssuerTable));

            builder.RegisterInstance<IPredefinedWatchListRepository>
                (new PredefinedWatchListRepository(predefinedWatchListTable));

            builder.RegisterInstance<IAssetConditionRepository>(
                new AssetConditionRepository(assetConditionTable));

            builder.RegisterInstance<IAssetConditionLayerRepository>(
                new AssetConditionLayerRepository(assetConditionLayerTable));

            builder.RegisterInstance<IAssetDefaultConditionRepository>(
                new AssetDefaultConditionRepository(assetDefaultConditionTable));

            builder.RegisterInstance<IAssetDefaultConditionLayerRepository>(
                new AssetDefaultConditionLayerRepository(assetDefaultConditionLayerTable));

            builder.RegisterInstance<IAssetConditionLayerLinkClientRepository>(
                new AssetConditionLayerLinkClientRepository(assetConditionLayerLinkClientTable));
        }

        private INoSQLTableStorage<T> CreateTable<T>(Func<ApplicationSettings, string> connectionString, string name)
            where T : TableEntity, new()
        {
            return AzureTableStorage<T>.Create(_settings.ConnectionString(connectionString), name, _log);
        }
    }
}
