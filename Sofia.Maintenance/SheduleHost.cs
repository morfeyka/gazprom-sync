using System.ServiceProcess;
using Sofia.Hosting;

namespace Sofia.Maintenance
{
    public partial class SheduleHost : ServiceBase
    {
        private readonly HostContainer _host;

        public SheduleHost()
        {
            _host = new HostContainer();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _host.StartServices(ServiceName);
        }

        protected override void OnStop()
        {
            _host.StopServices();
        }
    }
}