using MessagePack;

namespace HagiRestApi
{
    public class RequestPackageSerializer
    {
        public byte[] Serialize(RequestPackage requestPackage)
        {
            return MessagePackSerializer.Serialize(requestPackage);
        }

        public RequestPackage Deserialize(byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<RequestPackage>(bytes);
        }
    }
}
