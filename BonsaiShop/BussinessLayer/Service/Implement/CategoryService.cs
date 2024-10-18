using AutoMapper;
using BussinessLayer.RequestModel.Category;
using BussinessLayer.RequestModel.EcologicalCharacteristic;
using BussinessLayer.ResponseModel.BaseResponse;
using BussinessLayer.ResponseModel.Category;
using BussinessLayer.ResponseModel.EcologicalCharacteristic;
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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IConfiguration configuration, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<DynamicResponse<CategoryResponseModel>> GetList(GetAllCategoryRequestModel model)
        {
            try
            {
                var listCate = await _categoryRepository.GetAll();
                if (!string.IsNullOrEmpty(model.Name))
                {
                    listCate = listCate.Where(u => u.Name.Contains(model.Name)).ToList();
                }
                if (model.status != null)
                {
                    listCate = listCate.Where(u => u.Status == model.status).ToList();
                }
                var result = _mapper.Map<List<CategoryResponseModel>>(listCate);

                // Nếu không có lỗi, thực hiện phân trang
                var pagedEco = result// Giả sử result là danh sách người dùng
                    .OrderBy(u => u.Id) // Sắp xếp theo Id tăng dần
                    .ToPagedList(model.pageNum, model.pageSize); // Phân trang với X.PagedList
                return new DynamicResponse<CategoryResponseModel>()
                {
                    Code = 200,
                    Success = true,
                    Message = null,

                    Data = new MegaData<CategoryResponseModel>()
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
                return new DynamicResponse<CategoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server error!.",
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<CategoryResponseModel>> Create(CreateCategoryRequestModel model)
        {
            try
            {
                var cate = _mapper.Map<Category>(model);
                cate.Status = true;
                var query = await _categoryRepository.Create(cate);
                if (query)
                {
                    return new BaseResponse<CategoryResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Create CategoryResponseModel success!."
                    };
                }
                else
                {
                    return new BaseResponse<CategoryResponseModel>()
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
                return new BaseResponse<CategoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server error!.",
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<CategoryResponseModel>> Delete(int id)
        {
            try
            {
                var cate = await _categoryRepository.GetById(id);
                if (cate == null)
                {
                    return new BaseResponse<CategoryResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found CategoryResponseModel!.",
                        Data = null,
                    };
                }
                cate.Status = false;
                var query = await _categoryRepository.Update(cate);
                if (query)
                {
                    return new BaseResponse<CategoryResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Delete CategoryResponseModel success!.",
                        Data = null
                    };
                }
                else
                {
                    return new BaseResponse<CategoryResponseModel>()
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
                return new BaseResponse<CategoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server error!.",
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<CategoryResponseModel>> GetById(int id)
        {
            try
            {
                var cate = await _categoryRepository.GetById(id);
                if (cate == null)
                {
                    return new BaseResponse<CategoryResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found CategoryResponseModel!.",
                        Data = null
                    };
                }
                else
                {
                    return new BaseResponse<CategoryResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = null,
                        Data = _mapper.Map<CategoryResponseModel>(cate)
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<CategoryResponseModel>()
                {
                    Code = 500,
                    Success = false,
                    Message = "Server error!.",
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<CategoryResponseModel>> Update(int id, UpdateCategoryRequestModel model)
        {
            try
            {
                var cate = await _categoryRepository.GetById(id);
                if (cate == null)
                {
                    return new BaseResponse<CategoryResponseModel>()
                    {
                        Code = 404,
                        Success = false,
                        Message = "Not found CategoryResponseModel!.",
                        Data = null
                    };
                }
                var newCate = _mapper.Map(model, cate);
                var query = await _categoryRepository.Update(newCate);
                if (query)
                {
                    return new BaseResponse<CategoryResponseModel>()
                    {
                        Code = 200,
                        Success = true,
                        Message = "Create CategoryResponseModel success!."
                    };
                }
                else
                {
                    return new BaseResponse<CategoryResponseModel>()
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
                return new BaseResponse<CategoryResponseModel>()
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
