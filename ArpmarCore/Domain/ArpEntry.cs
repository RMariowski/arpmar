using System.Net;
using System.Net.NetworkInformation;

namespace ArpmarCore.Domain
{
    public class ArpEntry
    {
        public string Interface { get; }
        public IPAddress IpAddress { get; }
        public PhysicalAddress MacAddress { get; }
        public ArpEntryType Type { get; }

        public ArpEntry(string @interface, IPAddress ip, PhysicalAddress mac, ArpEntryType type)
        {
            Interface = @interface;
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
            return Interface == other.Interface &&
                   IpAddress.Equals(other.IpAddress) &&
                   MacAddress.Equals(other.MacAddress) &&
                   Type == other.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Interface != null ? Interface.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (IpAddress != null ? IpAddress.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (MacAddress != null ? MacAddress.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Type;
                return hashCode;
            }
        }

        public override string ToString() 
            => $"IP: {IpAddress} MAC: {MacAddress} Type {Type.ToString()}";
    }
}