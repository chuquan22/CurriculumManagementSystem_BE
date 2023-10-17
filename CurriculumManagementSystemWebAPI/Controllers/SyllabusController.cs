using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.CLOS;
using Repositories.GradingStruture;
using Repositories.Materials;
using Repositories.Session;
using Repositories.Syllabus;
using MiniExcelLibs;
using DataAccess.Models.DTO.excel;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyllabusController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ISyllabusRepository repo;


        public SyllabusController(IMapper mapper)
        {
            _mapper = mapper;
            repo = new SyllabusRepository();
        }
        [HttpGet]
        public ActionResult GetListSyllabus(int page,int limit, string? txtSearch)
        {
            List<Syllabus> rs = new List<Syllabus>();
            try
            {             
                int limit2 = repo.GetTotalSyllabus(txtSearch);
                List<Syllabus> list = repo.GetListSyllabus(page, limit, txtSearch);
                var result = _mapper.Map<List<SyllabusResponse>>(list);
                return Ok(new BaseResponse(false, "Sucess", new BaseListResponse(page,limit2, result)));
            }
            catch (Exception)
            {
                return BadRequest(new BaseResponse(true, "error", null));
            }
            return Ok(new BaseResponse(true, "False", null));
        }

        [HttpGet("GetSyllabusDetails")]
        public ActionResult SyllabusDetails(int syllabus_id)
        {
            try
            {
                Syllabus rs1 = repo.GetSyllabusById(syllabus_id);
                var result = _mapper.Map<SyllabusDetailsResponse>(rs1);
                List<PreRequisite> pre = repo.GetPre(rs1.subject_id);
               
                result.pre_required = _mapper.Map<List<PreRequisiteResponse2>>(pre);
                return Ok(new BaseResponse(true, "False", result));

            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }

        }
        [HttpGet("ImportSyllabus")]
        public ActionResult ImportSyllabus()
        {
            try
            {
                var filePath = "Syllabus.xlsx";

                using (var stream = System.IO.File.OpenRead(filePath))
                {
                    //Get SheetName
                    var sheetNames = MiniExcel.GetSheetNames(filePath);
                    List<object> rs = new List<object>();                 
                    for (int i = 0; i < sheetNames.Count; i++)
                    {
                        if (i == 0)
                        {
                            var row = MiniExcel.Query<SyllabusExcel>(filePath, sheetName: sheetNames[i]);
                            var value = new
                            {
                                Syllabus = row,
                            };
                            rs.Add(value);
                        }
                        else if(i==1)
                        {
                            var row = MiniExcel.Query<MaterialExcel>(filePath, sheetName: sheetNames[i]);
                            var value = new
                            {
                                Materials = row,
                            };
                            rs.Add(value);
                        }
                        else if (i == 2)
                        {
                            var row = MiniExcel.Query<CLOsExcel>(filePath, sheetName: sheetNames[i]);
                            var value = new
                            {
                                CLOs = row,
                            };
                            rs.Add(value);
                        }
                        else if (i == 5)
                        {
                            var row = MiniExcel.Query<GradingStrutureExcel>(filePath, sheetName: sheetNames[i]);
                            var value = new
                            {
                                GradingStruture = row,
                            };
                            rs.Add(value);
                        }
                    }
                    return Ok(new BaseResponse(true, "False", rs));

                }
                return Ok(new BaseResponse(true, "False", null));

            }
            catch (Exception)
            {

                return BadRequest(new BaseResponse(true, "error", null));
            }

        }
    }
}
