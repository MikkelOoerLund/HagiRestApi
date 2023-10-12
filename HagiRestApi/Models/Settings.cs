namespace HagiRestApi.Models
{
    public class Settings
    {
        public string BearerKey { get; set; }

        public int MinutesBeforeJsonWebTokenExpires { get; set; }
    }
}
