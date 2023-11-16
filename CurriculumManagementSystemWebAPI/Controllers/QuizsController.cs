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
using System.IO.Compression;
using NuGet.Packaging;
using Microsoft.AspNetCore.Routing.Template;
using System.IO;
using OfficeOpenXml;

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
        private IMajorRepository _majorRepository;

        private readonly HttpClient client = null;
        public static string API_PORT = "https://localhost:8080";
        public static string API_Quiz = "/api/Quizs/CreateQuiz";
        public static string API_Question = "/api/Quizs/CreateQuestion";

        public QuizsController(IMapper mapper)
        {
            _mapper = mapper;
            _quizRepository = new QuizRepository();
            _questionRepository = new QuestionRepository();
            _majorRepository = new MajorRepository();
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
            var quizResponse = _mapper.Map<QuizResponse>(quiz);
            return Ok(new BaseResponse(false, "Quiz", quizResponse));
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
            var quiz = _quizRepository.GetQuizById(quizId);
            var questionResponse = new QuestionDTOResponse();
            questionResponse.subject_id = quiz.subject_id;
            questionResponse.major_id = _majorRepository.GetMajorBySubjectId(questionResponse.subject_id).major_id;
            if (listQuestion.Count == 0)
            {
                return Ok(new BaseResponse(false, "Not Found Question In Quiz", questionResponse));
            }
            var listQuestionResponse = _mapper.Map<List<QuestionResponse>>(listQuestion);
            questionResponse.questionResponses = listQuestionResponse;
            return Ok(new BaseResponse(false, "List Question", questionResponse));
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
                            listCurriSubject.Add(quizId);
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
                            try
                            {
                                CreateQuestionsAPI(questionDTO);
                            }
                            catch(Exception ex)
                            {
                                return BadRequest(new BaseResponse(true, "Error:" + ex.InnerException.Message));
                            }
                            
                        }


                    }
                    return Ok(new BaseResponse(false, "Success", listCurriSubject));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error:" + ex.InnerException.Message));
            }
        }

        [HttpPost("ExportQuizXML/{quizId}")]
        public IActionResult ExportQuizXML(int quizId)
        {
            var templateQTI = "./TemplateQuizXML/Template__Quiz__qti.xml";
            var templateQPL = "./TemplateQuizXML/Template__Quiz__qpl.xml";
            var folder = "./TemplateQuizXML";

            // change file xml to DTDProcessing (fix error DTD prohibited)
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            XmlReader reader = XmlReader.Create(templateQTI, settings);
            XmlReader reader2 = XmlReader.Create(templateQPL, settings);

            // var listQuiz = _quizRepository.GetQUizBySubjectId(subjectId);
            var quiz = _quizRepository.GetQuizById(quizId);
            var subject = quiz.Subject;

            var listQuestion = _questionRepository.GetQuestionByQuiz(quiz.quiz_id);
            var listQuizExport = new List<Quiz_qti_xml>();

            // mapping question to quizExport
            foreach (var question in listQuestion)
            {
                var quizExport = new Quiz_qti_xml { answers = new List<string>(), corrects = new List<int>() };
                quizExport.question_name = question.question_name;
                quizExport.question_name_title = GetTitleByQuestionName(question.question_name);
                quizExport.question_type = question.question_type;

                if (question.answers_A != null)
                {
                    quizExport.answers.Add(question.answers_A.ToString());
                }
                if (question.answers_B != null)
                {
                    quizExport.answers.Add(question.answers_B.ToString());
                }
                if (question.answers_C != null)
                {
                    quizExport.answers.Add(question.answers_C.ToString());
                }
                if (question.answers_D != null)
                {
                    quizExport.answers.Add(question.answers_D.ToString());
                }

                quizExport.corrects = GetListCorrectAnswer(quizExport.answers, question.correct_answer);

                listQuizExport.Add(quizExport);

            }
            // complier set key value in file xml template
            var compiler = new Compiler()
                .AddKey("questions", listQuizExport)
                .AddKey("quiz", quiz)
                .AddKey("subject", subject);

            // complie mapper in file template
            var compiledQTI = compiler.CompileXml(reader);

            var compiledQPL = compiler.CompileXml(reader2);

            // Set the file name
            var zipFileName = $"{subject.subject_code}__{quiz.quiz_name}__qpl_1.zip";
            var zipFilePath = Path.Combine(folder, zipFileName);

            // Create a new zip file
            using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
            {
                var objectsEntry = zipArchive.CreateEntry("objects/");

                var qtiEntry = zipArchive.CreateEntry($"{subject.subject_code}__{quiz.quiz_name}__qti_1.xml");
                using (var qtiStream = qtiEntry.Open())
                using (var writer = new StreamWriter(qtiStream))
                {
                    writer.Write(compiledQTI);
                }

                var qplEntry = zipArchive.CreateEntry($"{subject.subject_code}__{quiz.quiz_name}_qpl_1.xml");
                using (var qplStream = qplEntry.Open())
                using (var writer = new StreamWriter(qplStream))
                {
                    writer.Write(compiledQPL);
                }
            }
            // Return the zip file to the client
            byte[] fileBytes = System.IO.File.ReadAllBytes(zipFilePath);
            // Delete the zip file after sending
            System.IO.File.Delete(zipFilePath);

            return File(fileBytes, "application/zip", zipFileName);

        }

        //[HttpPost("ExportQuizExcel/{subjectId}")]
        //public IActionResult ExportQuizExcel(int subjectId)
        //{
        //    var quizTemplate = "QuizTemplate.xlsx";
            
        //    var listQuiz = _quizRepository.GetQUizBySubjectId(subjectId);
           

        //    //return Ok(fileContents);
        //   // return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Quiz.xlsx");
        //}

        private async Task<int> CreateQuizsAPI(QuizDTORequest quiz)
        {
            string apiUrl = API_PORT + API_Quiz;
            var jsonData = JsonSerializer.Serialize(quiz);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            string[] token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ');
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token[1]);

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

            string[] token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ');
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token[1]);

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
                        question.answers_A = item.ANSWER;
                    }
                    else if (item.ABC.Equals("B"))
                    {
                        question.answers_B = item.ANSWER;
                    }
                    else if (item.ABC.Equals("C"))
                    {
                        question.answers_C = item.ANSWER;
                    }
                    // if final answer
                    else if (item.ABC.Equals("D"))
                    {
                        question.answers_D = item.ANSWER;
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
