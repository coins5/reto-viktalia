using AutoMapper;
using Clientes.Application.Clientes.DTOs;
using Clientes.Application.Clientes.Profiles;
using Clientes.Application.Clientes.Services;
using Clientes.Application.Common.Exceptions;
using Clientes.Application.Common.Interfaces;
using Clientes.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Clientes.Application.Tests.Clientes;

public class ClienteServiceTests
{
    private readonly Mock<IClienteRepository> _clienteRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();
    private readonly IMapper _mapper;
    private readonly ClienteService _sut;

    public ClienteServiceTests()
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<ClienteProfile>());
        _mapper = configuration.CreateMapper();

        var loggerMock = new Mock<ILogger<ClienteService>>();

        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(new DateTimeOffset(2024, 10, 15, 12, 0, 0, TimeSpan.Zero));

        _sut = new ClienteService(
            _clienteRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _dateTimeProviderMock.Object,
            _mapper,
            loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowConflictException_WhenRucExists()
    {
        // Arrange
        var request = new CreateClienteRequest
        {
            Ruc = "20100070970",
            RazonSocial = "Cliente Existente"
        };

        _clienteRepositoryMock
            .Setup(x => x.ExistsByRucAsync(request.Ruc, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        Func<Task> action = async () => await _sut.CreateAsync(request);

        // Assert
        await action.Should().ThrowAsync<ConflictException>()
            .WithMessage("Ya existe un cliente con el RUC 20100070970.");
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnClienteDto_WhenDataIsValid()
    {
        // Arrange
        var request = new CreateClienteRequest
        {
            Ruc = "20100070970",
            RazonSocial = "Cliente Nuevo",
            TelefonoContacto = "012345678",
            CorreoContacto = "cliente@ejemplo.com",
            Direccion = "Av. Siempre Viva 742"
        };

        Cliente? storedCliente = null;

        _clienteRepositoryMock
            .Setup(x => x.ExistsByRucAsync(request.Ruc, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _clienteRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Cliente>(), It.IsAny<CancellationToken>()))
            .Callback<Cliente, CancellationToken>((cliente, _) =>
            {
                storedCliente = cliente;
                typeof(Cliente).GetProperty(nameof(cliente.Id))!.SetValue(cliente, 1L);
            })
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _sut.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Ruc.Should().Be(request.Ruc);
        result.RazonSocial.Should().Be(request.RazonSocial);
        result.TelefonoContacto.Should().Be(request.TelefonoContacto);
        result.CorreoContacto.Should().Be(request.CorreoContacto);
        result.Direccion.Should().Be(request.Direccion);

        storedCliente.Should().NotBeNull();
        storedCliente!.CreatedAt.Should().Be(_dateTimeProviderMock.Object.UtcNow.UtcDateTime);
        storedCliente!.UpdatedAt.Should().Be(_dateTimeProviderMock.Object.UtcNow.UtcDateTime);
    }
}
