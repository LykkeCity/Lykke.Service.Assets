using Autofac;
using AzureStorage;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;
using Lykke.SettingsReader;

namespace Lykke.Service.Assets.Repositories
{
    public class RepositoriesModule : Module
    {
        private readonly ILog                                   _log;
        private readonly IReloadingManager<ApplicationSettings> _settings;

        public RepositoriesModule(ILog log, IReloadingManager<ApplicationSettings> settings)
        {
            _log      = log;
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            LoadRepositories(builder);
            LoadTables(builder);
        }

        private void LoadRepositories(ContainerBuilder builder)
        {
            builder
                .RegisterType<AssetAttributeRepository>()
                .As<IAssetAttributeRepository>()
                .SingleInstance();

            builder
                .RegisterType<AssetCategoryRepository>()
                .As<IAssetCategoryRepository>()
                .SingleInstance();

            builder
                .RegisterType<AssetRepository>()
                .As<IAssetRepository>()
                .SingleInstance();
        }
        
        private void LoadTables(ContainerBuilder builder)
        {
            // Assets

            var assetTable = AzureTableStorage<AssetEntity>.Create
            (
                _settings.ConnectionString(x => x.AssetsService.Dictionaries.DbConnectionString),
                "Dictionaries",
                _log
            );

            builder
                .RegisterInstance(assetTable)
                .As<INoSQLTableStorage<AssetEntity>>();

            // Asset attributes

            var assetAttributeTable = AzureTableStorage<AssetAttributeEntity>.Create
            (
                _settings.ConnectionString(x => x.AssetsService.Dictionaries.DbConnectionString),
                "AssetAttributes",
                _log
            );

            builder
                .RegisterInstance(assetAttributeTable)
                .As<INoSQLTableStorage<AssetAttributeEntity>>();

            // Asset category

            var assetCategoryTable = AzureTableStorage<AssetCategoryEntity>.Create
            (
                _settings.ConnectionString(x => x.AssetsService.Dictionaries.DbConnectionString),
                "AssetCategories",
                _log
            );

            builder
                .RegisterInstance(assetCategoryTable)
                .As<INoSQLTableStorage<AssetCategoryEntity>>();
        }
    }
}