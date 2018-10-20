using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using MQTTBD.Arquivos;
using MQTTBD.Entidades;

namespace MQTTBD.BancoDeDados
{
    public class MongoDB
    {
        private string conexao;
        private IMongoClient client;
        IMongoDatabase database;
        IMongoCollection<Dado> dados;

        public MongoDB(BD config)
        {
            conexao = $"mongodb://{config.Usuario}:{config.Senha}@{config.Servidor}";
            client = new MongoClient(conexao);
            database = client.GetDatabase(config.BaseDados);
        }

        public bool TesteConexao()
        {
            try
            {
                dados = database.GetCollection<Dado>("MQTTBD");
                return true;
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Conectar ao Servidor MongoDB " + ex.Message, "ERRO");
                return false;
            }
        }
        public void Gravar(Dado d)
        {
            try
            {
                dados.InsertOne(d);
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Gravar ao Servidor MongoDB " + ex.Message, "ERRO");
            }
        }
    }
}
