using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using DataAccess.Models.DTO;
using Repositories.Subjects;
using Moq;
using DataAccess.Models.DTO.request;
using Repositories.CurriculumSubjects;
using Repositories.PreRequisites;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class SubjectControllersTest
    {
        private SubjectsController _subjectController;
        private IMapper _mapper;
        private Mock<ISubjectRepository> _subjectRepository;
        private Mock<IPreRequisiteRepository> _preRequisiteRepositoryMock;
        private Mock<ICurriculumSubjectRepository> _curriculumSubjectRepositoryMock;


        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _subjectRepository = new Mock<ISubjectRepository>();
            _mapper = config.CreateMapper();
            _subjectController = new SubjectsController(_mapper);
            _preRequisiteRepositoryMock = new Mock<IPreRequisiteRepository>();
            _curriculumSubjectRepositoryMock = new Mock<ICurriculumSubjectRepository>();
        }

        [Test]
        public async Task GetSubjectById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int validSubjectId = 3;

            // Act
            var result = await _subjectController.GetSubject(validSubjectId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Success!", baseResponse.message);
        }

        [Test]
        public async Task GetSubjectById_WithInvalidId_ReturnsNotFoundResultWithMessage()
        {
            // Arrange
            int invalidSubjectId = 0;

            // Act
            var result = await _subjectController.GetSubject(invalidSubjectId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);

            var ObjectResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(ObjectResult);

            var baseResponse = ObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.That(baseResponse.message, Does.Contain("Can't Found this subject"));
        }

        [Test]
        public async Task PaginationSubject_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var page = 1;
            var limit = 10;
            var txtSearch = "example";
            var subjectQuery = new List<Subject>();
            _subjectRepository.Setup(repo => repo.GetAllSubject(It.IsAny<string>())).Returns(subjectQuery);

            // Act
            var result = await _subjectController.PaginationSubject(page, limit, txtSearch);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var paginationResponse = okObjectResult.Value as PaginationResponse<SubjectResponse>;
            Assert.IsNotNull(paginationResponse);

            // Add more specific assertions based on your expected result
        }

        [Test]
        public async Task PostSubjectWithPrerequisites_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var subjectPreRequisitesRequest = new SubjectPreRequisiteRequest(); // Provide a valid request object
            _subjectRepository.Setup(repo => repo.CreateNewSubject(It.IsAny<Subject>())).Returns("OK");

            // Act
            var result = await _subjectController.PostSubjectWithPrerequisites(subjectPreRequisitesRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.That(baseResponse.message, Does.Contain("Create Subject"));

            // Add more specific assertions based on your expected result
        }

        [Test]
        public async Task EditSubjectWithPrerequisites_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var subjectId = 1;
            var subjectPreRequisitesRequest = new SubjectPreRequisiteRequest(); // Provide a valid request object
            _subjectRepository.Setup(repo => repo.GetSubjectById(subjectId)).Returns(new Subject());
            _subjectRepository.Setup(repo => repo.UpdateSubject(It.IsAny<Subject>())).Returns("OK");

            // Act
            var result = await _subjectController.EditSubjectWithPrerequisites(subjectId, subjectPreRequisitesRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.That(baseResponse.message, Does.Contain("Edit subject"));

            // Add more specific assertions based on your expected result
        }

        [Test]
        public async Task DeleteSubject_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var subjectId = 1;
            var subject = new Subject(); // Provide a subject with the given ID
            _subjectRepository.Setup(repo => repo.GetSubjectById(subjectId)).Returns(subject);
            _curriculumSubjectRepositoryMock.Setup(repo => repo.GetListCurriculumBySubject(subjectId)).Returns(new List<CurriculumSubject>());
            _preRequisiteRepositoryMock.Setup(repo => repo.GetPreRequisitesBySubject(subjectId)).Returns(new List<PreRequisite>());
            _subjectRepository.Setup(repo => repo.DeleteSubject(It.IsAny<Subject>())).Returns("OK");

            // Act
            var result = await _subjectController.DeleteSubject(subjectId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.That(baseResponse.message, Does.Contain("Delete Subject"));

            // Add more specific assertions based on your expected result
        }


        [Test]
        public async Task PostSubjectWithPrerequisites_WithDuplicateSubjectCode_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            var subjectPreRequisitesRequest = new SubjectPreRequisiteRequest(); // Provide a valid request object
            _subjectRepository.Setup(repo => repo.CreateNewSubject(It.IsAny<Subject>())).Returns("Subject Duplicate!");

            // Act
            var result = await _subjectController.PostSubjectWithPrerequisites(subjectPreRequisitesRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.That(baseResponse.message, Does.Contain("Subject Duplicate!"));
        }

        [Test]
        public async Task EditSubjectWithPrerequisites_WithInvalidSubjectId_ReturnsNotFoundResultWithMessage()
        {
            // Arrange
            var subjectId = 1;
            var subjectPreRequisitesRequest = new SubjectPreRequisiteRequest(); // Provide a valid request object
            _subjectRepository.Setup(repo => repo.GetSubjectById(subjectId)).Returns((Subject)null);

            // Act
            var result = await _subjectController.EditSubjectWithPrerequisites(subjectId, subjectPreRequisitesRequest);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);

            var notFoundObjectResult = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var baseResponse = notFoundObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.That(baseResponse.message, Does.Contain("Cannot Found this subject"));
        }

        [Test]
        public async Task DeleteSubject_WithSubjectUsedByCurriculum_ReturnsBadRequestResultWithMessage()
        {
            // Arrange
            var subjectId = 1;
            var subject = new Subject(); // Provide a subject with the given ID
            _subjectRepository.Setup(repo => repo.GetSubjectById(subjectId)).Returns(subject);
            _curriculumSubjectRepositoryMock.Setup(repo => repo.GetListCurriculumBySubject(subjectId)).Returns(new List<CurriculumSubject> { new CurriculumSubject() });

            // Act
            var result = await _subjectController.DeleteSubject(subjectId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.That(baseResponse.message, Does.Contain("Subject used by curriculum. Can't Delete!"));
        }



    }
}
