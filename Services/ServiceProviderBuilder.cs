using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ConsoleTelegaBot2.Worker;

namespace ConsoleTelegaBot2.Services
{
    public class ServiceProviderBuilder
    {
        public static IServiceProvider GetServiceProvider(string[] args)
        {
            var confoguration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .AddUserSecrets(typeof(Program).Assembly)
                .AddCommandLine(args)
                .Build();
            var services = new ServiceCollection();
            services.Configure<OptionBot>(confoguration.GetSection(typeof(OptionBot).FullName));

            var provider = services.BuildServiceProvider();
            return provider;
        }

    }
}
