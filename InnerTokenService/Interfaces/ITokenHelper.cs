namespace InnerTokenService.Interfaces
{
    public interface ITokenHelper
    {
        string GenerateServiceToken(TimeSpan expiration);
    }
}
