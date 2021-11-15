namespace Nylium.Minecraft;

/// <summary>
/// 
/// </summary>
/// <param name="Version"></param>
/// <param name="DownloadUrl"></param>
/// <param name="ReleasedOn"></param>
public record struct Release(Version Version, string DownloadUrl,
    DateTimeOffset ReleasedOn);

/// <summary>
/// 
/// </summary>
/// <param name="Minecraft"></param>
/// <param name="Loader"></param>
/// <param name="IsPrerelease"></param>
public record struct Version(string Minecraft, string Loader, bool IsPrerelease);