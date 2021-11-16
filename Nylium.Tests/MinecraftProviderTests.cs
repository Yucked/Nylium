using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nylium.Minecraft.Providers;
using System.Threading.Tasks;

namespace Nylium.Tests;

[TestClass]
public sealed class MinecraftProviderTests {
    [TestMethod]
    public async Task FabricReleasesTest() {
        var provider = Statics.GetService<FabricProvider>();
        var releases = await provider.GetReleasesAsync();
        Assert.IsNotNull(releases);
    }
}