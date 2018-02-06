using ArpmarCore.Domain;
using ArpmarCore.Extensions;

namespace ArpmarApp.Models
{
    public class TableEntry
    {
        public string Interface { get; }
        public string IpAddress { get; }
        public string MacAddress { get; }
        public string Type { get; }
        public string Description { get; }

        public TableEntry(ArpEntry arpEntry)
        {
            Interface = arpEntry.Interface;
            IpAddress = arpEntry.IpAddress.ToString();
            MacAddress = arpEntry.MacAddress.ToString('-');
            Type = arpEntry.Type.ToString();
            Description = GetDescription(arpEntry);
        }

        private static string GetDescription(ArpEntry arpEntry)
        {
            string ip = arpEntry.IpAddress.ToString();
            string mac = arpEntry.MacAddress.ToString();

            if (ip == "255.255.255.255" && mac == "FFFFFFFFFFFF")
                return "Broadcast";

            return string.Empty;
        }
    }
}