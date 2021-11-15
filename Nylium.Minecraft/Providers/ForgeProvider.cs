namespace Nylium.Minecraft.Providers;
/// <inheritdoc />
public record ForgeProvider : IServerProvider {
    /// <inheritdoc />
    public string Url
        => "https://files.minecraftforge.net/net/minecraftforge/forge/";

    private readonly HttpClient _httpClient;
    internal ForgeProvider(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    /// <inheritdoc />
    public async ValueTask<IEnumerable<Release>?> GetReleasesAsync() {
        return default;
    }
}