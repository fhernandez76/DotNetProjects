using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using TestWinServ.Properties;

namespace TestWinServ.BRL
{
    public class cbrlMonitor
    {
        private System.Timers.Timer obTimer;
        private cbrlMainProcess obProcess;
        private Thread obThread;

        public cbrlMonitor(int viInterval)
        {
            obProcess = new cbrlMainProcess();
            obTimer = new System.Timers.Timer();
            obTimer.Elapsed += new ElapsedEventHandler(this.Tick);
            obTimer.Interval = viInterval * 1000;
        }

        public void Start()
        {
            obTimer.Start();
            this.Tick(null, null);
        }

        public void Stop()
        {
            obTimer.Stop();
        }

        private void Tick(object Sender, ElapsedEventArgs args)
        {
            //Parseamos y guardamos en un objeto DateTime la hora de inicio configurada en App.config.
            DateTime obStartTimedt = new DateTime();
            string sStartTime = Settings.Default.StartTime;
            obStartTimedt = obStartTimedt.AddHours(Convert.ToInt32(sStartTime.Substring(0, sStartTime.IndexOf(":"))));
            obStartTimedt = obStartTimedt.AddMinutes(Convert.ToInt32(sStartTime.Substring(sStartTime.IndexOf(":") + 1)));

            //Parseamos y guardamos en un objeto DateTime la hora de fin configurada en App.config.
            DateTime obFinishTimedt = new DateTime();
            string sFinishTime = Settings.Default.FinishTime;
            obFinishTimedt = obFinishTimedt.AddHours(Convert.ToInt32(sFinishTime.Substring(0, sFinishTime.IndexOf(":"))));
            obFinishTimedt = obFinishTimedt.AddMinutes(Convert.ToInt32(sFinishTime.Substring(sFinishTime.IndexOf(":") + 1)));

            //Verificamos que el horario para ejecutar el proceso esté dentro del horario actual del servidor.
            if (DateTime.Now.TimeOfDay >= obStartTimedt.TimeOfDay && DateTime.Now.TimeOfDay <= obFinishTimedt.TimeOfDay)
            {
                //Verificamos que el proceso haya terminado para crear un nuevo thread con la tarea a ejecutar.
                if (obProcess.Finished)
                {
                    ThreadStart initialThread = new ThreadStart(obProcess.StartProcess);
                    obThread = new Thread(initialThread);
                    obThread.IsBackground = true;
                    obThread.Start();
                }
            }
        }
    }
}
