using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ArpmarCore.Domain
{
    public class ArpTable : IEnumerable<ArpEntry>
    {
        private readonly List<ArpEntry> _entries = new List<ArpEntry>();
        private static readonly object Lock = new object();

        public string Interface { get; }

        public ArpEntry this[int index]
        {
            get
            {
                lock (Lock)
                    return _entries[index];
            }
        }

        public IEnumerable<ArpEntry> Entries
        {
            get
            {
                lock (Lock)
                    return _entries;
            }
        }

        public ArpTable(string @interface)
            => Interface = @interface;


        public void AddEntry(ArpEntry arpEntry)
        {
            lock (Lock)
                _entries.Add(arpEntry);
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

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.Append($"Interface: {Interface}\n");
            foreach (var entry in Entries)
                builder.Append($"{entry}\n");

            return builder.ToString();
        }
    }
}