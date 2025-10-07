using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Clientes.Application.Clientes.DTOs;
using FluentAssertions;

namespace Clientes.Api.IntegrationTests.Clientes;

public class ClientesEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ClientesEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetClientes_ShouldReturnSeededData()
    {
        var response = await _client.GetAsync("/api/clientes?page=1&pageSize=5");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await DeserializeResponse<IReadOnlyCollection<ClienteDto>>(response);
        payload.Data.Should().NotBeNull();
        payload.Data.Should().HaveCountGreaterThan(0);
        payload.Paging.Should().NotBeNull();
        payload.TraceId.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task CreateCliente_ShouldSucceed()
    {
        var request = new CreateClienteRequest
        {
            Ruc = "20987654321",
            RazonSocial = "Cliente Integración",
            TelefonoContacto = "012345678",
            CorreoContacto = "integracion@cliente.pe",
            Direccion = "Av. Integración 123"
        };

        var response = await _client.PostAsJsonAsync("/api/clientes", request);
        var responseBody = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.Created, $"Body: {responseBody}");

        var payload = await DeserializeResponse<ClienteDto>(response);
        payload.Data.Should().NotBeNull();
        payload.Data!.Ruc.Should().Be(request.Ruc);
        response.Headers.Location.Should().NotBeNull();
    }

    private async Task<ApiResponse<T>> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        var payload = JsonSerializer.Deserialize<ApiResponse<T>>(json, _jsonOptions);
        payload.Should().NotBeNull();
        return payload!;
    }

    private record ApiResponse<T>(T? Data, PagingMetadata? Paging, SortMetadata? Sort, List<ApiError>? Errors, string TraceId);

    private record PagingMetadata(int Page, int PageSize, int Total, int TotalPages);

    private record SortMetadata(string? By, string? Dir);

    private record ApiError(string Code, string Message, string? Field);
}
