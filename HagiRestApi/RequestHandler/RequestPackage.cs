using MessagePack;

namespace HagiRestApi
{
    [MessagePackObject]
    public class RequestPackage
    {
        [Key(0)] public RequestType RequestType { get; set; }
        [Key(1)] public byte[] DataInBytes { get; set; }
    }
}
