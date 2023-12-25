using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.AssessmentMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class AssessmentMethodsControllerTest
    {
        private AssessmentMethodsController _assessmentMethodsController;
        private IMapper _mapper;
        private Mock<IAssessmentMethodRepository> _assessmentMethodRepositoryMock;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _assessmentMethodRepositoryMock = new Mock<IAssessmentMethodRepository>();
            _assessmentMethodsController = new AssessmentMethodsController(_mapper);

        }

        [Test]
        public void GetAllAssessmentMethod_ReturnsOkResultWithListOfAssessmentMethods()
        {
            // Arrange
            var expectedAssessmentMethods = new List<AssessmentMethod>(); // Provide a list of assessment methods
            _assessmentMethodRepositoryMock.Setup(repo => repo.GetAllAssessmentMethod()).Returns(expectedAssessmentMethods);

            // Act
            var result = _assessmentMethodsController.GetAllAssessmentMethod();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Assessment Method", baseResponse.message);
            
        }

        [Test]
        public void GetAssessmentMethod_WithValidId_ReturnsOkResultWithAssessmentMethod()
        {
            // Arrange
            int validAssessmentMethodId = 1;
            var expectedAssessmentMethod = new AssessmentMethod();
            _assessmentMethodRepositoryMock.Setup(repo => repo.GetAsssentMethodById(validAssessmentMethodId)).Returns(expectedAssessmentMethod);

            // Act
            var result = _assessmentMethodsController.GetAssessmentMethod(validAssessmentMethodId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Assessment Method", baseResponse.message);
           
        }

        [Test]
        public void GetAssessmentMethod_WithInvalidId_ReturnsNotFoundResultWithMessage()
        {
            // Arrange
            int invalidAssessmentMethodId = 0;
            _assessmentMethodRepositoryMock.Setup(repo => repo.GetAsssentMethodById(invalidAssessmentMethodId)).Returns((AssessmentMethod)null);

            // Act
            var result = _assessmentMethodsController.GetAssessmentMethod(invalidAssessmentMethodId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var notFoundObjectResult = result as OkObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var baseResponse = notFoundObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsNull(baseResponse.data);
        }

        [Test]
        public void PaginationAssessmentMethod_ReturnsOkResultWithPagedAssessmentMethods()
        {
            // Arrange
            int page = 1;
            int limit = 10;
            string txtSearch = "a";
            var expectedAssessmentMethods = new List<AssessmentMethod>(); 
            _assessmentMethodRepositoryMock.Setup(repo => repo.PaginationAssessmentMethod(page, limit, txtSearch)).Returns(expectedAssessmentMethods);
            _assessmentMethodRepositoryMock.Setup(repo => repo.GetTotalAssessmentMethod(txtSearch)).Returns(expectedAssessmentMethods.Count);

            // Act
            var result = _assessmentMethodsController.PaginationAssessmentMethod(page, limit, txtSearch);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Assessment Method", baseResponse.message);

            var data = baseResponse.data as BaseListResponse;
            Assert.IsNotNull(data);
            Assert.AreEqual(page, data.page);
            Assert.AreEqual(limit, data.limit);
            Assert.IsNotNull(data.data);
        }

        [Test]
        public void PaginationAssessmentMethod_NotFounData_ReturnsOkResultWithPagedAssessmentMethods()
        {
            // Arrange
            int page = 1;
            int limit = 10;
            string txtSearch = "abcdef";
            var expectedAssessmentMethods = new List<AssessmentMethod>();
            _assessmentMethodRepositoryMock.Setup(repo => repo.PaginationAssessmentMethod(page, limit, txtSearch)).Returns(expectedAssessmentMethods);
            _assessmentMethodRepositoryMock.Setup(repo => repo.GetTotalAssessmentMethod(txtSearch)).Returns(expectedAssessmentMethods.Count);

            // Act
            var result = _assessmentMethodsController.PaginationAssessmentMethod(page, limit, txtSearch);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Assessment Method", baseResponse.message);

            var data = baseResponse.data as BaseListResponse;
            Assert.IsNotNull(data);
            Assert.AreEqual(page, data.page);
            Assert.AreEqual(limit, data.limit);
        }

        [Test]
        public void CreateAssessmentMethod_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var assessmentMethodRequest = new AssessmentMethodRequest { assessment_method_component = "Test Component", assessment_type_id = 1 }; // Provide a valid request object
            _assessmentMethodRepositoryMock.Setup(repo => repo.CheckAssmentMethodDuplicate(0, It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            _assessmentMethodRepositoryMock.Setup(repo => repo.CreateAssessmentMethod(It.IsAny<AssessmentMethod>())).Returns(Result.createSuccessfull.ToString());

            // Act
            var result = _assessmentMethodsController.CreateAssessmentMethod(assessmentMethodRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Create Success!", baseResponse.message);
        }

        [Test]
        public void CreateAssessmentMethod_WithDuplicateAssessmentMethod_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            var assessmentMethodRequest = new AssessmentMethodRequest { assessment_method_component = "Thi thực hành", assessment_type_id = 1 };
            _assessmentMethodRepositoryMock.Setup(repo => repo.CheckAssmentMethodDuplicate(0, It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            // Act
            var result = _assessmentMethodsController.CreateAssessmentMethod(assessmentMethodRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Assessment Method Duplicate!", baseResponse.message);
        }

        [Test]
        public void UpdateAssessmentMethod_WithValidIdAndData_ReturnsOkResult()
        {
            // Arrange
            int validAssessmentMethodId = 1;
            var assessmentMethodRequest = new AssessmentMethodRequest { assessment_method_component = "update component test", assessment_type_id = 1 };
            var existingAssessmentMethod = new AssessmentMethod();
            _assessmentMethodRepositoryMock.Setup(repo => repo.GetAsssentMethodById(validAssessmentMethodId)).Returns(existingAssessmentMethod);
            _assessmentMethodRepositoryMock.Setup(repo => repo.CheckAssmentMethodDuplicate(validAssessmentMethodId, It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            _assessmentMethodRepositoryMock.Setup(repo => repo.UpdateAssessmentMethod(It.IsAny<AssessmentMethod>())).Returns(Result.updateSuccessfull.ToString());

            // Act
            var result = _assessmentMethodsController.UpdateAssessmentMethod(validAssessmentMethodId, assessmentMethodRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Update Success!", baseResponse.message);
          
        }

        [Test]
        public void UpdateAssessmentMethod_WithInvalidId_ReturnsNotFoundResultWithMessage()
        {
            // Arrange
            int invalidAssessmentMethodId = 0;
            _assessmentMethodRepositoryMock.Setup(repo => repo.GetAsssentMethodById(invalidAssessmentMethodId)).Returns((AssessmentMethod)null);

            // Act
            var result = _assessmentMethodsController.UpdateAssessmentMethod(invalidAssessmentMethodId, new AssessmentMethodRequest());

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var baseResponse = notFoundObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Can't Found Assessment Method", baseResponse.message);
        }

        [Test]
        public void UpdateAssessmentMethod_WithDuplicateAssessmentMethod_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            int validAssessmentMethodId = 1;
            var assessmentMethodRequest = new AssessmentMethodRequest { assessment_method_component = "update component", assessment_type_id = 1 };
            var existingAssessmentMethod = new AssessmentMethod(); // Provide an existing assessment method with the given ID
            _assessmentMethodRepositoryMock.Setup(repo => repo.GetAsssentMethodById(validAssessmentMethodId)).Returns(existingAssessmentMethod);
            _assessmentMethodRepositoryMock.Setup(repo => repo.CheckAssmentMethodDuplicate(validAssessmentMethodId, It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            // Act
            var result = _assessmentMethodsController.UpdateAssessmentMethod(validAssessmentMethodId, assessmentMethodRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Assessment Method Duplicate!", baseResponse.message);
        }

        [Test]
        public void DeleteAssessmentMethod_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int validAssessmentMethodId = 13;
            var existingAssessmentMethod = new AssessmentMethod();
            _assessmentMethodRepositoryMock.Setup(repo => repo.GetAsssentMethodById(validAssessmentMethodId)).Returns(existingAssessmentMethod);
            _assessmentMethodRepositoryMock.Setup(repo => repo.CheckAssmentMethodExsit(validAssessmentMethodId)).Returns(false);
            _assessmentMethodRepositoryMock.Setup(repo => repo.DeleteAssessmentMethod(It.IsAny<AssessmentMethod>())).Returns(Result.deleteSuccessfull.ToString());

            // Act
            var result = _assessmentMethodsController.DeleteAssessmentMethod(validAssessmentMethodId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Delete Success!", baseResponse.message);
            
        }

        [Test]
        public void DeleteAssessmentMethod_WithInvalidId_ReturnsNotFoundResultWithMessage()
        {
            // Arrange
            int invalidAssessmentMethodId = 0;
            _assessmentMethodRepositoryMock.Setup(repo => repo.GetAsssentMethodById(invalidAssessmentMethodId)).Returns((AssessmentMethod)null);

            // Act
            var result = _assessmentMethodsController.DeleteAssessmentMethod(invalidAssessmentMethodId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var baseResponse = notFoundObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Can't Found Assessment Method", baseResponse.message);
        }

        [Test]
        public void DeleteAssessmentMethod_WithExistingAssessmentMethodInUse_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            int validAssessmentMethodId = 1;
            var existingAssessmentMethod = new AssessmentMethod(); // Provide an existing assessment method with the given ID
            _assessmentMethodRepositoryMock.Setup(repo => repo.GetAsssentMethodById(validAssessmentMethodId)).Returns(existingAssessmentMethod);
            _assessmentMethodRepositoryMock.Setup(repo => repo.CheckAssmentMethodExsit(validAssessmentMethodId)).Returns(true);

            // Act
            var result = _assessmentMethodsController.DeleteAssessmentMethod(validAssessmentMethodId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Assessment Method is Used! Can't Delete", baseResponse.message);
        }
    }

}
