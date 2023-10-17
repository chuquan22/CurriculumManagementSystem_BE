using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Batchs;
using Repositories.CLOS;

namespace CurriculumManagementSystemWebAPI.Controllers
{
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

        [HttpGet("GetAllBatch")]
        public ActionResult GetAllBatch()
        {
            var listBatch = _repo.GetAllBatch();
            var listBatchResponse = _mapper.Map<List<BatchDTOResponse>>(listBatch);
            return Ok(new BaseResponse(false, "List Batch", listBatchResponse));
        }
    }
}
