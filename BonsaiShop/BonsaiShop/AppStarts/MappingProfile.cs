using AutoMapper;
using BussinessLayer.RequestModel.User;
using BussinessLayer.ResponseModel.User;
using DataLayer.Entities;

namespace BonsaiShop.AppStarts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            //Users
            CreateMap<RegisterRequestModel, User>().ReverseMap();
            CreateMap<UserResponseModel, User>().ReverseMap();
            CreateMap<RegisterRequestModel, UserResponseModel>().ReverseMap();
            CreateMap<UpdateRequestModel, User>().ReverseMap();

        }
    }
}
