using System.Net;
using System.Net.NetworkInformation;

namespace ArpmarCore.Domain
{
    public class ArpEntry
    {
        public IPAddress IpAddress { get; }
        public PhysicalAddress MacAddress { get; }
        public ArpEntryType Type { get; }

        public ArpEntry(IPAddress ip, PhysicalAddress mac, ArpEntryType type)
        {
            IpAddress = ip;
            MacAddress = mac;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ArpEntry arpEntry))
                return false;

            return Equals(arpEntry);
        }

        public bool Equals(ArpEntry other)
        {
            return IpAddress.Equals(other.IpAddress) && 
                   MacAddress.Equals(other.MacAddress) &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = IpAddress != null ? IpAddress.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (MacAddress != null ? MacAddress.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Type;
                return hashCode;
            }
        }
    }
}