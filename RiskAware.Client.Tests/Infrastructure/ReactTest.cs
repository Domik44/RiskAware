using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Playwright;
using Program = RiskAware.Server.Program;

namespace RiskAware.Client.Tests.Infrastructure
{
    public class ReactTest : PageTest
    {
        protected static readonly Uri RootUri = new("http://127.0.0.1");

        private readonly WebApplicationFactory<Program> _webApplicationFactory = new();

        private HttpClient? _httpClient;

        [SetUp]
        public async Task ReactSetup()
        {
            _httpClient =
                _webApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions {BaseAddress = RootUri});

            await Context.RouteAsync($"{RootUri.AbsoluteUri}**", async route =>
            {
                IRequest request = route.Request;
                ByteArrayContent? content = request.PostDataBuffer is { } postDataBuffer
                    ? new ByteArrayContent(postDataBuffer)
                    : null;

                HttpRequestMessage requestMessage =
                    new(new HttpMethod(request.Method), request.Url) {Content = content};

                foreach (KeyValuePair<string, string> header in request.Headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }

                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);
                byte[] responseBody = await response.Content.ReadAsByteArrayAsync();
                IEnumerable<KeyValuePair<string, string>> responseHeaders =
                    response.Content.Headers.Select(h => KeyValuePair.Create(h.Key, string.Join(",", h.Value)));

                await route.FulfillAsync(new RouteFulfillOptions
                {
                    BodyBytes = responseBody, Headers = responseHeaders, Status = (int)response.StatusCode
                });
            });
        }

        [TearDown]
        public void ReacTearDown()
        {
            _httpClient?.Dispose();
        }
    }
}
