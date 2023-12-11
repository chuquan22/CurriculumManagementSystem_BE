using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.LearningResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class LearningResourcesControllerTest
    {
        private LearningResourcesController _learningResourcesController;
        private IMapper _mapper;
        private Mock<ILearningResourceRepository> _learningResourceRepositoryMock;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _learningResourceRepositoryMock = new Mock<ILearningResourceRepository>();
            _learningResourcesController = new LearningResourcesController(_mapper);
        }

        [Test]
        public void GetLearningResource_ReturnsOkResultWithListOfLearningResources()
        {
            // Arrange
            var expectedLearningResources = new List<LearningResource>(); // Provide a list of learning resources
            _learningResourceRepositoryMock.Setup(repo => repo.GetLearningResource()).Returns(expectedLearningResources);

            // Act
            var result = _learningResourcesController.GetLearningResource();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);

            var data = okObjectResult.Value as List<LearningResource>;
            Assert.IsNotNull(data);
            Assert.AreEqual(expectedLearningResources.Count, data.Count);
        }

        [Test]
        public void GetLearningResource_WithValidId_ReturnsOkResultWithLearningResource()
        {
            // Arrange
            int validLearningResourceId = 1;
            var expectedLearningResource = new LearningResource(); // Provide a learning resource with the given ID
            _learningResourceRepositoryMock.Setup(repo => repo.GetLearningResource(validLearningResourceId)).Returns(expectedLearningResource);

            // Act
            var result = _learningResourcesController.GetLearningResourceById(validLearningResourceId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Success", baseResponse.message);

            var data = okObjectResult.Value as LearningResource;
            Assert.IsNotNull(data);
            Assert.AreEqual(expectedLearningResource, data);
        }

        [Test]
        public void GetLearningResource_WithInvalidId_ReturnsNotFoundResultWithMessage()
        {
            // Arrange
            int invalidLearningResourceId = 0;
            _learningResourceRepositoryMock.Setup(repo => repo.GetLearningResource(invalidLearningResourceId)).Returns((LearningResource)null);

            // Act
            var result = _learningResourcesController.GetLearningResourceById(invalidLearningResourceId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var baseResponse = notFoundObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Learning Resource!", baseResponse.message);
        }

        [Test]
        public void PaginationLearningResource_ReturnsOkResultWithPagedLearningResources()
        {
            // Arrange
            int page = 1;
            int limit = 10;
            string txtSearch = "someSearchText";
            var expectedLearningResources = new List<LearningResource>(); // Provide a list of learning resources
            _learningResourceRepositoryMock.Setup(repo => repo.PaginationLearningResource(page, limit, txtSearch)).Returns(expectedLearningResources);
            _learningResourceRepositoryMock.Setup(repo => repo.GetTotalLearningResource(txtSearch)).Returns(expectedLearningResources.Count);

            // Act
            var result = _learningResourcesController.PaginationLearningResource(page, limit, txtSearch);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Learning Resource", baseResponse.message);

            var data = okObjectResult.Value as BaseListResponse;
            Assert.IsNotNull(data);
            Assert.AreEqual(page, data.page);
            Assert.AreEqual(limit, data.limit);
            Assert.AreEqual(expectedLearningResources.Count, data.totalElement);
            Assert.AreEqual(expectedLearningResources, data.data);
        }

        [Test]
        public void CreateLearningResource_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var learningResourceRequest = new LearningResourceRequest(); // Provide a valid request object
            _learningResourceRepositoryMock.Setup(repo => repo.CheckLearningResourceDuplicate(0, It.IsAny<string>())).Returns(false);
            _learningResourceRepositoryMock.Setup(repo => repo.CreateLearningResource(It.IsAny<LearningResource>())).Returns(Result.createSuccessfull.ToString());

            // Act
            var result = _learningResourcesController.CreateLearningResource(learningResourceRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Create Learning Resource Success!", baseResponse.message);

            var responseData = okObjectResult.Value as LearningResourceRequest;
            Assert.IsNotNull(responseData);
            Assert.AreEqual(learningResourceRequest, responseData);
        }

        [Test]
        public void CreateLearningResource_WithDuplicateLearningResource_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            var learningResourceRequest = new LearningResourceRequest(); // Provide a valid request object
            _learningResourceRepositoryMock.Setup(repo => repo.CheckLearningResourceDuplicate(0, It.IsAny<string>())).Returns(true);

            // Act
            var result = _learningResourcesController.CreateLearningResource(learningResourceRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Learning Resource is Duplicate!", baseResponse.message);
        }

        [Test]
        public void UpdateLearningResource_WithValidIdAndData_ReturnsOkResult()
        {
            // Arrange
            int validLearningResourceId = 1;
            var learningResourceRequest = new LearningResourceRequest(); // Provide a valid request object
            var existingLearningResource = new LearningResource(); // Provide an existing learning resource with the given ID
            _learningResourceRepositoryMock.Setup(repo => repo.GetLearningResource(validLearningResourceId)).Returns(existingLearningResource);
            _learningResourceRepositoryMock.Setup(repo => repo.CheckLearningResourceDuplicate(validLearningResourceId, It.IsAny<string>())).Returns(false);
            _learningResourceRepositoryMock.Setup(repo => repo.UpdateLearningResource(It.IsAny<LearningResource>())).Returns(Result.updateSuccessfull.ToString());

            // Act
            var result = _learningResourcesController.UpdateLearningResource(validLearningResourceId, learningResourceRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Update Learning Resource Success!", baseResponse.message);

            var responseData = okObjectResult.Value as LearningResourceRequest;
            Assert.IsNotNull(responseData);
            Assert.AreEqual(learningResourceRequest, responseData);
        }

        [Test]
        public void UpdateLearningResource_WithInvalidId_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            int invalidLearningResourceId = 0;
            _learningResourceRepositoryMock.Setup(repo => repo.GetLearningResource(invalidLearningResourceId)).Returns((LearningResource)null);

            // Act
            var result = _learningResourcesController.UpdateLearningResource(invalidLearningResourceId, new LearningResourceRequest());

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Learning Resource!", baseResponse.message);
        }

        [Test]
        public void UpdateLearningResource_WithDuplicateLearningResource_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            int validLearningResourceId = 1;
            var learningResourceRequest = new LearningResourceRequest(); // Provide a valid request object
            var existingLearningResource = new LearningResource(); // Provide an existing learning resource with the given ID
            _learningResourceRepositoryMock.Setup(repo => repo.GetLearningResource(validLearningResourceId)).Returns(existingLearningResource);
            _learningResourceRepositoryMock.Setup(repo => repo.CheckLearningResourceDuplicate(validLearningResourceId, It.IsAny<string>())).Returns(true);

            // Act
            var result = _learningResourcesController.UpdateLearningResource(validLearningResourceId, learningResourceRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Learning Resource is Duplicate!", baseResponse.message);
        }

        [Test]
        public void RemoveLearningResource_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int validLearningResourceId = 1;
            var existingLearningResource = new LearningResource(); // Provide an existing learning resource with the given ID
            _learningResourceRepositoryMock.Setup(repo => repo.GetLearningResource(validLearningResourceId)).Returns(existingLearningResource);
            _learningResourceRepositoryMock.Setup(repo => repo.CheckLearningResourceExsit(validLearningResourceId)).Returns(false);
            _learningResourceRepositoryMock.Setup(repo => repo.DeleteLearningResource(It.IsAny<LearningResource>())).Returns(Result.deleteSuccessfull.ToString());

            // Act
            var result = _learningResourcesController.RemoveLearningResource(validLearningResourceId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual($"Remove Learning Resource {existingLearningResource.learning_resource_type} Success!", baseResponse.message);

            var responseData = okObjectResult.Value as LearningResource;
            Assert.IsNotNull(responseData);
            Assert.AreEqual(existingLearningResource, responseData);
        }

        [Test]
        public void RemoveLearningResource_WithInvalidId_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            int invalidLearningResourceId = 0;
            _learningResourceRepositoryMock.Setup(repo => repo.GetLearningResource(invalidLearningResourceId)).Returns((LearningResource)null);

            // Act
            var result = _learningResourcesController.RemoveLearningResource(invalidLearningResourceId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Learning Resource!", baseResponse.message);
        }

        [Test]
        public void RemoveLearningResource_WithExistingLearningResourceInUse_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            int validLearningResourceId = 1;
            var existingLearningResource = new LearningResource(); // Provide an existing learning resource with the given ID
            _learningResourceRepositoryMock.Setup(repo => repo.GetLearningResource(validLearningResourceId)).Returns(existingLearningResource);
            _learningResourceRepositoryMock.Setup(repo => repo.CheckLearningResourceExsit(validLearningResourceId)).Returns(true);

            // Act
            var result = _learningResourcesController.RemoveLearningResource(validLearningResourceId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual($"Learning Resource Used by Material. Can't Delete!", baseResponse.message);
        }
    }

}
