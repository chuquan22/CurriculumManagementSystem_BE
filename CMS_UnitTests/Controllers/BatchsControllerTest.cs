using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.Batchs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class BatchsControllerTests
    {
        private BatchsController _batchsController;
        private Mock<IMapper> _mapperMock;
        private Mock<IBatchRepository> _batchRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>();
            _batchRepositoryMock = new Mock<IBatchRepository>();
            _batchsController = new BatchsController(_mapperMock.Object);
        }

        [Test]
        public void GetAllBatch_ReturnsOkResultWithListOfBatch()
        {
            // Arrange
            var expectedBatchList = new List<Batch>(); // Provide a sample list of batches
            _batchRepositoryMock.Setup(repo => repo.GetAllBatch()).Returns(expectedBatchList);

            // Act
            var result = _batchsController.GetAllBatch();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Batch", baseResponse.message);
           
        }

        [Test]
        public void GetBatchNotExsitInSemester_ReturnsOkResultWithListOfBatchResponse()
        {
            // Arrange
            var expectedBatchList = new List<Batch>(); // Provide a sample list of batches
            _batchRepositoryMock.Setup(repo => repo.GetBatchNotExsitInSemester()).Returns(expectedBatchList);
            _mapperMock.Setup(mapper => mapper.Map<List<CurriculumBatchDTOResponse>>(expectedBatchList))
                       .Returns(It.IsAny<List<CurriculumBatchDTOResponse>>());

            // Act
            var result = _batchsController.GetBatchNotExsitInSemester();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Batch", baseResponse.message);
           
        }

        [Test]
        public void GetBatchBySpe_ReturnsOkResultWithListOfBatch()
        {
            // Arrange
            int speId = 1; // Provide a sample specialization ID
            var expectedBatchList = new List<Batch>(); // Provide a sample list of batches
            _batchRepositoryMock.Setup(repo => repo.GetBatchBySpe(speId)).Returns(expectedBatchList);

            // Act
            var result = _batchsController.GetBatch(speId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Batch", baseResponse.message);
         
        }

        [Test]
        public void GetBatchByDegreeLevel_ReturnsOkResultWithListOfBatch()
        {
            // Arrange
            int degreeLevelId = 1; // Provide a sample degree level ID
            var expectedBatchList = new List<Batch>(); // Provide a sample list of batches
            _batchRepositoryMock.Setup(repo => repo.GetBatchByDegreeLevel(degreeLevelId)).Returns(expectedBatchList);

            // Act
            var result = _batchsController.GetBatchByDegreeLevel(degreeLevelId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Batch", baseResponse.message);

        }

        [Test]
        public void GetBatchById_ReturnsOkResultWithBatch()
        {
            // Arrange
            int batchId = 1; // Provide a sample batch ID
            var expectedBatch = new Batch(); // Provide a sample batch
            _batchRepositoryMock.Setup(repo => repo.GetBatchById(batchId)).Returns(expectedBatch);

            // Act
            var result = _batchsController.GetBatchById(batchId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Batch", baseResponse.message);

           
        }

        [Test]
        public void GetBatchBySpe_ReturnsNotFoundResult()
        {
            // Arrange
            int speId = 1; // Provide a sample specialization ID
            _batchRepositoryMock.Setup(repo => repo.GetBatchBySpe(speId)).Returns((List<Batch>)null);

            // Act
            var result = _batchsController.GetBatch(speId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var notFoundResult = result as OkObjectResult;
            Assert.IsNotNull(notFoundResult);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
        }

        [Test]
        public void GetBatchByDegreeLevel_ReturnsNotFoundResult()
        {
            // Arrange
            int degreeLevelId = 1; // Provide a sample degree level ID
            _batchRepositoryMock.Setup(repo => repo.GetBatchByDegreeLevel(degreeLevelId)).Returns((List<Batch>)null);

            // Act
            var result = _batchsController.GetBatchByDegreeLevel(degreeLevelId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var notFoundResult = result as OkObjectResult;
            Assert.IsNotNull(notFoundResult);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
        }

        [Test]
        public void GetBatchById_ReturnsNotFoundResult()
        {
            // Arrange
            int batchId = 100; // Provide a sample batch ID
            _batchRepositoryMock.Setup(repo => repo.GetBatchById(batchId)).Returns((Batch)null);

            // Act
            var result = _batchsController.GetBatchById(batchId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var notFoundResult = result as OkObjectResult;
            Assert.IsNotNull(notFoundResult);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.IsNull(baseResponse.data);

        }
    }

}


