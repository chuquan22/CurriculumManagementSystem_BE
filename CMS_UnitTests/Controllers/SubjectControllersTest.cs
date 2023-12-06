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

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class SubjectControllersTest
    {
        private SubjectsController _subjectController;
        private IMapper _mapper;
        private Mock<ISubjectRepository> _subjectRepository;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _subjectRepository = new Mock<ISubjectRepository>();
            _mapper = config.CreateMapper();
            _subjectController = new SubjectsController(_mapper);
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





    }
}
