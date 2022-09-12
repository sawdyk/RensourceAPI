using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RensourceDomain.Configurations;
using RensourceDomain.Interfaces;
using RensourceDomain.Interfaces.Helpers;
using RensourceDomain.Interfaces.Repos;
using RensourceDomain.Interfaces.Repos.Helpers;
using RensourcePersistence.AppDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RensourceDomain.Services
{
    public class AppServicesCollections
    {
        private readonly IServiceCollection? _service;
        private readonly IConfiguration? _configuration;

        public AppServicesCollections(IServiceCollection? service, IConfiguration? configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        //Service Dependencies
        public void AppServiceDependencies()
        {
            _service.AddScoped<IRolesRepo, RolesRepo>();
            _service.AddScoped<IAdminRepo, AdminRepo>();
            _service.AddScoped<IUserRepo, UserRepo>();
            _service.AddScoped<IPasswordEncryptRepo, PasswordEncryptRepo>();
            _service.AddScoped<IProjectsRepo, ProjectsRepo>();
            _service.AddScoped<IPressReleaseRepo, PressReleaseRepo>();
            _service.AddScoped<IContactUsRepo, ContactUsRepo>();
            _service.AddScoped<ICompanyInfoRepo, CompanyInfoRepo>();
            _service.AddScoped<IExecutiveRoleRepo, ExecutieRoleRepo>();
            _service.AddScoped<IExecutiveCategoryRepo, ExecutiveCategoryRepo>();
            _service.AddScoped<IExecutiveTeamRepo, ExecutiveTeamRepo>();
            _service.AddScoped<IFAQsRepo, FAQsRepo>();
            _service.AddScoped<IBlogRepo, BlogRepo>();
            _service.AddScoped<IPartnerRepo, PartnerRepo>();
            _service.AddScoped<IFileUploadRepo, FileUploadRepo>();
            // _service.AddScoped<IBaseRepo, BaseRepo>();
        }

        //Application Database Context
        public void AppDatabaseServiceDependencies()
        {
            _service.AddDbContext<ApplicationDBContext>(opt =>
            {
                opt.UseSqlServer(_configuration.GetConnectionString("AppDbConnection"), 
                    b => b.MigrationsAssembly("RensourceAPI").EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
            });
        }
    }
}
