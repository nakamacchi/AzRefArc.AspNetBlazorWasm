using System.Net.Http.Json;

namespace AzRefArc.AspNetBlazorWasm.Utilities
{
    public static class HttpExtension
    {
        public static async Task<TResult> PostAsJsonFromJsonAsync<TParam, TResult>(this HttpClient httpClient, string? requestUri, TParam value)
        {
            // HTTP 呼び出し
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, value);
            // 結果解析とデシリアライズ
            response.EnsureSuccessStatusCode();
            TResult? result = await response.Content.ReadFromJsonAsync<TResult>();
            return result!;
        }

        public static readonly string HTTP_CLIENT_NAME_WITH_RETRY = "HttpClientWithRetry";

        public static HttpClient CreateClientWithRetry(this IHttpClientFactory httpClientFactory)
        {
            return httpClientFactory.CreateClient(HTTP_CLIENT_NAME_WITH_RETRY);
        }

    }
}
