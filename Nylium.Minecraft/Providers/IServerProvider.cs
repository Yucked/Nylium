namespace Nylium.Minecraft.Providers;

/// <summary>
/// Test
/// </summary>
public interface IServerProvider {
    /// <summary>
    /// 
    /// </summary>
    string Url { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    ValueTask<IEnumerable<Release>?> GetReleasesAsync();
}