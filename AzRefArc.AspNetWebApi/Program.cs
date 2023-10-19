
using AzRefArc.AspNetWebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AzRefArc.AspNetWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // 例外ログのファイル出力機能の追加
            builder.Services.AddLogging();
            builder.Logging.AddProvider(new ExceptionFileLoggerProvider());

            // https://learn.microsoft.com/ja-jp/aspnet/core/security/cors?view=aspnetcore-8.0
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:7041",
                                                          "http://www.contoso.com")
                                      .AllowAnyHeader().AllowAnyMethod(); // これをしておかないと GET の CORS しか通らない
                                  });
            });

            builder.Services.AddDbContextFactory<PubsDbContext>(opt =>
            {
                // DbContext 構成設定
                // https://docs.microsoft.com/ja-jp/ef/core/dbcontext-configuration/#other-dbcontext-configuration
                if (builder.Environment.IsDevelopment())
                {
                    opt = opt.EnableSensitiveDataLogging().EnableDetailedErrors();
                }
                opt.UseSqlServer(
                    builder.Configuration.GetConnectionString("PubsDbContext"),
                    providerOptions =>
                    {
                        providerOptions.EnableRetryOnFailure();
                    });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // https://learn.microsoft.com/ja-jp/aspnet/core/security/cors?view=aspnetcore-8.0
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
