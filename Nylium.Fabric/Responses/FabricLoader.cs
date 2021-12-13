using System.Text.Json.Serialization;

namespace Nylium.Fabric.Responses;

/// <summary>
/// 
/// </summary>
/// <param name="Seperator"></param>
/// <param name="Build"></param>
/// <param name="Maven"></param>
/// <param name="Version"></param>
/// <param name="IsStable"></param>
public readonly record struct FabricLoader(char Seperator, int Build, string Maven, string Version,
                                           [property: JsonPropertyName("stable")] bool IsStable);

/// <summary>
/// 
/// </summary>
/// <param name="Loader"></param>
/// <param name="Intermediary"></param>
/// <param name="LauncherMeta"></param>
public readonly record struct FabricGameLoader(FabricLoader Loader,
                                               FabricIntermediary Intermediary,
                                               FabricLauncherMeta LauncherMeta);

/// <summary>
/// 
/// </summary>
/// <param name="Maven"></param>
/// <param name="Version"></param>
/// <param name="IsStable"></param>
public readonly record struct FabricIntermediary(string Maven,
                                                 string Version,
                                                 [property: JsonPropertyName("stable")] bool IsStable);

/// <summary>
/// 
/// </summary>
/// <param name="Version"></param>
/// <param name="Libraries"></param>
public readonly record struct FabricLauncherMeta(int Version, FabricLibraries Libraries);

/// <summary>
/// 
/// </summary>
/// <param name="Clients"></param>
/// <param name="Commons"></param>
/// <param name="Servers"></param>
public readonly record struct FabricLibraries(IReadOnlyCollection<object> Clients,
                                              IReadOnlyCollection<FabricCommon> Commons,
                                              IReadOnlyCollection<object> Servers);

/// <summary>
/// 
/// </summary>
/// <param name="Name"></param>
/// <param name="Url"></param>
public readonly record struct FabricCommon(string Name, string Url);