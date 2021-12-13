using Microsoft.Extensions.DependencyInjection;
using Nylium.Minecraft.Providers;
using System;
using System.Net.Http;

namespace Nylium.Tests;
internal readonly record struct Statics {

    public static IServiceProvider Provider
        => new ServiceCollection()
        .AddLogging()
        .AddSingleton<HttpClient>()
        .AddSingleton<FabricProvider>()
        .AddSingleton<VanillaProvider>()
        .BuildServiceProvider();

    public static T GetService<T>() {
#pragma warning disable CS8714
        return Provider.GetRequiredService<T>();
    }
}