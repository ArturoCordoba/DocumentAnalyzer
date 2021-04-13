using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIAuthLibrary;
using AuthLibrary.Factory;
using AuthLibrary.Configuration;
using DataHandlerMongoDB.Configuration;
using DataHandlerMongoDB.Factory;
using DataHandlerSQL.Factory;
using DataHandlerSQL.Configuration;
using Microsoft.AspNetCore.Authentication;


namespace DocumentAnalyzerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DocumentAnalyzerAPI", Version = "v1" });
            });

            // ------------------------------------------------------------------------/
            // Configuration related to the APIAuthLibrary & AuthLibrary
            // ------------------------------------------------------------------------/
            AuthServiceConfig.Config.SecretKey = Configuration["TokenConfiguration:SecretKey"];
            AuthServiceConfig.Config.IssuerToken = Configuration["TokenConfiguration:IssuerToken"];
            AuthServiceConfig.Config.AuthType = Configuration["TokenConfiguration:AuthType"];
            //AuthServiceConfig.Config.ExpirationTime = Int32.Parse(Configuration["TokenConfiguration:ExpirationTime"]);

            services.AddAuthentication("Authorized")
                .AddScheme<AuthenticationSchemeOptions, AuthHandler>("Authorized", "Authorized", opts => { });

            services.AddScoped<IAuthServiceFactory, AuthServiceFactory>();
            // ------------------------------------------------------------------------/
                      
             // ------------------------------------------------------------------------/
            // Configuration related to the DataHandlerSQL
            // ------------------------------------------------------------------------/
            string connStringPostgreSQL = Configuration["ConnectionStrings:PostgreSQL_DB"];
            DataHandlerSQLConfig.Config.ConnectionString = connStringPostgreSQL;
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
            //------------------------------------------------------------------------/
            string connStringMongoDB = Configuration["ConnectionStrings:MongoDB"];
            DataHandlerMongoDBConfig.Config.ConnectionString = connStringMongoDB;
            DataHandlerMongoDBConfig.Config.DataBaseName = "DocAnalyzerEntities";

            services.AddScoped<IMongoRepositoryFactory, MongoRepositoryFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocumentAnalyzerAPI v1"));
            }

            // TODO: Adjust CORS settings appropriately
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
