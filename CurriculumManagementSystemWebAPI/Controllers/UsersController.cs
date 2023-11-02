using AutoMapper;
using BusinessObject;
using DataAccess.Models.DTO.request;
using DataAccess.Models.DTO.response;
using DataAccess.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.AssessmentMethods;
using Repositories.Batchs;
using Repositories.Users;

namespace CurriculumManagementSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IUsersRepository _usersRepository;

        public UsersController(IMapper mapper)
        {
            _mapper = mapper;
            _usersRepository = new UsersRepository();
        }

        [HttpGet("Pagination/{page}/{limit}")]
        public ActionResult PaginationUser(int page, int limit, [FromQuery] string? txtSearch)
        {
            var listUser = _usersRepository.PaginationUser(page, limit, txtSearch);
            if (listUser.Count == 0)
            {
                Ok(new BaseResponse(false, "Not Found User!"));
            }
            var total = _usersRepository.GetTotalUser(txtSearch);
            var listUserResponse = _mapper.Map<List<UserResponse>>(listUser);
            return Ok(new BaseResponse(false, "List User", new BaseListResponse(page, limit, total, listUserResponse)));
        }

        [HttpGet("GetUserById/{id}")]
        public ActionResult GetUser(int id)
        {
            var user = _usersRepository.GetUserById(id);
            if(user == null)
            {
                return NotFound(new BaseResponse(true, "Not Found User"));
            }
            var userResponse = _mapper.Map<UserResponse>(user);
            return Ok(new BaseResponse(false, "User", userResponse));
        }

        [HttpPost("CreateUser")]
        public ActionResult CreateUser([FromBody] UserCreateRequest userCreateRequest)
        {
            if (_usersRepository.CheckUserDuplicate(userCreateRequest.user_email))
            {
                return BadRequest(new BaseResponse(true, $"Email {userCreateRequest.user_email} Duplicate!"));
            }

            var user = _mapper.Map<User>(userCreateRequest);
            user.user_password = "123@";
            user.is_active = true;
            
            string createResult = _usersRepository.CreateUser(user);
            if(!createResult.Equals(Result.createSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, createResult));
            }

            return Ok(new BaseResponse(false, "Create SuccessFull!", user));
        }

        [HttpPut("UpdateUserRole/{id}")]
        public ActionResult UpdateUser(int id, [FromBody]UserUpdateRequest userUpdateRequest)
        {

            var user = _usersRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound(new BaseResponse(true, "Not Found User"));
            }
            user.role_id = userUpdateRequest.role_id;
            user.is_active = userUpdateRequest.is_active;
            
            string updateResult = _usersRepository.UpdateUser(user);
            if (!updateResult.Equals(Result.updateSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, updateResult));
            }

            return Ok(new BaseResponse(false, "Update SuccessFull!", user));
        }

        [HttpDelete("DeleteUser/{id}")]
        public ActionResult DeleteUser(int id)
        {
            var user = _usersRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound(new BaseResponse(true, "Not Found User"));
            }
            string deleteResult = _usersRepository.DeleteUser(user);
            if (!deleteResult.Equals(Result.deleteSuccessfull.ToString()))
            {
                return BadRequest(new BaseResponse(true, deleteResult));
            }

            return Ok(new BaseResponse(false, "Delete SuccessFull!", user));
        }
    }
}
