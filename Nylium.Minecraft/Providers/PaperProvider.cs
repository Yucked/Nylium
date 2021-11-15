namespace Nylium.Minecraft.Providers;
/// <inheritdoc />
public record PaperProvider : IServerProvider {
    /// <inheritdoc />
    public string Url
        => "https://papermc.io/js/downloads.js";

    /// <summary>
    /// 
    /// </summary>
    public string LegacyBuilds
        => "https://papermc.io/legacy#Paper";

    /// <inheritdoc />
    public ValueTask<IEnumerable<Release>> GetReleasesAsync() {
        throw new NotImplementedException();
    }
}