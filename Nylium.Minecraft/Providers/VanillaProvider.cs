using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Nylium.Minecraft.Providers;

/// <inheritdoc />
public sealed class VanillaProvider : IServerProvider {
    /// <inheritdoc />
    public string Url
        => "https://launchermeta.mojang.com/mc/game/version_manifest.json";

    private readonly HttpClient _httpClient;
    private readonly ILogger<VanillaProvider> _logger;

    private static readonly JsonSerializerOptions SerializerOptions = new() {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="logger"></param>
    public VanillaProvider(HttpClient httpClient, ILogger<VanillaProvider> logger) {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async ValueTask<IEnumerable<Release>?> GetReleasesAsync() {
        using var responseMessage = await _httpClient.GetAsync(Url);
        if (!responseMessage.IsSuccessStatusCode) {
            _logger.LogError("{ErrorMessage}", responseMessage.ReasonPhrase);
            return default;
        }

        using var content = responseMessage.Content;
        await using var stream = await content!.ReadAsStreamAsync();
        var vanillaResponse = await JsonSerializer.DeserializeAsync<VanillaResponse>(stream, SerializerOptions);
        var releasesRequests = vanillaResponse.Versions.Select(x => GetReleaseAsync(x.Url));
        return await Task.WhenAll(releasesRequests);
    }

    private async Task<Release> GetReleaseAsync(string url) {
        var responseMessage = await _httpClient.GetAsync(url);
        if (!responseMessage.IsSuccessStatusCode) {
            _logger.LogError("{ErrorMessage}", responseMessage.ReasonPhrase);
            return default;
        }

        await using var stream = await responseMessage.Content.ReadAsStreamAsync();
        var root = (await JsonDocument.ParseAsync(stream)).RootElement;
        var release = new Release {
            Version = new Version {
                IsPrerelease = root.GetProperty("type").GetString()! == "snapshot",
                Minecraft = root.GetProperty("id").GetString()!,
                Loader = "Vanilla"
            },
            DownloadUrl = GetDownloadUrl(root),
            ReleasedOn = DateTimeOffset.Parse(root.GetProperty("releaseTime").GetString()!)
        };

        static string GetDownloadUrl(JsonElement jsonElement) {
            return (jsonElement.TryGetProperty("downloads", out var downloadsElement)
                ? downloadsElement.TryGetProperty("server", out var serverElement)
                    ? serverElement.TryGetProperty("url", out var urlElement)
                        ? urlElement.GetString()
                        : default
                    : default
                : default)!;
        }

        return release;
    }

    private readonly record struct VanillaResponse(VanillaLatest Latest, IReadOnlyCollection<VanillaVersion> Versions);

    private readonly record struct VanillaLatest(string Release, string Snapshot);

    private readonly record struct VanillaVersion(string Id, string Url);
}