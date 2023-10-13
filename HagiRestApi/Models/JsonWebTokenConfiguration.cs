namespace HagiRestApi.Models
{
    public class JsonWebTokenConfiguration
    {
        public string Key { get; set; }
        public string Subject { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public int MinutesBeforeJsonWebTokenExpires { get; set; }
    }
}
