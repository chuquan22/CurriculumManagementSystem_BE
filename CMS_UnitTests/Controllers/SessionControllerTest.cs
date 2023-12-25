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
using Repositories.Session;
using Repositories.SessionCLOs;
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
        private Mock<ISessionCLOsRepository> sessionCLOsRepositoryMock;

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
        public async Task GetSession_ValidSyllabusId_ReturnsOkResult()
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
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsEmpty(listSessionResponse);
        }
        [Test]
        public void CreateSession_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new SessionCreateRequest() {
                 session = new SessionRequest() {
                        assigment = 1,
                        ass_defense = 1,
                        eos_exam = 1,
                        ITU = "1",
                        IVQ = 1,
                        lecturer_material = "Sample",
                        lecturer_material_link = "Sample",
                        class_session_type_id = 1,
                        online_lab = 1,
                       online_test = 1,
                       remote_learning = 1,
                       schedule_content = "Sample",
                       schedule_lecturer_task = "Sample",
                       schedule_student_task = "Sample",
                       session_No = 1,
                       student_material = "Sample",
                       student_material_link = "Sample",
                       syllabus_id = 1,
                       video_learning = 1,
                 
                 },
                session_clo = new List<SessionCLOsRequest> {
                }

            };

            // Act
            var result = sessionController.CreateSession(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);
        }

        [Test]
        public void UpdateSession_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new SessionUpdateRequest() { };
            var updatedSession = new Session();
        
            // Act
            var result = sessionController.UpdateSesion(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully", baseResponse.message);
        }

        // Additional tests for scenarios where the update operation results in an error

        [Test]
        public void UpdatePatchSession_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var request = new List<SessionPatchRequest>
            {
                new SessionPatchRequest { /* initialize properties */ }
            };

            // Act
            var result = sessionController.UpdatePatchSesion(request);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);
        }

        // Additional tests for scenarios where the patch update operation results in an error

        [Test]
        public void DeleteSession_ValidId_ReturnsOkResult()
        {
            // Arrange
            var request = new SessionCreateRequest()
            {
                session = new SessionRequest()
                {
                    assigment = 1,
                    ass_defense = 1,
                    eos_exam = 1,
                    ITU = "1",
                    IVQ = 1,
                    lecturer_material = "Sample",
                    lecturer_material_link = "Sample",
                    class_session_type_id = 1,
                    online_lab = 1,
                    online_test = 1,
                    remote_learning = 1,
                    schedule_content = "Sample",
                    schedule_lecturer_task = "Sample",
                    schedule_student_task = "Sample",
                    session_No = 1,
                    student_material = "Sample",
                    student_material_link = "Sample",
                    syllabus_id = 2,
                    video_learning = 1,

                },
                session_clo = new List<SessionCLOsRequest>
                {
                }

            };
            var result1 = sessionController.CreateSession(request);
            var okObjectResult1 = result1 as OkObjectResult;
            var baseResponse1 = okObjectResult1.Value as BaseResponse;
            var data = baseResponse1.data as Session;
            int sessionId = data.schedule_id;

            // Act
            var result = sessionController.DeleteSession(sessionId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);
        }

    }
}
