namespace Server.Csharp.Presentation.Exceptions
{
    public class BadRequestException:Exception
    {
        public BadRequestException(string? message):base(message)
        {
            
        }
    }
}
