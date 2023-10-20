using AzRefArc.AspNetBlazorWasm.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Polly;
using Polly.Extensions.Http;

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
            if (baseAddress == null) baseAddress = builder.HostEnvironment.BaseAddress;

            // httpClient の使い方
            // https://qiita.com/TsuyoshiUshio@github/items/7092fbc510772ce5db63

            // 単一の HttpClient であれば下記の方法でよいが...
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

            // 複数の HttpClient や Policy を使いたい場合には、IHttpClientFactory を使う
            // https://learn.microsoft.com/ja-jp/aspnet/core/blazor/call-web-api?view=aspnetcore-8.0&pivots=webassembly#named-httpclient-with-ihttpclientfactory
            // 2 種類の HttpClient を作れるようにしておく
            builder.Services.AddHttpClient(Utilities.HttpExtension.HTTP_CLIENT_NAME_WITH_RETRY, client => client.BaseAddress = new Uri(baseAddress))
                .AddPolicyHandler(msg =>
                {
                    return HttpPolicyExtensions
                        .HandleTransientHttpError() // 408, 5xx エラー
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) // 404 エラー
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401 エラー
                        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 100)),
                            onRetry: (response, delay, retryCount, context) =>
                            {
                                Console.WriteLine($"Retrying: StatusCode: {response.Result.StatusCode} Message: {response.Result.ReasonPhrase} RequestUri: {msg.RequestUri}");
                            });
                });
            // 既定の HttpClient の挙動を変更する
            builder.Services.Configure<Microsoft.Extensions.Http.HttpClientFactoryOptions>(options =>
            {
                options.HttpClientActions.Add(client =>
                {
                    client.BaseAddress = new Uri(baseAddress);
                });
            });

            await builder.Build().RunAsync();
        }
    }
}
