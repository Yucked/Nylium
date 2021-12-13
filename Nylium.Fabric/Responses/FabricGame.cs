namespace Nylium.Fabric.Responses;

/// <summary>
/// 
/// </summary>
/// <param name="Version"></param>
/// <param name="IsStable"></param>
public readonly record struct FabricGame(string Version, bool IsStable);