using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurriculumManagementSystemWebAPI.Controllers;
using Repositories.SemesterPlans;
using AutoMapper;
using DataAccess.Models.DTO.response;
using BusinessObject;
using DataAccess.Models.DTO;

namespace CMS_UnitTests.Controllers
{
    public class SemesterPlanControllerTest
    {
        private Mock<ISemesterPlanRepository> semesterPlanRepositoryMock;
        private Mock<IMapper> mapperMock;
        private IMapper _mapper;
        private SemesterPlanController semesterPlanController;

        [SetUp]
        public void Setup()
        {
            semesterPlanRepositoryMock = new Mock<ISemesterPlanRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            mapperMock = new Mock<IMapper>();
            semesterPlanController = new SemesterPlanController();
        }

        [Test]
        public async Task CreateSemesterPlan_ReturnsOkResult()
        {
            // Arrange
            int semesterId = 1;
            var semesterPlanDetails = new SemesterPlanDetailsResponse();
            var createSemesterPlanResponse = new List<SemesterPlanDTO>();

            semesterPlanRepositoryMock.Setup(repo => repo.GetSemesterPlanDetails(semesterId))
                .Returns(semesterPlanDetails);

            semesterPlanRepositoryMock.Setup(repo => repo.GetSemesterPlan(semesterId))
                .Returns(createSemesterPlanResponse);

            // Act
            var result = semesterPlanController.CreateSemesterPlan(semesterId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Create new semester plan successfully!", baseResponse.message);

            Assert.IsNotNull(createSemesterPlanResponse);
        }

        [Test]
        public async Task GetSemesterPlanOverView_ReturnsOkResult()
        {
            // Arrange
            int semesterId = 1;
            var semesterPlanOverViewResponse = new List<CreateSemesterPlanResponse>();

            semesterPlanRepositoryMock.Setup(repo => repo.GetSemesterPlanOverView(semesterId))
                .Returns(semesterPlanOverViewResponse);

            // Act
            var result = semesterPlanController.GetSemesterPlanOverView(semesterId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsNotNull(semesterPlanOverViewResponse);
        }

        [Test]
        public async Task GetSemesterPlanSubject_ReturnsOkResult()
        {
            // Arrange
            int semesterId = 1;
            var semesterPlanDetails = new SemesterPlanDetailsResponse();

            semesterPlanRepositoryMock.Setup(repo => repo.GetSemesterPlanDetails(semesterId))
                .Returns(semesterPlanDetails);

            // Act
            var result = semesterPlanController.GetSemesterPlanSubject(semesterId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsNotNull(semesterPlanDetails);
        }

        [Test]
        public async Task GetSemesterPlanDetails_ReturnsOkResult()
        {
            // Arrange
            int semesterId = 1;
            var semesterPlanOverViewDetailsResponse = new List<SemesterPlanResponse>();

            semesterPlanRepositoryMock.Setup(repo => repo.GetSemesterPlanOverViewDetails(semesterId))
                .Returns(semesterPlanOverViewDetailsResponse);

            // Act
            var result = semesterPlanController.GetSemesterPlanDetails(semesterId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsNotNull(semesterPlanOverViewDetailsResponse);
        }

        [Test]
        public async Task DeleteSemesterPlan_ReturnsOkResult()
        {
            // Arrange
            int semesterId = 1;
            string err = null; 
            semesterPlanRepositoryMock.Setup(repo => repo.DeleteSemesterPlan(semesterId))
                .Returns(err);

            // Act
            var result = semesterPlanController.DeleteSemesterPlan(semesterId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

        }

        // Add similar tests for other actions in SemesterPlanController

        // ...
    }
}
