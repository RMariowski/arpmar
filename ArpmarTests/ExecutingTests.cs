using System.Linq;
using ArpmarCore.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArpmarTests
{
    [TestClass]
    public class ExecutingTests
    {
        [TestMethod]
        public void Check_if_arp_display_command_executing_is_working()
        {
            var arp = new Arp();

            arp.ExecuteDisplay();

            Assert.AreNotEqual(arp.Tables.Count, 0);
        }

        [TestMethod]
        public void Check_if_arp_add_command_executing_adds_entry()
        {
            var arp = new Arp();
            var firstInterfaceTable = arp.Tables.Values.First();
            const string entryIp = "123.123.123.123";
            const string entryMac = "ee-ee-ee-ee-ee-ee";
            int countBeforeAdd = firstInterfaceTable.Count();

            arp.ExecuteAdd(firstInterfaceTable.Interface, entryIp, entryMac);
            arp.ExecuteDisplay();

            Assert.AreEqual(countBeforeAdd + 1, arp.Tables.Values.First().Count());

            arp.ExecuteDelete(firstInterfaceTable.Interface, entryIp);
        }

        [TestMethod]
        public void Check_if_arp_delete_command_executing_deletes_entry()
        {
            var arp = new Arp();
            string @interface = arp.Tables.Values.First().Interface;
            const string entryIp = "124.124.124.124";
            const string entryMac = "ee-ee-ee-ee-ee-ee";

            arp.ExecuteAdd(@interface, entryIp, entryMac);
            arp.ExecuteDisplay();
            int countBeforeDelete = arp.Tables.Values.First().Count();
            arp.ExecuteDelete(@interface, entryIp);
            arp.ExecuteDisplay();

            Assert.AreEqual(countBeforeDelete - 1, arp.Tables.Values.First().Count());
        }
    }
}