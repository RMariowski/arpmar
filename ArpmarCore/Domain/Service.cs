using System.Security.Principal;

namespace ArpmarCore.Domain
{
    public class Service
    {
        public const string Name = "ArpmarService";
        public const string DisplayName = "arpMAR";
        public const string Description = "ARP notifier";

        public const string EventSourceName = "arpMAR";
        public const string EventLogName = "arpMAR";

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}