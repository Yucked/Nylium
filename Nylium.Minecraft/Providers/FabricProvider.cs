using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Nylium.Minecraft.Providers;

/// <inheritdoc />
public sealed class FabricProvider : IServerProvider {
    /// <inheritdoc />
    public string Url
        => "https://meta.fabricmc.net/v2/versions";

    private readonly HttpClient _httpClient;
    private readonly ILogger<FabricProvider> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="logger"></param>
    public FabricProvider(HttpClient httpClient, ILogger<FabricProvider> logger) {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async ValueTask<IEnumerable<Release>?> GetReleasesAsync() {
        var versions = await GetMinecraftVersions();

        return default;
    }

    private async Task<IEnumerable<Version>> GetMinecraftVersions() {
        await using var stream = await GetStreamAsync("game");
        if (stream == null || stream.Length == 0) {
            _logger.LogError("Fetching minecraft versions failed!");
            return default!;
        }

        using var document = await JsonDocument.ParseAsync(stream);
        return document.RootElement.EnumerateArray()
            .Select(x => new Version {
                Minecraft = x.GetProperty("version").GetString()!,
                IsPrerelease = x.GetProperty("stable").GetBoolean()
            })!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="version"></param>
    public async Task GetVersionLoaders(string version) {
        await using var stream = await GetStreamAsync($"loader/{version}");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<string>> GetInstallers() {
        await using var stream = await GetStreamAsync("installer");
        if (stream == null || stream.Length == 0) {
            _logger.LogError("Fetching minecraft versions failed!");
            return default!;
        }

        using var document = await JsonDocument.ParseAsync(stream);
        return document.RootElement.EnumerateArray()
            .Select(x => x.GetProperty("version").GetString()!);
    }

    private async Task<Stream?> GetStreamAsync(string path) {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{Url}/{path}");
        using var responseMessage = await _httpClient.SendAsync(requestMessage);
        if (!responseMessage.IsSuccessStatusCode) {
            _logger.LogError("{ReasonPhrase}", responseMessage.ReasonPhrase);
            return default;
        }

        using var content = responseMessage.Content;
        return await content.ReadAsStreamAsync();
    }
}