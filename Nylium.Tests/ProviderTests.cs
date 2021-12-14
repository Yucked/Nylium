using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nylium.Minecraft.Providers;
using System.Threading.Tasks;

namespace Nylium.Tests;

[TestClass]
public sealed class MinecraftProviderTests {
    [TestMethod]
    public Task FabricReleasesTest() {
        return TestProviderAsync(Statics.GetService<FabricProvider>());
    }

    [TestMethod]
    public Task VanillaReleasesTest() {
        return TestProviderAsync(Statics.GetService<VanillaProvider>());
    }

    [TestMethod]
    public Task PaperReleasesAsync() {
        return TestProviderAsync(Statics.GetService<PaperProvider>());
    }

    private static async Task TestProviderAsync(IServerProvider provider) {
        var releases = await provider.GetReleasesAsync();
        Assert.IsNotNull(releases);
        foreach (var release in releases) {
            Statics.CheckRelease(release);
        }
    }
}