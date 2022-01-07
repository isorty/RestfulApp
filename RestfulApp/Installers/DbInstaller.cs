﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestfulApp.Data;
using RestfulApp.Services;

namespace RestfulApp.Installers;

public class DbInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<DataContext>().Services
                .AddScoped<IItemService, ItemService>();
    }
}