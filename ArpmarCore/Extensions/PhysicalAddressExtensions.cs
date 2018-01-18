using System.Net.NetworkInformation;
using System.Text;

namespace ArpmarCore.Extensions
{
    public static class PhysicalAddressExtensions
    {
        public static string ToString(this PhysicalAddress macAddress, char separator)
        {
            string rawString = macAddress.ToString();

            var builder = new StringBuilder();
            for (var i = 0; i < rawString.Length; i += 2)
            {
                builder.Append(rawString.Substring(i, 2));

                if (i + 2 < rawString.Length)
                    builder.Append('-');
            }

            return builder.ToString();
        }
    }
}