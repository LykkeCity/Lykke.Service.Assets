using System;
using Autofac;
using Lykke.Service.Assets.NoSql;
using Lykke.Service.Assets.NoSql.Models;
using Lykke.Service.Assets.Services;
using Lykke.Service.Assets.Settings;
using Lykke.SettingsReader;
using MyNoSqlServer.Abstractions;

namespace Lykke.Service.Assets.Modules
{
    public class MyNoSqlModule : Module
    {
        private readonly string _myNoSqlServer;
        public MyNoSqlModule(IReloadingManager<ApplicationSettings> settingsManager)
        {
            var settings = settingsManager.CurrentValue.AssetsService.MyNoSqlServer;

            if (string.IsNullOrEmpty(settings?.WriterServiceUrl))
            {
                Console.WriteLine("Please fill in settings [AssetsService.MyNoSqlServer.WriterServiceUrl]");

                throw new Exception("Please fill in settings AssetsService.MyNoSqlServer.WriterServiceUrl");
            }

            _myNoSqlServer = settings.WriterServiceUrl;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterMyNoSqlWriter<AssetAttributeNoSql>(builder, AssetAttributeNoSql.TableName);
            RegisterMyNoSqlWriter<AssetCategoryNoSql>(builder, AssetCategoryNoSql.TableName);
            RegisterMyNoSqlWriter<AssetExtendedInfoNoSql>(builder, AssetExtendedInfoNoSql.TableName);
            RegisterMyNoSqlWriter<AssetNoSql>(builder, AssetNoSql.TableName);
            RegisterMyNoSqlWriter<AssetPairNoSql>(builder, AssetPairNoSql.TableName);
            RegisterMyNoSqlWriter<AssetConditionNoSql>(builder, AssetConditionNoSql.TableName);
            RegisterMyNoSqlWriter<WatchListCustomNoSql>(builder, WatchListCustomNoSql.TableNameCustomWatchList);
            RegisterMyNoSqlWriter<WatchListPredefinedNoSql>(builder, WatchListPredefinedNoSql.TableNamePredefinedWatchList);

        }

        private void RegisterMyNoSqlWriter<TEntity>(ContainerBuilder builder, string table)
            where TEntity : IMyNoSqlDbEntity, new()
        {
            builder.Register(ctx =>
                {
                    return new MyNoSqlServer.DataWriter.MyNoSqlServerDataWriter<TEntity>(() => _myNoSqlServer, table);
                })
                .As<IMyNoSqlServerDataWriter<TEntity>>()
                .SingleInstance();

            builder.RegisterType<MyNoSqlWriterWrapper<TEntity>>()
                .As<IMyNoSqlWriterWrapper<TEntity>>()
                .SingleInstance();

        }
    }
}
