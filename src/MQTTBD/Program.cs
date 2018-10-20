using MQTTBD.Arquivos;
using MQTTBD.BancoDeDados;
using MQTTBD.Entidades;
using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static System.Net.Mime.MediaTypeNames;

namespace MQTTBD
{
    class Program
    {
        static SQLServer ss;
        static PostgreSQL pg;
        static MySQL my;
        static MQTT mqtt = new MQTT();
        static BD bd = new BD();
        static BancoDeDados.MongoDB mongo;

        static void Main(string[] args)
        {
            mqtt = MQTTConfig.LerConfig();
            bd = BDConfig.LerConfig();
            switch (bd.Tipo)
            {
                case TipoBD.SQLServer:
                    ServidorSQLServer(bd);
                    break;
                case TipoBD.PostgreSQL:
                    ServidorPostgreSQL(bd);
                    break;
                case TipoBD.MySQL:
                    ServidorMySQL(bd);
                    break;
                case TipoBD.MongoDB:
                    ServidorMongoDB(bd);
                    break;
            }
            ServidorMQTT(mqtt);
        }

        static void ServidorMongoDB(BD bdsqlserver)
        {
            try
            {
                mongo = new BancoDeDados.MongoDB(bdsqlserver);
                if (mongo.TesteConexao())
                {
                    Log.GravarLog("Servidor MongoDB Conectado","OK");
                }
                else
                {
                    System.Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Conectar ao Servidor MongoDB "+ ex.Message, "ERRO");
                System.Environment.Exit(0);
            }
        }

        static void ServidorMySQL(BD bdsqlserver)
        {
            try
            {
                my = new MySQL(bdsqlserver);
                if (my.TesteConexao())
                {
                    Log.GravarLog("Servidor MySQL Conectado", "OK");
                }
                else
                {
                    System.Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Conectar ao Servidor MySQL " + ex.Message, "ERRO");
                System.Environment.Exit(0);
            }
        }

        static void ServidorPostgreSQL(BD bdsqlserver)
        {
            try
            {
                pg = new PostgreSQL(bdsqlserver);
                if (pg.TesteConexao())
                {
                    Log.GravarLog("Servidor PostgreSQL Conectado", "OK");
                }
                else
                {
                    System.Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Conectar ao Servidor PostgreSQL " + ex.Message, "ERRO");
                System.Environment.Exit(0);
            }
        }

        static void ServidorSQLServer(BD bdsqlserver)
        {
            try
            {
                ss = new SQLServer(bdsqlserver);
                if (ss.TesteConexao())
                {
                    Log.GravarLog("Servidor SQLServer Conectado", "OK");
                }
                else
                {
                    System.Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Conectar ao Servidor SQLServer " + ex.Message, "ERRO");
                System.Environment.Exit(0);
            }
        }

        static void ServidorMQTT(MQTT mqtt)
        {
            try
            {
                MqttClient client = new MqttClient(mqtt.Servidor, mqtt.Porta, false, null, null, MqttSslProtocols.None);
                client.Connect(Guid.NewGuid().ToString(), mqtt.Usuario, mqtt.Senha);
                client.Subscribe(mqtt.Topicos, mqtt.QoS);
                client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                Log.GravarLog("Servidor MQTT Conectado","OK");
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Conectar ao Servidor MQTT "+ ex.Message,"ERRO");
                System.Environment.Exit(0);
            }
        }
        public static void GravarEmBanco(Dado dado)
        {
            switch (bd.Tipo)
            {
                case TipoBD.SQLServer:
                    ss.Gravar(dado);
                    break;
                case TipoBD.PostgreSQL:
                    pg.Gravar(dado);
                    break;
                case TipoBD.MySQL:
                    my.Gravar(dado);
                    break;
                case TipoBD.MongoDB:
                    mongo.Gravar(dado);
                    break;
            }
        }
        private static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Dado dado = new Dado {
                DataRecebimento = DateTime.Now,
                Topico = e.Topic,
                Mensagen = Encoding.UTF8.GetString(e.Message),
                Retida = e.Retain
            };
            GravarEmBanco(dado);
        }

    }
}
