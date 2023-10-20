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

            // httpClient �̎g����
            // https://qiita.com/TsuyoshiUshio@github/items/7092fbc510772ce5db63

            // �P��� HttpClient �ł���Ή��L�̕��@�ł悢��...
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

            // ������ HttpClient �� Policy ���g�������ꍇ�ɂ́AIHttpClientFactory ���g��
            // https://learn.microsoft.com/ja-jp/aspnet/core/blazor/call-web-api?view=aspnetcore-8.0&pivots=webassembly#named-httpclient-with-ihttpclientfactory
            // 2 ��ނ� HttpClient ������悤�ɂ��Ă���
            builder.Services.AddHttpClient(Utilities.HttpExtension.HTTP_CLIENT_NAME_WITH_RETRY, client => client.BaseAddress = new Uri(baseAddress))
                .AddPolicyHandler(msg =>
                {
                    return HttpPolicyExtensions
                        .HandleTransientHttpError() // 408, 5xx �G���[
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound) // 404 �G���[
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized) // 401 �G���[
                        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 100)),
                            onRetry: (response, delay, retryCount, context) =>
                            {
                                Console.WriteLine($"Retrying: StatusCode: {response.Result.StatusCode} Message: {response.Result.ReasonPhrase} RequestUri: {msg.RequestUri}");
                            });
                });
            // ����� HttpClient �̋�����ύX����
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
