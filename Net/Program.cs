using System;
using System.Timers;
using dotnet.Parsers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ninject.Extensions.Conventions;
using Ninject;
using dotnet;

namespace Net
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string host = "localhost"; // Имя хоста
                string database = "app"; // Имя базы данных
                string user = "omcode"; // Имя пользователя
                string password = "mRA3Vgf@kEKM&5yIn#gG"; // Пароль пользователя
                //string user = "root"; // Имя пользователя
                //string password = "root"; // Пароль пользователя
                string ecoding = "utf8";
                string connect = $"Database={database};Datasource={host};User={user};Password={password};Charset={ecoding}";

                var kernel = new StandardKernel();
                kernel.Bind(x => x.FromThisAssembly().SelectAllClasses().InheritedFrom<IParser>().BindAllInterfaces());
                kernel.Bind<Connect>().ToConstant(new Connect(connect)).InSingletonScope();
                kernel.Bind<Timer>().ToConstant(new Timer(60 * 60 * 1000)).InSingletonScope();
                kernel.Get<ParseRunner>().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DOTNET ERROR: {ex.Message}");
            }
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
