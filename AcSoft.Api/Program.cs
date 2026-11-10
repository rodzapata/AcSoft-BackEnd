
using AcSoft.Application.Features.WorkOrders.Commands;
using AcSoft.Infrastructure;
using AcSoft.Infrastructure.Persistence;


namespace AcSoft.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddScoped<CreateWorkOrderHandler>();

        builder.Services.AddScoped<CreateWorkOrderFromMaintenancePlanHandler>();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        //temporalmente para probar la conexion
        /*
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider
                .GetRequiredService<AcDbContext>();

            var canConnect = await context.Database.CanConnectAsync();

            Console.WriteLine(
                $"Conexión a PostgreSQL: {canConnect}");
        }
        */

        Console.WriteLine("Iniciando prueba de conexión...");

        using (var scope = app.Services.CreateScope())
        {
            Console.WriteLine("Creando scope...");

            var context = scope.ServiceProvider
                .GetRequiredService<AcDbContext>();

            Console.WriteLine("DbContext obtenido correctamente.");

            var canConnect = await context.Database.CanConnectAsync();

            Console.WriteLine(
                $"Conexión a PostgreSQL: {(canConnect ? "Exitosa" : "Fallida")}");
        }

        Console.WriteLine("Finalizó la prueba de conexión.");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
