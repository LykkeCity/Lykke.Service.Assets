using System;
using Autofac;
using Lykke.Service.Assets.NoSql.Models.AssetAttributeModel;
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
        }
    }
}
