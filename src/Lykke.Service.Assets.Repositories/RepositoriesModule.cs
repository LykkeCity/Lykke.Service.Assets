using System;
using Autofac;
using AzureStorage;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;
using Lykke.SettingsReader;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories
{
    public class RepositoriesModule : Module
    {
        private readonly IReloadingManager<ApplicationSettings> _settings;

        public RepositoriesModule(IReloadingManager<ApplicationSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            const string assetAttributesTableName = "AssetAttributes";
            const string assetCategoriesTableName = "AssetCategories";
            const string assetGroupsTableName = "AssetGroups";
            const string assetSettingsTableName = "AssetSettings";
            const string assetIssuerTableName = "AssetIssuers";
            const string dictionaryTableName  = "Dictionaries";
            const string erc20TokenTableName  = "Erc20Tokens";
            const string watchListTableName   = "WatchLists";
            const string assetConditionTableName = "AssetCondition";
            const string assetConditionLayerTableName = "AssetConditionLayer";

            string ClientPersonalInfoConnectionString(ApplicationSettings x) => x.AssetsService.Db.ClientPersonalInfoConnString;
            string DictionariesConnectionString(ApplicationSettings x)       => x.AssetsService.Db.DictionariesConnectionString;

            builder.Register<IAssetAttributeRepository>(x => new AssetAttributeRepository(
                CreateTable<AssetAttributeEntity>(DictionariesConnectionString, assetAttributesTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetCategoryRepository>(x => new AssetCategoryRepository(
                CreateTable<AssetCategoryEntity>(DictionariesConnectionString, assetCategoriesTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetExtendedInfoRepository>(x => new AssetExtendedInfoRepository(
                CreateTable<AssetExtendedInfoEntity>(DictionariesConnectionString, dictionaryTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetGroupAssetLinkRepository>(x => new AssetGroupAssetLinkRepository(
                CreateTable<AssetGroupEntity>(ClientPersonalInfoConnectionString, assetGroupsTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetGroupClientLinkRepository>(x => new AssetGroupClientLinkRepository(
                CreateTable<AssetGroupEntity>(ClientPersonalInfoConnectionString, assetGroupsTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetGroupRepository>(x => new AssetGroupRepository(
                CreateTable<AssetGroupEntity>(ClientPersonalInfoConnectionString, assetGroupsTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetPairRepository>(x => new AssetPairRepository(
                CreateTable<AssetPairEntity>(DictionariesConnectionString, dictionaryTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetRepository>(x => new AssetRepository(
                CreateTable<AssetEntity>(DictionariesConnectionString, dictionaryTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetSettingsRepository>(x => new AssetSettingsRepository(
                CreateTable<AssetSettingsEntity>(DictionariesConnectionString, assetSettingsTableName, x.Resolve<ILogFactory>())));

            builder.Register<IClientAssetGroupLinkRepository>(x => new ClientAssetGroupLinkRepository(
                CreateTable<AssetGroupEntity>(ClientPersonalInfoConnectionString, assetGroupsTableName, x.Resolve<ILogFactory>())));

            builder.Register<ICustomWatchListRepository>(x => new CustomWatchListRepository(
                CreateTable<CustomWatchListEntity>(DictionariesConnectionString, watchListTableName, x.Resolve<ILogFactory>())));

            builder.Register<IErc20TokenRepository>(x => new Erc20TokenRepository(
                CreateTable<Erc20TokenEntity>(DictionariesConnectionString, erc20TokenTableName, x.Resolve<ILogFactory>()),
                CreateTable<AzureIndex>(DictionariesConnectionString, erc20TokenTableName, x.Resolve<ILogFactory>())));

            builder.Register<IIssuerRepository>(x => new IssuerRepository(
                CreateTable<IssuerEntity>(DictionariesConnectionString, assetIssuerTableName, x.Resolve<ILogFactory>())));

            builder.Register<IMarginAssetPairRepository>(x => new MarginAssetPairRepository(
                CreateTable<MarginAssetPairEntity>(DictionariesConnectionString, dictionaryTableName, x.Resolve<ILogFactory>())));

            builder.Register<IMarginAssetRepository>(x => new MarginAssetRepository(
                CreateTable<MarginAssetEntity>(DictionariesConnectionString, dictionaryTableName, x.Resolve<ILogFactory>())));

            builder.Register<IMarginIssuerRepository>(x => new MarginIssuerRepository(
                CreateTable<MarginIssuerEntity>(DictionariesConnectionString, assetIssuerTableName, x.Resolve<ILogFactory>())));

            builder.Register<IPredefinedWatchListRepository>(x => new PredefinedWatchListRepository(
                CreateTable<PredefinedWatchListEntity>(DictionariesConnectionString, watchListTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetConditionRepository>(x => new AssetConditionRepository(
                CreateTable<AssetConditionEntity>(DictionariesConnectionString, assetConditionLayerTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetConditionLayerRepository>(x => new AssetConditionLayerRepository(
                CreateTable<AssetConditionLayerEntity>(DictionariesConnectionString, assetConditionLayerTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetDefaultConditionRepository>(x => new AssetDefaultConditionRepository(
                CreateTable<AssetDefaultConditionEntity>(DictionariesConnectionString, assetConditionLayerTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetDefaultConditionLayerRepository>(x => new AssetDefaultConditionLayerRepository(
                CreateTable<AssetDefaultConditionLayerEntity>(DictionariesConnectionString, assetConditionLayerTableName, x.Resolve<ILogFactory>())));

            builder.Register<IAssetConditionLayerLinkClientRepository>(x => new AssetConditionLayerLinkClientRepository(
                CreateTable<AssetConditionLayerLinkClientEntity>(ClientPersonalInfoConnectionString, assetConditionTableName, x.Resolve<ILogFactory>())));
        }

        private INoSQLTableStorage<T> CreateTable<T>(Func<ApplicationSettings, string> connectionString, string name, ILogFactory logFactory)
            where T : TableEntity, new()
        {
            return AzureTableStorage<T>.Create(_settings.ConnectionString(connectionString), name, logFactory);
        }
    }
}
