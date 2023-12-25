using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CMS_UnitTests.Controllers
{
    public class LoginControllerTest
    {
        private Mock<IUsersRepository> userRepositoryMock;
        private Mock<IMapper> mapperMock;
        private Mock<IConfiguration> configurationMock;
        private IMapper _mapper;
        private LoginController loginController;
        private IFixture fixture;
        

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            userRepositoryMock = fixture.Freeze<Mock<IUsersRepository>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            configurationMock = new Mock<IConfiguration>();
            mapperMock = new Mock<IMapper>();
            loginController = new LoginController(configurationMock.Object, _mapper);
        }
        [Test]
        public async Task GoogleOAuthLogin_ReturnsOkResponse()
        {
          
            // Act
            var result = loginController.GoogleOAuthLogin();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GoogleOAuthCallback_ReturnsOkResponse()
        {

            // Act
            var result = loginController.GoogleOAuthLogin();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

    }
}
