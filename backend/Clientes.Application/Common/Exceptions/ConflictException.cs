using System.Net;

namespace Clientes.Application.Common.Exceptions;

public class ConflictException : ApplicationExceptionBase
{
    public ConflictException(string message)
        : base(message, HttpStatusCode.Conflict)
    {
    }
}
