using LibraProgramming.BlazEdit.Core;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BlazorWebAssemblyHost.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddSingleton<ITimeoutManager, TimeoutManager>()
                        .AddSingleton<IMessageDispatcher, MessageDispatcher>();
                })
                .UseBlazorStartup<Startup>()
                .Build();

            host.Run();
        }

        public class Startup
        {
            public void Configure(IComponentsApplicationBuilder app)
            {
                app.AddComponent<App>("app");
            }
        }
    }
}
