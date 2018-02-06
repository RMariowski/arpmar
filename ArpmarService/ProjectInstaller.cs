using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using ArpmarCore.Domain;

namespace ArpmarService
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            var serviceProcessInstaller = new ServiceProcessInstaller { Account = ServiceAccount.LocalSystem };
            Installers.Add(serviceProcessInstaller);

            var serviceInstaller = new ServiceInstaller
            {
                StartType = ServiceStartMode.Manual,
                ServiceName = Service.Name,
                DisplayName = Service.DisplayName,
                Description = Service.Description
            };
            Installers.Add(serviceInstaller);
        }
    }
}