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
                if (addr[0] >= start[0] &&
                    addr[1] >= start[1] &&
                    addr[2] >= start[2] &&
                    addr[3] >= start[3] &&
                    addr[0] <= end[0] &&
                    addr[1] <= end[1] &&
                    addr[2] <= end[2] &&
                    addr[3] <= end[3])
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