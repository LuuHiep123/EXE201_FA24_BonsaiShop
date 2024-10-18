using BussinessLayer.RequestModel.Category;
using BussinessLayer.RequestModel.EcologicalCharacteristic;
using BussinessLayer.ResponseModel.BaseResponse;
using BussinessLayer.ResponseModel.Category;
using BussinessLayer.ResponseModel.EcologicalCharacteristic;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public interface ICategoryService
    {
        //public Task<BaseResponse<CategoryResponseModel>> Create(CreateCategoryRequestModel model);
        public Task<BaseResponse<CategoryResponseModel>> Create(CreateCategoryRequestModel model);
        Task<DynamicResponse<CategoryResponseModel>> GetList(GetAllCategoryRequestModel model);
        Task<BaseResponse<CategoryResponseModel>> GetById(int id);
        Task<BaseResponse<CategoryResponseModel>> Update(int id, UpdateCategoryRequestModel model);
        Task<BaseResponse<CategoryResponseModel>> Delete(int id);
    }
}
