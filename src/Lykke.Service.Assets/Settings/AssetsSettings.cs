namespace Lykke.Service.Assets.Settings
{
    public partial class ApplicationSettings
    {
        public class AssetsSettings
        {
            public DbSettings Db { get; set; }
            public RabbitSettings Rabbit { get; set; }
            public RedisSettings RedisSettings { get; set; }
            public CqrsSettings Cqrs { get; set; }
            public InternalCacheSettings InternalCache { get; set; }
        }
    }
}
