namespace Auth.Interfaces
{
    public interface ITokenProvider
    {
        Task<string?> GetTokenAsync();
    }
}
