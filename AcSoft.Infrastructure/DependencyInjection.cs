using AcSoft.Application.Abstractions.Persistence;
using AcSoft.Infrastructure.Persistence;
using AcSoft.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AcSoft.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("AcSoftConnection")
            ?? throw new InvalidOperationException(
                "No se encontró la cadena de conexión AcSoftConnection.");

        services.AddDbContext<AcDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IMaintenancePlanRepository, MaintenancePlanRepository>();

        services.AddScoped<IActivityRepository, ActivityRepository>();

        services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}