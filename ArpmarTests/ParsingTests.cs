using System.Linq;
using ArpmarCore.Domain;
using ArpmarCore.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArpmarTests
{
    [TestClass]
    public class ParsingTests
    {
        private const string ArpOutput = @"
Interface: 192.168.0.100 --- 0xb
  Internet Address      Physical Address      Type
  192.168.0.1           e8-de-27-44-0a-59     dynamic
  192.168.0.255         ff-ff-ff-ff-ff-ff     static
  224.0.0.22            01-00-5e-00-00-16     static
  224.0.0.251           01-00-5e-00-00-fb     static
  224.0.0.252           01-00-5e-00-00-fc     static
  239.255.255.250       01-00-5e-7f-ff-fa     static
  255.255.255.255       ff-ff-ff-ff-ff-ff     static

Interface: 192.168.56.1 --- 0xd
  Internet Address      Physical Address      Type
  192.168.56.255        ff-ff-ff-ff-ff-ff     static
  224.0.0.22            01-00-5e-00-00-16     static
  224.0.0.251           01-00-5e-00-00-fb     static
  224.0.0.252           01-00-5e-00-00-fc     static
  239.255.255.250       01-00-5e-7f-ff-fa     static

Interface: 172.21.232.81 --- 0xf
  Internet Address      Physical Address      Type
  172.21.232.95         ff-ff-ff-ff-ff-ff     static
  224.0.0.22            01-00-5e-00-00-16     static
  224.0.0.251           01-00-5e-00-00-fb     static
  224.0.0.252           01-00-5e-00-00-fc     static
  239.255.255.250       01-00-5e-7f-ff-fa     static
  255.255.255.255       ff-ff-ff-ff-ff-ff     static";

        private static readonly string[] Interfaces = { "192.168.0.100", "192.168.56.1", "172.21.232.81" };

        [TestMethod]
        public void Check_if_all_interfaces_are_parsed()
        {
            var arp = new Arp();

            arp.Parse(ArpOutput);

            Assert.IsNotNull(arp[Interfaces[0]]);
            Assert.IsNotNull(arp[Interfaces[1]]);
            Assert.IsNotNull(arp[Interfaces[2]]);
        }

        [TestMethod]
        public void Accessing_to_non_parsed_interface_should_return_null()
        {
            var arp = new Arp();

            arp.Parse(ArpOutput);

            Assert.IsNull(arp["127.0.0.1"]);
        }

        [TestMethod]
        public void Check_if_entries_counts_for_all_interfaces_are_correct()
        {
            var arp = new Arp();

            arp.Parse(ArpOutput);

            Assert.AreEqual(7, arp[Interfaces[0]].Count());
            Assert.AreEqual(5, arp[Interfaces[1]].Count());
            Assert.AreEqual(6, arp[Interfaces[2]].Count());
        }

        [TestMethod]
        public void Check_if_ips_of_some_entries_are_correct()
        {
            var arp = new Arp();

            arp.Parse(ArpOutput);

            Assert.AreEqual("192.168.0.1", arp[Interfaces[0]][0].IpAddress.ToString());
            Assert.AreEqual("224.0.0.251", arp[Interfaces[1]][2].IpAddress.ToString());
            Assert.AreEqual("255.255.255.255", arp[Interfaces[2]][5].IpAddress.ToString());
        }

        [TestMethod]
        public void Check_if_macs_of_some_entries_are_correct()
        {
            var arp = new Arp();

            arp.Parse(ArpOutput);

            Assert.AreEqual("FF-FF-FF-FF-FF-FF", arp[Interfaces[0]][1].MacAddress.ToString('-'));
            Assert.AreEqual("01-00-5E-00-00-FB", arp[Interfaces[1]][2].MacAddress.ToString('-'));
            Assert.AreEqual("01-00-5E-00-00-FC", arp[Interfaces[2]][3].MacAddress.ToString('-'));
        }

        [TestMethod]
        public void Check_if_types_of_some_entries_are_correct()
        {
            var arp = new Arp();

            arp.Parse(ArpOutput);

            Assert.AreEqual(ArpEntryType.Dynamic, arp[Interfaces[0]][0].Type);
            Assert.AreEqual(ArpEntryType.Static, arp[Interfaces[1]][2].Type);
            Assert.AreEqual(ArpEntryType.Static, arp[Interfaces[2]][5].Type);
        }
    }
}
