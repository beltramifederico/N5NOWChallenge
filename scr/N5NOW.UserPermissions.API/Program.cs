using Microsoft.AspNetCore.Mvc;
using N5NOW.UserPermissions.API.DependencyInjections;
using Serilog;

namespace N5NOW.UserPermissions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddRepositories();
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddMapper();
            builder.Services.AddMediatr();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddProducer();
            builder.Services.AddElasticsearch(builder.Configuration);

            builder.Logging.ConfigSeriLog(builder.Configuration);
            builder.Services.AddSerilog();
            builder.Host.UseSerilog();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressInferBindingSourcesForParameters = true);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.AddUserMiddleware();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
