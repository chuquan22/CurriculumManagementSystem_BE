using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.LearningMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class LearningMethodsControllerTests
    {
        private LearningMethodsController _learningMethodsController;
        private IMapper _mapper;
        private Mock<ILearnningMethodRepository> _learningMethodRepositoryMock;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _learningMethodRepositoryMock = new Mock<ILearnningMethodRepository>();
            _learningMethodsController = new LearningMethodsController(_mapper);

        }

        [Test]
        public void GetAllLearningMethod_ReturnsOkResultWithListOfLearningMethods()
        {
            // Arrange
            var expectedLearningMethods = new List<LearningMethod>();
            _learningMethodRepositoryMock.Setup(repo => repo.GetAllLearningMethods()).Returns(expectedLearningMethods);

            // Act
            var result = _learningMethodsController.GetAllLearningMethod();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Learning Method", baseResponse.message);

            
        }

        [Test]
        public void GetLearningMethod_WithValidId_ReturnsOkResultWithLearningMethod()
        {
            // Arrange
            int validLearningMethodId = 1;
            var expectedLearningMethod = new LearningMethod();
            _learningMethodRepositoryMock.Setup(repo => repo.GetLearningMethodById(validLearningMethodId)).Returns(expectedLearningMethod);

            // Act
            var result = _learningMethodsController.GetLearningMethod(validLearningMethodId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Success!", baseResponse.message);

           
        }

        [Test]
        public void PaginationLearningMethod_ReturnsOkResultWithPagedLearningMethods()
        {
            // Arrange
            int page = 1;
            int limit = 10;
            string txtSearch = "a";
            var expectedLearningMethods = new List<LearningMethod>(); // Provide a list of learning methods
            _learningMethodRepositoryMock.Setup(repo => repo.PaginationLearningMethod(page, limit, txtSearch)).Returns(expectedLearningMethods);
            _learningMethodRepositoryMock.Setup(repo => repo.GetTotalLearningMethod(txtSearch)).Returns(expectedLearningMethods.Count);

            // Act
            var result = _learningMethodsController.PaginationLearningMethod(page, limit, txtSearch);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Learning Method", baseResponse.message);

            var data = baseResponse.data as BaseListResponse;
            Assert.IsNotNull(data);
            Assert.AreEqual(page, data.page);
            Assert.AreEqual(limit, data.limit);
            Assert.NotNull(data.data);
        }

        [Test]
        public void CreateLearningMethod_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var learningMethodRequest = new LearningMethodRequest { learning_method_name = "Test name"}; // Provide a valid request object
            _learningMethodRepositoryMock.Setup(repo => repo.CheckLearningMethodDuplicate(0, It.IsAny<string>())).Returns(false);
            _learningMethodRepositoryMock.Setup(repo => repo.CreateLearningMethod(It.IsAny<LearningMethod>())).Returns(Result.createSuccessfull.ToString());

            // Act
            var result = _learningMethodsController.CreateLearningMethod(learningMethodRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Create Learning Method Success!", baseResponse.message);

        }

        [Test]
        public void CreateLearningMethod_WithDuplicateLearningMethod_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            var learningMethodRequest = new LearningMethodRequest { learning_method_name = "Online" }; // Provide a valid request object
            _learningMethodRepositoryMock.Setup(repo => repo.CheckLearningMethodDuplicate(0, It.IsAny<string>())).Returns(true);

            // Act
            var result = _learningMethodsController.CreateLearningMethod(learningMethodRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Learning Method is Duplicate!", baseResponse.message);
        }

        [Test]
        public void UpdateLearningMethod_WithValidData_ReturnsOkResult()
        {
            // Arrange
            int validLearningMethodId = 1;
            var learningMethodRequest = new LearningMethodRequest(); // Provide a valid request object
            var existingLearningMethod = new LearningMethod(); // Provide an existing learning method with the given ID
            _learningMethodRepositoryMock.Setup(repo => repo.GetLearningMethodById(validLearningMethodId)).Returns(existingLearningMethod);
            _learningMethodRepositoryMock.Setup(repo => repo.CheckLearningMethodDuplicate(validLearningMethodId, It.IsAny<string>())).Returns(false);
            _learningMethodRepositoryMock.Setup(repo => repo.UpdateLearningMethod(It.IsAny<LearningMethod>())).Returns(Result.updateSuccessfull.ToString());

            // Act
            var result = _learningMethodsController.UpdateLearningMethod(validLearningMethodId, learningMethodRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Update Learning Method Success!", baseResponse.message);
           
        }

        [Test]
        public void UpdateLearningMethod_WithInvalidId_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            int invalidLearningMethodId = 0;
            var learningMethodRequest = new LearningMethodRequest(); // Provide a valid request object
            _learningMethodRepositoryMock.Setup(repo => repo.GetLearningMethodById(invalidLearningMethodId)).Returns((LearningMethod)null);

            // Act
            var result = _learningMethodsController.UpdateLearningMethod(invalidLearningMethodId, learningMethodRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Learning Method!", baseResponse.message);
        }

        [Test]
        public void UpdateLearningMethod_WithDuplicateLearningMethod_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            int validLearningMethodId = 1;
            var learningMethodRequest = new LearningMethodRequest { learning_method_name = "Test"}; // Provide a valid request object
            var existingLearningMethod = new LearningMethod(); // Provide an existing learning method with the given ID
            _learningMethodRepositoryMock.Setup(repo => repo.GetLearningMethodById(validLearningMethodId)).Returns(existingLearningMethod);
            _learningMethodRepositoryMock.Setup(repo => repo.CheckLearningMethodDuplicate(validLearningMethodId, It.IsAny<string>())).Returns(true);

            // Act
            var result = _learningMethodsController.UpdateLearningMethod(validLearningMethodId, learningMethodRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Learning Method is Duplicate!", baseResponse.message);
        }

        [Test]
        public void DeleteLearningMethod_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int validLearningMethodId = 141;
            var existingLearningMethod = new LearningMethod(); // Provide an existing learning method with the given ID
            _learningMethodRepositoryMock.Setup(repo => repo.GetLearningMethodById(validLearningMethodId)).Returns(existingLearningMethod);
            _learningMethodRepositoryMock.Setup(repo => repo.CheckLearningMethodExsit(validLearningMethodId)).Returns(false);
            _learningMethodRepositoryMock.Setup(repo => repo.DeleteLearningMethod(It.IsAny<LearningMethod>())).Returns(Result.deleteSuccessfull.ToString());

            // Act
            var result = _learningMethodsController.DeleteLearningMethod(validLearningMethodId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Delete Learning Method Success!", baseResponse.message);

        }

        [Test]
        public void DeleteLearningMethod_WithInvalidId_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            int invalidLearningMethodId = 0;
            _learningMethodRepositoryMock.Setup(repo => repo.GetLearningMethodById(invalidLearningMethodId)).Returns((LearningMethod)null);

            // Act
            var result = _learningMethodsController.DeleteLearningMethod(invalidLearningMethodId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Learning Method!", baseResponse.message);
        }

        [Test]
        public void DeleteLearningMethod_WithExistingLearningMethodInUse_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            int validLearningMethodId = 1;
            var existingLearningMethod = new LearningMethod(); // Provide an existing learning method with the given ID
            _learningMethodRepositoryMock.Setup(repo => repo.GetLearningMethodById(validLearningMethodId)).Returns(existingLearningMethod);
            _learningMethodRepositoryMock.Setup(repo => repo.CheckLearningMethodExsit(validLearningMethodId)).Returns(true);

            // Act
            var result = _learningMethodsController.DeleteLearningMethod(validLearningMethodId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
        }
    }

}
