using System.Buffers.Binary;
using System.Net;

public class IPRanges
{
    public class Range
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string ISP { get; set; }
    }

    private Range[] Values;

    public IPRanges(Range[] value) => this.Values = value;

    public Range GetRange(IPAddress ip)
    {
        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            byte[] addr = ip.GetAddressBytes();
            foreach (var v in Values)
            {
                byte[] start = IPAddress.Parse(v.Start).GetAddressBytes();
                byte[] end = IPAddress.Parse(v.End).GetAddressBytes();
                if (addr[0] < start[0]) break;
                if (BinaryPrimitives.ReadUInt32BigEndian(addr) >= BinaryPrimitives.ReadUInt32BigEndian(start) &&
                    BinaryPrimitives.ReadUInt32BigEndian(addr) <= BinaryPrimitives.ReadUInt32BigEndian(end))
                {
                    return v;
                }
            }
        }
        return new Range()
        {
            Start = IPAddress.None.ToString(),
            End = IPAddress.Broadcast.ToString(),
            Province = "未知",
            City = "未知",
            ISP = "未知"
        };
    }
}