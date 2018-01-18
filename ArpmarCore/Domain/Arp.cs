using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using InterfacesAndArpTables = System.Collections.Concurrent.ConcurrentDictionary<string, ArpmarCore.Domain.ArpTable>;

namespace ArpmarCore.Domain
{
    public class Arp : IArp
    {
        private readonly InterfacesAndArpTables _tables = new InterfacesAndArpTables();

        public IReadOnlyDictionary<string, ArpTable> Tables => _tables;       

        public ArpTable this[string @interface]
        {
            get
            {
                if (_tables.IsEmpty)
                    Execute();

                return _tables.ContainsKey(@interface) ? _tables[@interface] : null;
            }
        }

        public void Execute()
        {
            var process = Process.Start(new ProcessStartInfo("arp", "-a")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true
            });

            string output = process?.StandardOutput.ReadToEnd();
            process?.Close();

            Parse(output);
        }

        public void Parse(string arpOutput)
        {
            string currentInterface = string.Empty;

            var lines = arpOutput.Split('\n').Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
            foreach (string line in lines)
            {
                var items = Regex.Split(line, @"\s+").Where(i => !string.IsNullOrWhiteSpace(i)).ToArray();
                switch (items.Length)
                {
                    case 3:
                        ParseEntry(items, currentInterface);
                        break;

                    case 4:
                        currentInterface = ParseInterface(items);
                        break;
                }
            }
        }

        private string ParseInterface(IReadOnlyList<string> items)
        {
            string @interface = items[1];
            _tables[@interface] = new ArpTable();

            return @interface;
        }

        private void ParseEntry(IReadOnlyList<string> items, string @interface)
        {
            if (string.IsNullOrEmpty(@interface))
                return;

            var ip = IPAddress.Parse(items[0]);
            var mac = PhysicalAddress.Parse(items[1].ToUpperInvariant());
            var type = items[2][0] == 'd' ? ArpEntryType.Dynamic : ArpEntryType.Static;

            var arpEntry = new ArpEntry(ip, mac, type);
            _tables[@interface].AddEntry(arpEntry);
        }
    }
}