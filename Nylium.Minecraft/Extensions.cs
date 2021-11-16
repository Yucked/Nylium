namespace Nylium.Minecraft;

internal static class Extensions {
    public static HttpRequestMessage WithBrowserHeader(this HttpRequestMessage message) {
        message.Headers.Add("user-agent",
            "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:15.0) Gecko/20100101 Firefox/15.0.1");
        return message;
    }
}