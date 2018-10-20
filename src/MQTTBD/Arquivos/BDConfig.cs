using MQTTBD.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MQTTBD.Arquivos
{
    public class BDConfig
    {
        public static BD LerConfig()
        {
            var path = Directory.GetCurrentDirectory();
            string LocalLog = $@"{path}\Config";
            if (!Directory.Exists(LocalLog))
            {
                Directory.CreateDirectory(LocalLog);
            }
            string nomeArquivo = $@"{LocalLog}\bdconfig.json";
            StreamReader leitor = new StreamReader(nomeArquivo, true);
            string linha = leitor.ReadLine();
            return JsonConvert.DeserializeObject<BD>(linha);
        }
        public static void GravarConfig(BD bd)
        {
            var path = Directory.GetCurrentDirectory();
            string LocalLog = $@"{path}\Config";
            if (!Directory.Exists(LocalLog))
            {
                Directory.CreateDirectory(LocalLog);
            }
            string nomeArquivo = $@"{LocalLog}\bdconfig.json";
            StreamWriter writer = new StreamWriter(nomeArquivo, true);
            string output = JsonConvert.SerializeObject(bd);
            writer.Write("");
            writer.Write(output);
            writer.Close();
        }
    }
}
