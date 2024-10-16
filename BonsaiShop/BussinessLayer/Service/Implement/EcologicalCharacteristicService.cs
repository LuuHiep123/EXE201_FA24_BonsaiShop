﻿using AutoMapper;
using BussinessLayer.RequestModel.EcologicalCharacteristic;
using BussinessLayer.ResponseModel.BaseResponse;
using BussinessLayer.ResponseModel.EcologicalCharacteristic;
using BussinessLayer.ResponseModel.User;
using DataLayer.Entities;
using DataLayer.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace BussinessLayer.Service.Implement
{
    public class EcologicalCharacteristicService : IEcologicalCharacteristicService
    {
        private readonly IEcologicalCharacteristicRepository _EcoRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EcologicalCharacteristicService(IEcologicalCharacteristicRepository eco, IConfiguration configuration, IMapper mapper)
        {
            _EcoRepository = eco;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<BaseResponse<EcologicalCharacteristicResponseModel>> Create(CreateEcologicalCharacteristicRequestModel model)
        {
            try
            {
                var eco = new EcologicalCharacteristic()
                {
                    Name = model.Name,
                    Desciption = model.Desciption,
                    Status = true
                };
                var query = await _EcoRepository.Create(eco);
                if (query)
                {
                    return new BaseResponse<EcologicalCharacteristicResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Create EcologicalCharacteristic success!."
                    };
                }
                else
                {
                    return new BaseResponse<EcologicalCharacteristicResponseModel>()
                    {
                        Code = 500,
                        Success = false,
                        Message = "Server error!.",
                        Data = null,
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EcologicalCharacteristicResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server error!.",
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<EcologicalCharacteristicResponseModel>> Delete(int id)
        {
            try
            {
                var Eco = await _EcoRepository.GetById(id);
                if (Eco == null)
                {
                    return new BaseResponse<EcologicalCharacteristicResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found EcologicalCharacteristic!.",
                        Data = null,
                    };
                }
                Eco.Status = false;
                var query = await _EcoRepository.Update(Eco);
                if (query)
                {
                    return new BaseResponse<EcologicalCharacteristicResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Delete EcologicalCharacteristic success!.",
                        Data = null
                    };
                }
                else
                {
                    return new BaseResponse<EcologicalCharacteristicResponseModel>()
                    {
                        Code = 500,
                        Success = false,
                        Message = "Server error!.",
                        Data = null,
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EcologicalCharacteristicResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server error!.",
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<EcologicalCharacteristicResponseModel>> GetById(int id)
        {
            try
            {
                var Eco = await _EcoRepository.GetById(id);
                if (Eco == null)
                {
                    return new BaseResponse<EcologicalCharacteristicResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found EcologicalCharacteristic!.",
                        Data = null
                    };
                }
                else
                {
                    return new BaseResponse<EcologicalCharacteristicResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = _mapper.Map<EcologicalCharacteristicResponseModel>(Eco)
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EcologicalCharacteristicResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server error!.",
                    Data = null,
                };
            }
        }

        public async Task<DynamicResponse<EcologicalCharacteristicResponseModel>> GetList(GetAllEcologicalCharacteristicRequestModel model)
        {
            try
            {
                var listEco = await _EcoRepository.GetAll();
                if (!string.IsNullOrEmpty(model.Name))
                {
                    listEco = listEco.Where(u => u.Name.Contains(model.Name)).ToList();
                }
                if (model.status != null)
                {
                    listEco = listEco.Where(u => u.Status == model.status).ToList();
                }
                var result = _mapper.Map<List<EcologicalCharacteristicResponseModel>>(listEco);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedEco = result// Giả sử result là danh sách người dùng
                    .OrderBy(u => u.Id) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<EcologicalCharacteristicResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<EcologicalCharacteristicResponseModel>()
                    {
                        PageInfo = new PagingMetaData()
                        {
                            Page = pagedEco.PageNumber,
                            Size = pagedEco.PageSize,
                            Sort = "Ascending",
                            Order = "Id",
                            TotalPage = pagedEco.PageCount,
                            TotalItem = pagedEco.TotalItemCount,
                        },
                        SearchInfo = new SearchCondition()
                        {
                            keyWord = model.Name,
                            role = null,
                            status = model.status,
                        },
                        PageData = pagedEco.ToList(),
                    },
                };
            }
            catch (Exception ex)
            {
                return new DynamicResponse<EcologicalCharacteristicResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server error!.",
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<EcologicalCharacteristicResponseModel>> Update(int id, UpdateEcologicalCharacteristicRequestModel model)
        {
            try
            {
                var eco = await _EcoRepository.GetById(id);
                if (eco == null)
                {
                    return new BaseResponse<EcologicalCharacteristicResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found EcologicalCharacteristic!.",
                        Data = null
                    };
                }
                var newEco = _mapper.Map(model, eco);
                var query = await _EcoRepository.Update(newEco);
                if (query)
                {
                    return new BaseResponse<EcologicalCharacteristicResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Create EcologicalCharacteristic success!."
                    };
                }
                else
                {
                    return new BaseResponse<EcologicalCharacteristicResponseModel>()
                    {
                        Code = 500,
                        Success = false,
                        Message = "Server error!.",
                        Data = null,
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EcologicalCharacteristicResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server error!.",
                    Data = null,
                };
            }
        }
    }
}
