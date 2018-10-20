using MQTTBD.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MQTTBD.Arquivos
{
    public class MQTTConfig
    {
        public static MQTT LerConfig()
        {
            var path = Directory.GetCurrentDirectory();
            string LocalLog = $@"{path}\Config";
            if (!Directory.Exists(LocalLog))
            {
                Directory.CreateDirectory(LocalLog);
            }
            string nomeArquivo = $@"{LocalLog}\mqttconfig.json";
            StreamReader leitor = new StreamReader(nomeArquivo, true);
            string linha = leitor.ReadLine();
            return JsonConvert.DeserializeObject<MQTT>(linha);
        }
        public static void GravarConfig(MQTT mqtt)
        {
            var path = Directory.GetCurrentDirectory();
            string LocalLog = $@"{path}\Config";
            if (!Directory.Exists(LocalLog))
            {
                Directory.CreateDirectory(LocalLog);
            }
            string nomeArquivo = $@"{LocalLog}\mqttconfig.json";
            StreamWriter writer = new StreamWriter(nomeArquivo, true);
            string output = JsonConvert.SerializeObject(mqtt);
            writer.Write("");
            writer.Write(output);
            writer.Close();
        }
    }
}
