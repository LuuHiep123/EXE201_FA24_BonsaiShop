using BussinessLayer.RequestModel.User;
using BussinessLayer.ResponseModel.BaseResponse;
using BussinessLayer.ResponseModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public interface IUserService
    {
        public Task<BaseResponse<UserResponseModel>> CreateAccountAdmin(string account, string password, string name);
        public Task<BaseResponse<UserResponseModel>> CreateAccountStaff(string account, string password, string name);
        public Task<BaseResponse<UserResponseModel>> CreateAccountManager(string account, string password, string name);
        public Task<BaseResponse<UserResponseModel>> RegisterUser(RegisterRequestModel model);
        Task<BaseResponse<LoginResponseModel>> Login(LoginRequestModel model);
        Task<BaseResponse<LoginResponseModel>> LoginMail(string googleId);
        Task<DynamicResponse<UserResponseModel>> GetListUser(GetAllUserRequestModel model);
        Task<BaseResponse<UserResponseModel>> GetUserById(int id);
        Task<BaseResponse<UserResponseModel>> UpdateUser(int id, UpdateRequestModel model);
        Task<BaseResponse<UserResponseModel>> DeleteUser(int id, bool status);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);

    }
}
