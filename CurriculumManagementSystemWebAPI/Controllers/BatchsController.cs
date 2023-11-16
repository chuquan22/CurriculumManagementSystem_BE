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
    [Authorize(Roles = "Manager")]
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

        [HttpGet("GetAllBatch")]
        public ActionResult GetAllBatch()
        {
            var listBatch = batchRepository.GetAllBatch();
            return Ok(new BaseResponse(false, "List Batch", listBatch));
        }

        [HttpGet("GetBatchBySpe/{speId}")]
        public ActionResult GetBatch(int speId)
        {
            var listBatch = batchRepository.GetBatchBySpe(speId);
            return Ok(new BaseResponse(false, "List Batch", listBatch));
        }

        [HttpGet("GetBatchByDegreeLevel/{degree_level_Id}")]
        public ActionResult GetBatchByDegreeLevel(int degree_level_Id)
        {
            var listBatch = batchRepository.GetBatchByDegreeLevel(degree_level_Id);
            return Ok(new BaseResponse(false, "List Batch", listBatch));
        }

        [HttpGet("GetBatchById/{Id}")]
        public ActionResult GetBatchById(int Id)
        {
            var Batch = batchRepository.GetBatchById(Id);
            return Ok(new BaseResponse(false, "Batch", Batch));
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
