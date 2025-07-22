using DevIO.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Api.Configurations
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder WebConfig(this WebApplicationBuilder builder)
        {

            builder.Services.AddControllers();

            builder.Services.AddDbContext<MeuDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddIdentityConfiguration(builder.Configuration);

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Development",
                                  builder => builder.WithOrigins("http://localhost:4200")
                                  .AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .AllowCredentials());
            });

            builder.Services.ResolveDependencies();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            return builder;
        }
    }
}
