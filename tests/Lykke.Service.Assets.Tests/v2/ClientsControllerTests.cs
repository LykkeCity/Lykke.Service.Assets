using System.Threading.Tasks;
using Lykke.Service.Assets.Controllers.V2;
using Lykke.Service.Assets.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Lykke.Service.Assets.Tests.v2
{
    [TestClass]
    public class ClientsControllerTests
    {
        public async Task GetAssetIds__Client_And_Asset_Ids_Exist__Correct_OkResponse_Returned()
        {
            
        }

        public async Task GetAssetIds__Client_Or_Asset_Ids_Not_Exist__Correct_OkResponse_Returned()
        {

        }


        private static ClientsController CreateController(IMock<IAssetGroupService> serviceMock)
        {
            return new ClientsController(serviceMock.Object);
        }

        private static Mock<IAssetGroupService> CreateServiceMock()
        {
            return new Mock<IAssetGroupService>();
        }
    }
}