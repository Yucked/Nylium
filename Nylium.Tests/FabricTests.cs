using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nylium.Fabric;

namespace Nylium.Tests;

[TestClass]
public sealed class FabricTests {
    private readonly FabricApi _fabricApi;

    public FabricTests() {
        _fabricApi = Statics.GetService<FabricApi>();
    }

    [TestMethod]
    public async Task GetVersionsAsync() {
        var fabricGames = await _fabricApi.GetMinecraftVersionsAsync();
        Assert.IsTrue(fabricGames != null);
        Assert.IsTrue(fabricGames!.Count != 0);
    }

    [TestMethod]
    public async Task GetLoadersAsync() {
        var fabricLoaders = await _fabricApi.GetLoadersAsync();
        Assert.IsTrue(fabricLoaders != null);
        Assert.IsTrue(fabricLoaders!.Count != 0);
    }

    [DataTestMethod]
    [DataRow("1.16")]
    [DataRow("1.16.1")]
    [DataRow("1.17")]
    [DataRow("1.17.1")]
    [DataRow("1.18")]
    [DataRow("1.18.1")]
    public async Task GetLoaderVersionsAsync(string version) {
        var fabricLoaders = await _fabricApi.GetLoadersAsync(version);
        Assert.IsTrue(fabricLoaders != null);
        Assert.IsTrue(fabricLoaders!.Count != 0);
    }

    [DataTestMethod]
    [DataRow("1.16.1", "0.12.11", "0.10.2")]
    [DataRow("1.17.1", "0.12.11", "0.9.1")]
    [DataRow("1.18", "0.12.11", "0.10.2")]
    [DataRow("1.18.1", "0.12.11", "0.9.1")]
    public async Task DownloadServerAsync(string gameVersion, string loaderVersion, string installerVersion) {
        await _fabricApi.GetServerJarAsync(gameVersion, loaderVersion, installerVersion);
    }

    [DataTestMethod]
    [DataRow("1.16")]
    [DataRow("1.16.1")]
    [DataRow("1.17")]
    [DataRow("1.17.1")]
    [DataRow("1.18")]
    [DataRow("1.18.1")]
    public async Task DownloadServerWithVersionOnlyAsync(string gameVersion) {
        await _fabricApi.GetLatestJarAsync(gameVersion);
    }
}