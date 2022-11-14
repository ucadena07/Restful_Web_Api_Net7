namespace MagicVillaApi.Helpers.Interfaces
{
    public interface IPasswordService
    {
        PasswordHelperTO CreatePasswordHash(string password);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}
