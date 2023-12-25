using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using AutoMapper;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using BusinessObject;
using Repositories.Users;
using AutoFixture;
using DataAccess.Models.DTO;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]

    public class UsersControllerTests
    {
        private IMapper _mapper;
        private Mock<IMapper> mapperMock;
        private Mock<IUsersRepository> usersRepositoryMock;
        private UsersController usersController;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            usersRepositoryMock = new Mock<IUsersRepository>();
            usersController = new UsersController(_mapper);
        }

        [Test]
        public void PaginationUser_ValidParameters_ReturnsOkResult()
        {
            // Arrange
            int page = 1;
            int limit = 10;
            string txtSearch = "John";
        
            // Act
            var result = usersController.PaginationUser(page, limit, txtSearch);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List User", baseResponse.message);
        }


        [Test]
        public void GetUser_ValidId_ReturnsOkResult()
        {
            // Arrange
            int userId = 2;

            // Act
            var result = usersController.GetUser(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("User", baseResponse.message);
        }

        [Test]
        public void GetUser_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int userId = 99999;
            usersRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(null as User);

            // Act
            var result = usersController.GetUser(userId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var baseResponse = notFoundObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found User", baseResponse.message);
        }

        [Test]
        public void CreateUser_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var userCreateRequest = new UserCreateRequest
            {
                full_name = "123",
                role_id = 1,
                user_email = "32231@fpt.edu.vn",
                user_name = "123"
             
            };
            // Act
            var result = usersController.CreateUser(userCreateRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Create SuccessFull!", baseResponse.message);
        }

        [Test]
        public void CreateUser_DuplicateEmail_ReturnsBadRequestResult()
        {
            // Arrange
            var userCreateRequest = new UserCreateRequest
            {
                full_name = "Sample Full Name",
                role_id = 1,
               user_email = "thunthe151440@fpt.edu.vn",
               user_name = "Sample Name"
            };
            // Act
            var result = usersController.CreateUser(userCreateRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual($"Email {userCreateRequest.user_email} Duplicate!", baseResponse.message);
        }

        [Test]
        public void UpdateUserRole_ValidIdAndRequest_ReturnsOkResult()
        {
            // Arrange
            int userId = 2;
            var userUpdateRequest = new UserUpdateRequest()
            {
                is_active = true,
                role_id = 1
            };

            // Act
            var result = usersController.UpdateUserRole(userId, userUpdateRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Update SuccessFull!", baseResponse.message);
        }

        [Test]
        public void UpdateUserRole_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int userId = 99999;
            var userUpdateRequest = new UserUpdateRequest()
            {
                is_active=true,
                role_id = 1
            };

            // Act
            var result = usersController.UpdateUserRole(userId, userUpdateRequest);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var baseResponse = notFoundObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found User", baseResponse.message);
        }

        [Test]
        public void DeleteUser_ValidId_ReturnsOkResult()
        {
            // Arrange
            var userCreateRequest = new UserCreateRequest
            {
                full_name = "Sample Full Name",
                role_id = 1,
                user_email = "test123 "+ fixture.Create<int>()+"@fpt.edu.vn" ,
                user_name = "Sample Name"
            };
     
            var result1 = usersController.CreateUser(userCreateRequest);
            Assert.IsInstanceOf<OkObjectResult>(result1);
            var badRequestObjectResult = result1 as OkObjectResult;
            var baseResponse2 = badRequestObjectResult.Value as BaseResponse;
            var data = baseResponse2.data as User;
            int userId = data.user_id;

            // Act
            var result = usersController.DeleteUser(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Delete SuccessFull!", baseResponse.message);
        }

        [Test]
        public void DeleteUser_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int userId = 99999;
            usersRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(null as User);

            // Act
            var result = usersController.DeleteUser(userId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundObjectResult);

            var baseResponse = notFoundObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found User", baseResponse.message);

            usersRepositoryMock.Verify(repo => repo.DeleteUser(It.IsAny<User>()), Times.Never);
        }

    }
}

