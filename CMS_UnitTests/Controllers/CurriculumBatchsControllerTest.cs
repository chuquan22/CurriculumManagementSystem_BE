using AutoMapper;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
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
        private Mock<IMapper> _mapperMock;
        private Mock<ICurriculumBatchRepository> _curriculumBatchRepositoryMock;
        private Mock<IBatchRepository> _batchRepositoryMock;
        private Mock<ICurriculumRepository> _curriculumRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _mapperMock = new Mock<IMapper>();
            _curriculumBatchRepositoryMock = new Mock<ICurriculumBatchRepository>();
            _batchRepositoryMock = new Mock<IBatchRepository>();
            _curriculumRepositoryMock = new Mock<ICurriculumRepository>();

            _curriculumBatchController = new CurriculumBatchController(
                _mapperMock.Object
            );
        }

        // Test for GetCurriculumBatch
        [Test]
        public void GetCurriculumBatch_ReturnsOkResultWithListOfCurriculumBatch()
        {
            // Arrange
            var expectedBatchList = new List<Batch>
        {
            new Batch { batch_id = 1, batch_name = "Batch 1", batch_order = 1, degree_level_id = 1 },
            new Batch { batch_id = 2, batch_name = "Batch 2", batch_order = 2, degree_level_id = 1 }
        };
            _batchRepositoryMock.Setup(repo => repo.GetAllBatch()).Returns(expectedBatchList);

            var expectedCurriculumBatchList = new List<CurriculumBatch>
        {
            new CurriculumBatch { batch_id = 1, curriculum_id = 101 },
            new CurriculumBatch { batch_id = 2, curriculum_id = 102 },
            new CurriculumBatch { batch_id = 2, curriculum_id = 103 }
        };
            _curriculumBatchRepositoryMock.Setup(repo => repo.GetAllCurriculumBatch()).Returns(expectedCurriculumBatchList);

            var expectedCurriculumBatchDTOList = new List<CurriculumBatchDTOResponse>
        {
            new CurriculumBatchDTOResponse
            {
                batch_id = 1,
                batch_name = "Batch 1",
                batch_order = 1,
                degree_level_id = 1,
                curriculum = new List<CurriculumResponse>
                {
                    new CurriculumResponse { curriculum_id = 101 },
                }
            },
            new CurriculumBatchDTOResponse
            {
                batch_id = 2,
                batch_name = "Batch 2",
                batch_order = 2,
                degree_level_id = 1,
                curriculum = new List<CurriculumResponse>
                {
                    new CurriculumResponse { curriculum_id = 102 },
                    new CurriculumResponse { curriculum_id = 103 }
                }
            }
        };
            _mapperMock.Setup(mapper => mapper.Map<CurriculumBatchDTOResponse>(It.IsAny<Batch>()))
                       .Returns((Batch batch) => expectedCurriculumBatchDTOList
                                                       .FirstOrDefault(dto => dto.batch_id == batch.batch_id));

            _mapperMock.Setup(mapper => mapper.Map<List<CurriculumResponse>>(It.IsAny<List<Curriculum>>()))
                       .Returns((List<Curriculum> curricula) => curricula
                                                                 .Select(curriculum =>
                                                                     new CurriculumResponse { curriculum_id = curriculum.curriculum_id })
                                                                 .ToList());

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

            var data = okObjectResult.Value as List<CurriculumBatchDTOResponse>;
            Assert.IsNotNull(data);
            Assert.AreEqual(expectedCurriculumBatchDTOList.Count, data.Count);

            for (var i = 0; i < expectedCurriculumBatchDTOList.Count; i++)
            {
                Assert.AreEqual(expectedCurriculumBatchDTOList[i].batch_id, data[i].batch_id);
                Assert.AreEqual(expectedCurriculumBatchDTOList[i].batch_name, data[i].batch_name);
                Assert.AreEqual(expectedCurriculumBatchDTOList[i].batch_order, data[i].batch_order);
                Assert.AreEqual(expectedCurriculumBatchDTOList[i].degree_level_id, data[i].degree_level_id);

                CollectionAssert.AreEqual(
                    expectedCurriculumBatchDTOList[i].curriculum.Select(c => c.curriculum_id).ToList(),
                    data[i].curriculum.Select(c => c.curriculum_id).ToList()
                );
            }
        }

        [Test]
        public void PaginationLearningMethod_ReturnsOkResultWithPagedList()
        {
            // Arrange
            var expectedBatchList = new List<Batch>
        {
            new Batch { batch_id = 1, batch_name = "Batch 1", batch_order = 1, degree_level_id = 1 },
            new Batch { batch_id = 2, batch_name = "Batch 2", batch_order = 2, degree_level_id = 1 }
        };
            _batchRepositoryMock.Setup(repo => repo.PaginationCurriculumBatch(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                                .Returns(expectedBatchList);

            var expectedCurriculumBatchDTOList = new List<CurriculumBatchDTOResponse>
        {
            new CurriculumBatchDTOResponse
            {
                batch_id = 1,
                batch_name = "Batch 1",
                batch_order = 1,
                degree_level_id = 1,
                curriculum = new List<CurriculumResponse>
                {
                    new CurriculumResponse { curriculum_id = 101 },
                }
            },
            new CurriculumBatchDTOResponse
            {
                batch_id = 2,
                batch_name = "Batch 2",
                batch_order = 2,
                degree_level_id = 1,
                curriculum = new List<CurriculumResponse>
                {
                    new CurriculumResponse { curriculum_id = 102 },
                    new CurriculumResponse { curriculum_id = 103 }
                }
            }
        };
            _mapperMock.Setup(mapper => mapper.Map<CurriculumBatchDTOResponse>(It.IsAny<Batch>()))
                       .Returns((Batch batch) => expectedCurriculumBatchDTOList
                                                       .FirstOrDefault(dto => dto.batch_id == batch.batch_id));

            _mapperMock.Setup(mapper => mapper.Map<List<CurriculumResponse>>(It.IsAny<List<Curriculum>>()))
                       .Returns((List<Curriculum> curricula) => curricula
                                                                 .Select(curriculum =>
                                                                     new CurriculumResponse { curriculum_id = curriculum.curriculum_id })
                                                                 .ToList());

            // Act
            var result = _curriculumBatchController.PaginationLearningMethod(1, 10, "searchTerm");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Curriculum Batch", baseResponse.message);

            var data = okObjectResult.Value as BaseListResponse;
            Assert.IsNotNull(data);

            Assert.AreEqual(expectedBatchList.Count, data.totalElement);

            var pagedList = data.data as List<CurriculumBatchDTOResponse>;
            Assert.IsNotNull(pagedList);
            Assert.AreEqual(expectedCurriculumBatchDTOList.Count, pagedList.Count);

            for (var i = 0; i < expectedCurriculumBatchDTOList.Count; i++)
            {
                Assert.AreEqual(expectedCurriculumBatchDTOList[i].batch_id, pagedList[i].batch_id);
                // Additional assertions for other properties
            }
        }

        // Test for GetListCurriculumByBatchName
        [Test]
        public async Task GetListCurriculumByBatchName_ReturnsOkResultWithListOfCurriculum()
        {
            // Arrange
            var expectedCurriculumList = new List<Curriculum>
        {
            new Curriculum { curriculum_id = 101, curriculum_name = "Curriculum 1" },
            new Curriculum { curriculum_id = 102, curriculum_name = "Curriculum 2" }
        };
            _curriculumRepositoryMock.Setup(repo => repo.GetListCurriculumByBatchName(It.IsAny<int>(), It.IsAny<string>()))
                                     .Returns(expectedCurriculumList);

            var expectedCurriculumResponseList = new List<CurriculumResponse>
        {
            new CurriculumResponse { curriculum_id = 101, curriculum_name = "Curriculum 1" },
            new CurriculumResponse { curriculum_id = 102, curriculum_name = "Curriculum 2" }
        };
            _mapperMock.Setup(mapper => mapper.Map<List<CurriculumResponse>>(It.IsAny<List<Curriculum>>()))
                       .Returns(expectedCurriculumResponseList);

            // Act
            var result = await _curriculumBatchController.GetListCurriculumByBatchName(1, "Batch1");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("list Curriculum", baseResponse.message);

            var data = okObjectResult.Value as List<CurriculumResponse>;
            Assert.IsNotNull(data);
            Assert.AreEqual(expectedCurriculumResponseList.Count, data.Count);

            for (var i = 0; i < expectedCurriculumResponseList.Count; i++)
            {
                Assert.AreEqual(expectedCurriculumResponseList[i].curriculum_id, data[i].curriculum_id);
                // Additional assertions for other properties
            }
        }

        [Test]
        public async Task GetListCurriculumByBatch_ReturnsOkResultWithListOfCurriculum()
        {
            // Arrange
            var expectedCurriculumList = new List<Curriculum>
    {
        new Curriculum { curriculum_id = 101, curriculum_name = "Curriculum 1" },
        new Curriculum { curriculum_id = 102, curriculum_name = "Curriculum 2" }
    };
            _curriculumRepositoryMock.Setup(repo => repo.GetCurriculumByBatch(It.IsAny<int>(), It.IsAny<string>()))
                                     .Returns(expectedCurriculumList);

            var expectedCurriculumResponseList = new List<CurriculumResponse>
    {
        new CurriculumResponse { curriculum_id = 101, curriculum_name = "Curriculum 1" },
        new CurriculumResponse { curriculum_id = 102, curriculum_name = "Curriculum 2" }
    };
            _mapperMock.Setup(mapper => mapper.Map<List<CurriculumResponse>>(It.IsAny<List<Curriculum>>()))
                       .Returns(expectedCurriculumResponseList);

            var curriculumCode = new List<string> { "Code-101", "Code-102" };

            // Act
            var result = await _curriculumBatchController.GetListCurriculumByBatch(1, "Batch1", curriculumCode);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("list Curriculum", baseResponse.message);

            var data = okObjectResult.Value as List<CurriculumResponse>;
            Assert.IsNotNull(data);
            Assert.AreEqual(expectedCurriculumResponseList.Count, data.Count);

            for (var i = 0; i < expectedCurriculumResponseList.Count; i++)
            {
                Assert.AreEqual(expectedCurriculumResponseList[i].curriculum_id, data[i].curriculum_id);
                // Additional assertions for other properties
            }
        }

        // Test for GetCurriculumBatchByBatchId
        [Test]
        public void GetCurriculumBatchByBatchId_ReturnsOkResultWithCurriculumBatchDTO()
        {
            // Arrange
            var expectedBatch = new Batch { batch_id = 1, batch_name = "Batch 1", batch_order = 1, degree_level_id = 1 };
            var expectedCurriculumBatchList = new List<CurriculumBatch>
    {
        new CurriculumBatch { Curriculum = new Curriculum { curriculum_id = 101 } },
        new CurriculumBatch { Curriculum = new Curriculum { curriculum_id = 102 } }
    };
            _batchRepositoryMock.Setup(repo => repo.GetBatchById(It.IsAny<int>()))
                                .Returns(expectedBatch);
            _curriculumBatchRepositoryMock.Setup(repo => repo.GetCurriculumBatchByBatchId(It.IsAny<int>()))
                                          .Returns(expectedCurriculumBatchList);

            var expectedCurriculumBatchDTO = new CurriculumBatchDTOResponse
            {
                batch_id = 1,
                batch_name = "Batch 1",
                batch_order = 1,
                degree_level_id = 1,
                curriculum = new List<CurriculumResponse>
        {
            new CurriculumResponse { curriculum_id = 101 },
            new CurriculumResponse { curriculum_id = 102 }
        }
            };
            _mapperMock.Setup(mapper => mapper.Map<CurriculumBatchDTOResponse>(It.IsAny<Batch>()))
                       .Returns(expectedCurriculumBatchDTO);
            _mapperMock.Setup(mapper => mapper.Map<List<CurriculumResponse>>(It.IsAny<List<CurriculumBatch>>()))
                       .Returns(expectedCurriculumBatchDTO.curriculum);

            // Act
            var result = _curriculumBatchController.GetCurriculumBatch(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Curriculum Batch", baseResponse.message);

            var data = okObjectResult.Value as CurriculumBatchDTOResponse;
            Assert.IsNotNull(data);

            Assert.AreEqual(expectedCurriculumBatchDTO.batch_id, data.batch_id);
            // Additional assertions for other properties
        }

        // Test for CreateCurriculumBatch
        [Test]
        public void CreateCurriculumBatch_ReturnsOkResultWithSuccessMessage()
        {
            // Arrange
            var curriculumBatchRequest = new CurriculumBatchRequest
            {
                batch_name = "Batch 1",
                batch_order = 1,
                degree_level_id = 1,
                list_curriculum_id = new List<int> { 101, 102 }
            };
            var expectedBatch = new Batch
            {
                batch_name = "Batch 1",
                batch_order = 1,
                degree_level_id = 1
            };
            var expectedCreateBatchResult = Result.createSuccessfull.ToString();
            var expectedCurriculumBatch = new CurriculumBatch
            {
                batch_id = 1,
                curriculum_id = 101
            };
            var expectedCreateCurriculumBatchResult = Result.createSuccessfull.ToString();
            _mapperMock.Setup(mapper => mapper.Map<Batch>(It.IsAny<CurriculumBatchRequest>()))
                       .Returns(expectedBatch);
            _batchRepositoryMock.Setup(repo => repo.CheckBatchDuplicate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                                .Returns(false);
            _batchRepositoryMock.Setup(repo => repo.CreateBatch(It.IsAny<Batch>()))
                                .Returns(expectedCreateBatchResult);
            _curriculumBatchRepositoryMock.Setup(repo => repo.CreateCurriculumBatch(It.IsAny<CurriculumBatch>()))
                                          .Returns(expectedCreateCurriculumBatchResult);

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

            var data = okObjectResult.Value as CurriculumBatchRequest;
            Assert.IsNotNull(data);
            // Additional assertions for other properties
        }

        [Test]
        public void CreateCurriculumBatch_ReturnsBadRequestWhenBatchOrderDuplicate()
        {
            // Arrange
            var curriculumBatchRequest = new CurriculumBatchRequest { batch_order = 2 };
            _batchRepositoryMock.Setup(repo => repo.CheckBatchDuplicate(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                                .Returns(true);

            // Act
            var result = _curriculumBatchController.CreateCurriculumBatch(curriculumBatchRequest);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);

            var baseResponse = badRequestResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual($"Batch {curriculumBatchRequest.batch_name} or Batch Order {curriculumBatchRequest.batch_order} is Duplicate!", baseResponse.message);
        }


        [Test]
        public void UpdateCurriculumBatch_ReturnsOkResultWithSuccessMessage()
        {
            // Arrange
            var curriculumBatchRequest = new CurriculumBatchRequest
            {
                batch_name = "Updated Batch",
                batch_order = 2,
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
            var result = _curriculumBatchController.UpdateCurriculumBatch(1, curriculumBatchRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Update Successfull", baseResponse.message);

            var data = okObjectResult.Value as CurriculumBatchRequest;
            Assert.IsNotNull(data);
            // Additional assertions for other properties
        }

        // Test for DeleteCurriculumBatch
        [Test]
        public void DeleteCurriculumBatch_ReturnsOkResultWithSuccessMessage()
        {
            // Arrange
            var expectedBatch = new Batch { batch_id = 1, batch_name = "Batch 1", batch_order = 1, degree_level_id = 1 };
            var expectedDeleteBatchResult = Result.deleteSuccessfull.ToString();
            var expectedCurriculumBatchList = new List<CurriculumBatch>
    {
        new CurriculumBatch { Curriculum = new Curriculum { curriculum_id = 101 } },
        new CurriculumBatch { Curriculum = new Curriculum { curriculum_id = 102 } }
    };
            _batchRepositoryMock.Setup(repo => repo.GetBatchById(It.IsAny<int>()))
                                .Returns(expectedBatch);
            _batchRepositoryMock.Setup(repo => repo.CheckBatchExsit(It.IsAny<int>()))
                                .Returns(false);
            _batchRepositoryMock.Setup(repo => repo.DeleteBatch(It.IsAny<Batch>()))
                                .Returns(expectedDeleteBatchResult);
            _curriculumBatchRepositoryMock.Setup(repo => repo.GetCurriculumBatchByBatchId(It.IsAny<int>()))
                                          .Returns(expectedCurriculumBatchList);
            _curriculumBatchRepositoryMock.Setup(repo => repo.DeleteCurriculumBatch(It.IsAny<CurriculumBatch>()))
                                          .Returns(Result.deleteSuccessfull.ToString());

            // Act
            var result = _curriculumBatchController.DeleteCurriculumBatch(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual($"Delete Batch {expectedBatch.batch_name} Successfull", baseResponse.message);
        }

        [Test]
        public void UpdateCurriculumBatch_ReturnsNotFoundWhenBatchNotFound()
        {
            // Arrange
            _batchRepositoryMock.Setup(repo => repo.GetBatchById(It.IsAny<int>()))
                                .Returns((Batch)null);

            // Act
            var result = _curriculumBatchController.UpdateCurriculumBatch(1, new CurriculumBatchRequest());

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Can't Found Batch", baseResponse.message);
        }

        // Test for UpdateCurriculumBatch when Batch Order is Duplicate
        [Test]
        public void UpdateCurriculumBatch_ReturnsBadRequestWhenBatchOrderDuplicate()
        {
            // Arrange
            var curriculumBatchRequest = new CurriculumBatchRequest { batch_order = 2 };
            var expectedBatch = new Batch { batch_id = 1, batch_name = "Batch 1", batch_order = 1, degree_level_id = 1 };
            _batchRepositoryMock.Setup(repo => repo.GetBatchById(It.IsAny<int>()))
                                .Returns(expectedBatch);
            _batchRepositoryMock.Setup(repo => repo.CheckBatchUpdateDuplicate(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                                .Returns(true);

            // Act
            var result = _curriculumBatchController.UpdateCurriculumBatch(1, curriculumBatchRequest);

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
            var result = _curriculumBatchController.DeleteCurriculumBatch(1);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Can't Found Batch", baseResponse.message);
        }
    }

}
