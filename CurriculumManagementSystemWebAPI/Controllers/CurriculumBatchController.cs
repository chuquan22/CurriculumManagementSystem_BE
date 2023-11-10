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
                var curriResponse = _mapper.Map<List<CurriculumResponse>>(listCurriBatch.Where(x => x.batch_id == batch.batch_id).Select(x => x.Curriculum).ToList());
                curriBacthDTO.curriculum = curriResponse;

                listCurriculumBatch.Add(curriBacthDTO);


            }

            return Ok(new BaseResponse(false, "Curriculum Batch", listCurriculumBatch));
        }

        [HttpGet("GetCurriculumByBatchName/{batchName}")]
        public IActionResult GetCurriculumByBatchName(string batchName)
        {
            var listCurriculum = _curriculumRepository.GetListCurriculumByBatchName(batchName);
            var listCurriResponse = _mapper.Map<List<CurriculumResponse>>(listCurriculum);

            return Ok(new BaseResponse(false, "List Curriculum", listCurriResponse));
        }



        [HttpGet("GetCurriculumBatchByBatchId/{batchId}")]
        public IActionResult GetCurriculumBatch(int batchId)
        {
            var listCurriBatch = _curriculumBatchRepository.GetCurriculumBatchByBatchId(batchId);
            var curriBacthDTO = new CurriculumBatchDTOResponse { curriculum = new List<CurriculumResponse>() };
            curriBacthDTO.batch_id = listCurriBatch.FirstOrDefault().batch_id;
            curriBacthDTO.batch_name = listCurriBatch.FirstOrDefault().Batch.batch_name;
            foreach (var curriBatch in listCurriBatch)
            {
                var curriResponse = _mapper.Map<CurriculumResponse>(curriBatch.Curriculum);
                curriBacthDTO.curriculum.Add(curriResponse);
            }

            return Ok(new BaseResponse(false, "Curriculum Batch", curriBacthDTO));
        }

        [HttpPost("CreateCurriculumBatch")]
        public IActionResult CreateCurriculumBatch([FromBody] CurriculumBatchRequest curriculumBatchRequest)
        {
            if (_batchRepository.CheckBatchDuplicate(curriculumBatchRequest.batch_name))
            {
                return BadRequest(new BaseResponse(true, "Batch is Duplicate!"));
            }
            var batch = new Batch
            {
                batch_name = curriculumBatchRequest.batch_name,
                batch_order = curriculumBatchRequest.batch_order
            };
            string createResult = _batchRepository.CreateBatch(batch);
            if (createResult != Result.createSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            foreach (var curriId in curriculumBatchRequest.list_curriculum_id)
            {
                var curriBatch = new CurriculumBatch
                {
                    batch_id = batch.batch_id,
                    curriculum_id = curriId
                };
                // if curriculum_id exsit in db
                if (curriId > 0)
                {
                    string createcurriBatch = _curriculumBatchRepository.CreateCurriculumBatch(curriBatch);
                    if (createcurriBatch != Result.createSuccessfull.ToString())
                    {
                        return BadRequest(new BaseResponse(true, createcurriBatch));
                    }
                }
            }


            return Ok(new BaseResponse(false, "Create Successfull", curriculumBatchRequest));
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

            string updateResult = _batchRepository.UpdateBatch(batch);
            if (updateResult != Result.updateSuccessfull.ToString())
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            foreach (var curriId in curriculumBatchRequest.list_curriculum_id)
            {
                var curriBatch = new CurriculumBatch
                {
                    batch_id = batch.batch_id,
                    curriculum_id = curriId
                };
                string updatecurriBatch = _curriculumBatchRepository.UpdateCurriculumBatch(curriBatch);
                if (updatecurriBatch != Result.updateSuccessfull.ToString())
                {
                    return BadRequest(new BaseResponse(true, updatecurriBatch));
                }
            }

            return Ok(new BaseResponse(false, "Update Successfull", curriculumBatchRequest));
        }

    }
}
