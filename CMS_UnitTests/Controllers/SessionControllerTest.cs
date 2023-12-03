using AutoFixture;
using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Repositories.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    public class SessionControllerTest
    {
        private Mock<ISessionRepository> sessionRepositoryMock;
        private Mock<IMapper> mapperMock;
        private IMapper _mapper;
        private Mock<IWebHostEnvironment> hostingEnvironmentMock;
        private SessionController sessionController;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            sessionRepositoryMock = fixture.Freeze<Mock<ISessionRepository>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            var configurationMock = new Mock<IConfiguration>();
            mapperMock = new Mock<IMapper>();
            hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            sessionController = new SessionController(_mapper);
        }

        [Test]
        public async Task GetSession_ReturnsOkResponse()
        {
            // Arrange
            var syllabus_id = 1;

            var listSession = new List<Session>();
            var listSessionResponse = new List<SessionResponse>();

            sessionRepositoryMock.Setup(repo => repo.GetSession(syllabus_id))
                .Returns(listSession);

            mapperMock.Setup(mapper => mapper.Map<List<SessionResponse>>(listSession)).Returns(listSessionResponse);

            // Act
            var result = sessionController.GetSession(syllabus_id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsEmpty(listSessionResponse);
        }


    }
}
