using System.IO;
using System.Threading.Tasks;
using BusinessCardApi.Controllers;
using BusinessCardApi.Repo;  
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BusinessCardUnitTest
{
    public class BusinessCardControllerTests
    {
        private readonly BusinessCardController _businessCardController;
        private readonly Mock<IBusinessCardRepo> _repoMock;

        public BusinessCardControllerTests()
        {
            _repoMock = new Mock<IBusinessCardRepo>();
            _businessCardController = new BusinessCardController(_repoMock.Object);
        }

        [Fact]
        public async Task Import_NullFile_ReturnBadRequest()
        {
            var result = await _businessCardController.ImportCards(null, null);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file uploaded.", badRequestResult.Value);
        }


    }
}
