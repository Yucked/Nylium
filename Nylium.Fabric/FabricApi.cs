using System.Text.Json;
using Microsoft.Extensions.Logging;
using Nylium.Fabric.Responses;

namespace Nylium.Fabric;

/// <summary>
/// 
/// </summary>
public sealed class FabricApi {
    /// <summary>
    /// 
    /// </summary>
    public const string META_BASE_URL = "https://meta.fabricmc.net/v2/versions";

    private readonly HttpClient _httpClient;
    private readonly ILogger<FabricApi> _logger;

    private static readonly JsonSerializerOptions Defaults = new() {
        PropertyNameCaseInsensitive = false
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="logger"></param>
    public FabricApi(HttpClient httpClient, ILogger<FabricApi> logger) {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<IReadOnlyCollection<FabricLoader>?> GetLoadersAsync() {
        return GetJsonAsync<IReadOnlyCollection<FabricLoader>>("loader");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <returns></returns>
    public Task<IReadOnlyCollection<FabricGameLoader>?> GetLoadersAsync(string gameVersion) {
        return GetJsonAsync<IReadOnlyCollection<FabricGameLoader>>($"loader/{gameVersion}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <returns></returns>
    public Task<FabricGameLoader> GetLoadersAsync(string gameVersion, string loaderVersion) {
        return GetJsonAsync<FabricGameLoader>($"loader/{gameVersion}/{loaderVersion}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<IReadOnlyCollection<FabricGame>?> GetMinecraftVersionsAsync() {
        return GetJsonAsync<IReadOnlyCollection<FabricGame>>("game");
    }

    private async Task<T?> GetJsonAsync<T>(string path) {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{META_BASE_URL}/{path}");
        using var responseMessage = await _httpClient.SendAsync(requestMessage);
        if (!responseMessage.IsSuccessStatusCode) {
            _logger.LogError("Response code returned {Code} with reason: {Reason}",
                responseMessage.StatusCode, responseMessage.ReasonPhrase);
            throw new Exception(responseMessage.ReasonPhrase);
        }

        using var content = responseMessage.Content;
        await using var stream = await content.ReadAsStreamAsync();
        if (stream.Length == 0 || !stream.CanRead) {
            throw new Exception("Failed to read http content stream.");
        }

        return await JsonSerializer.DeserializeAsync<T>(stream, Defaults);
    }
}