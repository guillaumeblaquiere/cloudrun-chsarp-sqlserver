using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Microsoft.Data.SqlClient;

namespace helloworld_csharp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    string conString = Environment.GetEnvironmentVariable("constring") ?? "Uid=sqlserver;Pwd=root;Server=127.0.0.1;Database=master";
                    //Uid=sqlserver;Pwd=root;Server=10.107.81.3;Database=master
                    SqlConnection  connection = new SqlConnection(conString);
                    connection.Open();
                    var selectCommand = connection.CreateCommand();
                    selectCommand.CommandText = "SELECT 'text from query'";
                    SqlDataReader reader = selectCommand.ExecuteReader();
                    reader.Read();
                    await context.Response.WriteAsync("Hello World! -> " + reader[0]);
                });
            });
        }
    }
}
