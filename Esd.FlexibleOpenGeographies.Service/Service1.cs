using Esd.FlexibleOpenGeographies.Service.Functionality;
using System.ServiceProcess;

namespace Esd.FlexibleOpenGeographies.Service
{
    public partial class Service1 : ServiceBase
    {
        private readonly Controller _controller = new Controller();

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _controller.Start();
        }

        protected override void OnStop()
        {
            _controller.Stop();
        }
    }
}
