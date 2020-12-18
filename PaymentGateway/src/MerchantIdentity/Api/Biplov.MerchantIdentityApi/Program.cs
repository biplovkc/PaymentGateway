using System;
using System.IO;
using System.Threading.Tasks;
using Biplov.MerchantIdentity.Infrastructure.Persistence;
using Biplov.MerchantIdentity.Infrastructure.Persistence.SeedDataGenerator;
using Biplov.MerchantIdentityApi.WebHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Biplov.MerchantIdentityApi
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);
            try
            {
                Log.Information("Configuring web host ({MerchantIdentityContext})...", AppName);
                var host = BuildWebHost(configuration, args);

                Log.Information("Applying migrations ({MerchantIdentityContext})...", AppName);
                host.MigrateDbContext<MerchantIdentityContext>((context, services) =>
                {
                    var logger = services.GetService<ILogger<MerchantIdentityContextSeed>>();

                    Task.FromResult(new MerchantIdentityContextSeed()
                       .Seed(context, logger));
                });

                //CreateHostBuilder(args).Build().Run();
                Log.Information("Starting web host ({MerchantIdentityContext})...", AppName);
                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({MerchantIdentityContext})!", AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
            Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
                .CaptureStartupErrors(false)
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configuration)
                .UseSerilog()
                .Build();

        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(seqServerUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            return builder.Build();
        }
    }
}
