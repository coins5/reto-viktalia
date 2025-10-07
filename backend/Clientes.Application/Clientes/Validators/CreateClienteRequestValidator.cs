using System.Text.RegularExpressions;
using Clientes.Application.Clientes.DTOs;
using FluentValidation;

namespace Clientes.Application.Clientes.Validators;

public class CreateClienteRequestValidator : AbstractValidator<CreateClienteRequest>
{
    private static readonly Regex RucRegex = new("^\\d{11}$", RegexOptions.Compiled);
    private static readonly Regex TelefonoRegex = new("^\\d{7,15}$", RegexOptions.Compiled);

    public CreateClienteRequestValidator()
    {
        RuleFor(x => x.Ruc)
            .NotEmpty().WithMessage("El RUC es requerido.")
            .Must(ruc => RucRegex.IsMatch(ruc)).WithMessage("El RUC debe tener 11 dígitos numéricos.");

        RuleFor(x => x.RazonSocial)
            .NotEmpty().WithMessage("La Razón Social es requerida.")
            .MinimumLength(3).WithMessage("La Razón Social debe tener al menos 3 caracteres.")
            .MaximumLength(150).WithMessage("La Razón Social no debe exceder 150 caracteres.");

        RuleFor(x => x.TelefonoContacto)
            .Cascade(CascadeMode.Stop)
            .Must(telefono => string.IsNullOrWhiteSpace(telefono) || TelefonoRegex.IsMatch(telefono!))
            .WithMessage("El teléfono debe contener entre 7 y 15 dígitos.")
            .When(x => !string.IsNullOrWhiteSpace(x.TelefonoContacto));

        RuleFor(x => x.CorreoContacto)
            .EmailAddress().WithMessage("El correo debe tener un formato válido.")
            .When(x => !string.IsNullOrWhiteSpace(x.CorreoContacto));

        RuleFor(x => x.Direccion)
            .MaximumLength(200).WithMessage("La dirección no debe exceder 200 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Direccion));
    }
}
