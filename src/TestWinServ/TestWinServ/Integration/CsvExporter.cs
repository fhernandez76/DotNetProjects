using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TestWinServ.Properties;

namespace TestWinServ.Integration
{
    public class CsvExporter
    {
        public static void Export2Csv(string vsContent)
        {
            try
            {
                byte[] byBuffer = Encoding.ASCII.GetBytes(vsContent);
                string sDirectoryPath = Settings.Default.OutputFile.Substring(0, Settings.Default.OutputFile.LastIndexOf("\\"));
                if (!Directory.Exists(sDirectoryPath))
                    Directory.CreateDirectory(sDirectoryPath);
                FileStream obFS = File.Open(Settings.Default.OutputFile.Insert(Settings.Default.OutputFile.LastIndexOf('.'), "_" + DateTime.Now.ToString("yyyyMMddHHmmss")), FileMode.Append, FileAccess.Write, FileShare.Read);
                obFS.Write(byBuffer, 0, byBuffer.Length);
                obFS.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error log: " + ex.Message);
            }
        }
    }
}
