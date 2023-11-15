using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.Excel;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using Repositories.CLOS;
using Repositories.Major;
using Repositories.Questions;
using Repositories.Quizs;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SuperXML;
using System.Numerics;
using DataAccess.Models.DTO.XML;
using System.Linq;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly CMSDbContext cMSDbContext = new CMSDbContext();
        private IQuizRepository _quizRepository;
        private IQuestionRepository _questionRepository;
        private readonly HttpClient client = null;
        public static string API_PORT = "https://localhost:8080";
        public static string API_Quiz = "/api/Quizs/CreateQuiz";
        public static string API_Question = "/api/Quizs/CreateQuestion";

        public QuizsController(IMapper mapper)
        {
            _mapper = mapper;
            _quizRepository = new QuizRepository();
            _questionRepository = new QuestionRepository();
            client = new HttpClient();

            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        [HttpGet("GetAllQuiz")]
        public IActionResult GetAllQuiz()
        {
            var listQuiz = _quizRepository.GetAllQUiz();
            return Ok(new BaseResponse(false, "List Quiz", listQuiz));
        }

        [HttpGet("GetListQuizBySubject/{subjectId}")]
        public IActionResult GetListQuizBySubject(int subjectId)
        {
            var listQuiz = _quizRepository.GetQUizBySubjectId(subjectId);
            if (listQuiz.Count == 0)
            {
                return Ok(new BaseResponse(false, "Subject no contain Quiz"));
            }
            var listQuizResponse = _mapper.Map<List<QuizDTOResponse>>(listQuiz);
            return Ok(new BaseResponse(false, "List Quiz", listQuizResponse));
        }

        [HttpGet("GetQuizById/{Id}")]
        public IActionResult GetQuizById(int Id)
        {
            var quiz = _quizRepository.GetQuizById(Id);
            if (quiz == null)
            {
                return Ok(new BaseResponse(true, "Not Found Quiz"));
            }
            return Ok(new BaseResponse(false, "Quiz", quiz));
        }

        [HttpPost("CreateQuiz")]
        public IActionResult CreateQuiz([FromBody] QuizDTORequest quizDTO)
        {
            var quiz = _mapper.Map<Quiz>(quizDTO);
            string createResult = _quizRepository.CreateQUiz(quiz);
            if (createResult != Result.createSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, createResult));
            }
            return Ok(new BaseResponse(false, "Create Success", quiz));

        }

        [HttpDelete("DeleteQuiz/{id}")]
        public IActionResult DeleteQuiz(int id)
        {
            var quiz = _quizRepository.GetQuizById(id);
            string deleteResult = _quizRepository.DeleteQUiz(quiz);
            if (deleteResult != Result.deleteSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }
            return Ok(new BaseResponse(false, "Delete Success", quiz));

        }

        [HttpGet("GetListQuestionByQuiz/{quizId}")]
        public IActionResult GetListQuestionByQuiz(int quizId)
        {
            var listQuestion = _questionRepository.GetQuestionByQuiz(quizId);
            if (listQuestion.Count == 0)
            {
                return Ok(new BaseResponse(false, "Not Found Question In Quiz"));
            }
            return Ok(new BaseResponse(false, "List Question", listQuestion));
        }

        [HttpGet("GetQuestionById/{Id}")]
        public IActionResult GetQuestionById(int Id)
        {
            var question = _questionRepository.GetQuestionById(Id);
            if (question == null)
            {
                return Ok(new BaseResponse(true, "Not Found Question"));
            }
            return Ok(new BaseResponse(false, "Question", question));
        }

        [HttpPost("CreateQuestion")]
        public IActionResult CreateQuestion([FromBody] QuestionDTORequest questionDTO)
        {
            if (_questionRepository.CheckQuestionDuplicate(0, questionDTO.question_name, questionDTO.quiz_id))
            {
                return BadRequest(new BaseResponse(true, $"Question {questionDTO.question_name} is Duplicate!"));
            }
            var question = _mapper.Map<Question>(questionDTO);
            string createResult = _questionRepository.CreateQuestion(question);
            if (createResult != Result.createSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, createResult));
            }
            return Ok(new BaseResponse(false, "Create Question Success", question));
        }

        [HttpPut("UpdateQuestion/{id}")]
        public IActionResult UpdateQuestion(int id, [FromBody] QuestionDTORequest questionDTO)
        {
            var question = _questionRepository.GetQuestionById(id);
            if (question == null)
            {
                return BadRequest(new BaseResponse(true, $"Not Found Question"));
            }
            if (_questionRepository.CheckQuestionDuplicate(id, questionDTO.question_name, questionDTO.quiz_id))
            {
                return BadRequest(new BaseResponse(true, $"Question {questionDTO.question_name} is Duplicate!"));
            }
            _mapper.Map(questionDTO, question);
            string updateResult = _questionRepository.UpdateQuestion(question);
            if (updateResult != Result.updateSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }
            return Ok(new BaseResponse(false, "Update Question Success", question));
        }

        [HttpDelete("DeleteQuestion/{id}")]
        public IActionResult DeleteQuestion(int id)
        {
            var question = _questionRepository.GetQuestionById(id);
            if (question == null)
            {
                return BadRequest(new BaseResponse(true, $"Not Found Question"));
            }
            string deleteResult = _questionRepository.DeleteQuestion(question);
            if (deleteResult != Result.deleteSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }
            return Ok(new BaseResponse(false, "Delete Question Success", question));
        }


        [HttpPost("ImportQuizExcel")]
        public async Task<IActionResult> ImportQuizInExcel(IFormFile fileQuiz)
        {
            var config = new OpenXmlConfiguration()
            {
                FillMergedCells = true
            };
            try
            {
                List<object> listCurriSubject = new List<object>();

                var filePath = Path.GetTempFileName();
                var curriculum_id = 0;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileQuiz.CopyToAsync(stream);
                    //Get SheetName
                    var sheetNames = MiniExcel.GetSheetNames(filePath);
                    foreach (var sheetName in sheetNames)
                    {
                        var Listquestion = MiniExcel.Query<QuizExcel>(filePath, sheetName: sheetName, configuration: config, excelType: ExcelType.XLSX);
                        var value = new
                        {
                            ListQuestion = Listquestion,
                        };

                        //Create Quiz
                        var quiz = new QuizDTORequest { quiz_name = sheetName, subject_id = 1 };
                        var quizId = 0;
                        try
                        {
                            quizId = await CreateQuizsAPI(quiz);
                        }
                        catch (Exception ex)
                        {
                            return BadRequest(new BaseResponse(true, "Error:" + ex.InnerException.Message));
                        }

                        //get Question In file Excel
                        var listQuestion = GetListQuestionInQuiz(Listquestion, quizId);
                        // get a question in list question
                        foreach (var question in listQuestion)
                        {
                            var questionDTO = _mapper.Map<QuestionDTORequest>(question);
                            CreateQuestionsAPI(questionDTO);
                            listCurriSubject.Add(question);
                        }


                    }
                    return Ok(listCurriSubject);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error:" + ex.InnerException.Message));
            }
        }

        private async Task<int> CreateQuizsAPI(QuizDTORequest quiz)
        {
            string apiUrl = API_PORT + API_Quiz;
            var jsonData = JsonSerializer.Serialize(quiz);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            response.EnsureSuccessStatusCode();
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var responseObject = JsonSerializer.Deserialize<JsonDocument>(strData, options).RootElement;

            int quizId = responseObject.GetProperty("data").GetProperty("quiz_id").GetInt32();

            return quizId;
        }

        private async Task<Question> CreateQuestionsAPI(QuestionDTORequest question)
        {
            string apiUrl = API_PORT + API_Question;
            var jsonData = JsonSerializer.Serialize(question);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            response.EnsureSuccessStatusCode();
            string strData = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            Question rs = JsonSerializer.Deserialize<Question>(strData, options);
            return rs;
        }

        [HttpPost("ExportQuizXML/{quizId}")]
        public IActionResult ExportQuizXML(int quizId)
        {
            var template = "D:/1699949862__0__qti_77351.xml";
            // var listQuiz = _quizRepository.GetQUizBySubjectId(subjectId);
            var quiz = _quizRepository.GetQuizById(quizId);
            
            var listQuestion = _questionRepository.GetQuestionByQuiz(quiz.quiz_id);
            var listQuizExport = new List<Quiz_qti_xml>();
            

            foreach (var question in listQuestion)
            {
                var quizExport = new Quiz_qti_xml { answers = new List<string>(), corrects = new List<int>()};
                quizExport.question_name = question.question_name;
                quizExport.question_name_title = GetTitleByQuestionName(question.question_name);
                quizExport.question_type = question.question_type;

                if (question.answers_1 != null)
                {
                    quizExport.answers.Add(question.answers_1.ToString());
                }
                 if (question.answers_2 != null)
                {
                    quizExport.answers.Add(question.answers_2.ToString());
                }
                if (question.answers_3 != null)
                {
                    quizExport.answers.Add(question.answers_3.ToString());
                }
                if (question.answers_4 != null)
                {
                    quizExport.answers.Add(question.answers_4.ToString());
                }

                quizExport.corrects = GetListCorrectAnswer(quizExport.answers, question.correct_answer);

                listQuizExport.Add(quizExport);

            }
            var compiler = new Compiler()
                .AddKey("questions", listQuizExport)
                .AddKey("quiz", quiz);

            var compiled = compiler.CompileXml(template);

            var xmlBytes = Encoding.UTF8.GetBytes(compiled);

            // Set the file name
            var fileName = $"{quiz.quiz_name}.xml";

            // Return the XML file
            return File(xmlBytes, "application/xml", fileName);
        }

        private List<int> GetListCorrectAnswer(List<string> answers, string correct_answer)
        {
            string[] answer = { "A", "B", "C", "D" };
            var list = new List<int>();

            for (int i = 0; i < answers.Count; i++)
            {
                list.Add((correct_answer.Contains(answer[i])) ? 1 : 0);
                
            }

           return list;
        }

        private string GetTitleByQuestionName(string questionName)
        {
            string originalString = questionName;

            int maxLength = 50;

            if (originalString.Length > maxLength)
            {
                int index = maxLength;
                while (index > 0 && !char.IsWhiteSpace(originalString[index - 1]) && char.IsLetter(originalString[index - 1]))
                {
                    index--;
                }

                originalString = originalString.Substring(0, index);
            }
            return originalString;
        }


        private List<Question> GetListQuestionInQuiz(IEnumerable<QuizExcel>? questionExcel, int quizId)
        {
            try
            {
                var listQuestions = new List<Question>();
                var question = new Question();
                // get item in list question excel
                foreach (var item in questionExcel)
                {
                    question.question_name = item.QUESTION;

                    question.quiz_id = quizId;

                    // if correct answer not null
                    if (item.CORRECT != null && item.CORRECT != "")
                    {
                        question.correct_answer = item.CORRECT;
                        question.question_type = item.CORRECT.Length > 1 ? "Mutiple Choice" : "Single Choice";
                    }

                    if (item.ABC.Equals("A"))
                    {
                        question.answers_1 = item.ANSWER;
                    }
                    else if (item.ABC.Equals("B"))
                    {
                        question.answers_2 = item.ANSWER;
                    }
                    else if (item.ABC.Equals("C"))
                    {
                        question.answers_3 = item.ANSWER;
                    }
                    // if final answer
                    else if (item.ABC.Equals("D"))
                    {
                        question.answers_4 = item.ANSWER;
                        listQuestions.Add(question);
                        question = new Question();
                    }
                }
                return listQuestions;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
