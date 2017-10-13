namespace Lykke.Service.Assets.Responses
{
    /// <summary>
    ///    Checks service is alive response
    /// </summary>
    public class IsAlive
    {
        /// <summary>
        ///     Environment variables
        /// </summary>
        public string Env { get; set; }

        /// <summary>
        ///     API version
        /// </summary>
        public string Version { get; set; }
    }
}