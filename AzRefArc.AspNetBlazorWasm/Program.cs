using AzRefArc.AspNetBlazorWasm.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace AzRefArc.AspNetBlazorWasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            string? baseAddress = builder.Configuration.GetValue<string>("BaseUrl");
            if (baseAddress == null)
            {
                builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            }
            else
            { 
                builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });
            }

            await builder.Build().RunAsync();
        }
    }
}
