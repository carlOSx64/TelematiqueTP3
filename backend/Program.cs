using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using WebApi.Data;
using WebApi.Services;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Configuration
                var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                var configuration = builder.Build();

                // Database
                var optionsBuilder = new DbContextOptionsBuilder<SqliteContext>()
                                .UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                var context = new SqliteContext(optionsBuilder.Options);
                context.Database.EnsureCreated();

                // Web Server
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
