using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace To_Do_API.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base (message)
        {
        }
    }
}
