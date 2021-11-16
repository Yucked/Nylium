namespace Nylium.Minecraft.Providers;

/// <inheritdoc />
public sealed class PaperProvider : IServerProvider {
    /// <inheritdoc />
    public string Url
        => "https://papermc.io/js/downloads.js";

    /// <summary>
    /// 
    /// </summary>
    public static string LegacyBuilds
        => "https://papermc.io/legacy#Paper";

    /// <inheritdoc />
    public ValueTask<IEnumerable<Release>?> GetReleasesAsync() {
        throw new NotImplementedException();
    }
}