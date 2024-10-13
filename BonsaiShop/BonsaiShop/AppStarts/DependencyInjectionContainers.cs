using BussinessLayer.Service;
using BussinessLayer.Service.Implement;
using DataLayer.DBContext;
using DataLayer.Repository;
using DataLayer.Repository.Implement;
using Microsoft.EntityFrameworkCore;

namespace BonsaiShop.AppStarts
{
    public static class DependencyInjectionContainers
    {
        public static void ServiceContainer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true; ;
                options.LowercaseQueryStrings = true;
            });
            //Add_DbContext
            services.AddDbContext<db_aad141_exe201Context>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("hosting"));
            });

            //AddService
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEcologicalCharacteristicService, EcologicalCharacteristicService>();

            //AddRepository
            services.AddScoped<IUserRepositoty, UserRepository>();
            services.AddScoped<IEcologicalCharacteristicRepository, EcologicalCharacteristicRepository>();

        }
    }
}
