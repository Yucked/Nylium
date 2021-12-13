using System.Text.Json;

namespace Nylium.Common;

public static class HttpExtension {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="url"></param>
    /// <param name="savePath"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task DownloadAsync(this HttpClient httpClient, string url, string? savePath) {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        using var responseMessage = await httpClient.SendAsync(requestMessage);

        if (!responseMessage.IsSuccessStatusCode) {
            throw new Exception($"Response code returned {responseMessage.StatusCode} with reason: {responseMessage.ReasonPhrase}");
        }

        await using var fileStream = File.OpenWrite(savePath!);
        using var content = responseMessage.Content;
        await content.CopyToAsync(fileStream);
        await fileStream.FlushAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="httpClient"></param>
    /// <param name="url"></param>
    /// <param name="serializerOptions"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<T?> GetJsonAsync<T>(this HttpClient httpClient, string url,
        JsonSerializerOptions? serializerOptions = default) {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        using var responseMessage = await httpClient.SendAsync(requestMessage);
        if (!responseMessage.IsSuccessStatusCode) {
            throw new Exception($"Response code returned {responseMessage.StatusCode} with reason: {responseMessage.ReasonPhrase}");
        }

        using var content = responseMessage.Content;
        await using var stream = await content.ReadAsStreamAsync();
        if (stream.Length == 0 || !stream.CanRead) {
            throw new Exception("Failed to read http content stream.");
        }

        return await JsonSerializer.DeserializeAsync<T>(stream, serializerOptions);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="url"></param>
    /// <param name="documentOptions"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static async Task<JsonElement> GetJsonAsync(this HttpClient httpClient, string url,
        JsonDocumentOptions documentOptions = default) {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        using var responseMessage = await httpClient.SendAsync(requestMessage);
        if (!responseMessage.IsSuccessStatusCode) {
            throw new Exception($"Response code returned {responseMessage.StatusCode} with reason: {responseMessage.ReasonPhrase}");
        }

        using var content = responseMessage.Content;
        await using var stream = await content.ReadAsStreamAsync();
        if (stream.Length == 0 || !stream.CanRead) {
            throw new Exception("Failed to read http content stream.");
        }

        var document = await JsonDocument.ParseAsync(stream, documentOptions);
        return document.RootElement;
    }
}