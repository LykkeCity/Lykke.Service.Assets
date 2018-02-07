using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lykke.Service.Assets.Tests
{
    [TestClass]
    public class TestAssembly
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext testContext)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
                cfg.AddProfile<Services.AutoMapperProfile>();
                cfg.AddProfile<Repositories.AutoMapperProfile>();
            });
            Mapper.AssertConfigurationIsValid();
        }
    }
}
