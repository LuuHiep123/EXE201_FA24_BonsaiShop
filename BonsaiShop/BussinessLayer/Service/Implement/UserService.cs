﻿using BussinessLayer.ResponseModel.BaseResponse;
using DataLayer.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BussinessLayer.RequestModel.User;
using BussinessLayer.ResponseModel.User;
using DataLayer.Repository;
using AutoMapper;
using Newtonsoft.Json.Linq;
using Google.Apis.Auth;
using X.PagedList;
using System.Linq;

namespace BussinessLayer.Service.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepositoty _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(IUserRepositoty userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }


        public string HashPassword(string password)
        {
            try
            {
                byte[] salt = new byte[16];
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);

                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);
                string hashedPassword = Convert.ToBase64String(hashBytes);

                return hashedPassword;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                byte[] hashBytes = Convert.FromBase64String(hashedPassword);
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);
                byte[] hash = new byte[20];
                Array.Copy(hashBytes, 16, hash, 0, 20);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
                byte[] computedHash = pbkdf2.GetBytes(20);

                for (int i = 0; i < 20; i++)
                {
                    if (hash[i] != computedHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GenerateJwtToken(string username, string roleName, int userId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, roleName),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                }),
                    Expires = DateTime.UtcNow.AddHours(24),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<LoginResponseModel>> Login(LoginRequestModel model)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(model.Account);
                if (user != null)
                {
                    if (VerifyPassword(model.Password, user.Password))
                    {
                        
                        if (user.Status == false)
                        {
                            return new BaseResponse<LoginResponseModel>()
                            {
                                Code = 401,
                                Success = false,
                                Message = "User has been delete!.",
                                Data = null,
                            };
                        }

                        string token = GenerateJwtToken(user.UserName, user.RoleName, user.Id);
                        return new BaseResponse<LoginResponseModel>()
                        {
                            Code = 200,
                            Success = true,
                            Message = "Login success!",
                            Data = new LoginResponseModel()
                            {
                                token = token,
                                user = _mapper.Map<UserResponseModel>(user)
                            },
                        };
                    }
                    else
                    {
                        return new BaseResponse<LoginResponseModel>()
                        {
                            Code = 404,
                            Success = false,
                            Message = "Incorrect User or password!"
                        };
                    }
                }
                else
                {
                    return new BaseResponse<LoginResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Incorrect User or password!"
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<LoginResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<LoginResponseModel>> LoginMail(string googleId)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleId);
                var email = payload.Email;
                var user = await _userRepository.GetUserByEmail(email);
                if (user != null)
                {

                    if (user.Status == false)
                    {
                        return new BaseResponse<LoginResponseModel>()
                        {
                            Code = 401,
                            Success = false,
                            Message = "User has been delete!.",
                            Data = null,
                        };
                    }

                    string token = GenerateJwtToken(user.UserName, user.RoleName, user.Id);
                    return new BaseResponse<LoginResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Login success!",
                        Data = new LoginResponseModel()
                        {
                            token = token,
                            user = _mapper.Map<UserResponseModel>(user)
                        },
                    };
                }
                else
                {
                    var expirationTime = DateTimeOffset.FromUnixTimeSeconds(payload.ExpirationTimeSeconds.Value).UtcDateTime;
                    var currentTime = DateTime.UtcNow;
                    if (currentTime > expirationTime)
                    {
                        return new BaseResponse<LoginResponseModel>()
                        {
                            Code = 401,
                            Success = false,
                            Message = "Google id expired!."
                        };
                    }
                    string password = "loginmail";
                    string hashPassword = HashPassword(password);
                    User userMail = new User()
                    {
                        UserName = payload.Name,
                        Email = email,
                        ImgUrl = payload.Picture,
                        Password = hashPassword,
                        CreatedDate = DateTime.UtcNow,
                        RoleName = "User",
                        Status = true,  
                    };
                    bool check = await _userRepository.CreateUser(userMail);
                    if (!check)
                    {
                        return new BaseResponse<LoginResponseModel>()
                        {
                            Code = 500,
                            Success = false,
                            Message = "Server Error!"
                        };
                    }

                    var response = _mapper.Map<LoginResponseModel>(user);
                    return new BaseResponse<LoginResponseModel>()
                    {
                        Code = 201,
                        Success = true,
                        Message = "Register success. Please go to mail and verify account!",
                        Data = response
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<LoginResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!.",
                    Data = null,
                };
            }

        }

        public async Task<DynamicResponse<UserResponseModel>> GetListUser(GetAllUserRequestModel model)
        {
            try
            {
                var listUser = await _userRepository.GetAllUser();
                if (!string.IsNullOrEmpty(model.keyWord))
                {
                    List<User> listUserByName = listUser.Where(u => u.UserName.Contains(model.keyWord)).ToList();

                    List<User> listUserByEmail = listUser.Where(u => u.Email.Contains(model.keyWord)).ToList();

                    listUser = listUserByName
                               .Concat(listUserByEmail)
                               .GroupBy(u => u.Id)
                               .Select(g => g.First())
                               .ToList();
                }
                if (!string.IsNullOrEmpty(model.role))
                {
                    if (!model.role.Equals("ALL") && !model.role.Equals("all") && !model.role.Equals("All"))
                    {
                        listUser = listUser.Where(u => u.RoleName.Equals(model.role)).ToList();
                    }
                }
                if (model.status != null)
                {
                    listUser = listUser.Where(u => u.Status == model.status).ToList();
                }
                var result = _mapper.Map<List<UserResponseModel>>(listUser);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedUsers = result// Giả sử result là danh sách người dùng
                    .OrderBy(u => u.Id) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<UserResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<UserResponseModel>()
                    {
                        PageInfo = new PagingMetaData()
                        {
                            Page = pagedUsers.PageNumber,
                            Size = pagedUsers.PageSize,
                            Sort = "Ascending",
                            Order = "Id",
                            TotalPage = pagedUsers.PageCount,
                            TotalItem = pagedUsers.TotalItemCount,
                        },
                        SearchInfo = new SearchCondition()
                        {
                            keyWord = model.keyWord,
                            role = model.role,
                            status = model.status,
                        },
                        PageData = pagedUsers.ToList(),
                    },
                };
            }
            catch (Exception ex)
            {
                return new DynamicResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = null,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<UserResponseModel>> GetUserById(int id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user != null)
                {
                    var result = _mapper.Map<UserResponseModel>(user);
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = result
                    };
                }
                else
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found User!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<UserResponseModel>> UpdateUser(int id, UpdateRequestModel model)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user != null)
                {
                    var result = _mapper.Map(model, user);
                    result.ModifiedDate = DateTime.Now;
                    await _userRepository.UpdateUser(result);
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Update success!.",
                        Data = _mapper.Map<UserResponseModel>(result)
                    };
                }
                else
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found User!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<UserResponseModel>> DeleteUser(int id, bool status)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user != null)
                {
                    user.ModifiedDate = DateTime.Now;
                    user.Status = status;
                    await _userRepository.UpdateUser(user);
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Delete success!.",
                        Data = _mapper.Map<UserResponseModel>(user)
                    };
                }
                else
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found User!.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<UserResponseModel>> RegisterUser(RegisterRequestModel model)
        {
            try
            {
                User checkExit = await _userRepository.GetUserByEmail(model.Account);
                if (checkExit != null)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 409,
                        Success = false,
                        Message = "User has been exits!"
                    };
                }
                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 409,
                        Success = false,
                        Message = "Password and Confirm Password not equal!."
                    };
                }
                var password = HashPassword(model.Password);
                var User = _mapper.Map<User>(model);
                User.Email = model.Account;
                User.Status = true;
                User.CreatedDate = DateTime.Now;
                User.Password = password;
                User.RoleName = "User";
                bool check = await _userRepository.CreateUser(User);
                if (!check)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 500,
                        Success = false,
                        Message = "Server Error!"
                    };
                }
                var response = _mapper.Map<UserResponseModel>(User);
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 201,
                    Success = true,
                    Message = "Register success. Please go to mail and verify account!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }

        public async Task<BaseResponse<UserResponseModel>> CreateAccountAdmin(string account, string password, string name)
        {
            try
            {
                User checkExit = await _userRepository.GetUserByEmail(account);
                if (checkExit != null)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 409,
                        Success = false,
                        Message = "User has been exits!"
                    };
                }
                string hashPassword = HashPassword(password);
                User user = new User()
                {
                    Email = account,
                    UserName = name,
                    Status = true,
                    CreatedDate = DateTime.Now,
                    Password = hashPassword,
                    PhoneNumber = "Admin",
                    Address = "Admin",
                    Gender = "Male",
                    RoleName = "Admin",
                };
                bool check = await _userRepository.CreateUser(user);
                if (!check)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 500,
                        Success = false,
                        Message = "Server Error!"
                    };
                }
                var response = _mapper.Map<UserResponseModel>(user);
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 201,
                    Success = true,
                    Message = "Register admin success!.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
        public async Task<BaseResponse<UserResponseModel>> CreateAccountStaff(string account, string password, string name)
        {
            try
            {
                User checkExit = await _userRepository.GetUserByEmail(account);
                if (checkExit != null)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 409,
                        Success = false,
                        Message = "User has been exits!"
                    };
                }
                string hashPassword = HashPassword(password);
                User user = new User()
                {
                    Email = account,
                    UserName = name,
                    Status = true,
                    CreatedDate = DateTime.Now,
                    Password = hashPassword,
                    RoleName = "Staff",
                };
                bool check = await _userRepository.CreateUser(user);
                if (!check)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 500,
                        Success = false,
                        Message = "Server Error!"
                    };
                }
                var response = _mapper.Map<UserResponseModel>(user);
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 201,
                    Success = true,
                    Message = "Register admin success!.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
        public async Task<BaseResponse<UserResponseModel>> CreateAccountManager(string account, string password, string name)
        {
            try
            {
                User checkExit = await _userRepository.GetUserByEmail(account);
                if (checkExit != null)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 409,
                        Success = false,
                        Message = "User has been exits!"
                    };
                }
                string hashPassword = HashPassword(password);
                User user = new User()
                {
                    Email = account,
                    UserName = name,
                    Status = true,
                    CreatedDate = DateTime.Now,
                    Password = hashPassword,
                    RoleName = "Manager",
                };
                bool check = await _userRepository.CreateUser(user);
                if (!check)
                {
                    return new BaseResponse<UserResponseModel>()
                    {
                        Code = 500,
                        Success = false,
                        Message = "Server Error!"
                    };
                }
                var response = _mapper.Map<UserResponseModel>(user);
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 201,
                    Success = true,
                    Message = "Register admin success!.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server Error!"

                };
            }
        }
    }
}
