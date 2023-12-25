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
    [Authorize(Roles = "Manager, Dispatcher")]
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
        [Authorize(Roles = "Dispatcher, Manager")]
        [HttpGet("GetAllBatch")]
        public ActionResult GetAllBatch()
        {
            var listBatch = batchRepository.GetAllBatch();
            return Ok(new BaseResponse(false, "List Batch", listBatch));
        }
        [Authorize(Roles = "Dispatcher, Manager")]
        [HttpGet("GetBatchNotExsitInSemester")]
        public ActionResult GetBatchNotExsitInSemester()
        {
            var listBatch = batchRepository.GetBatchNotExsitInSemester();
            var listBatchResponse = _mapper.Map<List<CurriculumBatchDTOResponse>>(listBatch);
            return Ok(new BaseResponse(false, "List Batch", listBatchResponse));
        }
        [Authorize(Roles = "Dispatcher, Manager")]
        [HttpGet("GetBatchBySpe/{speId}")]
        public ActionResult GetBatch(int speId)
        {
            var listBatch = batchRepository.GetBatchBySpe(speId);
            return Ok(new BaseResponse(false, "List Batch", listBatch));
        }
        [Authorize(Roles = "Dispatcher, Manager")]
        [HttpGet("GetBatchByDegreeLevel/{degree_level_Id}")]
        public ActionResult GetBatchByDegreeLevel(int degree_level_Id)
        {
            var listBatch = batchRepository.GetBatchByDegreeLevel(degree_level_Id);
            return Ok(new BaseResponse(false, "List Batch", listBatch));
        }
        [Authorize(Roles = "Dispatcher, Manager")]
        [HttpGet("GetBatchById/{Id}")]
        public ActionResult GetBatchById(int Id)
        {
            var Batch = batchRepository.GetBatchById(Id);
            return Ok(new BaseResponse(false, "Batch", Batch));
        }
    }
}
