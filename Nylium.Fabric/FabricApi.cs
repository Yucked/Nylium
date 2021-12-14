using System.Text.Json;
using Nylium.Common;
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

    private static readonly JsonSerializerOptions Defaults = new() {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    public FabricApi(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<IReadOnlyList<FabricLoader>?> GetLoadersAsync() {
        return _httpClient.GetJsonAsync<IReadOnlyList<FabricLoader>>($"{META_BASE_URL}/loader", Defaults);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <returns></returns>
    public Task<IReadOnlyList<FabricGameLoader>?> GetLoadersAsync(string gameVersion) {
        return _httpClient.GetJsonAsync<IReadOnlyList<FabricGameLoader>>($"{META_BASE_URL}/loader/{gameVersion}",
            Defaults);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <returns></returns>
    public Task<FabricGameLoader> GetLoaderAsync(string gameVersion, string loaderVersion) {
        return _httpClient.GetJsonAsync<FabricGameLoader>($"{META_BASE_URL}/loader/{gameVersion}/{loaderVersion}",
            Defaults);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <returns></returns>
    public Task<FabricGameLoader> GetLoaderZipAsync(string gameVersion, string loaderVersion) {
        return _httpClient.GetJsonAsync<FabricGameLoader>(
            $"{META_BASE_URL}/loader/{gameVersion}/{loaderVersion}/profile/zip", Defaults);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <returns></returns>
    public Task<FabricGameLoader> GetLoaderProfileAsync(string gameVersion, string loaderVersion) {
        return _httpClient.GetJsonAsync<FabricGameLoader>(
            $"{META_BASE_URL}/loader/{gameVersion}/{loaderVersion}/profile/json", Defaults);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <returns></returns>
    public Task<FabricGameLoader> GetLoaderServerAsync(string gameVersion, string loaderVersion) {
        return _httpClient.GetJsonAsync<FabricGameLoader>(
            $"{META_BASE_URL}/loader/{gameVersion}/{loaderVersion}/server/json", Defaults);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameVersion"></param>
    /// <param name="loaderVersion"></param>
    /// <param name="installerVersion"></param>
    /// <param name="savePath"></param>
    public Task GetServerJarAsync(string gameVersion, string loaderVersion, string installerVersion,
                                  string? savePath = default) {
        return _httpClient.DownloadAsync(
            GetDownloadUrl(gameVersion, loaderVersion, installerVersion),
            savePath ?? GetServerFileName(gameVersion, loaderVersion, installerVersion));
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
        return _httpClient.GetJsonAsync<IReadOnlyList<FabricInstaller>>($"{META_BASE_URL}/installer", Defaults);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Task<IReadOnlyCollection<FabricGame>?> GetMinecraftVersionsAsync() {
        return _httpClient.GetJsonAsync<IReadOnlyCollection<FabricGame>>($"{META_BASE_URL}/game", Defaults);
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
}