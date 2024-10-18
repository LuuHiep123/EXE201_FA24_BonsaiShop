using BussinessLayer.RequestModel.EcologicalCharacteristic;
using BussinessLayer.RequestModel.User;
using BussinessLayer.ResponseModel.BaseResponse;
using BussinessLayer.ResponseModel.EcologicalCharacteristic;
using BussinessLayer.ResponseModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer.Service
{
    public interface IEcologicalCharacteristicService
    {
        public Task<BaseResponse<EcologicalCharacteristicResponseModel>> Create(CreateEcologicalCharacteristicRequestModel model);
        Task<DynamicResponse<EcologicalCharacteristicResponseModel>> GetList(GetAllEcologicalCharacteristicRequestModel model);
        Task<BaseResponse<EcologicalCharacteristicResponseModel>> GetById(int id);
        Task<BaseResponse<EcologicalCharacteristicResponseModel>> Update(int id, UpdateEcologicalCharacteristicRequestModel model);
        Task<BaseResponse<EcologicalCharacteristicResponseModel>> Delete(int id);

    }
}
