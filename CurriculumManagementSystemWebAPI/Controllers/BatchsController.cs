using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Batchs;
using Repositories.CLOS;

namespace CurriculumManagementSystemWebAPI.Controllers
{   
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BatchsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IBatchRepository batchRepository;

        public BatchsController(IMapper mapper)
        {
            _mapper = mapper;
            batchRepository = new BatchRepository();
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAllBatch")]
        public ActionResult GetAllBatch()
        {
            var listBatch = batchRepository.GetAllBatch();
            return Ok(new BaseResponse(false, "List Batch", listBatch));
        }

        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationBatch(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listBatch = batchRepository.PaginationBatch(page, limit, txtSearch);
            if (listBatch.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found Batch!"));
            }
            var total = batchRepository.GetTotalBatch(txtSearch);
            return Ok(new BaseResponse(false, "List Batch", new BaseListResponse(page, limit, total, listBatch)));
        }

        [HttpGet("GetBatchBySpe/{speId}")]
        public ActionResult GetBatch(int speId)
        {
            var listBatch = batchRepository.GetBatchBySpe(speId);
            return Ok(new BaseResponse(false, "List Batch", listBatch));
        }

        [HttpPost("CreateBatch")]
        public ActionResult CreateBatch([FromBody] BatchRequest batchRequest)
        {
            if (batchRepository.CheckBatchDuplicate(batchRequest.batch_name))
            {
                return BadRequest(new BaseResponse(true, "Batch is duplicate!"));
            }

            var batch = _mapper.Map<Batch>(batchRequest);
            string createResult = batchRepository.CreateBatch(batch);
            if(!createResult.Equals(Result.createSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, $"Fail to create Batch {batch.batch_name}"));
            }

            return Ok(new BaseResponse(false, $"Create Batch {batch.batch_name} Success!", batchRequest));
        }

        [HttpDelete("DeleteBatch/{id}")]
        public ActionResult DeleteBatch(int id)
        {
            var batch = batchRepository.GetBatchById(id);
            if(batch == null)
            {
                return BadRequest(new BaseResponse(true, "Not Found Batch"));
            }

            if (batchRepository.CheckBatchExsit(id))
            {
                return BadRequest(new BaseResponse(true, "Batch is Used. Can't Delete"));
            }

            string deleteResult = batchRepository.DeleteBatch(batch);
            if (!deleteResult.Equals(Result.deleteSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, $"Fail to remove Batch {batch.batch_name}"));
            }

            return Ok(new BaseResponse(false, $"Remove Batch {batch.batch_name} Success!", batch));
        }
    }
}
