using Microsoft.Extensions.Logging;

namespace Nylium.Minecraft.Providers;

/// <inheritdoc />
public sealed class ForgeProvider : IServerProvider {
    /// <inheritdoc />
    public string Url
        => "https://files.minecraftforge.net/net/minecraftforge/forge/";

    private readonly HttpClient _httpClient;
    private readonly ILogger<ForgeProvider> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="logger"></param>
    public ForgeProvider(HttpClient httpClient, ILogger<ForgeProvider> logger) {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <inheritdoc />
    public async ValueTask<IEnumerable<Release>?> GetReleasesAsync() {
        return default;
    }
}