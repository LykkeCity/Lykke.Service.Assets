namespace Lykke.Service.Assets.Settings
{
    public class MyNoSqlSettings
    {
        public string WriterServiceUrl { get; set; }
        public int MaxClientsInCache { get; set; } = 10000;
    }
}
