using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using TestWinServ.Model;
using TestWinServ.Properties;

namespace TestWinServ.Integration
{
    public class OpenWeatherCaller
    {
        private string _uri;

        public OpenWeatherCaller()
        {
            _uri = string.Format(Settings.Default.URL, Settings.Default.City, Settings.Default.Country, Settings.Default.OWApiID, Settings.Default.Units);
        }

        public ResponseOpenWeather GetWeather()
        {
            ResponseOpenWeather obResponse = null;

            try {
                var client = new RestClient(_uri);
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                obResponse = JsonConvert.DeserializeObject<ResponseOpenWeather>(response.Content);
            }
            catch
            {
                obResponse = null;
            }

            return obResponse;
        }
    }
}
