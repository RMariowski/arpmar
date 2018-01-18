using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ArpmarCore.Domain
{
    public class ArpTable : IEnumerable<ArpEntry>
    {
        private readonly List<ArpEntry> _entries = new List<ArpEntry>();
        private static readonly object Lock = new object();

        public ArpEntry this[int index]
        {
            get
            {
                lock (Lock)
                    return _entries[index];
            }
        }

        public IReadOnlyList<ArpEntry> Entries
        {
            get
            {
                lock (Lock)
                    return _entries;
            }
        }

        public void AddEntry(ArpEntry arpEntry)
        {
            lock (Lock)
                _entries.Add(arpEntry);
        }

        public ArpEntry GetEntryByIp(string ip)
        {
            lock (Lock)
                return _entries.FirstOrDefault(x => x.IpAddress.ToString() == ip);
        }

        public ArpEntry[] GetEntriesByMac(string mac)
        {
            lock (Lock)
                return _entries.Where(x => x.MacAddress.ToString() == mac).ToArray();
        }

        public ArpEntry[] GetEntriesByType(ArpEntryType arpEntryType)
        {
            lock (Lock)
                return _entries.Where(x => x.Type == arpEntryType).ToArray();
        }

        public IEnumerator<ArpEntry> GetEnumerator()
        {
            lock (Lock)
            {
                foreach (var entry in _entries)
                    yield return entry;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}