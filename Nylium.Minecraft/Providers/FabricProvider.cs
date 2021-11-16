using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Nylium.Minecraft.Providers;

/// <inheritdoc />
public sealed class FabricProvider : IServerProvider {
    /// <inheritdoc />
    public string Url
        => "https://api.github.com/repos/FabricMC/fabric/releases";

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
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, Url)
            .WithBrowserHeader();
        using var responseMessage = await _httpClient.SendAsync(requestMessage);

        if (!responseMessage.IsSuccessStatusCode) {
            _logger.LogError("{ReasonPhrase}", responseMessage.ReasonPhrase);
            return default;
        }

        using var content = responseMessage.Content;
        await using var stream = await content.ReadAsStreamAsync();

        var gitHubReleases = await JsonSerializer.DeserializeAsync<IEnumerable<GitHubRelease>>(stream);
        return gitHubReleases!.Select(release => new Release {
            DownloadUrl = release.Url,
            ReleasedOn = release.CreatedOn,
            Version = new Version(release.Target, release.Tag, release.IsPrerelease)
        });
    }

    private readonly record struct GitHubRelease {
        [JsonPropertyName("browser_download_url")]
        public string Url { get; init; }

        [JsonPropertyName("tag_name")]
        public string Tag { get; init; }

        [JsonPropertyName("target_commitish")]
        public string Target { get; init; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedOn { get; init; }

        [JsonPropertyName("prerelease")]
        public bool IsPrerelease { get; init; }
    }
}