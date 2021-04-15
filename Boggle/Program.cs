using System;
using System.Threading.Tasks;
using Boggle.Controllers;
using Boggle.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Boggle
{
    public class Program
    {
        public static void Main(string[] args)
        {
            User u = new User("Eylon");
            GameController gc = new GameController();
            gc.runGame(u);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
