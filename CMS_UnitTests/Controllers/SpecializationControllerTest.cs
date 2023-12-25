using AutoFixture;
using AutoMapper;
using AutoMapper.Configuration;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.Specialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    public class SpecializationControllerTest
    {
        private Mock<ISpecializationRepository> specializationRepositoryMock;

        private Mock<IMapper> mapperMock;
        private IMapper _mapper;
        private Mock<IWebHostEnvironment> hostingEnvironmentMock;
        private SpecializationController specializationController;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            specializationRepositoryMock = fixture.Freeze<Mock<ISpecializationRepository>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            mapperMock = new Mock<IMapper>();
            hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            specializationController = new SpecializationController( _mapper);
        }
        [Test]
        public async Task GetListAllSpecialization_ReturnsOkResponse()
        {
            // Arrange


            var listSpecialization = new List<Specialization>();
            var listSpecializationResponse = new List<SpecializationResponse>();

            specializationRepositoryMock.Setup(repo => repo.GetSpecialization())
                .Returns(listSpecialization);

            mapperMock.Setup(mapper => mapper.Map<List<SpecializationResponse>>(listSpecialization)).Returns(listSpecializationResponse);

            // Act
            var result = specializationController.GetListAllSpecialization();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsEmpty(listSpecializationResponse);
        }

        [Test]
        public async Task GetListSpecialization_ReturnsOkResponse()
        {
            // Arrange
            int page = 1;
            int limit = 1;
            string txtSeach = "a";
            string major_id = "1";

            var listSpecialization = new List<Specialization>();
            var listSpecializationResponse = new List<SpecializationResponse>();

            specializationRepositoryMock.Setup(repo => repo.GetListSpecialization(page, limit, txtSeach, major_id))
                .Returns(listSpecialization);

            mapperMock.Setup(mapper => mapper.Map<List<SpecializationResponse>>(listSpecialization)).Returns(listSpecializationResponse);

            // Act
            var result = specializationController.GetListSpecialization(page, limit, txtSeach, major_id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsEmpty(listSpecializationResponse);
        }

        [Test]
        public async Task GetSpecialization_ReturnsOkResponse()
        {
            // Arrange
            int specializationId = 1;
            var specialization = new Specialization();

            specializationRepositoryMock.Setup(repo => repo.GetSpeById(specializationId))
                .Returns(specialization);

            // Act
            var result = specializationController.GetSpecialization(specializationId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!!", baseResponse.message);

            var specializationResponse = baseResponse.data as Specialization;
            Assert.IsNotNull(specializationResponse);
        }
        [Test]
        public async Task GetSpecialization_ReturnsNotFoundResponse()
        {
            // Arrange
            int nonExistSpecializationId = 9999;

            specializationRepositoryMock.Setup(repo => repo.GetSpeById(nonExistSpecializationId))
                .Returns((Specialization)null);

            // Act
            var result = specializationController.GetSpecialization(nonExistSpecializationId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found This Specialization!", baseResponse.message);
            Assert.IsNull(baseResponse.data);
        }
        [Test]
        public async Task CreateSpecialization_ReturnsOkResponse()
        {
            // Arrange


            var specializationRequest = new SpecializationRequest
            {
                specialization_code = fixture.Create<string>(),
                specialization_name = fixture.Create<string>(),
                specialization_english_name = fixture.Create<string>(),
                major_id = 1,
                semester_id = 1,
                is_active = true
            };

            specializationRepositoryMock.Setup(repo => repo.IsCodeExist(specializationRequest.specialization_code))
                .Returns(false);

            specializationRepositoryMock.Setup(repo => repo.CreateSpecialization(It.IsAny<Specialization>()))
                .Returns(new Specialization());

            // Act
            var result = specializationController.CreateSpecialization(specializationRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);
            Assert.IsNotNull(baseResponse.data);
        }
        [Test]
        public async Task CreateSpecialization_ReturnsBadRequestResponse_SpecializationCodeExists()
        {
            // Arrange
            var listSpecialization = new List<Specialization>
               {
                   new Specialization
                             {
                                specialization_code = "ExistingCode",
                                specialization_name = "Existing Specialization",
                                specialization_english_name = "Existing English Specialization",
                                major_id = 1,
                                semester_id = 1,
                                is_active = true
                                 }
                  };

            specializationRepositoryMock.Setup(repo => repo.GetSpecialization())
                .Returns(listSpecialization);

            var code = listSpecialization[0].specialization_code;

            var specializationRequest = new SpecializationRequest
            {
                specialization_code = code,
                specialization_name = "Sample Specialization",
                specialization_english_name = "Sample English Specialization",
                major_id = 1,
                semester_id = 1,
                is_active = true
            };

            specializationRepositoryMock.Setup(repo => repo.IsCodeExist(specializationRequest.specialization_code))
                .Returns(true);

            // Act
            var result = specializationController.CreateSpecialization(specializationRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);

            var baseResponse = badRequestResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Specialization code already exists in the system!", baseResponse.message);
            Assert.AreEqual(baseResponse.data, specializationRequest.specialization_code);
        }

        [Test]
        public async Task CreateSpecialization_ReturnsBadRequestResponse_Exception()
        {
            // Arrange
            var specializationRequest = new SpecializationRequest
            {
                specialization_code = "ABC123",
                specialization_name = "Sample Specialization",
                specialization_english_name = "Sample English Specialization",
                major_id = 1,
                semester_id = 1,
                is_active = true
            };

            specializationRepositoryMock.Setup(repo => repo.IsCodeExist(specializationRequest.specialization_code))
                .Returns(false);

            specializationRepositoryMock.Setup(repo => repo.CreateSpecialization(It.IsAny<Specialization>()))
                .Throws(new Exception("Specialization code already exists in the system!"));

            // Act
            var result = specializationController.CreateSpecialization(specializationRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);

            var baseResponse = badRequestResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Specialization code already exists in the system!", baseResponse.message);
            Assert.IsNotNull(baseResponse.data);
        }
    }
}
