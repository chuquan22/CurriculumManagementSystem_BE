using Antlr.Runtime.Tree;
using AutoFixture;
using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.Batchs;
using Repositories.Combos;
using Repositories.Curriculums;
using Repositories.CurriculumSubjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class CurriculumSubjectControllerTest
    {
        private Mock<ICurriculumSubjectRepository> _curriculumSubjectRepository;
        private IMapper _mapper;
        private CurriculumSubjectsController _controller;
        [SetUp]
        public void Setup()
        {
            _curriculumSubjectRepository = new Mock<ICurriculumSubjectRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _controller = new CurriculumSubjectsController(_mapper);
        }

        [Test]
        public async Task GetCurriculumBySubject_ReturnNotFoundResponse_WhenNotFoundSubject()
        {
            // Arrange
            int subjectIdTest = 500;
            _curriculumSubjectRepository.Setup(repo => repo.GetListCurriculumBySubject(It.IsAny<int>()))
                                            .Returns(new List<CurriculumSubject>());

            // Act
            var result = await _controller.GetCurriculumBySubject(subjectIdTest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
        }

        [Test]
        public async Task GetSubjectByCurriculum_ReturnSuccessResponse_WhenSubjectsExist()
        {
            // Arrange
            int curriculumIdTest = 5;
            var curriculumSubjectList = new List<CurriculumSubject>(); 
            _curriculumSubjectRepository.Setup(repo => repo.GetListCurriculumSubject(It.IsAny<int>()))
                                            .Returns(curriculumSubjectList);

            // Act
            var result = await _controller.GetSubjectByCurriculum(curriculumIdTest);

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
        public async Task GetSubjectNotExistCurriculum_ReturnNotFoundResponse_WhenNoSubjectsExist()
        {
            // Arrange
            int curriculumIdTest = 19;
            _curriculumSubjectRepository.Setup(repo => repo.GetListSubject(It.IsAny<int>()))
                                            .Returns(new List<Subject>()); 

            // Act
            var result = await _controller.GetSubjectNotExistCurriculum(curriculumIdTest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
        }

        [TestCase(1, 0, 0, null, null, 0)]
        [TestCase(1, 101, 0, "Group A", null, 2)]
        public async Task PostCurriculumSubject_ReturnsBadRequest_WhenCreateFails(int subjectId, int curriId, int termNo, string subjectGroup, int? combo, int? option)
        {
            // Arrange
            var curriculumSubjectRequestList = new List<CurriculumSubjectRequest>();
            var request1 = new CurriculumSubjectRequest
            {
                subject_id = subjectId,
                curriculum_id = curriId,
                term_no = termNo,
                subject_group = subjectGroup,
                combo_id = combo,
                option = option
            };
            

            // Add instances to the list
            curriculumSubjectRequestList.Add(request1);
            _curriculumSubjectRepository.Setup(repo => repo.CreateCurriculumSubject(It.IsAny<CurriculumSubject>()))
                                            .Returns(Result.createSuccessfull.ToString());

            // Act
            var result = await _controller.PostCurriculumSubject(curriculumSubjectRequestList);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequestObjectResult = result.Result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
        }

        [Test]
        public async Task DeleteCurriculumSubject_ReturnsNotFound_WhenSubjectDoesNotExist()
        {
            // Arrange
            int curriculumIdTest = 1;
            int subjectIdTest = 2;
            _curriculumSubjectRepository.Setup(repo => repo.GetCurriculumSubjectById(It.IsAny<int>(), It.IsAny<int>()))
                                            .Returns(new CurriculumSubject());

            // Act
            var result = await _controller.DeleteCurriculumSubject(curriculumIdTest, subjectIdTest);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var baseResponse = notFoundObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not found this Curriculum Subject", baseResponse.message);
        }


        [TestCase(1, 3, 1, "Group A", 0, null)]
        [TestCase(4, 3, 2, "Group B", 0, null)]
        [TestCase(3, 3, 1, "Group A", 0, null)]
        public async Task PostCurriculumSubject_ReturnsOk_WhenCreateSucceeds(int subjectId, int curriId, int termNo, string subjectGroup, int? combo, int? option)
        {
            // Arrange
            var curriculumSubjectRequestList = new List<CurriculumSubjectRequest>();
            var request1 = new CurriculumSubjectRequest
            {
                subject_id = subjectId,
                curriculum_id = curriId,
                term_no = termNo,
                subject_group = subjectGroup,
                combo_id = combo,
                option = option
            };


            // Add instances to the list
            curriculumSubjectRequestList.Add(request1);

            _curriculumSubjectRepository.Setup(repo => repo.CreateCurriculumSubject(It.IsAny<CurriculumSubject>()))
                                            .Returns(Result.createSuccessfull.ToString());

            // Act
            var result = await _controller.PostCurriculumSubject(curriculumSubjectRequestList);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Create success!", baseResponse.message); 
        }

        [Test]
        public async Task DeleteCurriculumSubject_ReturnsOk_WhenDeleteSucceeds()
        {
            // Arrange
            int curriculumIdTest = 3;
            int subjectIdTest = 1;
            var mockCurriculumSubject = new CurriculumSubject(); 
            _curriculumSubjectRepository.Setup(repo => repo.GetCurriculumSubjectById(It.IsAny<int>(), It.IsAny<int>()))
                                            .Returns(mockCurriculumSubject);
     
            _curriculumSubjectRepository.Setup(repo => repo.DeleteCurriculumSubject(It.IsAny<CurriculumSubject>()))
                                            .Returns(Result.deleteSuccessfull.ToString());

            // Act
            var result = await _controller.DeleteCurriculumSubject(curriculumIdTest, subjectIdTest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
        }


    }
}

