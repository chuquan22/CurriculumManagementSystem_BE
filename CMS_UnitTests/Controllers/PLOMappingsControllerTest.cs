using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.CurriculumSubjects;
using Repositories.PLOMappings;
using Repositories.PLOS;
using Repositories.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class PLOMappingsControllerTests
    {
        private IMapper _mapperMock;
        private PLOMappingsController _ploMappingsController;
        private Mock<IPLOMappingRepository> _repoMock;
        private Mock<IPLOsRepository> _repo1Mock;
        private Mock<ISubjectRepository> _repo2Mock;
        private Mock<ICurriculumSubjectRepository> _curriSubjectRepoMock;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>().Object;
            _repoMock = new Mock<IPLOMappingRepository>();
            _repo1Mock = new Mock<IPLOsRepository>();
            _repo2Mock = new Mock<ISubjectRepository>();
            _curriSubjectRepoMock = new Mock<ICurriculumSubjectRepository>();

            _ploMappingsController = new PLOMappingsController(_mapperMock);
        }

        // Test for GetAllPLOMapping action returning Ok
        [Test]
        public void GetAllPLOMapping_WhenCurriculumIdValid_ReturnsOk()
        {
            // Arrange
            var curriculumId = 9;
            var listPLOMapping = new List<PLOMapping>();

            _repoMock.Setup(repo => repo.GetPLOMappingsInCurriculum(curriculumId)).Returns(listPLOMapping);

            // Act
            var result = _ploMappingsController.GetAllPLOMapping(curriculumId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List PLO Mapping", baseResponse.message);

            var listPLOMappingResponse = okResult.Value as List<PLOMappingDTO>;
            // Add assertions for the content of listPLOMappingResponse based on your expectations.
        }

        // Test for GetAllPLOMapping action returning NotFound
        [Test]
        public void GetAllPLOMapping_WhenNoPLOMappingFound_ReturnsOkWithMessage()
        {
            // Arrange
            var curriculumId = 10;
            var listPLOMapping = new List<PLOMapping>();

            _repoMock.Setup(repo => repo.GetPLOMappingsInCurriculum(curriculumId)).Returns(listPLOMapping);

            // Act
            var result = _ploMappingsController.GetAllPLOMapping(curriculumId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsFalse(baseResponse.error);

        }

        // Test for UpdatePLOMapping action returning Ok
        [Test]
        public void UpdatePLOMapping_WhenUpdateSuccess_ReturnsOk()
        {
            // Arrange
            var pLOMappingRequests = new List<PLOMappingRequest>
                {
                    new PLOMappingRequest
                    {
                        subject_id = 6,
                        PLOs = new Dictionary<string, bool>
                        {
                            { "11-PLO1", true },
                            { "12-PLO2", false },
                            { "13-PLO3", true }
                        }
                    },
                    new PLOMappingRequest
                    {
                        subject_id = 9,
                        PLOs = new Dictionary<string, bool>
                        {
                            { "17-PLO1", false },
                            { "28-PLO2", true },
                            { "39-PLO3", false }
                        }
                    },
                };

            _repoMock.Setup(repo => repo.CheckPLOMappingExsit(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            _repoMock.Setup(repo => repo.DeletePLOMapping(It.IsAny<PLOMapping>())).Returns(Result.deleteSuccessfull.ToString());
            _repoMock.Setup(repo => repo.CreatePLOMapping(It.IsAny<PLOMapping>())).Returns(Result.createSuccessfull.ToString());

            // Act
            var result = _ploMappingsController.UpdatePLOMapping(pLOMappingRequests);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var baseResponse = okResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Update PLOMapping Success", baseResponse.message);
        }


        [Test]
        [TestCase(0, new object[] { new object[] { "17-PLO1", false }, new object[] { "18-PLO2", true }, new object[] { "19-PLO3", false } })]
        public void UpdatePLOMapping_WhenDeleteFails_ThrowsException(int subjectId, object[] plosArray)
        {
            // Convert the array to Dictionary<string, bool>
            var pLOMappingRequests = new List<PLOMappingRequest>
            {
                new PLOMappingRequest
                {
                    subject_id = subjectId,
                    PLOs = plosArray.ToDictionary(x => (string)((object[])x)[0], x => (bool)((object[])x)[1]),
                }
            };

            // Set up repository mock to simulate failure
            _repoMock.Setup(repo => repo.CheckPLOMappingExsit(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            _repoMock.Setup(repo => repo.DeletePLOMapping(It.IsAny<PLOMapping>())).Returns("Delete failed message");

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => _ploMappingsController.UpdatePLOMapping(pLOMappingRequests));
        }

        [Test]
        [TestCase(10, new object[] { new object[] { "17 PLO1", false }, new object[] { "18-PLO2", true }, new object[] { "19-PLO3", false } })]
        [TestCase(10, new object[] { new object[] { "PLO1", false }, new object[] { "18-PLO2", true }, new object[] { "19-PLO3", false } })]
        public void UpdatePLOMapping_WhenUpdateFails_ThrowsException(int subjectId, object[] plosArray)
        {
            // Convert the array to Dictionary<string, bool>
            var pLOMappingRequests = new List<PLOMappingRequest>
            {
                new PLOMappingRequest
                {
                    subject_id = subjectId,
                    PLOs = plosArray.ToDictionary(x => (string)((object[])x)[0], x => (bool)((object[])x)[1]),
                }
            };

            // Set up repository mock to simulate failure
            _repoMock.Setup(repo => repo.CheckPLOMappingExsit(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
            _repoMock.Setup(repo => repo.DeletePLOMapping(It.IsAny<PLOMapping>())).Returns("Delete failed message");

            // Act and Assert
            Assert.Throws<FormatException>(() => _ploMappingsController.UpdatePLOMapping(pLOMappingRequests));
        }

    }
}
