namespace MagicVillaApi.Helpers
{
    public class PasswordHelperTO
    {

        public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
    }
}
