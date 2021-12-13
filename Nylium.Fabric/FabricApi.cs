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
        PropertyNameCaseInsensitive = true
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
    public Task<IReadOnlyList<FabricLoader>?> GetLoadersAsync() {
        return GetJsonAsync<IReadOnlyList<FabricLoader>>("loader");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <returns></returns>
    public Task<IReadOnlyList<FabricGameLoader>?> GetLoadersAsync(string gameVersion) {
        return GetJsonAsync<IReadOnlyList<FabricGameLoader>>($"loader/{gameVersion}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <returns></returns>
    public Task<FabricGameLoader> GetLoaderAsync(string gameVersion, string loaderVersion) {
        return GetJsonAsync<FabricGameLoader>($"loader/{gameVersion}/{loaderVersion}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <returns></returns>
    public Task<FabricGameLoader> GetLoaderZipAsync(string gameVersion, string loaderVersion) {
        return GetJsonAsync<FabricGameLoader>($"loader/{gameVersion}/{loaderVersion}/profile/zip");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <returns></returns>
    public Task<FabricGameLoader> GetLoaderProfileAsync(string gameVersion, string loaderVersion) {
        return GetJsonAsync<FabricGameLoader>($"loader/{gameVersion}/{loaderVersion}/profile/json");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <returns></returns>
    public Task<FabricGameLoader> GetLoaderServerAsync(string gameVersion, string loaderVersion) {
        return GetJsonAsync<FabricGameLoader>($"loader/{gameVersion}/{loaderVersion}/server/json");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <param name="installerVersion"></param>
    /// <returns></returns>
    public static string GetDownloadUrl(string gameVersion, string loaderVersion, string installerVersion) {
        return $"{META_BASE_URL}/loader/{gameVersion}/{loaderVersion}/{installerVersion}/server/jar";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <param name="installerVersion"></param>
    /// <returns></returns>
    public static string GetServerFileName(string gameVersion, string loaderVersion, string installerVersion) {
        return $"Fabric_{gameVersion}_{loaderVersion}_{installerVersion}.jar";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <param name="installerVersion"></param>
    /// <param name="savePath"></param>
    public async Task GetServerJarAsync(string gameVersion, string loaderVersion, string installerVersion,
                                        string? savePath = default) {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get,
            GetDownloadUrl(gameVersion, loaderVersion, installerVersion));
        using var responseMessage = await _httpClient.SendAsync(requestMessage);

        if (!responseMessage.IsSuccessStatusCode) {
            _logger.LogError("Response code returned {Code} with reason: {Reason}",
                responseMessage.StatusCode, responseMessage.ReasonPhrase);
            throw new Exception(responseMessage.ReasonPhrase);
        }

        await using var fileStream =
            File.OpenWrite(savePath ?? GetServerFileName(gameVersion, loaderVersion, installerVersion));
        using var content = responseMessage.Content;
        await content.CopyToAsync(fileStream);
        await fileStream.FlushAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="path"></param>
    public async Task GetLatestJarAsync(string gameVersion, string? path = default) {
        var installers = await GetInstallersAsync();
        var loaders = await GetLoadersAsync(gameVersion);
        await GetServerJarAsync(gameVersion,
            loaders![0].Loader.Version,
            installers![0].Version, path);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<IReadOnlyList<FabricInstaller>?> GetInstallersAsync() {
        return GetJsonAsync<IReadOnlyList<FabricInstaller>>("installer");
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