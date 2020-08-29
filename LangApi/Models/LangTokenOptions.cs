namespace LangApi.Models
{
    public static class LangTokenOptions
    {
        public const string Issuer = "LangAPI";            // издатель токена
        public const string Audience = "LangAPI";          // потребитель токена
        public const string Key = "01234567890123456";    // ключ для шифрования >= 16 chars
    }
}
