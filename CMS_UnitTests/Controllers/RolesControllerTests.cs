using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AutoMapper;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO.response;
using Repositories.Roles;
using BusinessObject;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class RolesControllerTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<IRoleRepository> roleRepositoryMock;
        private RolesController rolesController;

        [SetUp]
        public void Setup()
        {
            mapperMock = new Mock<IMapper>();
            roleRepositoryMock = new Mock<IRoleRepository>();
            rolesController = new RolesController(mapperMock.Object);
        }

        [Test]
        public void GetAllRole_ReturnsOkResult()
        {
            // Arrange
            var roles = new List<Role>();
            roleRepositoryMock.Setup(repo => repo.GetAllRole()).Returns(roles);

            // Act
            var result = rolesController.GetAllRole();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("list Role", baseResponse.message);
        }

        // Additional tests for scenarios where the GetAllRole operation results in an error or roles not found
    }
}
