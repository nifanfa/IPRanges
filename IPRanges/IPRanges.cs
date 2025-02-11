using System.Buffers.Binary;
using System.Net;

public class IPRanges
{
    public class Range
    {
        public required string Start { get; set; }
        public required string End { get; set; }
        public required string Province { get; set; }
        public required string City { get; set; }
        public required string ISP { get; set; }
    }

    private Range[] Values;

    private int[] Indexing = new int[byte.MaxValue + 1];

    public IPRanges(Range[] value)
    {
        int last = -1;
        for (int i = 0; i < value.Length; i++)
        {
            var curr = IPAddress.Parse(value[i].Start).GetAddressBytes().First();
            if (last != curr)
            {
                Indexing[curr] = i;
                last = curr;
            }
        }
        this.Values = value;
    }

    public Range GetRange(IPAddress ip)
    {
        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
            byte[] addr = ip.GetAddressBytes();
            for (int i = Indexing[addr.First()]; i < Values.Length; i++)
            {
                byte[] start = IPAddress.Parse(Values[i].Start).GetAddressBytes();
                byte[] end = IPAddress.Parse(Values[i].End).GetAddressBytes();

                if (BinaryPrimitives.ReadUInt32BigEndian(addr) < BinaryPrimitives.ReadUInt32BigEndian(start)) break;

                if (BinaryPrimitives.ReadUInt32BigEndian(addr) >= BinaryPrimitives.ReadUInt32BigEndian(start) &&
                    BinaryPrimitives.ReadUInt32BigEndian(addr) <= BinaryPrimitives.ReadUInt32BigEndian(end))
                {
                    return Values[i];
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