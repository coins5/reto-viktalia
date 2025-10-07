using Clientes.Application.Clientes.DTOs;
using FluentValidation;

namespace Clientes.Application.Clientes.Validators;

public class ClienteQueryParametersValidator : AbstractValidator<ClienteQueryParameters>
{
    private static readonly string[] AllowedSortColumns = ["ruc", "razonsocial", "createdat"];

    public ClienteQueryParametersValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("El número de página debe ser mayor o igual a 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("El tamaño de página debe estar entre 1 y 100.");

        RuleFor(x => x.SortDir)
            .Must(dir => string.IsNullOrWhiteSpace(dir) ||
                         string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase) ||
                         string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("sortDir debe ser 'asc' o 'desc'.");

        RuleFor(x => x.SortBy)
            .Must(IsValidSortColumn)
            .WithMessage($"sortBy debe ser uno de los siguientes valores: {string.Join(", ", AllowedSortColumns)}.")
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy));

        RuleFor(x => x.Ruc)
            .MaximumLength(11)
            .WithMessage("El RUC no debe exceder 11 caracteres.");

        RuleFor(x => x.RazonSocial)
            .MaximumLength(150)
            .WithMessage("La Razón Social no debe exceder 150 caracteres.");
    }

    private static bool IsValidSortColumn(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return true;
        }

        return AllowedSortColumns.Contains(sortBy.ToLowerInvariant());
    }
}
