using AutoFixture;
using AutoMapper;
using AutoMapper.Configuration;
using BusinessObject;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Repositories.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS_UnitTests.Controllers
{
    public class MaterialControllerTest
    {
        private Mock<IMaterialRepository> materialsRepositoryMock;
        private Mock<IMapper> mapperMock;
        private IMapper _mapper;
        private Mock<IWebHostEnvironment> hostingEnvironmentMock;
        private MaterialsController materialsController;
        private IFixture fixture;

        [SetUp]
        public void Setup()
        {
            fixture = new Fixture();
            materialsRepositoryMock = fixture.Freeze<Mock<IMaterialRepository>>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
            var configurationMock = new Mock<IConfiguration>();
            mapperMock = new Mock<IMapper>();
            hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            materialsController = new MaterialsController(_mapper);
        }

        [Test]
        public async Task GetMaterial_ReturnsOkResponse()
        {
            // Arrange
            var syllabus_id = 1;

            var listMaterials = new List<Material>();
            var listMaterialsResponse = new List<MaterialsResponse>();

            materialsRepositoryMock.Setup(repo => repo.GetMaterial(syllabus_id))
                .Returns(listMaterials);

            mapperMock.Setup(mapper => mapper.Map<List<MaterialsResponse>>(listMaterials)).Returns(listMaterialsResponse);

            // Act
            var result = materialsController.GetMaterial(syllabus_id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);

            var okObjectResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            Assert.IsEmpty(listMaterialsResponse);
        }
        [Test]
        public void CreateMaterial_ReturnsOkResult()
        {
            // Arrange
            var materialRequest = new MaterialRequest
            {
                material_description = "TestMaterialDescription",
                material_purpose = "TestPurpose",
                material_ISBN = "TestISBN",
                material_type = "TestType",
                syllabus_id = 2,
                material_note = "TestNote",
                learning_resource_id = 2,
                material_author = "TestAuthor",
                material_publisher = "TestPublisher",
                material_published_date = DateTime.Now,
                material_edition = "TestEdition"
            };

            // Act
            var result = materialsController.CreateMaterial(materialRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            var materialResponse = baseResponse.data as Material;
            Assert.IsNotNull(materialResponse);
        }
        [Test]
        public void EditMaterial_ReturnsOkResult()
        {
            // Arrange
            var materialUpdateRequest = new MaterialUpdateRequest
            {
                material_id = 1,
                material_description = "Updated Material Description",
                material_purpose = "Updated Purpose",
                material_ISBN = "Updated ISBN",
                material_type = "Updated Type",
                syllabus_id = 2,
                material_note = "Updated Note",
                learning_resource_id = 2,
                material_author = "Updated Author",
                material_publisher = "Updated Publisher",
                material_published_date = DateTime.Now,
                material_edition = "Updated Edition"
            };

         

            // Act
            var result = materialsController.EditMaterial(materialUpdateRequest);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            var materialResponse = baseResponse.data as Material;
            Assert.IsNotNull(materialResponse);
            Assert.AreEqual(materialUpdateRequest.material_description, materialResponse.material_description);
            Assert.AreEqual(materialUpdateRequest.material_purpose, materialResponse.material_purpose);
        }
        [Test]
        public void EditMaterial_ReturnsNotFoundResult()
        {
            // Arrange
            var materialUpdateRequest = new MaterialUpdateRequest
            {
                material_id = 9999,
                material_description = "Updated Material Description",
            };

            materialsRepositoryMock.Setup(repo => repo.EditMaterial(It.IsAny<Material>()))
                                  .Returns((Material)null); // Assuming your repository returns null for non-existing Material

            // Act
            var result = materialsController.EditMaterial(materialUpdateRequest);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found The Material!", baseResponse.message);
            Assert.IsNull(baseResponse.data);
        }

        [Test]
        public void DeleteMaterial_ReturnsNotFoundResult()
        {
            // Arrange
            var materialId = 9999;
         
            // Act
            var result = materialsController.DeleteMaterial(materialId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found The Material!", baseResponse.message);
            Assert.IsNull(baseResponse.data);
        }

      
        [Test]
        public async Task DeleteMaterial_ReturnsOkResult()
        {
            // Arrange
            var materialDeteleTestRequest = new MaterialRequest
            {
                material_description = "Updated Material Description",
                material_purpose = "Updated Purpose",
                material_ISBN = "Updated ISBN",
                material_type = "Updated Type",
                syllabus_id = 2,
                material_note = "Updated Note",
                learning_resource_id = 2,
                material_author = "Updated Author",
                material_publisher = "Updated Publisher",
                material_published_date = DateTime.Now,
                material_edition = "Updated Edition"
            };

            // Act
            var createResult = materialsController.CreateMaterial(materialDeteleTestRequest);
            Assert.IsInstanceOf<OkObjectResult>(createResult);


            var okObjectResult = createResult as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;


            var material_id = ((Material)baseResponse.data).material_id;
            var deleteResult = materialsController.DeleteMaterial(material_id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(deleteResult);

            var deleteOkObjectResult = deleteResult as OkObjectResult;
            Assert.IsNotNull(deleteOkObjectResult);

            var baseResponse2 = deleteOkObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse2);

            Assert.IsFalse(baseResponse2.error);
            Assert.AreEqual("Successfully!", baseResponse2.message);
        }
        [Test]
        public async Task GetMaterialById_ReturnsOkResult()
        {
            // Arrange
            var materialId = 1;
            // Act
            var result = materialsController.GetMaterialById(materialId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Successfully!", baseResponse.message);

            var materialResponse = baseResponse.data as MaterialsResponse;
            Assert.IsNotNull(materialResponse);
        }

        [Test]
        public void GetMaterialById_ReturnsNotFoundResult()
        {
            // Arrange
            var materialId = 9999;

            materialsRepositoryMock.Setup(repo => repo.GetMaterialById(It.IsAny<int>()))
                                  .Returns((Material)null);

            // Act
            var result = materialsController.GetMaterialById(materialId);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            var baseResponse = notFoundResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found The Material!", baseResponse.message);
            Assert.IsNull(baseResponse.data);
        }


    }
}
