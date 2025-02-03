using System.Net;
using System.Text.Json;

IPRanges ranges = new IPRanges(JsonSerializer.Deserialize<IPRanges.Range[]>(File.ReadAllText("IPRanges.json")));
IPRanges.Range range = ranges.GetRange(IPAddress.Parse("182.146.169.105"));
Console.WriteLine(range.Province + range.City + range.ISP);