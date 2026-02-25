using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace OptiDemoCms.Tests;

/// <summary>
/// Smoke tests for health endpoints. Verifies /health and /health/ready return JSON and expected status.
/// </summary>
public class HealthEndpointIntegrationTests : IClassFixture<OptiDemoCmsWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HealthEndpointIntegrationTests(OptiDemoCmsWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Health_Returns_200_Or_503()
    {
        var response = await _client.GetAsync("/health");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.ServiceUnavailable);
    }

    [Fact]
    public async Task Get_Health_Returns_Json()
    {
        var response = await _client.GetAsync("/health");

        response.Content.Headers.ContentType?.MediaType
            .Should().Contain("application/json");
    }

    [Fact]
    public async Task Get_Health_Returns_Status_In_Json()
    {
        var response = await _client.GetStringAsync("/health");

        response.Should().Contain("status");
    }

    [Fact]
    public async Task Get_Health_Ready_Returns_200_Or_503()
    {
        var response = await _client.GetAsync("/health/ready");

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.ServiceUnavailable);
    }

    [Fact]
    public async Task Get_Health_Ready_Returns_Json()
    {
        var response = await _client.GetAsync("/health/ready");

        response.Content.Headers.ContentType?.MediaType
            .Should().Contain("application/json");
    }
}
