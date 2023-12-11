using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.Batchs;
using Repositories.Semesters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class SemestersControllerTests
    {
        private SemestersController _semestersController;
        private ISemestersRepository _semesterRepositoryMock;
        private IBatchRepository _batchRepositoryMock;
        private IMapper _mapper;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            // Setup mock objects and dependencies
            _semesterRepositoryMock = Mock.Of<ISemestersRepository>();
            _batchRepositoryMock = Mock.Of<IBatchRepository>();
            _mapperMock = new Mock<IMapper> { };
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();

            // Initialize the controller with the mock dependencies
            _semestersController = new SemestersController(_mapper);
        }

        // Test for GetAllSemester action
        [Test]
        public void GetAllSemester_ReturnsOkResult()
        {
            // Arrange
            var semesters = new List<Semester>(); // Initialize with your sample data
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.GetSemesters()).Returns(semesters);

            // Act
            var result = _semestersController.GetAllSemester();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Semester", baseResponse.message);
            Assert.IsNotNull(baseResponse.data);

            var listSemesterResponse = baseResponse.data as List<SemesterResponse>;
            Assert.IsNotNull(listSemesterResponse);

        }

        [Test]
        public void GetSemesterById_WhenSemesterNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            int nonExistentSemesterId = 999;
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.GetSemester(nonExistentSemesterId)).Returns((Semester)null);

            // Act
            var result = _semestersController.GetSemester(nonExistentSemesterId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Semester!", baseResponse.message);
            Assert.IsNull(baseResponse.data);
        }

        // Test for UpdateSemester action returning BadRequest
        [Test]
        public void UpdateSemester_WhenSemesterNotFound_ReturnsBadRequestResult()
        {
            // Arrange
            int nonExistentSemesterId = 999;
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.GetSemester(nonExistentSemesterId)).Returns((Semester)null);

            // Act
            var result = _semestersController.UpdateSemester(nonExistentSemesterId, new SemesterRequest());

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var baseResponse = badRequestResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Semester!", baseResponse.message);
            Assert.IsNull(baseResponse.data);
        }

        [Test]
        public void GetAllSemester_WhenNoSemesters_ReturnsNotFoundResult()
        {
            // Arrange
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.GetSemesters()).Returns(new List<Semester>());

            // Act
            var result = _semestersController.GetAllSemester();

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("List Semester", baseResponse.message);
            Assert.IsNull(baseResponse.data);
        }

        // Test for PaginationSemester action returning NotFound
        [Test]
        public void PaginationSemester_WhenNoSemesters_ReturnsNotFoundResult()
        {
            // Arrange
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.PaginationSemester(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(new List<Semester>());

            // Act
            var result = _semestersController.PaginationSemester(1, 10, "searchTerm");

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Semester!", baseResponse.message);
            Assert.IsNull(baseResponse.data);
        }

        [Test]
        public void GetAllSemester_WhenSemestersExist_ReturnsOkResult()
        {
            // Arrange
            var semesterList = new List<Semester> { /* add sample semesters here */ };
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.GetSemesters()).Returns(semesterList);
            var expectedResponse = new List<SemesterResponse>();
            _mapperMock.Setup(mapper => mapper.Map<List<SemesterResponse>>(semesterList)).Returns(expectedResponse);

            // Act
            var result = _semestersController.GetAllSemester();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Semester", baseResponse.message);
            Assert.AreEqual(expectedResponse, baseResponse.data);
        }

        // Test for PaginationSemester action returning Ok
        [Test]
        public void PaginationSemester_WhenSemestersExist_ReturnsOkResult()
        {
            // Arrange
            var semesterList = new List<Semester> { /* add sample semesters here */ };
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.PaginationSemester(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(semesterList);
            var expectedResponse = new List<SemesterResponse>();
            _mapperMock.Setup(mapper => mapper.Map<List<SemesterResponse>>(semesterList)).Returns(expectedResponse);

            // Act
            var result = _semestersController.PaginationSemester(1, 10, "searchTerm");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Semester", baseResponse.message);
            Assert.AreEqual(expectedResponse, baseResponse.data);
        }

        [Test]
        public void GetSemester_WhenSemesterExists_ReturnsOkResult()
        {
            // Arrange
            var semesterId = 1;
            var semester = new Semester { /* add sample semester details here */ };
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.GetSemester(semesterId)).Returns(semester);
            var expectedResponse = new SemesterResponse();
            _mapperMock.Setup(mapper => mapper.Map<SemesterResponse>(semester)).Returns(expectedResponse);

            // Act
            var result = _semestersController.GetSemester(semesterId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Semester", baseResponse.message);
            Assert.AreEqual(expectedResponse, baseResponse.data);
        }

        // Test for GetSemesterByDegreeLevel action returning Ok
        [Test]
        public void GetSemesterByDegreeLevel_WhenSemesterExists_ReturnsOkResult()
        {
            // Arrange
            var degreeLevelId = 1;
            var semesters = new List<Semester> { /* add sample semesters here */ };
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.GetSemesterByDegreeLevel(degreeLevelId)).Returns(semesters);
            var expectedResponse = new List<SemesterResponse>();
            _mapperMock.Setup(mapper => mapper.Map<List<SemesterResponse>>(semesters)).Returns(expectedResponse);

            // Act
            var result = _semestersController.GetSemesterByDegreeLevel(degreeLevelId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Semester", baseResponse.message);
        }

        // Test for GetSemesterBySpeId action returning Ok
        [Test]
        public void GetSemesterBySpeId_WhenSemesterExists_ReturnsOkResult()
        {
            // Arrange
            var speId = 1;
            var semesters = new List<Semester> { /* add sample semesters here */ };
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.GetSemesterBySpe(speId)).Returns(semesters);
            var expectedResponse = new List<SemesterResponse>();
            _mapperMock.Setup(mapper => mapper.Map<List<SemesterResponse>>(semesters)).Returns(expectedResponse);

            // Act
            var result = _semestersController.GetSemesterBy(speId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Semester", baseResponse.message);
        }

        [Test]
        public void CreateSemester_WhenSemesterDoesNotExist_ReturnsOkResult()
        {
            // Arrange
            var semesterRequest = new SemesterRequest { /* add sample semester request details here */ };
            var semester = _mapper.Map<Semester>(semesterRequest);

            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.CheckSemesterDuplicate(0, semester.semester_name, semester.school_year)).Returns(false);
            _mapperMock.Setup(mapper => mapper.Map<Semester>(semesterRequest)).Returns(semester);
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.CreateSemester(semester)).Returns(Result.createSuccessfull.ToString());

            // Act
            var result = _semestersController.CreateSemester(semesterRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Create Success!", baseResponse.message);
            Assert.AreEqual(semesterRequest, baseResponse.data);
        }

        // Test for UpdateSemester action returning Ok
        [Test]
        public void UpdateSemester_WhenSemesterExists_ReturnsOkResult()
        {
            // Arrange
            var semesterId = 1;
            var semesterRequest = new SemesterRequest { /* add sample semester request details here */ };
            var semester = _mapper.Map<Semester>(semesterRequest);

            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.GetSemester(semesterId)).Returns(semester);
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.CheckSemesterDuplicate(semesterId, semester.semester_name, semester.school_year)).Returns(false);
            _mapperMock.Setup(mapper => mapper.Map(semesterRequest, semester));
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.UpdateSemester(semester)).Returns(Result.updateSuccessfull.ToString());

            // Act
            var result = _semestersController.UpdateSemester(semesterId, semesterRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Update Success!", baseResponse.message);
            Assert.AreEqual(semesterRequest, baseResponse.data);
        }

        // Test for DeleteSemester action returning Ok
        [Test]
        public void DeleteSemester_WhenSemesterExistsAndNotUsed_ReturnsOkResult()
        {
            // Arrange
            var semesterId = 1;
            var semester = new Semester { /* add sample semester details here */ };

            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.GetSemester(semesterId)).Returns(semester);
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.CheckSemesterExsit(semesterId)).Returns(false);
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.DeleteSemester(semester)).Returns(Result.deleteSuccessfull.ToString());

            // Act
            var result = _semestersController.DeleteSemester(semesterId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Delete Success!", baseResponse.message);
        }

        [Test]
        public void CreateSemester_WhenSemesterDuplicate_ReturnsBadRequest()
        {
            // Arrange
            var semesterRequest = new SemesterRequest { /* add sample semester request details here */ };
            var semester = _mapper.Map<Semester>(semesterRequest);

            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.CheckSemesterDuplicate(0, semester.semester_name, semester.school_year)).Returns(true);

            // Act
            var result = _semestersController.CreateSemester(semesterRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var baseResponse = badRequestResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual($"Semester {semester.semester_name + "-" + semester.school_year} is Duplicate!", baseResponse.message);
        }

        // Test for UpdateSemester action returning BadRequest
        [Test]
        public void UpdateSemester_WhenSemesterNotFound_ReturnsNotFound()
        {
            // Arrange
            var semesterId = 1;
            var semesterRequest = new SemesterRequest { /* add sample semester request details here */ };

            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.GetSemester(semesterId)).Returns((Semester)null);

            // Act
            var result = _semestersController.UpdateSemester(semesterId, semesterRequest);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;

            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Semester!", baseResponse.message);
        }

        // Test for DeleteSemester action returning BadRequest
        [Test]
        public void DeleteSemester_WhenSemesterUsed_ReturnsBadRequest()
        {
            // Arrange
            var semesterId = 1;
            var semester = new Semester { /* add sample semester details here */ };

            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.GetSemester(semesterId)).Returns(semester);
            Mock.Get(_semesterRepositoryMock).Setup(repo => repo.CheckSemesterExsit(semesterId)).Returns(true);

            // Act
            var result = _semestersController.DeleteSemester(semesterId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;

            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);

            var baseResponse = badRequestResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Semester Used. Can't Delete!", baseResponse.message);
        }
    }

}
