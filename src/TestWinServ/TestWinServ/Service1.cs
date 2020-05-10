using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TestWinServ.BRL;
using TestWinServ.Properties;

namespace TestWinServ
{
    public partial class Service1 : ServiceBase
    {
        private cbrlMonitor obMonitor;

        public Service1()
        {
            InitializeComponent();
            obMonitor = new cbrlMonitor(Convert.ToInt32(Settings.Default.Interval));
        }

        protected override void OnStart(string[] args)
        {
            this.explicitStart();
        }

        protected override void OnStop()
        {
            obMonitor.Stop();
            obMonitor = null;
        }

        internal void explicitStart()
        {
            obMonitor.Start();
        }
    }
}
