using Nylium.Fabric;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Nylium.Minecraft.Providers;

/// <inheritdoc />
public sealed class FabricProvider : IServerProvider {
    /// <inheritdoc />
    public string Url
        => "https://meta.fabricmc.net/v2/versions";

    private readonly FabricApi _fabricApi;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fabricApi"></param>
    public FabricProvider(FabricApi fabricApi) {
        _fabricApi = fabricApi;
    }

    /// <inheritdoc />
    public async ValueTask<IEnumerable<Release>?> GetReleasesAsync() {
        var versions = await _fabricApi.GetMinecraftVersionsAsync();
        var installer = (await _fabricApi.GetInstallersAsync())![0];
        var releases = new HashSet<Release>();

        await Parallel.ForEachAsync(versions!.Where(x => x.IsStable).Take(5),
            async (game, _) => {
                var loaders = await _fabricApi.GetLoadersAsync(game.Version);
                Parallel.ForEach(loaders!.Where(x => x.Loader.IsStable)
                        .Take(10)
                        .Select(x => x.Loader),
                    (loader, _) => {
                        releases.Add(new Release {
                            Version = new Version {
                                Minecraft = game.Version,
                                IsPrerelease = game.IsStable,
                                Loader = loader.Version
                            },
                            ReleasedOn = default,
                            DownloadUrl = FabricApi.GetDownloadUrl(game.Version, loader.Version, installer.Version)
                        });
                    });
            });

        return releases;
    }
}