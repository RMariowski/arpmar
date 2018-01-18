using ArpmarCore.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArpmarTests
{
    [TestClass]
    public class ExecutingTests
    {
        [TestMethod]
        public void Check_if_arp_command_executing_is_working()
        {
            var arp = new Arp();

            arp.Execute();

            Assert.AreNotEqual(arp.Tables.Count, 0);
        }
    }
}