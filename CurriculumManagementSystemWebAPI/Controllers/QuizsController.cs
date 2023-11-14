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
using System.Xml;
using System.Xml.Linq;

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

        public QuizsController(IMapper mapper)
        {
            _mapper = mapper;
            _quizRepository = new QuizRepository();
            _questionRepository = new QuestionRepository();
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
            if(listQuiz.Count == 0)
            {
                return Ok(new BaseResponse(false, "Subject no contain Quiz"));
            }
            return Ok(new BaseResponse(false, "List Quiz", listQuiz));
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
            if(createResult != Result.createSuccessfull.ToString())
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
            if( listQuestion.Count == 0)
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
            if(_questionRepository.CheckQuestionDuplicate(0, questionDTO.question_name, questionDTO.quiz_id))
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
                        var quiz = new Quiz { quiz_name = sheetName, subject_id = 1 };
                        cMSDbContext.Quiz.Add(quiz);
                        cMSDbContext.SaveChanges();

                        //get Question In file Excel
                        var listQuestion = GetListQuestionInQuiz(Listquestion, quiz.quiz_id);
                        // get a question in list question
                        foreach (var question in listQuestion)
                        {
                            //cMSDbContext.Question.Add(question);
                            //cMSDbContext.SaveChanges();
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

        [HttpPost("ImportQuizXML")]
        public async Task<IActionResult> ImportQuizXML(IFormFile fileQuiz)
        {
            try
            {
                List<Question> listCurriSubject = new List<Question>();
                string baseNode = "/questestinterop/item/presentation";
                


                using (var stream = new MemoryStream())
                {
                    await fileQuiz.CopyToAsync(stream);
                    stream.Position = 0; 

                    var xmlDocument = XDocument.Load(stream);

                    var data = xmlDocument.Descendants("item");

                    foreach (var item in data)
                    {
                        var question = new Question();
                        question.question_name = item.Descendants("material").FirstOrDefault().Value;
                        question.question_type = item.Descendants("qtimetadatafield").Where(x => x.Element("fieldlabel").Value.Equals("QUESTIONTYPE")).Select(x => x.Element("fieldentry").Value).FirstOrDefault();
                        var listAnswer = item.Descendants("response_label");

                        question.answers_1 = listAnswer.Where(x => x.Attribute("ident").Value.Equals("0")).Select(x => x.Element("material").Value).First();
                        question.answers_2 = listAnswer.Where(x => x.Attribute("ident").Value.Equals("1")).Select(x => x.Element("material").Value).First();
                        question.answers_3 = listAnswer.Where(x => x.Attribute("ident").Value.Equals("2")).Select(x => x.Element("material").Value).FirstOrDefault();
                        question.answers_4 = listAnswer.Where(x => x.Attribute("ident").Value.Equals("3")).Select(x => x.Element("material").Value).FirstOrDefault();

                        question.correct_answer = item.Descendants("respcondition").Where(x => x.Element("setvar").Value.Equals("1")).Select(x => x.Element("conditionvar").Value).FirstOrDefault();
                        
                        listCurriSubject.Add(question);

                    }

                    return Ok(listCurriSubject);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse(true, "Error: " + ex.Message));
            }
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
