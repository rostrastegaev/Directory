namespace Auth
{
    public class AuthConfig
    {
        public string Issuer { get; private set; }
        public string Audience { get; private set; }
        public int Lifetime { get; private set; }
        public int PasswordLength { get; private set; }
        public string Key { get; private set; }
    }
}
