using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MQTTBD.Arquivos
{
    public class Log
    {
        public static void GravarLog(string messagen, string status)
        {
            var path = Directory.GetCurrentDirectory();
            string LocalLog = $@"{path}\Log";
            if (!Directory.Exists(LocalLog))
            {
                Directory.CreateDirectory(LocalLog);
            }
            string nomeArquivo = $@"{LocalLog}\{DateTime.Now.ToString("dd_MM_yyyy")}.txt";
            StreamWriter writer = new StreamWriter(nomeArquivo, true);
            writer.WriteLine($"{DateTime.Now} {status} {messagen}\n");
            writer.Close();
        }
    }
}
