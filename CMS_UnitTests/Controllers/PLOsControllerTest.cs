using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.CurriculumSubjects;
using Repositories.PLOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class PLOsControllerTest
    {
        private Mock<IPLOsRepository> _plosRepository;
        private IMapper _mapper;
        private PLOsController _controller;

        [SetUp]
        public void Setup()
        {
            _plosRepository = new Mock<IPLOsRepository>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            _controller = new PLOsController(_mapper);
        }

        [Test]
        public void GetListPLOsByCurriculum_ReturnsOkResult()
        {
            // Arrange
            int curriculumId = 1;
            var plosData = new List<PLOs>(); // Your sample data here
            _plosRepository.Setup(repo => repo.GetListPLOsByCurriculum(It.IsAny<int>())).Returns(plosData);

            // Act
            var result = _controller.GetListPLOsByCurriculum(curriculumId).Result;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsInstanceOf<BaseResponse>(okResult.Value);
            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsFalse(baseResponse.error);
            // Add more assertions based on your specific response structure
        }

        [Test]
        public void GetPLOs_ReturnsNotFoundResult()
        {
            // Arrange
            int id = 1;
            PLOs plosData = null; // Simulating data not found
            _plosRepository.Setup(repo => repo.GetPLOsById(It.IsAny<int>())).Returns(plosData);

            // Act
            var result = _controller.GetPLOs(id).Result;

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.IsInstanceOf<BaseResponse>(notFoundResult.Value);
            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsTrue(baseResponse.error);
            // Add more assertions based on your specific response structure
        }

        [Test]
        public void PutPLOs_WithValidData_ReturnsOkResult()
        {
            // Arrange
            int id = 1;
            var pLOsDTORequest = new PLOsDTORequest
            {
                PLO_name = "TestPLO",
                curriculum_id = 1,
                PLO_description = "Description"
            };
            var plosData = new PLOs();

            _plosRepository.Setup(repo => repo.CheckPLONameUpdateDuplicate(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            _plosRepository.Setup(repo => repo.GetPLOsById(It.IsAny<int>())).Returns(plosData);
            _plosRepository.Setup(repo => repo.UpdatePLOs(It.IsAny<PLOs>())).Returns(Result.updateSuccessfull.ToString());

            // Act
            var result = _controller.PutPLOs(id, pLOsDTORequest).Result;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<BaseResponse>(okResult.Value);
            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsFalse(baseResponse.error);
            // Add more assertions based on your specific response structure
        }

        [Test]
        public void PostPLOs_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var pLOsDTORequest = new PLOsDTORequest
            {
                PLO_name = "TestPLO",
                curriculum_id = 1,
                PLO_description = "Description"
            };
            var plosData = new PLOs();

            _plosRepository.Setup(repo => repo.CheckPLONameExsit(It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            _plosRepository.Setup(repo => repo.CreatePLOs(It.IsAny<PLOs>())).Returns(Result.createSuccessfull.ToString());

            // Act
            var result = _controller.PostPLOs(pLOsDTORequest).Result;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsInstanceOf<BaseResponse>(okResult.Value);
            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsFalse(baseResponse.error);
            // Add more assertions based on your specific response structure
        }

        [Test]
        public void PutPLOs_WithDuplicateName_ReturnsBadRequest()
        {
            // Arrange
            int id = 1;
            var pLOsDTORequest = new PLOsDTORequest
            {
                PLO_name = "TestPLO",
                curriculum_id = 1,
                PLO_description = "Description"
            };

            _plosRepository.Setup(repo => repo.CheckPLONameUpdateDuplicate(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            // Act
            var result = _controller.PutPLOs(id, pLOsDTORequest).Result;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsInstanceOf<BaseResponse>(badRequestResult.Value);
            var baseResponse = badRequestResult.Value as BaseResponse;
            Assert.IsTrue(baseResponse.error);
            // Add more assertions based on your specific response structure
        }


        [Test]
        public void PostPLOs_WithDuplicateName_ReturnsBadRequest()
        {
            // Arrange
            var pLOsDTORequest = new PLOsDTORequest
            {
                PLO_name = "DuplicateName",
                curriculum_id = 1,
                PLO_description = "Description"
            };

            _plosRepository.Setup(repo => repo.CheckPLONameExsit(It.IsAny<string>(), It.IsAny<int>())).Returns(true);

            // Act
            var result = _controller.PostPLOs(pLOsDTORequest).Result;

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.IsInstanceOf<BaseResponse>(badRequestResult.Value);
            var baseResponse = badRequestResult.Value as BaseResponse;
            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("DuplicateName is Duplicate!", baseResponse.message);
            // Add more assertions based on your specific response structure
        }


        [Test]
        public void PutPLOs_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int id = 1;
            var pLOsDTORequest = new PLOsDTORequest
            {
                PLO_name = "TestPLO",
                curriculum_id = 1,
                PLO_description = "Description"
            };

            _plosRepository.Setup(repo => repo.CheckPLONameUpdateDuplicate(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>())).Returns(false);
            _plosRepository.Setup(repo => repo.GetPLOsById(It.IsAny<int>())).Returns((PLOs)null);

            // Act
            var result = _controller.PutPLOs(id, pLOsDTORequest).Result;

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsInstanceOf<BaseResponse>(notFoundResult.Value);
            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Cannot found PLOs", baseResponse.message);
        }


        [Test]
        public void DeletePLOs_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int id = 1;
            var pLOs = new PLOs();

            _plosRepository.Setup(repo => repo.GetPLOsById(It.IsAny<int>())).Returns(pLOs);
            _plosRepository.Setup(repo => repo.DeletePLOs(It.IsAny<PLOs>())).Returns(Result.deleteSuccessfull.ToString());

            // Act
            var result = _controller.DeletePLOs(id).Result;

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOf<BaseResponse>(okResult.Value);
            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsFalse(baseResponse.error);
            // Add more assertions based on your specific response structure
        }


        [Test]
        public void DeletePLOs_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int id = 1;

            _plosRepository.Setup(repo => repo.GetPLOsById(It.IsAny<int>())).Returns((PLOs)null);

            // Act
            var result = _controller.DeletePLOs(id).Result;

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
            // Add more assertions based on your specific response structure
        }

    }

}
