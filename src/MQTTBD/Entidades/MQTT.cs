using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTBD.Entidades
{
    public class MQTT
    {
        public string Servidor { get; set; }

        public int Porta { get; set; }

        public string Usuario { get; set; }

        public string Senha { get; set; }

        public string[] Topicos { get; set; }

        public byte[] QoS { get; set; }
    }
}
