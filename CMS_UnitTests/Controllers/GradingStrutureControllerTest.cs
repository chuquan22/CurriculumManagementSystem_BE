using AutoFixture;
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
        [Test]
        public async Task CreateGradingStructure_ReturnsOkResponse()
        {
            // Arrange
            var request = new GradingStrutureCreateRequest
            {
                gradingStruture = new GradingStrutureRequest
                {
                    assessment_component = "1",
                    assessment_type = "On-Going",
                    grading_weight = 1,
                    grading_part = 1,
                    minimum_value_to_meet_completion = 1,
                    grading_duration = "1",
                    type_of_questions = "1",
                    number_of_questions = "1",
                    scope_knowledge = "1",
                    how_granding_structure = "11",
                    grading_note = "1",
                    session_no = 5,
                    references = "Assignment",
                    syllabus_id = 2
                }
            };
            gradingStrutureRepositoryMock.Setup(repo => repo.CreateGradingStruture(It.IsAny<GradingStruture>()))
                                        .Returns(new GradingStruture()); // Assume successful grading structure creation

            gradingCLOsRepositoryMock.Setup(repo => repo.CreateGradingCLO(It.IsAny<GradingCLO>()))
                                    .Returns(new GradingCLO());

            // Act
            var result = gradingStrutureController.CreateGradingStructure(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);
            Assert.IsNotNull(baseResponse.data);
        }
    }
}
