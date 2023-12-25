using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using AutoMapper;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Repositories.CLOS;
using DataAccess.Models.DTO;
using BusinessObject;
using AutoFixture;

namespace CMS_UnitTests.Controllers
{
    public class CLOsControllerTest
    {
        private Mock<IMapper> mapperMock;
        private Mock<ICLORepository> cloRepositoryMock;
        private IMapper _mapper;
        private CLOsController closController;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();

            mapperMock = new Mock<IMapper>();
            cloRepositoryMock = new Mock<ICLORepository>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
            closController = new CLOsController(_mapper);
        }

        [Test]
        public void GetCLOs_WithValidSyllabusId_ReturnsOkResult()
        {
            // Arrange
            int syllabusId = 1;
            var listCLOs = new List<CLO>();
            cloRepositoryMock.Setup(repo => repo.GetCLOs(syllabusId)).Returns(listCLOs);
            var expectedResponse = new BaseResponse(false, "Sucessfully", listCLOs);

            // Act
            var result = closController.GetCLOs(syllabusId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);
        }

        [Test]
        public void GetCLOs_WithInvalidSyllabusId_ReturnsBadRequestResult()
        {
            // Arrange
            int syllabusId = 1;
            cloRepositoryMock.Setup(repo => repo.GetCLOs(syllabusId)).Throws(new Exception("Error message"));

            // Act
            var result = closController.GetCLOs(2);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("error", baseResponse.message);
        }

        [Test]
        public void CreateCLOs_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var closRequest = new CLOsRequest()
            {
                CLO_name = "CLO" + fixture.Create<int>(),
                CLO_description = "Sample Description",
                syllabus_id = 2
            };
            var clo = new CLO();
            mapperMock.Setup(mapper => mapper.Map<CLO>(closRequest)).Returns(clo);
            cloRepositoryMock.Setup(repo => repo.CreateCLOs(clo)).Returns(clo);
            var expectedResponse = new BaseResponse(false, "Sucessfully", clo);

            // Act
            var result = closController.CreateCLOs(closRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);
        }

        [Test]
        public void CreateCLOs_WithFailedCreation_ReturnsBadRequestResult()
        {
            // Arrange
            var closRequest = new CLOsRequest();
            var clo = new CLO();
            mapperMock.Setup(mapper => mapper.Map<CLO>(closRequest)).Returns(clo);
            cloRepositoryMock.Setup(repo => repo.CreateCLOs(clo)).Returns(null as CLO);

            // Act
            var result = closController.CreateCLOs(closRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Error: Object reference not set to an instance of an object.", baseResponse.message);
        }

        [Test]
        public void CreateCLOs_WithException_ReturnsBadRequestResult()
        {
            // Arrange
            var closRequest = new CLOsRequest();
            mapperMock.Setup(mapper => mapper.Map<CLO>(closRequest)).Throws(new Exception("Object reference not set to an instance of an object."));

            // Act
            var result = closController.CreateCLOs(closRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Object reference not set to an instance of an object.", baseResponse.message);
        }

        [Test]
        public void UpdateCLOs_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var cloUpdateRequest = new CLOsUpdateRequest();
            var clo = new CLO();
            mapperMock.Setup(mapper => mapper.Map<CLO>(cloUpdateRequest)).Returns(clo);
            cloRepositoryMock.Setup(repo => repo.UpdateCLOs(clo)).Returns(clo);
            var expectedResponse = new BaseResponse(false, "Sucessfully", clo);

            // Act
            var result = closController.UpdateCLOs(cloUpdateRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);

            Assert.AreEqual(expectedResponse, okObjectResult.Value);
        }

        [Test]
        public void UpdateCLOs_WithFailedUpdate_ReturnsBadRequestResult()
        {
            // Arrange
            var cloUpdateRequest = new CLOsUpdateRequest();
            var clo = new CLO();
            mapperMock.Setup(mapper => mapper.Map<CLO>(cloUpdateRequest)).Returns(clo);
            cloRepositoryMock.Setup(repo => repo.UpdateCLOs(clo)).Returns(null as CLO);

            // Act
            var result = closController.UpdateCLOs(cloUpdateRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Error: Object reference not set to an instance of an object.", baseResponse.message);
        }

        [Test]
        public void UpdateCLOs_WithException_ReturnsBadRequestResult()
        {
            // Arrange
            var cloUpdateRequest = new CLOsUpdateRequest();
            var clo = new CLO();
            mapperMock.Setup(mapper => mapper.Map<CLO>(cloUpdateRequest)).Throws(new Exception("Error message"));

            // Act
            var result = closController.UpdateCLOs(cloUpdateRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Error: Error message", baseResponse.message);
        }

        [Test]
        public void DeleteCLOs_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int cloId = 1;
            var clo = new CLO();
            cloRepositoryMock.Setup(repo => repo.DeleteCLOs(cloId)).Returns(clo);
            var expectedResponse = new BaseResponse(false, "Sucessfully", clo);

            // Act
            var result = closController.DeleteCLOs(cloId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);

            Assert.AreEqual(expectedResponse, okObjectResult.Value);
        }

        [Test]
        public void DeleteCLOs_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            int cloId = 1;
            cloRepositoryMock.Setup(repo => repo.DeleteCLOs(cloId)).Throws(new Exception("Error message"));

            // Act
            var result = closController.DeleteCLOs(2);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Error: Error message", baseResponse.message);
        }

        [Test]
        public void GetCLOsById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int cloId = 1;
            var clo = new CLO();
            cloRepositoryMock.Setup(repo => repo.GetCLOsById(cloId)).Returns(clo);
            var expectedResponse = new BaseResponse(false, "Sucessfully", clo);

            // Act
            var result = closController.GetCLOsById(cloId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);
        }

        [Test]
        public void GetCLOsById_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            int cloId = 1;
            cloRepositoryMock.Setup(repo => repo.GetCLOsById(cloId)).Throws(new Exception("Error message"));

            // Act
            var result = closController.GetCLOsById(2);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("error", baseResponse.message);
        }
    }

}
