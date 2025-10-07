using System.Net;

namespace Clientes.Application.Common.Exceptions;

public class NotFoundException : ApplicationExceptionBase
{
    public NotFoundException(string resourceName, object key)
        : base($"{resourceName} con identificador '{key}' no fue encontrado.", HttpStatusCode.NotFound)
    {
    }
}
