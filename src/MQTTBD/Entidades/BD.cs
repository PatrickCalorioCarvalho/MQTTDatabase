using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTBD.Entidades
{
    public class BD
    {
        public TipoBD Tipo { get; set; }

        public string Servidor{ get; set; }

        public int Porta { get; set; }

        public string BaseDados { get; set; }

        public string Usuario { get; set; }

        public string Senha { get; set; }

    }
    public enum TipoBD
    {
        SQLServer,
        MySQL,
        PostgreSQL,
        MongoDB
    }

}
