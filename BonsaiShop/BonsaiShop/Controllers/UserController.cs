using BussinessLayer.ResponseModel.User;
using BussinessLayer.ResponseModel.BaseResponse;
using BussinessLayer.RequestModel.User;
using BussinessLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Claims;
using X.PagedList;

namespace BonsaiShop.Controllers
{
    [Route("API/User/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService services)
        {
            _service = services;
        }
        [HttpPost("Admin")]
        public async Task<IActionResult> RegisterAdmin(string account, string password, string name)
        {
            try
            {
                var result = await _service.CreateAccountAdmin(account, password, name);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Staff")]
        public async Task<IActionResult> RegisterStaff(string account, string password, string name)
        {
            try
            {
                var result = await _service.CreateAccountStaff(account, password, name);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Manager")]
        public async Task<IActionResult> RegisterManager(string account, string password, string name)
        {
            try
            {
                var result = await _service.CreateAccountManager(account, password, name);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPost()]
        public async Task<IActionResult> Register(RegisterRequestModel model)
        {
            try
            {
                var result = await _service.RegisterUser(model);
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestModel model)
        {
            try
            {
                var result = await _service.Login(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("LoginMail")]
        public async Task<IActionResult> LoginMail(string googleId)
        {
            try
            {
                var result = await _service.LoginMail(googleId);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Search")]
        public async Task<IActionResult> GetAllUser(GetAllUserRequestModel model)
        {
            try
            {
                var result = await _service.GetListUser(model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ex.Message });
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var result = await _service.GetUserById(id);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateRequestModel model)
        {
            try
            {
                var result = await _service.UpdateUser(id, model);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Change-Status/{id}")]
        public async Task<IActionResult> DeleteUser(int id, bool status)
        {
            try
            {
                var result = await _service.DeleteUser(id, status);
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("Current-User")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return StatusCode(401, new BaseResponse()
                    {
                        Code = 401,
                        Success = false,
                        Message = "User information not found, user may not be authenticated into the system!."
                    });
                }
                else
                {
                    var result = await _service.GetUserById(int.Parse(userId));
                    return StatusCode(result.Code, result);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
