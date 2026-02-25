using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace OptiDemoCms.Tests;

/// <summary>
/// Integration tests for the home page (StartPage at /).
/// Verifies HTTP 200 and that MainHeading and block areas are rendered (structure and data-testid).
/// </summary>
public class HomePageIntegrationTests : IClassFixture<OptiDemoCmsWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HomePageIntegrationTests(OptiDemoCmsWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Root_Returns_200()
    {
        var response = await _client.GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_Root_Returns_Html()
    {
        var response = await _client.GetAsync("/");

        response.Content.Headers.ContentType?.MediaType
            .Should().Contain("text/html");
    }

    [Fact]
    public async Task Get_Root_Renders_StartPage_Structure()
    {
        var response = await _client.GetStringAsync("/");

        response.Should().Contain("data-testid=\"start-page\"");
    }

    [Fact]
    public async Task Get_Root_Renders_MainHeading_Element()
    {
        var response = await _client.GetStringAsync("/");

        response.Should().Contain("data-testid=\"main-heading\"");
    }

    [Fact]
    public async Task Get_Root_Renders_Main_Content_Area_Structure()
    {
        var response = await _client.GetStringAsync("/");

        response.Should().Contain("data-testid=\"main-content\"");
    }

    [Fact]
    public async Task Get_Root_Renders_Site_Header_And_Footer()
    {
        var response = await _client.GetStringAsync("/");

        response.Should().Contain("data-testid=\"site-header\"");
        response.Should().Contain("data-testid=\"site-footer\"");
    }

    [Fact]
    public async Task Get_Root_Renders_Navigation()
    {
        var response = await _client.GetStringAsync("/");

        response.Should().Contain("data-testid=\"site-nav\"");
    }

    [Fact]
    public async Task Get_Root_Contains_Expected_Text()
    {
        var response = await _client.GetStringAsync("/");

        response.Should().Contain("Optimizely CMS is running");
    }
}
