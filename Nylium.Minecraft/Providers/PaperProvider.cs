using Nylium.Common;

namespace Nylium.Minecraft.Providers;

/// <inheritdoc />
public sealed class PaperProvider : IServerProvider {
    /// <inheritdoc />
    public string Url
        => "https://papermc.io/js/downloads.js";

    /// <summary>
    /// 
    /// </summary>
    public const string API_URL
        = "https://papermc.io/api/v2/projects/paper/";

    private readonly HttpClient _httpClient;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    public PaperProvider(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    /// <inheritdoc />
    public async ValueTask<IEnumerable<Release>?> GetReleasesAsync() {
        var releases = new List<Release>();
        var versions = await GetVersionsAsync();
        await Parallel.ForEachAsync(versions, async (version, _) => {
            var builds = await GetBuildsAsync(version);
            await Parallel.ForEachAsync(builds, _, async (build, _) => {
                releases.Add(new Release {
                    Version = new Version {
                        Minecraft = version,
                        IsPrerelease = true,
                        Loader = $"{build}"
                    },
                    DownloadUrl = (await GetDownloadUrlAsync(version, build)).Url,
                    ReleasedOn = default
                });
            });
        });
        return releases;
    }

    private async Task<(string Url, string FileName)> GetDownloadUrlAsync(string version, int build) {
        var baseUrl = $"{API_URL}/versions/{version}/builds/{build}";
        var element = await _httpClient.GetJsonAsync(baseUrl);
        var downloadName = element
            .GetProperty("downloads")
            .GetProperty("application")
            .GetProperty("name")
            .GetString()!;

        return ($"{baseUrl}/{build}/downloads/{downloadName}", downloadName);
    }

    private async Task<IEnumerable<int>> GetBuildsAsync(string version) {
        var element = await _httpClient.GetJsonAsync($"{API_URL}/versions/{version}");
        return element.GetProperty("builds")
            .EnumerateArray()
            .Select(x => x.GetInt32());
    }

    private async Task<IEnumerable<string>> GetVersionsAsync() {
        var element = await _httpClient.GetJsonAsync(API_URL);
        return element.GetProperty("versions")
            .EnumerateArray()
            .Select(x => x.GetString()!);
    }
}