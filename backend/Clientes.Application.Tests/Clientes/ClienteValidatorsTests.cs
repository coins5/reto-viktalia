using Clientes.Application.Clientes.DTOs;
using Clientes.Application.Clientes.Validators;
using FluentValidation.TestHelper;

namespace Clientes.Application.Tests.Clientes;

public class ClienteValidatorsTests
{
    private readonly CreateClienteRequestValidator _createValidator = new();
    private readonly ClienteQueryParametersValidator _queryValidator = new();

    [Fact]
    public void CreateValidator_ShouldHaveError_WhenRucIsInvalid()
    {
        var model = new CreateClienteRequest
        {
            Ruc = "123",
            RazonSocial = "Cliente Test"
        };

        var result = _createValidator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Ruc);
    }

    [Fact]
    public void CreateValidator_ShouldNotHaveErrors_WhenModelIsValid()
    {
        var model = new CreateClienteRequest
        {
            Ruc = "20100070970",
            RazonSocial = "Cliente Test",
            TelefonoContacto = "012345678",
            CorreoContacto = "cliente@test.com",
            Direccion = "Av. Principal 123"
        };

        var result = _createValidator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void QueryValidator_ShouldHaveError_WhenSortDirInvalid()
    {
        var parameters = new ClienteQueryParameters
        {
            SortBy = "ruc",
            SortDir = "invalid"
        };

        var result = _queryValidator.TestValidate(parameters);
        result.ShouldHaveValidationErrorFor(x => x.SortDir);
    }
}
