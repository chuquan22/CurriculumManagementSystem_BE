using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CurriculumManagementSystemWebAPI.Controllers;
using DataAccess.Models.DTO.Excel;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Repositories.Questions;
using Repositories.Quizs;
using DataAccess.Models.DTO;
using BusinessObject;
using AutoMapper.Configuration;

namespace CMS_UnitTests.Controllers
{
    public class QuizsControllerTest
    {
        private Mock<IMapper> mapperMock;
        private Mock<IWebHostEnvironment> hostingEnvironmentMock;
        private Mock<IQuizRepository> quizRepositoryMock;
        private Mock<IQuestionRepository> questionRepositoryMock;
        private IMapper _mapper;
        private QuizsController quizsController;

        [SetUp]
        public void Setup()
        {
            mapperMock = new Mock<IMapper>();
            var configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            hostingEnvironmentMock = new Mock<IWebHostEnvironment>();
            quizRepositoryMock = new Mock<IQuizRepository>();
            questionRepositoryMock = new Mock<IQuestionRepository>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
            quizsController = new QuizsController(configurationMock.Object, _mapper, hostingEnvironmentMock.Object);
        }

        [Test]
        public void GetAllQuiz_ReturnsOkResult()
        {
            // Arrange
            var listQuiz = new List<Quiz>();
            quizRepositoryMock.Setup(repo => repo.GetAllQUiz()).Returns(listQuiz);

            // Act
            var result = quizsController.GetAllQuiz();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.AreEqual("List Quiz", baseResponse.message);
        }

        [Test]
        public void GetListQuizBySubject_WithValidSubjectId_ReturnsOkResult()
        {
            int subjectId = 1;
            var listQuiz = new List<Quiz>();
            quizRepositoryMock.Setup(repo => repo.GetQUizBySubjectId(subjectId)).Returns(listQuiz);
            mapperMock.Setup(mapper => mapper.Map<List<QuizDTOResponse>>(listQuiz)).Returns(new List<QuizDTOResponse>());

            // Act
            var result = quizsController.GetListQuizBySubject(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.AreEqual("List Quiz", baseResponse.message);
        }

        [Test]
        public void GetListQuizBySubject_WithInvalidSubjectId_ReturnsOkResult()
        {
            // Arrange
            int subjectId = 999;
            var listQuiz = new List<Quiz>();
            quizRepositoryMock.Setup(repo => repo.GetQUizBySubjectId(subjectId)).Returns(listQuiz);
            mapperMock.Setup(mapper => mapper.Map<List<QuizDTOResponse>>(listQuiz)).Returns(new List<QuizDTOResponse>());

            // Act
            var result = quizsController.GetListQuizBySubject(999);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.AreEqual("Subject no contain Quiz", baseResponse.message);
        }

        [Test]
        public void GetQuizById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var quizId = 1;
            var quiz = new Quiz { quiz_id = quizId };
            // Act
            var result = quizsController.GetQuizById(quizId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);
            Assert.AreEqual("Quiz", baseResponse.message);
        }

        [Test]
        public void GetQuizById_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int quizId = 1;
            var quiz = new Quiz { quiz_id = quizId };
            quizRepositoryMock.Setup(repo => repo.GetQuizById(quizId)).Returns(null as Quiz);

            // Act
            var result = quizsController.GetQuizById(2);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Quiz", baseResponse.message);
        }

        [Test]
        public void CreateQuiz_WithValidData_ReturnsOkResult()
        {
            // Arrange
            var quizDTO = new QuizDTORequest();
            var quiz = new Quiz();
            mapperMock.Setup(mapper => mapper.Map<Quiz>(quizDTO)).Returns(quiz);
            quizRepositoryMock.Setup(repo => repo.CheckQuizDuplicate(quiz.quiz_name, quiz.subject_id)).Returns(false);
            quizRepositoryMock.Setup(repo => repo.CreateQUiz(quiz)).Returns(Result.createSuccessfull.ToString());

            // Act
            var result = quizsController.CreateQuiz(quizDTO);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Create Success", baseResponse.message);

            Assert.AreEqual(quiz, okObjectResult.Value);
        }

        [Test]
        public void CreateQuiz_WithDuplicateData_ReturnsBadRequestResult()
        {
            // Arrange
            var quizDTO = new QuizDTORequest();
            var quiz = new Quiz();
            mapperMock.Setup(mapper => mapper.Map<Quiz>(quizDTO)).Returns(quiz);
            quizRepositoryMock.Setup(repo => repo.CheckQuizDuplicate(quiz.quiz_name, quiz.subject_id)).Returns(true);

            // Act
            var result = quizsController.CreateQuiz(quizDTO);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual($"{quiz.quiz_name} is Duplicate in Subject", baseResponse.message);
        }

        [Test]
        public void CreateQuiz_WithFailedCreation_ReturnsBadRequestResult()
        {
            // Arrange
            var quizDTO = new QuizDTORequest();
            var quiz = new Quiz();
            mapperMock.Setup(mapper => mapper.Map<Quiz>(quizDTO)).Returns(quiz);
            quizRepositoryMock.Setup(repo => repo.CheckQuizDuplicate(quiz.quiz_name, quiz.subject_id)).Returns(false);
            quizRepositoryMock.Setup(repo => repo.CreateQUiz(quiz)).Returns("Creation failed");

            // Act
            var result = quizsController.CreateQuiz(quizDTO);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Creation failed", baseResponse.message);
        }

        [Test]
        public void DeleteQuiz_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int quizId = 1;
            var quiz = new Quiz { quiz_id = quizId };
            quizRepositoryMock.Setup(repo => repo.GetQuizById(quizId)).Returns(quiz);
            quizRepositoryMock.Setup(repo => repo.DeleteQUiz(quiz)).Returns(Result.deleteSuccessfull.ToString());

            // Act
            var result = quizsController.DeleteQuiz(quizId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Delete Success", baseResponse.message);

            Assert.AreEqual(quiz, okObjectResult.Value);
        }

        [Test]
        public void DeleteQuiz_WithInvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            int quizId = 1;
            var quiz = new Quiz { quiz_id = quizId };
            quizRepositoryMock.Setup(repo => repo.GetQuizById(quizId)).Returns(null as Quiz);

            // Act
            var result = quizsController.DeleteQuiz(2);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Not Found Quiz", baseResponse.message);
        }

        [Test]
        public void DeleteQuiz_WithFailedDeletion_ReturnsBadRequestResult()
        {
            // Arrange
            int quizId = 1;
            var quiz = new Quiz { quiz_id = quizId };
            quizRepositoryMock.Setup(repo => repo.GetQuizById(quizId)).Returns(quiz);
            quizRepositoryMock.Setup(repo => repo.DeleteQUiz(quiz)).Returns("Deletion failed");

            // Act
            var result = quizsController.DeleteQuiz(quizId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            var badRequestObjectResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestObjectResult);

            var baseResponse = badRequestObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsTrue(baseResponse.error);
            Assert.AreEqual("Deletion failed", baseResponse.message);
        }

        [Test]
        public void GetListQuestionByQuiz_WithValidQuizId_ReturnsOkResult()
        {
            // Arrange
            int quizId = 1;
            var listQuestion = new List<Question>();
            var quiz = new Quiz { quiz_id = quizId, subject_id = 1 };
            var questionResponse = new QuestionDTOResponse();
            questionRepositoryMock.Setup(repo => repo.GetQuestionByQuiz(quizId)).Returns(listQuestion);
            quizRepositoryMock.Setup(repo => repo.GetQuizById(quizId)).Returns(quiz);
            mapperMock.Setup(mapper => mapper.Map<List<QuestionResponse>>(listQuestion)).Returns(new List<QuestionResponse>());
            var expectedResponse = new BaseResponse(false, "List Question", questionResponse);

            // Act
            var result = quizsController.GetListQuestionByQuiz(quizId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("List Question", baseResponse.message);

            Assert.AreEqual(expectedResponse, okObjectResult.Value);
        }

        [Test]
        public void GetListQuestionByQuiz_WithInvalidQuizId_ReturnsOkResult()
        {
            // Arrange
            int quizId = 1;
            var listQuestion = new List<Question>();
            var quiz = new Quiz { quiz_id = quizId, subject_id = 1 };
            var questionResponse = new QuestionDTOResponse();
            questionRepositoryMock.Setup(repo => repo.GetQuestionByQuiz(quizId)).Returns(listQuestion);
            quizRepositoryMock.Setup(repo => repo.GetQuizById(quizId)).Returns(null as Quiz);
            var expectedResponse = new BaseResponse(false, "Not Found Question In Quiz", questionResponse);

            // Act
            var result = quizsController.GetListQuestionByQuiz(quizId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Not Found Question In Quiz", baseResponse.message);

            Assert.AreEqual(expectedResponse, okObjectResult.Value);
        }

        [Test]
        public void GetQuestionById_WithValidId_ReturnsOkResult()
        {
            // Arrange
            int questionId = 1;
            var question = new Question { question_id = questionId };
            questionRepositoryMock.Setup(repo => repo.GetQuestionById(questionId)).Returns(question);

            // Act
            var result = quizsController.GetQuestionById(questionId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);

            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);

            var baseResponse = okObjectResult.Value as BaseResponse;
            Assert.IsNotNull(baseResponse);

            Assert.IsFalse(baseResponse.error);
            Assert.AreEqual("Question", baseResponse.message);

            Assert.AreEqual(question, okObjectResult.Value);
        }
    }
}
