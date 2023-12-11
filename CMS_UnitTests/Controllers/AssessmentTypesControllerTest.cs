using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.AssessmentTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class AssessmentTypesControllerTests
    {
        private AssessmentTypesController _assessmentTypesController;
        private IMapper _mapper;
        private Mock<IAssessmentTypeRepository> _assessmentTypeRepositoryMock;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _assessmentTypeRepositoryMock = new Mock<IAssessmentTypeRepository>();
            _assessmentTypesController = new AssessmentTypesController(_mapper);

        }

        [Test]
        public void GetAllAssessmentType_ReturnsOkResultWithListOfAssessmentTypes()
        {
            // Arrange
            var expectedAssessmentTypes = new List<AssessmentType>(); // Provide a list of assessment types
            _assessmentTypeRepositoryMock.Setup(repo => repo.GetAllAssessmentType()).Returns(expectedAssessmentTypes);

            // Act
            var result = _assessmentTypesController.GetAllAssessmentType();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Assessment Method", baseResponse.message);

            var data = okObjectResult.Value as List<AssessmentTypeResponse>;
            Assert.IsNotNull(data);
            Assert.AreEqual(expectedAssessmentTypes.Count, data.Count);
        }

        [Test]
        public void PaginationAssessmentType_ReturnsOkResultWithPagedAssessmentTypes()
        {
            // Arrange
            int page = 1;
            int limit = 10;
            string txtSearch = "someSearchText";
            var expectedAssessmentTypes = new List<AssessmentType>(); // Provide a list of assessment types
            _assessmentTypeRepositoryMock.Setup(repo => repo.PaginationAssessmentType(page, limit, txtSearch)).Returns(expectedAssessmentTypes);
            _assessmentTypeRepositoryMock.Setup(repo => repo.GetTotalAssessmentType(txtSearch)).Returns(expectedAssessmentTypes.Count);

            // Act
            var result = _assessmentTypesController.PaginationAssessmentType(page, limit, txtSearch);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Assessment Method", baseResponse.message);

            var data = okObjectResult.Value as BaseListResponse;
            Assert.IsNotNull(data);
            Assert.AreEqual(page, data.page);
            Assert.AreEqual(limit, data.limit);
            Assert.AreEqual(expectedAssessmentTypes.Count, data.totalElement);
            Assert.AreEqual(expectedAssessmentTypes, data.data);
        }

        [Test]
        public void GetAssessmentType_WithValidId_ReturnsOkResultWithAssessmentType()
        {
            // Arrange
            int validAssessmentTypeId = 1;
            var expectedAssessmentType = new AssessmentType(); // Provide an assessment type with the given ID
            _assessmentTypeRepositoryMock.Setup(repo => repo.GetAsssentTypeById(validAssessmentTypeId)).Returns(expectedAssessmentType);

            // Act
            var result = _assessmentTypesController.GetAssessmentType(validAssessmentTypeId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);

            var data = okObjectResult.Value as AssessmentTypeResponse;
            Assert.IsNotNull(data);
        }

        [Test]
        public void GetAssessmentType_WithInvalidId_ReturnsNotFoundResultWithMessage()
        {
            // Arrange
            int invalidAssessmentTypeId = 0;
            _assessmentTypeRepositoryMock.Setup(repo => repo.GetAsssentTypeById(invalidAssessmentTypeId)).Returns((AssessmentType)null);

            // Act
            var result = _assessmentTypesController.GetAssessmentType(invalidAssessmentTypeId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var baseResponse = notFoundObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Assessment Type!", baseResponse.message);
        }

        [Test]
        public void CreateAssessmentType_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var assessmentTypeRequest = new AssessmentTypeRequest(); // Provide a valid request object
            _assessmentTypeRepositoryMock.Setup(repo => repo.CheckAssmentTypeDuplicate(0, It.IsAny<string>())).Returns(false);
            _assessmentTypeRepositoryMock.Setup(repo => repo.CreateAssessmentType(It.IsAny<AssessmentType>())).Returns(Result.createSuccessfull.ToString());

            // Act
            var result = _assessmentTypesController.CreateAssessmentType(assessmentTypeRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Create Success!", baseResponse.message);

            var responseData = okObjectResult.Value as AssessmentTypeRequest;
            Assert.IsNotNull(responseData);
            Assert.AreEqual(assessmentTypeRequest, responseData);
        }

        [Test]
        public void CreateAssessmentType_WithDuplicateAssessmentType_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            var assessmentTypeRequest = new AssessmentTypeRequest(); // Provide a valid request object
            _assessmentTypeRepositoryMock.Setup(repo => repo.CheckAssmentTypeDuplicate(0, It.IsAny<string>())).Returns(true);

            // Act
            var result = _assessmentTypesController.CreateAssessmentType(assessmentTypeRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Assessment Type is duplicate!", baseResponse.message);
        }

        [Test]
        public void UpdateAssessmentType_WithValidIdAndData_ReturnsOkResult()
        {
            // Arrange
            int validAssessmentTypeId = 1;
            var assessmentTypeRequest = new AssessmentTypeRequest(); // Provide a valid request object
            var existingAssessmentType = new AssessmentType(); // Provide an existing assessment type with the given ID
            _assessmentTypeRepositoryMock.Setup(repo => repo.GetAsssentTypeById(validAssessmentTypeId)).Returns(existingAssessmentType);
            _assessmentTypeRepositoryMock.Setup(repo => repo.CheckAssmentTypeDuplicate(validAssessmentTypeId, It.IsAny<string>())).Returns(false);
            _assessmentTypeRepositoryMock.Setup(repo => repo.UpdateAssessmentType(It.IsAny<AssessmentType>())).Returns(Result.updateSuccessfull.ToString());

            // Act
            var result = _assessmentTypesController.UpdateAssessmentType(validAssessmentTypeId, assessmentTypeRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Update Success!", baseResponse.message);

            var responseData = okObjectResult.Value as AssessmentTypeRequest;
            Assert.IsNotNull(responseData);
            Assert.AreEqual(assessmentTypeRequest, responseData);
        }

        [Test]
        public void UpdateAssessmentType_WithInvalidId_ReturnsNotFoundResultWithMessage()
        {
            // Arrange
            int invalidAssessmentTypeId = 0;
            _assessmentTypeRepositoryMock.Setup(repo => repo.GetAsssentTypeById(invalidAssessmentTypeId)).Returns((AssessmentType)null);

            // Act
            var result = _assessmentTypesController.UpdateAssessmentType(invalidAssessmentTypeId, new AssessmentTypeRequest());

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var baseResponse = notFoundObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Assessment Type!", baseResponse.message);
        }

        [Test]
        public void UpdateAssessmentType_WithDuplicateAssessmentType_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            int validAssessmentTypeId = 1;
            var assessmentTypeRequest = new AssessmentTypeRequest(); // Provide a valid request object
            var existingAssessmentType = new AssessmentType(); // Provide an existing assessment type with the given ID
            _assessmentTypeRepositoryMock.Setup(repo => repo.GetAsssentTypeById(validAssessmentTypeId)).Returns(existingAssessmentType);
            _assessmentTypeRepositoryMock.Setup(repo => repo.CheckAssmentTypeDuplicate(validAssessmentTypeId, It.IsAny<string>())).Returns(true);

            // Act
            var result = _assessmentTypesController.UpdateAssessmentType(validAssessmentTypeId, assessmentTypeRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Assessment Type is duplicate!", baseResponse.message);
        }

        [Test]
        public void DeleteAssessmentType_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int validAssessmentTypeId = 1;
            var existingAssessmentType = new AssessmentType(); // Provide an existing assessment type with the given ID
            _assessmentTypeRepositoryMock.Setup(repo => repo.GetAsssentTypeById(validAssessmentTypeId)).Returns(existingAssessmentType);
            _assessmentTypeRepositoryMock.Setup(repo => repo.CheckAssmentTypeExsit(validAssessmentTypeId)).Returns(false);
            _assessmentTypeRepositoryMock.Setup(repo => repo.DeleteAssessmentType(It.IsAny<AssessmentType>())).Returns(Result.deleteSuccessfull.ToString());

            // Act
            var result = _assessmentTypesController.DeleteAssessmentType(validAssessmentTypeId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Delete Success!", baseResponse.message);

            var responseData = okObjectResult.Value as AssessmentTypeResponse;
            Assert.IsNotNull(responseData);
            Assert.AreEqual(_mapper.Map<AssessmentTypeResponse>(existingAssessmentType), responseData);
        }

        [Test]
        public void DeleteAssessmentType_WithInvalidId_ReturnsNotFoundResultWithMessage()
        {
            // Arrange
            int invalidAssessmentTypeId = 0;
            _assessmentTypeRepositoryMock.Setup(repo => repo.GetAsssentTypeById(invalidAssessmentTypeId)).Returns((AssessmentType)null);

            // Act
            var result = _assessmentTypesController.DeleteAssessmentType(invalidAssessmentTypeId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var baseResponse = notFoundObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Assessment Type!", baseResponse.message);
        }

        [Test]
        public void DeleteAssessmentType_WhenAssessmentTypeUsed_ReturnsBadRequestWithErrorMessage()
        {
            // Arrange
            int validAssessmentTypeId = 1;
            var existingAssessmentType = new AssessmentType();
            _assessmentTypeRepositoryMock.Setup(repo => repo.GetAsssentTypeById(validAssessmentTypeId)).Returns(existingAssessmentType);
            _assessmentTypeRepositoryMock.Setup(repo => repo.CheckAssmentTypeExsit(validAssessmentTypeId)).Returns(true);

            // Act
            var result = _assessmentTypesController.DeleteAssessmentType(validAssessmentTypeId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Assessment Type is Used by Assessment Method. Can't Delete!", baseResponse.message);
        }

    }
}
