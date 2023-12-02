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
using Repositories.Major;

namespace CMS_UnitTests.Controllers
{
    public class MajorControllerTest
    {
        private Mock<IMajorRepository> majorRepositoryMock;
        private Mock<IMapper> mapperMock;
        private IMapper _mapper;
        private MajorsController majorController;
        private IFixture fixture;


        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            majorRepositoryMock = fixture.Freeze<Mock<IMajorRepository>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            mapperMock = new Mock<IMapper>();
            majorController = new MajorsController(_mapper);
        }

        [Test]
        public async Task GetAllMajor_ReturnsOkResult()
        {
            // Arrange
            var listMajor = new List<Major>();
            var listMajorResponse = new List<MajorResponse>();

            majorRepositoryMock.Setup(repo => repo.GetAllMajor())
                .Returns(listMajor);

            mapperMock.Setup(mapper => mapper.Map<List<MajorResponse>>(listMajor)).Returns(listMajorResponse);

            // Act
            var result = majorController.GetAllMajor();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsEmpty(listMajorResponse);
        }

    }
}
