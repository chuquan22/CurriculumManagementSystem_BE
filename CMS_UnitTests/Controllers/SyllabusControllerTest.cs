using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CurriculumManagementSystemWebAPI.Controllers;
using Repositories.Syllabus;
using Repositories.Batchs;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using DataAccess.Models.DTO.response;
using BusinessObject;
using System.Reflection;
using System.Net;
using DataAccess.Models.DTO.request;
using AutoFixture;
using DataAccess.Models.DTO;
using Google.Apis.Gmail.v1.Data;
using Microsoft.Extensions.Configuration;

namespace CMS_UnitTests.Controllers
{
    public class SyllabusControllerTest
    {
        private Mock<ISyllabusRepository> syllabusRepositoryMock;
        private Mock<IMapper> mapperMock;
        private IMapper _mapper;
        private Mock<IWebHostEnvironment> hostingEnvironmentMock;
        private SyllabusController syllabusController;
        private IFixture fixture;


        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            syllabusRepositoryMock = fixture.Freeze<Mock<ISyllabusRepository>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            mapperMock = new Mock<IMapper>();
            hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            syllabusController = new SyllabusController(_mapper, hostingEnvironmentMock.Object);
        }

        [Test]
        public async Task GetListSyllabus_ReturnsOkResult()
        {
            // Arrange
            var page = 1;
            var limit = 10;
            var txtSearch = "a";
            var subjectCode = "";

            var listSyllabus = new List<Syllabus>();
            var listSyllabusResponse = new List<SyllabusResponse>();

            syllabusRepositoryMock.Setup(repo => repo.GetListSyllabus(page, limit, txtSearch, subjectCode))
                .Returns(listSyllabus);


            mapperMock.Setup(mapper => mapper.Map<List<SyllabusResponse>>(listSyllabus)).Returns(listSyllabusResponse);
            // Act
            var result = await syllabusController.GetListSyllabus(page, limit, txtSearch, subjectCode);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Get List Syllabus Sucessfully!", baseResponse.message);

            Assert.IsEmpty(listSyllabusResponse);
        }

        [Test]
        public async Task CreateSyllabus_ReturnsOkResult()
        {
            // Arrange
            var syllabusRequest = new SyllabusRequest
            {
                document_type = "Sample Document Type",
                program = "Sample Program",
                decision_No = "Sample Decision No",
                degree_level_id = 1, 
                syllabus_description = "Sample Syllabus Description",
                subject_id = 1,
                student_task = "Sample Student Task",
                time_allocation = "Sample Time Allocation",
                syllabus_tool = "Sample Syllabus Tool",
                syllabus_note = "Sample Syllabus Note",
                min_GPA_to_pass = 5,
                scoring_scale = 5,
                approved_date = DateTime.Now,
                syllabus_status = true,
                syllabus_approved = true
            };
            // Act
            var result = await syllabusController.CreateSyllabus(syllabusRequest);

             // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Create Syllabus Successfully!", baseResponse.message);
        }
        [Test]
        public async Task CreateSyllabus_ReturnsBadRequestResult()
        {
            // Arrange
            var syllabusRequest = new SyllabusRequest();

            // Act
            var result = await syllabusController.CreateSyllabus(syllabusRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.IsTrue(baseResponse.message.Contains("Error"));
        }

    }
}
