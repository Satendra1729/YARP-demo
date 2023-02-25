using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json; 

namespace YARP_Proxy.Tests;

public class BasicTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private static string StreamToString(Stream stream)
    {
        stream.Position = 0;
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            return reader.ReadToEnd();
        }
    }

    [Theory]
    [InlineData("/post")]
    public async Task EchoedHeaderAndBodyIsCorrect(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        HttpResponseMessage response; 

        using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url))
            {
                requestMessage.Headers.Add("Autherization", "Bearer Token");

                requestMessage.Content = JsonContent.Create(new { Name = "Mr. Anderson" }); 
                
                // Act
                response = await client.SendAsync(requestMessage);
            }

        // Assert
        response.EnsureSuccessStatusCode(); 

        string responseString = StreamToString(await response.Content.ReadAsStreamAsync()); 

        Console.WriteLine(responseString); 

        var responseJObject = JsonSerializer
                                    .Deserialize<JsonDocument>(responseString);

        Assert.Equal("Bearer Token",responseJObject?.RootElement.GetStrProperty("headers:autherization").ToString()); 

        Assert.Equal("Mr. Anderson",responseJObject?.RootElement.GetStrProperty("json:name").ToString()); 

    }
}