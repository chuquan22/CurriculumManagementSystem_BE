using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Batchs;
using Repositories.CurriculumBatchs;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumBatchController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ICurriculumBatchRepository _curriculumBatchRepository;
        private IBatchRepository _batchRepository;

        public CurriculumBatchController(IMapper mapper)
        {
            _mapper = mapper;
            _curriculumBatchRepository = new CurriculumBatchRepository();
            _batchRepository = new BatchRepository();
        }

        [HttpGet("GetAllCurriculumBatch")]
        public IActionResult GetCurriculumBatch()
        {
            var listCurriBatch = _curriculumBatchRepository.GetAllCurriculumBatch();
            List<CurriculumBatchDTOResponse> listCurriculumBatch = new List<CurriculumBatchDTOResponse>();

            foreach (var curriBatch in listCurriBatch)
            {
                var curriBacthDTO = new CurriculumBatchDTOResponse { curriculum = new List<CurriculumResponse>() };
                curriBacthDTO.batch_id = curriBatch.batch_id;
                curriBacthDTO.batch_name = curriBatch.Batch.batch_name;
                var curriResponse = _mapper.Map<CurriculumResponse>(curriBatch.Curriculum);
                curriBacthDTO.curriculum.Add(curriResponse);

                bool found = false;
                foreach (var item in listCurriculumBatch)
                {
                    if (item.batch_id == curriBacthDTO.batch_id)
                    {
                        item.curriculum.Add(curriResponse);
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    listCurriculumBatch.Add(curriBacthDTO);
                }

            }

            return Ok(new BaseResponse(false, "Curriculum Batch", listCurriculumBatch));
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
            if(createResult != Result.createSuccessfull.ToString())
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
                string createcurriBatch = _curriculumBatchRepository.CreateCurriculumBatch(curriBatch);
                if (createcurriBatch != Result.createSuccessfull.ToString())
                {
                    return BadRequest(new BaseResponse(true, createcurriBatch));
                }
            }

            return Ok(new BaseResponse(false, "Create Successfull" ));
        }

        [HttpPost("UpdateCurriculumBatch")]
        public IActionResult UpdateCurriculumBatch([FromBody] CurriculumBatchRequest curriculumBatchRequest)
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
                string createcurriBatch = _curriculumBatchRepository.CreateCurriculumBatch(curriBatch);
                if (createcurriBatch != Result.createSuccessfull.ToString())
                {
                    return BadRequest(new BaseResponse(true, createcurriBatch));
                }
            }

            return Ok(new BaseResponse(false, "Create Successfull"));
        }

    }
}
