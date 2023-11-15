using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Batchs;
using Repositories.CurriculumBatchs;
using Repositories.Curriculums;
using Repositories.LearningMethods;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumBatchController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ICurriculumBatchRepository _curriculumBatchRepository;
        private IBatchRepository _batchRepository;
        private ICurriculumRepository _curriculumRepository;

        public CurriculumBatchController(IMapper mapper)
        {
            _mapper = mapper;
            _curriculumBatchRepository = new CurriculumBatchRepository();
            _batchRepository = new BatchRepository();
            _curriculumRepository = new CurriculumRepository();
        }

        [HttpGet("GetAllCurriculumBatch")]
        public IActionResult GetCurriculumBatch()
        {
            var listBatch = _batchRepository.GetAllBatch();
            var listCurriBatch = _curriculumBatchRepository.GetAllCurriculumBatch();
            List<CurriculumBatchDTOResponse> listCurriculumBatch = new List<CurriculumBatchDTOResponse>();

            foreach (var batch in listBatch)
            {
                var curriBacthDTO = new CurriculumBatchDTOResponse { curriculum = new List<CurriculumResponse>() };
                curriBacthDTO.batch_id = batch.batch_id;
                curriBacthDTO.batch_name = batch.batch_name;
                curriBacthDTO.batch_order = batch.batch_order;
                var curriResponse = _mapper.Map<List<CurriculumResponse>>(listCurriBatch.Where(x => x.batch_id == batch.batch_id).Select(x => x.Curriculum).ToList());
                curriBacthDTO.curriculum = curriResponse;

                listCurriculumBatch.Add(curriBacthDTO);
            }

            return Ok(new BaseResponse(false, "Curriculum Batch", listCurriculumBatch));
        }

        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationLearningMethod(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listBatch = _batchRepository.PaginationCurriculumBatch(page, limit, txtSearch);

            if (listBatch.Count == 0)
            {
                Ok(new BaseResponse(false, $"Not Found Batch {txtSearch}!"));
            }

            List<CurriculumBatchDTOResponse> listCurriculumBatch = new List<CurriculumBatchDTOResponse>();

            foreach (var batch in listBatch)
            {
                var listcurriBatch = _curriculumBatchRepository.GetCurriculumBatchByBatchId(batch.batch_id);
                var curriBacthDTO = new CurriculumBatchDTOResponse { curriculum = new List<CurriculumResponse>() };
                curriBacthDTO.batch_id = batch.batch_id;
                curriBacthDTO.batch_name = batch.batch_name;
                curriBacthDTO.batch_order = batch.batch_order;
                var curriResponse = _mapper.Map<List<CurriculumResponse>>(listcurriBatch.Select(x => x.Curriculum).ToList());
                curriBacthDTO.curriculum = curriResponse;

                listCurriculumBatch.Add(curriBacthDTO);
            }

            var total = _batchRepository.GetTotalCurriculumBatch(txtSearch);
            return Ok(new BaseResponse(false, "List Curriculum Batch", new BaseListResponse(page, limit, total, listCurriculumBatch)));
        }


        // GET: api/Curriculums/GetListCurriculumByBatchName/batchName
        [HttpGet("GetListCurriculumByBatchName/{batchId}/{batchName}")]
        public async Task<IActionResult> GetListCurriculumByBatchName(int batchId, string batchName)
        {
            var listCurriculum = _curriculumRepository.GetListCurriculumByBatchName(batchId, batchName);
            if (listCurriculum.Count == 0)
            {
                return BadRequest(new BaseResponse(true, $"Cannot Found Curriculum Have Start Batch Smaller than {batchName}"));
            }
            var CurriculumRespone = _mapper.Map<List<CurriculumResponse>>(listCurriculum);
            return Ok(new BaseResponse(false, "list Curriculum", CurriculumRespone));
        }


        [HttpGet("GetCurriculumBatchByBatchId/{batchId}")]
        public IActionResult GetCurriculumBatch(int batchId)
        {
            var curriBacthDTO = new CurriculumBatchDTOResponse { curriculum = new List<CurriculumResponse>() };

            var batch = _batchRepository.GetBatchById(batchId);
            var listCurriBatch = _curriculumBatchRepository.GetCurriculumBatchByBatchId(batchId);

            curriBacthDTO.batch_id = batch.batch_id;
            curriBacthDTO.batch_name = batch.batch_name;
            curriBacthDTO.batch_order = batch.batch_order;
            foreach (var curriBatch in listCurriBatch)
            {
                var curriResponse = _mapper.Map<CurriculumResponse>(curriBatch.Curriculum);
                curriBacthDTO.curriculum.Add(curriResponse);
            }

            return Ok(new BaseResponse(false, "Curriculum Batch", curriBacthDTO));
        }

        [HttpPost("CreateCurriculumBatch")]
        public ActionResult CreateCurriculumBatch([FromBody] CurriculumBatchRequest curriculumBatchRequest)
        {
            var batch = new Batch
            {
                batch_name = curriculumBatchRequest.batch_name,
                batch_order = curriculumBatchRequest.batch_order,
                degree_level_id = curriculumBatchRequest.degree_level_id
            };

            if (_batchRepository.CheckBatchDuplicate(batch.batch_name))
            {
                return BadRequest(new BaseResponse(true, $"Batch {batch.batch_name} is Duplicate!"));
            }
            string create = _batchRepository.CreateBatch(batch);
            if (!create.Equals(Result.createSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, create));
            }

            foreach (var curriculum_id in curriculumBatchRequest.list_curriculum_id)
            {
                var curriBatch = new CurriculumBatch
                {
                    batch_id = batch.batch_id,
                    curriculum_id = curriculum_id,
                };
                _curriculumBatchRepository.CreateCurriculumBatch(curriBatch);
            }

            return Ok(new BaseResponse(false, "Create Batch SuccessFull!", curriculumBatchRequest));
        }


        [HttpPut("UpdateCurriculumBatch/{id}")]
        public IActionResult UpdateCurriculumBatch(int id, [FromBody] CurriculumBatchRequest curriculumBatchRequest)
        {
            var batch = _batchRepository.GetBatchById(id);
            if (_batchRepository.CheckBatchUpdateDuplicate(id, batch.batch_name))
            {
                return BadRequest(new BaseResponse(true, "Batch is Duplicate!"));
            }
            batch.batch_name = curriculumBatchRequest.batch_name;
            batch.batch_order = curriculumBatchRequest.batch_order;
            batch.degree_level_id = curriculumBatchRequest.degree_level_id;

            string updateResult = _batchRepository.UpdateBatch(batch);
            if (updateResult != Result.updateSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            var listCurriBatch = _curriculumBatchRepository.GetCurriculumBatchByBatchId(id);
            foreach (var cb in listCurriBatch)
            {
                string removecurriBatch = _curriculumBatchRepository.DeleteCurriculumBatch(cb);
                if (removecurriBatch != Result.deleteSuccessfull.ToString())
                {
                    return BadRequest(new BaseResponse(true, "Update Curriculum Batch Fail!"));
                }
            }

            foreach (var curriId in curriculumBatchRequest.list_curriculum_id)
            {
                var curriBatch = new CurriculumBatch
                {
                    batch_id = batch.batch_id,
                    curriculum_id = curriId
                };
                string createcurriBatch = _curriculumBatchRepository.CreateCurriculumBatch(curriBatch);
                if (createcurriBatch != Result.createSuccessfull.ToString())
                {
                    return BadRequest(new BaseResponse(true, "Update Curriculum Batch Fail!"));
                }
            }

            return Ok(new BaseResponse(false, "Update Successfull", curriculumBatchRequest));
        }

        [HttpDelete("DeleteCurriculumBatch/{id}")]
        public IActionResult DeleteCurriculumBatch(int id)
        {
            var batch = _batchRepository.GetBatchById(id);
            if (_batchRepository.CheckBatchExsit(id))
            {
                return BadRequest(new BaseResponse(true, $"Batch {batch.batch_name} is Used. Can't Delete!"));
            }

            string deleteBatch = _batchRepository.DeleteBatch(batch);
            if (deleteBatch != Result.deleteSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, deleteBatch));
            }

            var listCurriBatch = _curriculumBatchRepository.GetCurriculumBatchByBatchId(id);
            foreach (var cb in listCurriBatch)
            {
                string removecurriBatch = _curriculumBatchRepository.DeleteCurriculumBatch(cb);
                if (removecurriBatch != Result.deleteSuccessfull.ToString())
                {
                    return BadRequest(new BaseResponse(true, "Update Curriculum Batch Fail!"));
                }
            }

            return Ok(new BaseResponse(false, $"Delete Batch {batch.batch_name} Successfull"));
        }

    }
}
