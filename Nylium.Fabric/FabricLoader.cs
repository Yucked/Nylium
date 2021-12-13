namespace Nylium.Fabric;

/// <summary>
/// 
/// </summary>
/// <param name="Seperator"></param>
/// <param name="Build"></param>
/// <param name="Maven"></param>
/// <param name="Version"></param>
/// <param name="IsStable"></param>
public readonly record struct FabricLoader(char Seperator, int Build, string Maven, string Version, bool IsStable);