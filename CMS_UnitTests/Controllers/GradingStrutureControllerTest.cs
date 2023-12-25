﻿using AutoFixture;
using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Repositories.GradingCLOs;
using Repositories.GradingStruture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    public class GradingStrutureControllerTest
    {

        private Mock<IGradingStrutureRepository> gradingStrutureRepositoryMock;
        private Mock<IGradingCLOsRepository> gradingCLOsRepositoryMock;

        private Mock<IMapper> mapperMock;
        private IMapper _mapper;
        private Mock<IWebHostEnvironment> hostingEnvironmentMock;
        private GradingStrutureController gradingStrutureController;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            gradingStrutureRepositoryMock = fixture.Freeze<Mock<IGradingStrutureRepository>>();
            gradingCLOsRepositoryMock = fixture.Freeze<Mock<IGradingCLOsRepository>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            var configurationMock = new Mock<IConfiguration>();
            mapperMock = new Mock<IMapper>();
            hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            gradingStrutureController = new GradingStrutureController(_mapper);
        }
        [Test]
        public async Task GetGradingStruture_ReturnsOkResponse()
        {
            // Arrange
            var syllabus_id = 1;

            var listGradingStruture = new List<GradingStruture>();
            var listGradingStrutureResponse = new List<GradingStrutureResponse>();

            gradingStrutureRepositoryMock.Setup(repo => repo.GetGradingStruture(syllabus_id))
                .Returns(listGradingStruture);

            mapperMock.Setup(mapper => mapper.Map<List<GradingStrutureResponse>>(listGradingStruture)).Returns(listGradingStrutureResponse);

            // Act
            var result = gradingStrutureController.GetGradingStruture(syllabus_id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsEmpty(listGradingStrutureResponse);
        }
        
    }
}
