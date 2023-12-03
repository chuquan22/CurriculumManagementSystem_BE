using AutoFixture;
using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Repositories.CLOS;

namespace CMS_UnitTests.Controllers
{
    public class CLOsControllerTest
    {
        private Mock<ICLORepository> closRepositoryMock;
        private Mock<IMapper> mapperMock;
        private IMapper _mapper;
        private Mock<IWebHostEnvironment> hostingEnvironmentMock;
        private CLOsController closController;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            closRepositoryMock = fixture.Freeze<Mock<ICLORepository>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            var configurationMock = new Mock<IConfiguration>();
            mapperMock = new Mock<IMapper>();
            hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            closController = new CLOsController(_mapper);
        }

        [Test]
        public async Task GetCLOs_ReturnsOkResponse()
        {
            // Arrange
            var syllabus_id = 1;

            var listCLOs = new List<CLO>();

            closRepositoryMock.Setup(repo => repo.GetCLOs(syllabus_id))
                .Returns(listCLOs);

            // Act
            var result = closController.GetCLOs(syllabus_id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsEmpty(listCLOs);
        }
        [Test]
        public async Task CreateCLOs_ReturnsOkResponse()
        {
            // Arrange
            var CLOsCreateRequest = new CLOsRequest
            {
                CLO_name = "CLO" + fixture.Create<int>(),
                syllabus_id = 5,
                CLO_description = fixture.Create<string>()

            };

            // Act
            var result = closController.CreateCLOs(CLOsCreateRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);
        }
        [Test]
        public async Task CreateCLOs_ReturnsBadRequestResponse_WithInvalidCLOName()
        {
            // Arrange
            var CLOsCreateRequest = new CLOsRequest
            {
                CLO_name = "123",
                syllabus_id = 1,
                CLO_description = fixture.Create<string>()
                // Set other properties as needed
            };

            // Set up your mock repository or the corresponding logic to throw an exception
            closRepositoryMock.Setup(repo => repo.CreateCLOs(It.IsAny<CLO>()))
                             .Throws(new Exception("Invalid CLO name. It must start with 'CLO' and be followed by at least one number, and not contain spaces."));

            // Act
            var result = closController.CreateCLOs(CLOsCreateRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);

            var baseResponse = badRequestResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Invalid CLO name. It must start with 'CLO' and be followed by at least one number, and not contain spaces.", baseResponse.message);
            Assert.IsNull(baseResponse.data);
        }
        [Test]
        public async Task CreateCLOs_ReturnsBadRequestResponse_WhenNotFoundSyllabus()
        {
            // Arrange
            var CLOsCreateRequest = new CLOsRequest
            {
                CLO_name = "CLO" + fixture.Create<int>(),
                syllabus_id = 9999,
                CLO_description = fixture.Create<string>()
                // Set other properties as needed
            };

            // Set up your mock repository or the corresponding logic to throw an exception
            closRepositoryMock.Setup(repo => repo.CreateCLOs(It.IsAny<CLO>()))
                             .Throws(new Exception("An error occurred while saving the entity changes. See the inner exception for details."));

            // Act
            var result = closController.CreateCLOs(CLOsCreateRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);

            var baseResponse = badRequestResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("An error occurred while saving the entity changes. See the inner exception for details.", baseResponse.message);
            Assert.IsNull(baseResponse.data);
        }
        [Test]
        public async Task UpdateCLOs_ReturnsOkResponse()
        {
            // Arrange
            var CLOsCreateRequest = new CLOsRequest
            {
                CLO_name = "CLO" + fixture.Create<int>(),
                syllabus_id = 5,
                CLO_description = fixture.Create<string>()

            };
            var resultCreate = closController.CreateCLOs(CLOsCreateRequest);
            Assert.IsInstanceOf<OkObjectResult>(resultCreate.Result);
            var okObjectResultCreate = resultCreate.Result as OkObjectResult;
            var baseResponseCreate = okObjectResultCreate.Value as BaseResponse;
  
            var CLO = baseResponseCreate.data as CLO;

            var CLOsUpdateRequest = new CLOsUpdateRequest
            {
                CLO_id = CLO.CLO_id,
                CLO_name = fixture.Create<string>(),
                syllabus_id = CLO.syllabus_id,
                CLO_description = fixture.Create<string>()
            };
            closRepositoryMock.Setup(repo => repo.UpdateCLOs(It.IsAny<CLO>()))
                              .Returns(new CLO()); // Assuming your repository returns the updated Material


            // Act
            var result = closController.UpdateCLOs(CLOsUpdateRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            var closResponse = baseResponse.data as CLO;
            Assert.IsNotNull(closResponse);
            Assert.AreEqual(CLOsUpdateRequest.syllabus_id, closResponse.syllabus_id);
            Assert.AreEqual(CLOsUpdateRequest.CLO_id, closResponse.CLO_id);
        }

        [Test]
        public async Task UpdateCLOs_ReturnsNotFound()
        {
            // Arrange
            var CLOsCreateRequest = new CLOsUpdateRequest
            {
                CLO_id = 9999,
                CLO_name = fixture.Create<string>(),
                syllabus_id = 1,
                CLO_description = fixture.Create<string>()
            };

            // Act
            var result = closController.UpdateCLOs(CLOsCreateRequest);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found This CLOs!", baseResponse.message);
            Assert.IsNull(baseResponse.data);
        }
    }
}
