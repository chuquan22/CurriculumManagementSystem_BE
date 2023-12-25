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

namespace CMS_UnitTests.Controllers
{
    [TestFixture]

    public class UsersControllerTests
    {
        private Mock<IMapper> mapperMock;
        private Mock<IUsersRepository> usersRepositoryMock;
        private UsersController usersController;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();

            mapperMock = new Mock<IMapper>();
            usersRepositoryMock = new Mock<IUsersRepository>();
            usersController = new UsersController(mapperMock.Object);
        }

        [Test]
        public void PaginationUser_ValidParameters_ReturnsOkResult()
        {
            // Arrange
            int page = 1;
            int limit = 10;
            string txtSearch = "John";
            var listUser = new List<User>();
            var baseListResponse = new BaseListResponse(page, limit, 20, new List<UserResponse>());
            usersRepositoryMock.Setup(repo => repo.PaginationUser(page, limit, txtSearch)).Returns(listUser);
            usersRepositoryMock.Setup(repo => repo.GetTotalUser(txtSearch)).Returns(20);
            mapperMock.Setup(mapper => mapper.Map<List<UserResponse>>(listUser)).Returns(new List<UserResponse>());
            var expectedResponse = new BaseResponse(false, "List User", baseListResponse);

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
            var user = new User();
            var userResponse = new UserResponse();
            usersRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(user);
            mapperMock.Setup(mapper => mapper.Map<UserResponse>(user)).Returns(userResponse);
            var expectedResponse = new BaseResponse(false, "User", userResponse);

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
            int userId = 1;
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
            var user = new User();
            mapperMock.Setup(mapper => mapper.Map<User>(userCreateRequest)).Returns(user);
            usersRepositoryMock.Setup(repo => repo.CheckUserDuplicate(userCreateRequest.user_email)).Returns(true);

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
            var user = new User();
            mapperMock.Setup(mapper => mapper.Map<User>(userUpdateRequest)).Returns(user);
            usersRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(user);
            usersRepositoryMock.Setup(repo => repo.UpdateUser(user)).Returns(Result.updateSuccessfull.ToString());
            var expectedResponse = new BaseResponse(false, "Update SuccessFull!", It.IsAny<UserResponse>());

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
            var userUpdateRequest = new UserUpdateRequest();
            usersRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(null as User);

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

            mapperMock.Verify(mapper => mapper.Map<User>(userUpdateRequest), Times.Never);
            usersRepositoryMock.Verify(repo => repo.UpdateUser(It.IsAny<User>()), Times.Never);
        }

        [Test]
        public void DeleteUser_ValidId_ReturnsOkResult()
        {
            // Arrange
            //Create
            var userCreateRequest = new UserCreateRequest
            {
                full_name = "Sample Full Name",
                role_id = 1,
                user_email = "test123@fpt.edu.vn",
                user_name = "Sample Name"
            };
            var user = new User();
            mapperMock.Setup(mapper => mapper.Map<User>(userCreateRequest)).Returns(user);
            usersRepositoryMock.Setup(repo => repo.CheckUserDuplicate(userCreateRequest.user_email)).Returns(true);
            var result1 = usersController.CreateUser(userCreateRequest);
            Assert.IsInstanceOf<BadRequestObjectResult>(result1);
            var badRequestObjectResult = result1 as BadRequestObjectResult;
            var baseResponse2 = badRequestObjectResult.Value as BaseResponse;
            var data = baseResponse2.data as User;
            // Arrange
            //Delete
            int userId = data.user_id;
            usersRepositoryMock.Setup(repo => repo.GetUserById(userId)).Returns(user);
            usersRepositoryMock.Setup(repo => repo.DeleteUser(user)).Returns(Result.deleteSuccessfull.ToString());
            var expectedResponse = new BaseResponse(false, "Delete SuccessFull!", It.IsAny<User>());

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

            usersRepositoryMock.Verify(repo => repo.DeleteUser(user), Times.Once);
            Assert.AreEqual(expectedResponse, okObjectResult.Value);
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

        // Additional tests for scenarios where the delete operation results in an error
    }
}

