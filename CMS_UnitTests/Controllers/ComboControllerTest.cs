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
using Repositories.Combos;
using DataAccess.Models.DTO;
using BusinessObject;
using AutoFixture;

namespace CMS_UnitTests.Controllers
{
    public class ComboControllerTest
    {
        private Mock<IMapper> mapperMock;
        public Mock<IComboRepository> comboRepositoryMock;
        private IMapper _mapper;
        public ComboController comboController;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            mapperMock = new Mock<IMapper>();
            fixture = new Fixture();

            comboRepositoryMock = new Mock<IComboRepository>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
            comboController = new ComboController(_mapper);
        }
        [Test]
        public void GetListCombo_WithValidSpecializationId_ReturnsOkResult()
        {
            // Arrange
            int specializationId = 1;
            var listCombo = new List<Combo>();
            comboRepositoryMock.Setup(repo => repo.GetListCombo(specializationId)).Returns(listCombo);
            mapperMock.Setup(mapper => mapper.Map<List<ComboResponse>>(listCombo)).Returns(new List<ComboResponse>());
            var expectedResponse = new BaseResponse(false, "Sucessfully", new List<ComboResponse>());

            // Act
            var result = comboController.GetListCombo(specializationId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);
        }

        [Test]
        public void GetListCombo_WithInvalidSpecializationId_ReturnsBadRequestResult()
        {
            // Arrange
            int specializationId = 1;
            comboRepositoryMock.Setup(repo => repo.GetListCombo(specializationId)).Throws(new Exception("Error message"));

            // Act
            var result = comboController.GetListCombo(2);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Error: Error message", baseResponse.message);
        }

        [Test]
        public void GetCombo_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int comboId = 1;
            var combo = new Combo { combo_id = comboId };
            comboRepositoryMock.Setup(repo => repo.FindComboById(comboId)).Returns(combo);
            mapperMock.Setup(mapper => mapper.Map<ComboResponse>(combo)).Returns(new ComboResponse());
            var expectedResponse = new BaseResponse(false, "Sucessfully", new ComboResponse());

            // Act
            var result = comboController.GetCombo(comboId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);
        }

        [Test]
        public void GetCombo_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            int comboId = 1;
            comboRepositoryMock.Setup(repo => repo.FindComboById(comboId)).Throws(new Exception("Error message"));

            // Act
            var result = comboController.GetCombo(2);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Error: Error message", baseResponse.message);
        }

        [Test]
        public void GetListComboByCurriculum_WithValidCurriculumId_ReturnsOkResult()
        {
            // Arrange
            int curriculumId = 1;
            var listCombo = new List<Combo>();
            comboRepositoryMock.Setup(repo => repo.GetListComboByCurriId(curriculumId)).Returns(listCombo);
            var expectedResponse = new BaseResponse(false, "Sucessfully", listCombo);

            // Act
            var result = comboController.GetListComboByCurriculum(curriculumId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);
        }

        [Test]
        public void GetListComboByCurriculum_WithInvalidCurriculumId_ReturnsBadRequestResult()
        {
            // Arrange
            int curriculumId = 1;
            comboRepositoryMock.Setup(repo => repo.GetListComboByCurriId(curriculumId)).Throws(new Exception("Error message"));

            // Act
            var result = comboController.GetListComboByCurriculum(2);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Error: Error message", baseResponse.message);
        }

        [Test]
        public void CreateCombo_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var comboRequest = new ComboRequest()
            {
                combo_code = "ABC" + fixture.Create<int>(),
                specialization_id = 1,
                combo_english_name = "Sample English Name",
                combo_name = "Sample Name",
                is_active = true
            }; 
            var combo = new Combo();
            mapperMock.Setup(mapper => mapper.Map<Combo>(comboRequest)).Returns(combo);
            comboRepositoryMock.Setup(repo => repo.IsCodeExist(combo.combo_code, combo.specialization_id)).Returns(false);
            comboRepositoryMock.Setup(repo => repo.CreateCombo(combo)).Returns(combo);

            // Act
            var result = comboController.CreateCombo(comboRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);
        }

        [Test]
        public void CreateCombo_WithDuplicateData_ReturnsBadRequestResult()
        {
            // Arrange
            var comboRequest = new ComboRequest()
            {
                combo_code = "ABC",
                specialization_id = 1,
                combo_english_name = "Sample English Name",
                combo_name = "Sample Name",
                is_active = true
            }; 
            var combo = new Combo();
            mapperMock.Setup(mapper => mapper.Map<Combo>(comboRequest)).Returns(combo);
            comboRepositoryMock.Setup(repo => repo.IsCodeExist(combo.combo_code, combo.specialization_id)).Returns(true);

            // Act
            var result = comboController.CreateCombo(comboRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Combo code already exist!", baseResponse.message);
        }

        [Test]
        public void CreateCombo_WithFailedCreation_ReturnsBadRequestResult()
        {
            // Arrange
            var comboRequest = new ComboRequest();
            var combo = new Combo();
            mapperMock.Setup(mapper => mapper.Map<Combo>(comboRequest)).Returns(combo);
            comboRepositoryMock.Setup(repo => repo.IsCodeExist(combo.combo_code, combo.specialization_id)).Returns(false);
            comboRepositoryMock.Setup(repo => repo.CreateCombo(combo)).Returns(null as Combo);

            // Act
            var result = comboController.CreateCombo(comboRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Error: An error occurred while saving the entity changes. See the inner exception for details.", baseResponse.message);
        }

        [Test]
        public void DisableCombo_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int comboId = 1;
            comboRepositoryMock.Setup(repo => repo.DisableCombo(comboId)).Returns(true);
            var expectedResponse = new BaseResponse(false, "Sucessfully", true);

            // Act
            var result = comboController.DisableCombo(comboId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);
        }

        [Test]
        public void DisableCombo_WithInvalidId_ReturnsOkResult()
        {
            // Arrange
            int comboId = 99999;
            comboRepositoryMock.Setup(repo => repo.DisableCombo(comboId)).Throws(new Exception("Error message"));

            // Act
            var result = comboController.DisableCombo(2);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var badRequestObjectResult = result as OkObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
        }

        [Test]
        public void UpdateCombo_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var comboUpdateRequest = new ComboUpdateRequest();
            var combo = new Combo();
            mapperMock.Setup(mapper => mapper.Map(comboUpdateRequest, combo)).Returns(combo);
            comboRepositoryMock.Setup(repo => repo.FindComboById(comboUpdateRequest.combo_id)).Returns(combo);
            comboRepositoryMock.Setup(repo => repo.UpdateCombo(combo)).Returns(combo);
            var expectedResponse = new BaseResponse(false, "Sucessfully", combo);

            // Act
            var result = comboController.UpdateCombo(comboUpdateRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);

            Assert.AreEqual(expectedResponse, okObjectResult.Value);
        }

        [Test]
        public void UpdateCombo_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var comboUpdateRequest = new ComboUpdateRequest();
            comboRepositoryMock.Setup(repo => repo.FindComboById(comboUpdateRequest.combo_id)).Returns(null as Combo);

            // Act
            var result = comboController.UpdateCombo(comboUpdateRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Error: Object reference not set to an instance of an object.", baseResponse.message);
        }

        [Test]
        public void UpdateCombo_WithFailedUpdate_ReturnsBadRequestResult()
        {
            // Arrange
            var comboUpdateRequest = new ComboUpdateRequest();
            var combo = new Combo();
            mapperMock.Setup(mapper => mapper.Map(comboUpdateRequest, combo)).Returns(combo);
            comboRepositoryMock.Setup(repo => repo.FindComboById(comboUpdateRequest.combo_id)).Returns(combo);
            comboRepositoryMock.Setup(repo => repo.UpdateCombo(combo)).Returns(null as Combo);

            // Act
            var result = comboController.UpdateCombo(comboUpdateRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Error: Object reference not set to an instance of an object.", baseResponse.message);
        }

        [Test]
        public void DeleteCombo_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int comboId = 1;
            comboRepositoryMock.Setup(repo => repo.DeleteCombo(comboId)).Returns("Delete sucessfully.");
            var expectedResponse = new BaseResponse(false, "Sucessfully", "Delete sucessfully.");

            // Act
            var result = comboController.DeleteCombo(comboId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Sucessfully", baseResponse.message);

            Assert.AreEqual(expectedResponse, okObjectResult.Value);
        }
    }
}
