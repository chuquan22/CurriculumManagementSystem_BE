using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Batchs;
using Repositories.CLOS;

namespace CurriculumManagementSystemWebAPI.Controllers
{   
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BatchsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IBatchRepository _repo;

        public BatchsController(IMapper mapper)
        {
            _mapper = mapper;
            _repo = new BatchRepository();
        }
        //[Authorize(Roles = "Admin")]
        [HttpGet("GetAllBatch")]
        public ActionResult GetAllBatch()
        {
            var listBatch = _repo.GetAllBatch();
            return Ok(new BaseResponse(false, "List Batch", listBatch));
        }

        [HttpGet("GetBatchBySpe/{speId}")]
        public ActionResult GetBatch(int speId)
        {
            var listBatch = _repo.GetBatchBySpe(speId);
            return Ok(new BaseResponse(false, "List Batch", listBatch));
        }
    }
}
