

using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurriculumManagementSystemWebAPI.Controllers;
using Repositories.Curriculums;
using Repositories.Batchs;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using DataAccess.Models.DTO.response;
using BusinessObject;
using System.Reflection;
using System.Net;
using DataAccess.Models.DTO.request;
using AutoFixture;
using DataAccess.Models.DTO;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class CurriculumControllerTest
    {
        private Mock<ICurriculumRepository> curriculumRepositoryMock;
        private Mock<IBatchRepository> batchRepositoryMock;
        private Mock<IMapper> mapperMock;
        private IMapper _mapper;
        private Mock<IWebHostEnvironment> hostingEnvironmentMock;
        private CurriculumsController curriculumController;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            curriculumRepositoryMock = fixture.Freeze<Mock<ICurriculumRepository>>();
            batchRepositoryMock = new Mock<IBatchRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            var configurationMock = new Mock<IConfiguration>();
            mapperMock = new Mock<IMapper>();
            hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            curriculumController = new CurriculumsController(configurationMock.Object, _mapper, hostingEnvironmentMock.Object);

        }

        [Test]
        public async Task GetCurriculumByBatch_ReturnsOkResponse()
        {
            // Arrange
            var curriculumCode = fixture.Create<string>();
            var batchId = fixture.Create<int>();

            // Act
            var result = await curriculumController.GetCurriculumByBatch(curriculumCode, batchId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public async Task GetCurriculumByBatch_ReturnsNotFoundResponse()
        {
            // Arrange
            string curriculumCode = null;
            int batchId = fixture.Create<int>();

            // Act
            var result = await curriculumController.GetCurriculumByBatch(curriculumCode, batchId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Curriculum", baseResponse.message);
        }


        [Test]
        public async Task PaginationCurriculum_ReturnsOkResult()
        {
            // Arrange
            var page = 1;
            var limit = 10;
            var degreeLevelId = 1;
            var txtSearch = "a";
            var majorId = 4;

            var listCurri = new List<Curriculum>();
            var listCurriResponse = new List<CurriculumResponse>();

            curriculumRepositoryMock.Setup(repo => repo.PanigationCurriculum(page, limit, degreeLevelId, txtSearch, majorId))
                .Returns(listCurri);
            mapperMock.Setup(mapper => mapper.Map<List<CurriculumResponse>>(listCurri)).Returns(listCurriResponse);
            // Act
            var result = await curriculumController.PaginationCurriculum(page, limit, degreeLevelId, txtSearch, majorId);

            // Assert


            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Get List Curriculum Sucessfully", baseResponse.message);

            Assert.IsEmpty(listCurriResponse);

        }

        [Test]
        public async Task PaginationCurriculum_ReturnsBadRequestResponse()
        {
            // Arrange
            var page = 1;
            var limit = 10;
            var degreeLevelId = 3;
            var txtSearch = "a";
            var majorId = 4;

            // Act
            var result = await curriculumController.PaginationCurriculum(page, limit, degreeLevelId, txtSearch, majorId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("List Curriculum is Empty", baseResponse.message);


        }


        [Test]
        public async Task GetCurriculum_ReturnsOkResult()
        {
            // Arrange
            var curriculumId = 4;
            var expectedCurriculumResponse = new CurriculumResponse();
            var curriculum = new Curriculum();

            curriculumRepositoryMock.Setup(repo => repo.GetCurriculumById(curriculumId)).Returns(curriculum);
            mapperMock.Setup(mapper => mapper.Map<CurriculumResponse>(curriculum)).Returns(expectedCurriculumResponse);

            // Act
            var result = await curriculumController.GetCurriculum(curriculumId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Curriculum", baseResponse.message);

            Assert.IsNotNull(expectedCurriculumResponse);
        }

        [Test]
        public async Task GetCurriculum_ReturnsNotFoundResult()
        {
            // Arrange
            var curriculumId = 200;

            // Act
            var result = await curriculumController.GetCurriculum(curriculumId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);

            var ObjectResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(ObjectResult);

            var baseResponse = ObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found This Curriculum!", baseResponse.message);

        }


        [Test]
        public async Task GetListBatchNotInCurriculum_ReturnsOkResult()
        {
            // Arrange
            var curriculumCode = "GD-GD-CD-19.2";

            // Act
            var result = await curriculumController.GetlistBatch(curriculumCode);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [TestCase("Curriculum Test", "Curriculum English Test", "Curriculum Description Test", 7, 3, 5, "Sample Formality", "Sample Decision No", "12/12/2001")]
        [TestCase("Curriculum Test", "Curriculum English Test", "Curriculum Description Test", 9, 2, 4, "Sample Formality", "Sample Decision No", "12/12/2001")]
        public async Task PostCurriculum_ReturnsOkResult(string curriculumName, string englishCurriculumName, string curriculumDescription, int totalSemester, int speId, int batchId,
            string formal, string decisionNo, DateTime date)
        {
            // Arrange
            var curriculumRequest = new CurriculumRequest
            {
                curriculum_name = curriculumName,
                english_curriculum_name = englishCurriculumName,
                curriculum_description = curriculumDescription,
                total_semester = totalSemester,
                specialization_id = speId,
                batch_id = batchId,
                Formality = formal,
                decision_No = decisionNo,
                approved_date = date
            };
            var curriculum = new Curriculum();
            string curriculumCode = string.Empty;
            int total = 0;

            curriculumRepositoryMock.Setup(repo => repo.GetCurriculumCode(batchId, speId)).Returns(curriculumCode);  // Mock the repository method
            curriculumRepositoryMock.Setup(repo => repo.GetTotalSemester(speId, batchId)).Returns(total);  // Mock the repository method

            // Act
            var result = await curriculumController.PostCurriculum(curriculumRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual(baseResponse.message, "Create Curriculum Success!");
        }

        [TestCase("Curriculum Test", "Curriculum English Test", "Curriculum Description Test", 9, 8, 2, "Sample Formality", "Sample Decision No", "12/12/2001")]
        [TestCase("Curriculum Test", "Curriculum English Test", "Curriculum Description Test", 7, 8, 3, null, "Sample Decision No", "12/12/2001")]
        [TestCase(null, "Curriculum English Test", "Curriculum Description Test", 9, 2, 6, "Sample Formality", "Sample Decision No", "12/12/2001")]
        [TestCase("Curriculum Test", null, "Curriculum Description Test", 5, 1, 5, "Sample Formality", "Sample Decision No", "12/12/2001")]
        [TestCase("Curriculum Test", "Curriculum English Test", null, 3, 2, 6, "Sample Formality", "Sample Decision No", "12/12/2001")]
        public async Task PostCurriculum_ReturnsFailResult(string curriculumName, string englishCurriculumName, string curriculumDescription, int? totalSemester, int? speId, int? batchId,
            string formal, string decisionNo, DateTime? date)
        {
            // Arrange
            var curriculumRequest = new CurriculumRequest
            {
                curriculum_name = curriculumName,
                english_curriculum_name = englishCurriculumName,
                curriculum_description = curriculumDescription,
                total_semester = (int)totalSemester,
                specialization_id = (int)speId,
                batch_id = (int)batchId,
                Formality = formal,
                decision_No = decisionNo,
                approved_date = date
            };

            // Act
            var result = await curriculumController.PostCurriculum(curriculumRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);

        }

        [Test]
        public async Task PostCurriculum_ReturnsBadRequestResult_WhenCurriculumDuplicate()
        {
            // Arrange
            var curriculumRequest = new CurriculumRequest
            {
                curriculum_name = "Sample Curriculum",
                english_curriculum_name = "Sample English Curriculum",
                curriculum_description = "Sample Curriculum Description",
                total_semester = 7,
                specialization_id = 7,
                batch_id = 5,
                Formality = "Sample Formality",
                decision_No = "Sample Decision No",
                approved_date = DateTime.Now
            };

            // Act
            var result = await curriculumController.PostCurriculum(curriculumRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.IsTrue(baseResponse.message.Contains("Duplicate!"));

        }

        [Test]
        public async Task DeleteCurriculum_ReturnsOkResult()
        {
            // Arrange
            int curriculumId = 19;

            curriculumRepositoryMock.Setup(repo => repo.GetCurriculumById(It.IsAny<int>())).Returns(new Curriculum());
            curriculumRepositoryMock.Setup(repo => repo.RemoveCurriculum(It.IsAny<Curriculum>())).Returns("OK");

            // Act
            var result = await curriculumController.DeleteCurriculum(curriculumId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task DeleteCurriculum_ReturnsNotFoundResult_WhenNotFoundCurriculum()
        {
            // Arrange
            int curriculumId = 200;

            curriculumRepositoryMock.Setup(repo => repo.GetCurriculumById(It.IsAny<int>())).Returns(new Curriculum());
            curriculumRepositoryMock.Setup(repo => repo.RemoveCurriculum(It.IsAny<Curriculum>())).Returns("OK");

            // Act
            var result = await curriculumController.DeleteCurriculum(curriculumId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
        

        [TestCase("curriculum name test", "curriculum english name Test", "test", "test", "12/12/2001")]
        [TestCase("curriculum name test1", "curriculum english name Test", "test", "test", "12/12/2001")]
        [TestCase("curriculum name test2", "curriculum english name Test", "test", "test", "12/12/2001")]
        [TestCase("curriculum name test3", "curriculum english name Test", "test", "test", "12/12/2001")]
        [TestCase("curriculum name test4", "curriculum english name Test", "test", "test", "12/12/2001")]
        public async Task PutCurriculum_ReturnsOkResult(string curriculumName, string englishCurriculumName, string description, string decisionNo, DateTime date)
        {
            // Arrange
            var curriculumId = 4;
            var curriculumRequest = new CurriculumUpdateRequest
            {
                curriculum_name = curriculumName,
                english_curriculum_name = englishCurriculumName,
                curriculum_description = "Sample Curriculum Description",
                decision_No = "Sample Update Decision No",
                approved_date = DateTime.Now,
                
            };

            curriculumRepositoryMock.Setup(repo => repo.GetCurriculumById(It.IsAny<int>())).Returns(new Curriculum());
            curriculumRepositoryMock.Setup(repo => repo.UpdateCurriculum(It.IsAny<Curriculum>())).Returns("OK");

            // Act
            var result = await curriculumController.PutCurriculum(curriculumId, curriculumRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Update Curriculum Success!", baseResponse.message);
        }

        [TestCase(400,"curriculum name test", "curriculum english name Test", "test", "test", "12/12/2001")]
        [TestCase(3,"curriculum name test", null, "test", "test", "12/12/2001")]
        [TestCase(4,null, "curriculum english name Test", "test", "test", "12/12/2001")]
        [TestCase(4,"curriculum name test", "curriculum english name Test", null, "test", "12/12/2001")]
        [TestCase(4,"curriculum name test", "curriculum english name Test", "Test", null, "12/12/2001")]
        [TestCase(4,"curriculum name test", "curriculum english name Test", "test", "test", "10/15/2002")]
        public async Task PutCurriculum_ReturnsFailResult(int id, string curriculumName, string englishCurriculumName, string description, string decisionNo, DateTime date)
        {
            // Arrange
            var curriculumId = 400;
            var curriculumRequest = new CurriculumUpdateRequest
            {
                curriculum_name = "Sample Update Curriculum",
                english_curriculum_name = "Sample Update English Curriculum",
                curriculum_description = "Sample Curriculum Description",
                decision_No = "Sample Update Decision No",
                approved_date = DateTime.Now
            };

            // Act
            var result = await curriculumController.PutCurriculum(curriculumId, curriculumRequest);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var objectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(objectResult);

            var baseResponse = objectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Can't Update because not found curriculum", baseResponse.message);
        }
    }

}

