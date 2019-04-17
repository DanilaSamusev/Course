﻿using System.IO;
using AccountingSystem.Repository;
using AccountingSystem.Services;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace AccountingSystem
{
    public class Startup
    {
        
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;                      
            
            using (MySqlConnection connection = new MySqlConnection("server=localhost;user=root;database=student_rating_base;password=1234;"))
            {        
                StreamReader reader1 = new StreamReader("wwwroot/sql/tableCreationScript.sql");
                StreamReader reader2 = new StreamReader("wwwroot/sql/tablesFillingScript.sql");
                string script1 = reader1.ReadToEnd();
                string script2 = reader2.ReadToEnd();
                connection.Open();
                connection.Query(script1);
                connection.Query(script2);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {       
            
            string connectionString = Configuration.GetConnectionString("ConnectionString");

            services.AddSession();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IUserRepository, UserRepository>(ur => new UserRepository(connectionString));
            services.AddSingleton<StudentRepository>(sr => new StudentRepository(connectionString));
            services.AddSingleton<RatingRepository>(rr => new RatingRepository(connectionString));
            services.AddSingleton<Validator>();
            services.AddSingleton<ExamsRatingValidator>();
            services.AddSingleton<ScoresRatingValidator>();
            services.AddSingleton<StudentValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {                
                app.UseDeveloperExceptionPage();
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseHttpsRedirection();                      
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Preview}/{action=Preview}/{id?}");
            });
        }
    }
}