namespace Nylium.Fabric.Responses;

/// <summary>
/// 
/// </summary>
/// <param name="Seperator"></param>
/// <param name="Build"></param>
/// <param name="Maven"></param>
/// <param name="Version"></param>
/// <param name="IsStable"></param>
public readonly record struct FabricLoader(char Seperator, int Build, string Maven, string Version, bool IsStable);

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
public readonly record struct FabricIntermediary(string Maven, string Version, bool IsStable);

/// <summary>
/// 
/// </summary>
/// <param name="Version"></param>
/// <param name="Libraries"></param>
/// <param name="MainClass"></param>
public readonly record struct FabricLauncherMeta(string Version, FabricLibraries Libraries, FabricMainClass MainClass);

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

/// <summary>
/// 
/// </summary>
/// <param name="Client"></param>
/// <param name="Server"></param>
public readonly record struct FabricMainClass(string Client, string Server);