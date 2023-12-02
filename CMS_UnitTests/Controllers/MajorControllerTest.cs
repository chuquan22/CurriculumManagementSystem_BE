using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurriculumManagementSystemWebAPI.Controllers;
using Repositories.Syllabus;
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
using Repositories.Major;

namespace CMS_UnitTests.Controllers
{
    public class MajorControllerTest
    {
        private Mock<IMajorRepository> majorRepositoryMock;
        private Mock<IMapper> mapperMock;
        private IMapper _mapper;
        private MajorsController majorController;
        private IFixture fixture;


        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            majorRepositoryMock = fixture.Freeze<Mock<IMajorRepository>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            mapperMock = new Mock<IMapper>();
            majorController = new MajorsController(_mapper);
        }

        [Test]
        public async Task GetAllMajor_ReturnsOkResult()
        {
            // Arrange
            var listMajor = new List<Major>();
            var listMajorResponse = new List<MajorResponse>();

            majorRepositoryMock.Setup(repo => repo.GetAllMajor())
                .Returns(listMajor);

            mapperMock.Setup(mapper => mapper.Map<List<MajorResponse>>(listMajor)).Returns(listMajorResponse);

            // Act
            var result = majorController.GetAllMajor();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsEmpty(listMajorResponse);
        }
        [Test]
        public async Task GetMajor_ReturnsOkResult()
        {
            // Arrange
            int batchId = 1;
            var listMajor = new List<Major>();
            var listMajorResponse = new List<MajorSpeResponse>();

            majorRepositoryMock.Setup(repo => repo.GetMajorByBatch(batchId))
                .Returns(listMajor);

            mapperMock.Setup(mapper => mapper.Map<List<MajorSpeResponse>>(listMajor)).Returns(listMajorResponse);

            // Act
            var result = majorController.GetMajor(batchId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsEmpty(listMajorResponse);
        }

        [Test]
        public async Task GetMajor_ReturnsNotFoundResult()
        {
            // Arrange
            var batchId = 200;

            // Act
            var result = majorController.GetMajor(batchId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);

            var ObjectResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(ObjectResult);

            var baseResponse = ObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found This Major!", baseResponse.message);

        }
        [Test]
        public void CreateMajor_ReturnsOkResult()
        {
            // Arrange
            var majorRequest = new MajorRequest
            {
                major_code = fixture.Create<string>().Substring(0, Math.Min(5, fixture.Create<string>().Length)),
                major_name = "TestMajorName",
                major_english_name = "TestMajorEnglishName",
                is_active = true,
                degree_level_id = 1
            };

            // Act
            var result = majorController.CreateMajor(majorRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Add Major Successfully!", baseResponse.message);
        }
        [Test]
        public void CreateMajor_ReturnsBadRequest_DuplicateCode()
        {
            // Arrange
            var majorRequest = new MajorRequest
            {
                major_code = "CheckDupli",
                major_name = "CheckDuplicateMajorName",
                major_english_name = "CheckDuplicateMajorEnglishName",
                is_active = true,
                degree_level_id = 1
            };

            // Mock the behavior of CheckMajorbyMajorCode to return a duplicate major
            majorRepositoryMock.Setup(m => m.CheckMajorbyMajorCode(It.IsAny<string>(), It.IsAny<int>()))
                           .Returns(new Major());

            // Act
            //majorController.CreateMajor(majorRequest);
            var result = majorController.CreateMajor(majorRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Major Code Duplicate!.", baseResponse.message);
        }

        [Test]
        public async Task DeleteMajor_ReturnsOkResult()
        {
            // Arrange
            var majorTestDelete = new MajorRequest
            {
                major_code = fixture.Create<string>().Substring(0, Math.Min(5, fixture.Create<string>().Length)),
                major_name = "TestMajorName",
                major_english_name = "TestMajorEnglishName",
                is_active = true,
                degree_level_id = 1
            };

            var majorRequest = _mapper.Map<Major>(majorTestDelete);

            // Act
            var createResult = majorController.CreateMajor(majorTestDelete);
            Assert.IsInstanceOf<OkObjectResult>(createResult);


            var okObjectResult = createResult as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;


            var majorId = ((MajorResponse)baseResponse.data).major_id;
            var deleteResult = majorController.DeleteMajor(majorId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(deleteResult);

            var deleteOkObjectResult = deleteResult as OkObjectResult;
            Assert.IsNotNull(deleteOkObjectResult);

            var baseResponse2 = deleteOkObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse2);

            Assert.IsFalse(baseResponse2.error);
            Assert.AreEqual("Delete Major Successfully!", baseResponse2.message);
        }



        [Test]
        public async Task DeleteMajor_ReturnsNotFoundResult()
        {
            // Arrange
            int majorId = 999;

            majorRepositoryMock.Setup(repo => repo.FindMajorById(It.IsAny<int>())).Returns((Major)null);

            // Act
            var result = majorController.DeleteMajor(majorId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var objectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(objectResult);

            var baseResponse = objectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found This Major!", baseResponse.message);
        }

    }
}
