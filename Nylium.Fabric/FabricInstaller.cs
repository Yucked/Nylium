namespace Nylium.Fabric;

/// <summary>
/// 
/// </summary>
/// <param name="Url"></param>
/// <param name="Maven"></param>
/// <param name="Version"></param>
/// <param name="IsStable"></param>
public readonly record struct FabricInstaller(string Url, string Maven, string Version, bool IsStable);