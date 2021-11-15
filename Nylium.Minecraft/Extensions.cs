namespace Nylium.Minecraft;
internal static class Extensions {
    public static HttpRequestMessage WithBrowserHeader(this HttpRequestMessage message) {
        message.Headers.Add("user-agent",
           "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4688.0 Safari/537.36 Edg/97.0.1069.0");
        return message;
    }
}