namespace Auth.Interfaces
{
    public interface IAuthProvider
    {
        Task<string?> GetTokenAsync();
    }
}
