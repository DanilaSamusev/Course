using System.IO;
using AccountingSystem.Models;
using AccountingSystem.Repository;
using AccountingSystem.Services;
using Dapper;
using FluentValidation;
using FluentValidation.AspNetCore;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddFluentValidation();
            services.AddSingleton<IUserRepository, UserRepository>(ur => new UserRepository(connectionString));
            services.AddSingleton<IStudentRepository, StudentRepository>(sr => new StudentRepository(connectionString));
            services.AddSingleton<IRatingRepository, RatingRepository>(rr => new RatingRepository(connectionString));
            services.AddSingleton<Validator>();
            services.AddSingleton<AbstractValidator<ExamsRating>, ExamsRatingValidator>();
            services.AddSingleton<AbstractValidator<ScoresRating>, ScoresRatingValidator>();
            services.AddSingleton<AbstractValidator<Student>, StudentValidator>();
            services.AddSingleton<AbstractValidator<LoginModel>, LoginModelValidator>();
            services.AddSingleton<AbstractValidator<User>, UserValidator>();
            services.AddSingleton<StudentComparerByDebts>();
            services.AddSingleton<StudentComparerByName>();
            services.AddSingleton<StudentComparerByGroupNumber>();
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