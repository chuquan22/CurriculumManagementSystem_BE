using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.Batchs;
using Repositories.CurriculumBatchs;
using Repositories.Curriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    [TestFixture]
    public class CurriculumBatchControllerTests
    {
        private CurriculumBatchController _curriculumBatchController;
        private IMapper _mapperMock;
        private Mock<ICurriculumBatchRepository> _curriculumBatchRepositoryMock;
        private Mock<IBatchRepository> _batchRepositoryMock;
        private Mock<ICurriculumRepository> _curriculumRepositoryMock;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapperMock = config.CreateMapper();
            _curriculumBatchRepositoryMock = new Mock<ICurriculumBatchRepository>();
            _batchRepositoryMock = new Mock<IBatchRepository>();
            _curriculumRepositoryMock = new Mock<ICurriculumRepository>();

            _curriculumBatchController = new CurriculumBatchController(
                _mapperMock
            );
        }

        // Test for GetCurriculumBatch
        [Test]
        public void GetCurriculumBatch_ReturnsOkResultWithListOfCurriculumBatch()
        {
            // Arrange

            // Act
            var result = _curriculumBatchController.GetCurriculumBatch();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Curriculum Batch", baseResponse.message);
          
        }

        [Test]
        public void PaginationLearningMethod_ReturnsOkResultWithPagedList()
        {
            // Arrange
          

            // Act
            var result = _curriculumBatchController.PaginationLearningMethod(1, 1, 10, "a");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Curriculum Batch", baseResponse.message);
          
        }

        // Test for GetListCurriculumByBatchName
        [Test]
        public async Task GetListCurriculumByBatchName_ReturnsOkResultWithListOfCurriculum()
        {
            // Arrange
          

            // Act
            var result = await _curriculumBatchController.GetListCurriculumByBatchName(5, "19.2");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("list Curriculum", baseResponse.message);
           
        }

        [Test]
        public async Task GetListCurriculumByBatch_ReturnsOkResultWithListOfCurriculum()
        {
            // Arrange
            var curriculumCode = new List<string> { "GD-GD-CD-19.2"};

            // Act
            var result = await _curriculumBatchController.GetListCurriculumByBatch(1, "19.2", curriculumCode);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("list Curriculum", baseResponse.message);
           
        }

        // Test for GetCurriculumBatchByBatchId
        [Test]
        public void GetCurriculumBatchByBatchId_ReturnsOkResultWithCurriculumBatchDTO()
        {
            // Arrange
            int id = 5;
            // Act
            var result = _curriculumBatchController.GetCurriculumBatch(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Curriculum Batch", baseResponse.message);
        }

        // Test for CreateCurriculumBatch
        [Test]
        [TestCase("Batch 1", 10, 1, new int[] { 5, 6 })]
        [TestCase("Batch 2", 8, 1, new int[] { 7, 8 })]
        public void CreateCurriculumBatch_ReturnsOkResultWithSuccessMessage(string batchName, int batchOrder, int degreeLevelId, int[] listCurriculumId)
        {
            // Arrange
            var curriculumBatchRequest = new CurriculumBatchRequest
            {
                batch_name = batchName,
                batch_order = batchOrder,
                degree_level_id = degreeLevelId,
                list_curriculum_id = listCurriculumId.ToList()
            };

            // Act
            var result = _curriculumBatchController.CreateCurriculumBatch(curriculumBatchRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Create Batch SuccessFull!", baseResponse.message);
        }


        [Test]
        [TestCase(2, "19.2", 1, new int[] { 6, 3 }, true)]
        [TestCase(2, "19.2", 3, new int[] { 6, 3 }, true)]
        [TestCase(2, "19.2", 4, new int[] { 6, 3 }, true)]
        [TestCase(2, "19.2", 8, new int[] { 6, 3 }, true)]
        public void CreateCurriculumBatch_ReturnsBadRequestWhenBatchOrderDuplicate(int batchOrder, string batchName, int degreeLevelId, int[] listCurriculumId, bool isDuplicate)
        {
            // Arrange
            var curriculumBatchRequest = new CurriculumBatchRequest
            {
                batch_order = batchOrder,
                batch_name = batchName,
                degree_level_id = degreeLevelId,
                list_curriculum_id = listCurriculumId.ToList()
            };

            _batchRepositoryMock.Setup(repo => repo.CheckBatchDuplicate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                                .Returns(isDuplicate);

            // Act
            var result = _curriculumBatchController.CreateCurriculumBatch(curriculumBatchRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);

            var baseResponse = badRequestResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
        }



        [Test]
        public void UpdateCurriculumBatch_ReturnsOkResultWithSuccessMessage()
        {
            // Arrange
            var curriculumBatchRequest = new CurriculumBatchRequest
            {
                batch_name = "Updated Batch",
                batch_order = 8,
                degree_level_id = 1,
                list_curriculum_id = new List<int> { 103, 104 }
            };
            var expectedBatch = new Batch
            {
                batch_id = 1,
                batch_name = "Batch 1",
                batch_order = 1,
                degree_level_id = 1
            };
            var expectedUpdateBatchResult = Result.updateSuccessfull.ToString();
            var expectedCurriculumBatchList = new List<CurriculumBatch>
    {
        new CurriculumBatch { Curriculum = new Curriculum { curriculum_id = 103 } },
        new CurriculumBatch { Curriculum = new Curriculum { curriculum_id = 104 } }
    };
            var expectedCreateCurriculumBatchResult = Result.createSuccessfull.ToString();
            _batchRepositoryMock.Setup(repo => repo.GetBatchById(It.IsAny<int>()))
                                .Returns(expectedBatch);
            _batchRepositoryMock.Setup(repo => repo.CheckBatchUpdateDuplicate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                                .Returns(false);
            _batchRepositoryMock.Setup(repo => repo.UpdateBatch(It.IsAny<Batch>()))
                                .Returns(expectedUpdateBatchResult);
            _curriculumBatchRepositoryMock.Setup(repo => repo.GetCurriculumBatchByBatchId(It.IsAny<int>()))
                                          .Returns(expectedCurriculumBatchList);
            _curriculumBatchRepositoryMock.Setup(repo => repo.DeleteCurriculumBatch(It.IsAny<CurriculumBatch>()))
                                          .Returns(Result.deleteSuccessfull.ToString());
            _curriculumBatchRepositoryMock.Setup(repo => repo.CreateCurriculumBatch(It.IsAny<CurriculumBatch>()))
                                          .Returns(expectedCreateCurriculumBatchResult);

            // Act
            var result = _curriculumBatchController.UpdateCurriculumBatch(2, curriculumBatchRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Update Successfull", baseResponse.message);
          
        }

        // Test for DeleteCurriculumBatch
        [Test]
        public void DeleteCurriculumBatch_ReturnsOkResultWithSuccessMessage()
        {
            // Arrange
            int id = 10;

            // Act
            var result = _curriculumBatchController.DeleteCurriculumBatch(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
        }

        [Test]
        public void UpdateCurriculumBatch_ReturnsNotFoundWhenBatchNotFound()
        {
            // Arrange
            _batchRepositoryMock.Setup(repo => repo.GetBatchById(It.IsAny<int>()))
                                .Returns((Batch)null);

            // Act
            var result = _curriculumBatchController.UpdateCurriculumBatch(1, new CurriculumBatchRequest { batch_name = "19.2", batch_order = 1, degree_level_id = 1 });

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
        }

        // Test for UpdateCurriculumBatch when Batch Order is Duplicate
        [Test]
        public void UpdateCurriculumBatch_ReturnsBadRequestWhenBatchOrderDuplicate()
        {
            // Arrange
            var curriculumBatchRequest = new CurriculumBatchRequest { batch_name = "19.2", batch_order = 1, degree_level_id = 1 };
            var expectedBatch = new Batch { batch_id = 4, batch_name = "19.2", batch_order = 1, degree_level_id = 1 };
            _batchRepositoryMock.Setup(repo => repo.GetBatchById(It.IsAny<int>()))
                                .Returns(expectedBatch);
            _batchRepositoryMock.Setup(repo => repo.CheckBatchUpdateDuplicate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                                .Returns(true);

            // Act
            var result = _curriculumBatchController.UpdateCurriculumBatch(4, curriculumBatchRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);

            var baseResponse = badRequestResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual($"Batch Order {curriculumBatchRequest.batch_order} is Duplicate!", baseResponse.message);
        }

        // Test for DeleteCurriculumBatch when Batch is not found
        [Test]
        public void DeleteCurriculumBatch_ReturnsNotFoundWhenBatchNotFound()
        {
            // Arrange
            _batchRepositoryMock.Setup(repo => repo.GetBatchById(It.IsAny<int>()))
                                .Returns((Batch)null);

            // Act
            var result = _curriculumBatchController.DeleteCurriculumBatch(0);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
        }
    }

}
