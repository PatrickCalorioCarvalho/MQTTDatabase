using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTBD.Entidades
{
    public class Dado
    {
        public DateTime DataRecebimento { get; set; }

        public string Topico { get; set; }

        public string Mensagen { get; set; }

        public bool Retida { get; set; }
    }
}