using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GameServer.Models.Factory;

namespace GameServer
{
    public class Program
    {
       

        public static void Main(string[] args)
        {
            // CreateWebHostBuilder(args).Build().Run();
            Console.WriteLine("pasirinkite kliuti R(ed), B(lue), G(reen)");
            string a = Console.ReadLine();
            if(a == "R")
            {
                factory = new 
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
