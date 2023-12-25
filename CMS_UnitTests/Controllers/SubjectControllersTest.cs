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
        [TestCase("a")]
        public async Task GetSubjectById_WithValidId_ReturnsOkResult(string search)
        {
            // Arrange
            string txtSearch = search;

            // Act
            var result = await _subjectController.GetSubject(txtSearch);

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
        [TestCase("abcscdsd")]
        public async Task GetSubjectById_WithInvalidId_ReturnsNotFoundResultWithMessage(string search)
        {
            // Arrange
            string txtSearch = search;

            // Act
            var result = await _subjectController.GetSubject(txtSearch);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);

            var ObjectResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(ObjectResult);

            var baseResponse = ObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
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
        [TestCase(1, 3, "test", 20, true, 1, "Test", "test", 70, 20, 5, 10)]
        [TestCase(1, 3, "test", 20, true, 1, "Test2", "test", 70, 30, 6, 10)]
        public async Task PostSubjectWithPrerequisites_WithValidData_ReturnsOkResult(int assessmentMethodId, int credit, string englishSubjectName, int examTotal, bool isActive, int learningMethodId, string subjectCode, string subjectName, int totalTime, int totalTimeClass, int preRequisiteTypeId, int preSubjectId)
        {
            // Arrange
            var subjectPreRequisitesRequest = new SubjectPreRequisiteRequest
            {
                SubjectRequest = new SubjectRequest
                {
                    assessment_method_id = assessmentMethodId,
                    credit = credit,
                    english_subject_name = englishSubjectName,
                    exam_total = examTotal,
                    is_active = isActive,
                    learning_method_id = learningMethodId,
                    subject_code = subjectCode,
                    subject_name = subjectName,
                    total_time = totalTime,
                    total_time_class = totalTimeClass
                },
                PreRequisiteRequest = new List<PreRequisiteRequest>
            {
                new PreRequisiteRequest { pre_requisite_type_id = preRequisiteTypeId, pre_subject_id = preSubjectId}
            }
            };
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
        [TestCase(5,1, 3, "test", 20, true, 1, "Code", "test", 70, 20, 5, 10)]
        [TestCase(5,1, 3, "test", 20, true, 1, "Demo", "test", 70, 20, 5, 10)]
        public async Task EditSubjectWithPrerequisites_WithValidData_ReturnsOkResult(int subjectId, int assessmentMethodId, int credit, string englishSubjectName, int examTotal, bool isActive, int learningMethodId, string subjectCode, string subjectName, int totalTime, int totalTimeClass, int preRequisiteTypeId, int preSubjectId)
        {
            // Arrange
            var subjectPreRequisitesRequest = new SubjectPreRequisiteRequest
            {
                SubjectRequest = new SubjectRequest
                {
                    assessment_method_id = assessmentMethodId,
                    credit = credit,
                    english_subject_name = englishSubjectName,
                    exam_total = examTotal,
                    is_active = isActive,
                    learning_method_id = learningMethodId,
                    subject_code = subjectCode,
                    subject_name = subjectName,
                    total_time = totalTime,
                    total_time_class = totalTimeClass
                },
                PreRequisiteRequest = new List<PreRequisiteRequest>
            {
                new PreRequisiteRequest { pre_requisite_type_id = preRequisiteTypeId, pre_subject_id = preSubjectId}
            }
            };
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
            var subjectId = 200;
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
        [TestCase(1, 3, "test", 20, true, 1, "GAM201", "test", 50, 20, 1, 10)]
        [TestCase(1, 3, "test", 20, true, 1, "GAM202", "test", 50, 20, 1, 10)]
        [TestCase(1, 3, "test", 20, true, 1, "GAM203", "test", 50, 20, 1, 10)]
        public async Task PostSubjectWithPrerequisites_WithDuplicateSubjectCode_ReturnsBadRequestResultWithMessage(int assessmentMethodId, int credit, string englishSubjectName, int examTotal, bool isActive, int learningMethodId, string subjectCode, string subjectName, int totalTime, int totalTimeClass, int preRequisiteTypeId, int preSubjectId)
        {
            // Arrange
            var subjectPreRequisitesRequest = new SubjectPreRequisiteRequest
            {
                SubjectRequest = new SubjectRequest
                {
                    assessment_method_id = assessmentMethodId,
                    credit = credit,
                    english_subject_name = englishSubjectName,
                    exam_total = examTotal,
                    is_active = isActive,
                    learning_method_id = learningMethodId,
                    subject_code = subjectCode,
                    subject_name = subjectName,
                    total_time = totalTime,
                    total_time_class = totalTimeClass
                },
                PreRequisiteRequest = new List<PreRequisiteRequest>
            {
                new PreRequisiteRequest { pre_requisite_type_id = preRequisiteTypeId, pre_subject_id = preSubjectId}
            }
            };
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
        }

        [TestCase(0, 1, 3, "test", 20, true, 1, "test", "test", 50, 20, 1, 10)]
        [TestCase(-1, 1, 3, "test", 20, true, 1, "test", "test", 50, 20, 1, 10)]
        public async Task EditSubjectWithPrerequisites_ReturnNotFound(int subjectId, int assessmentMethodId, int credit, string englishSubjectName, int examTotal, bool isActive, int learningMethodId, string subjectCode, string subjectName, int totalTime, int totalTimeClass, int preRequisiteTypeId, int preSubjectId)
        {
            // Arrange
            var subjectPreRequisitesRequest = new SubjectPreRequisiteRequest
            {
                SubjectRequest = new SubjectRequest
                {
                    assessment_method_id = assessmentMethodId,
                    credit = credit,
                    english_subject_name = englishSubjectName,
                    exam_total = examTotal,
                    is_active = isActive,
                    learning_method_id = learningMethodId,
                    subject_code = subjectCode,
                    subject_name = subjectName,
                    total_time = totalTime,
                    total_time_class = totalTimeClass
                },
                PreRequisiteRequest = new List<PreRequisiteRequest>
            {
                new PreRequisiteRequest { pre_requisite_type_id = preRequisiteTypeId, pre_subject_id = preSubjectId}
            }
            };

            Subject s = new Subject();
            _subjectRepository.Setup(repo => repo.GetSubjectById(subjectId)).Returns(s);

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

        [Test]
        public async Task DeleteSubject_ReturnsNotFoundResultWithMessage()
        {
            // Arrange
            var subjectId = 0;
            var subject = new Subject(); // Provide a subject with the given ID
            _subjectRepository.Setup(repo => repo.GetSubjectById(subjectId)).Returns(subject);
            _curriculumSubjectRepositoryMock.Setup(repo => repo.GetListCurriculumBySubject(subjectId)).Returns(new List<CurriculumSubject> { new CurriculumSubject() });

            // Act
            var result = await _subjectController.DeleteSubject(subjectId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var badRequestObjectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.That(baseResponse.message, Does.Contain("Subject you want delete Not Found!"));
        }


    }
}
