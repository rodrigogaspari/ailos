using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Questao5.Application.SwaggerGen
{
    public static class Questao5SwaggerGenExtensions
    {
        public static IServiceCollection AddSwaggerAilosCustomizations(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<AddRequiredHeaderParameterIdempotencyKey>();
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Ailos Teste Questão 5 - Baking API",
                    Description = "Uma Web API ASP.NET Core para desenvolvimento de testes de candidato",
                });

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            return services;
        }

        public static WebApplication AddSwaggerAilosCustomizations(this WebApplication app) 
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseSwagger(options =>
                {
                    options.SerializeAsV2 = true;
                });
            }

            return app;
        } 
    }
}
