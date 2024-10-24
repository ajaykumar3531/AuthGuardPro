using AuthGuardPro_Application.Repos.Contracts;
using AuthGuardPro_Application.Repos.Services;
using AuthGuardPro_Domain.Entities;
using AuthGuardPro_Infrastucture.Repository.Contracts;
using AuthGuardPro_Infrastucture.Repository.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthGuardPro_Application
{
    public static class DependencyIn
    {
        public static IServiceCollection ApplicationDI(this IServiceCollection services)
        {
            // Register the repository for dependency injection
            // Register your repositories and services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();


            return services;
        }
    }
}
