using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using TestWinServ.Integration;
using TestWinServ.Model;
using TestWinServ.Properties;

namespace TestWinServ.BRL
{
    public class cbrlMainProcess
    {
        public bool Finished { get; set;  }

        public cbrlMainProcess()
        {
            Finished = true;
        }

        public void StartProcess()
        {
            Finished = false;
            try
            {
                SaveResult2Csv(GenerateCsvContent(GetWeather()));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

            }
            finally
            {
                Finished = true;
            }
        }

        private ResponseOpenWeather GetWeather()
        {
            OpenWeatherCaller obCaller = new OpenWeatherCaller();
            ResponseOpenWeather obResult = obCaller.GetWeather();
            obCaller = null;

            return obResult;
        }

        private string GenerateCsvContent(ResponseOpenWeather obResponse)
        {
            string sDelimiter = Settings.Default.FieldDelimiter;
            StringBuilder sbContent = new StringBuilder();

            //Add headers
            string sHeaders = "Temperature" + sDelimiter + "Units" + sDelimiter + "Precipitation";
            sbContent.AppendLine(sHeaders);

            //Add Temperature
            sbContent.Append(obResponse.Main.Temp);
            sbContent.Append(sDelimiter);

            //Add Units
            sbContent.Append("C");
            sbContent.Append(sDelimiter);

            //Add Precipitation indicator
            if (obResponse.Weather.Any(w => w.Main.Equals("Rain")))
                sbContent.AppendLine("true");
            else
                sbContent.AppendLine("false");

            return sbContent.ToString();
        }

        private void SaveResult2Csv(string vsContent)
        {
            CsvExporter.Export2Csv(vsContent);
        }
    }
}
