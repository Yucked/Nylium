using Microsoft.Extensions.DependencyInjection;
using Nylium.Minecraft.Providers;
using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nylium.Fabric;
using Nylium.Minecraft;

namespace Nylium.Tests;
internal readonly record struct Statics {

    public static IServiceProvider Provider
        => new ServiceCollection()
        .AddLogging()
        .AddSingleton<HttpClient>()
        .AddSingleton<FabricProvider>()
        .AddSingleton<FabricApi>()
        .AddSingleton<PaperProvider>()
        .AddSingleton<VanillaProvider>()
        .BuildServiceProvider();

    public static T GetService<T>() {
#pragma warning disable CS8714
        return Provider.GetRequiredService<T>();
    }

    public static void CheckRelease(Release release) {
        Assert.IsNotNull(release);
        Assert.IsNotNull(release.DownloadUrl);
        Assert.IsNotNull(release.Version);
        Assert.IsNotNull(release.Version.Loader);
        Assert.IsNotNull(release.Version.Minecraft);
        Assert.IsNotNull(release.Version.IsPrerelease);
    }
}