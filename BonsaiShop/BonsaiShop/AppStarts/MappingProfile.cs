using AutoMapper;
using BussinessLayer.RequestModel.Category;
using BussinessLayer.RequestModel.EcologicalCharacteristic;
using BussinessLayer.RequestModel.User;
using BussinessLayer.ResponseModel.Category;
using BussinessLayer.ResponseModel.EcologicalCharacteristic;
using BussinessLayer.ResponseModel.Product;
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
            CreateMap<LoginResponseModel, User>().ReverseMap();

            //EcologicalCharacteristic
            CreateMap<CreateEcologicalCharacteristicRequestModel, EcologicalCharacteristic>().ReverseMap();
            CreateMap<UpdateEcologicalCharacteristicRequestModel, EcologicalCharacteristic>().ReverseMap();
            CreateMap<EcologicalCharacteristic, EcologicalCharacteristicResponseModel>()
                .ForMember(dest => dest.listCategory, opt => opt.MapFrom(src => src.Categories));
            //Product
            CreateMap<Product, ProductResponseModel>();

            //Category
            CreateMap<Category, CategoryResponseModel>()
                .ForMember(dest => dest.listProducts, opt => opt.MapFrom(src => src.Products));
            CreateMap<CreateCategoryRequestModel, Category>();
            CreateMap<UpdateCategoryRequestModel, Category>();
        }
    }
}
