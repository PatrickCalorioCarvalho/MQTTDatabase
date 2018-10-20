using MQTTBD.Arquivos;
using MQTTBD.Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTBD.BancoDeDados
{
    public class MySQL
    {
        private string conexao;
        private MySqlConnection mysqlConn;
        public MySQL(BD config)
        {
            conexao = $"Persist Security Info=True;Server={ config.Servidor};Database={ config.BaseDados};Uid={ config.Usuario };Pwd={ config.Senha};SslMode=none;";
            mysqlConn = new MySqlConnection(conexao);
        }
        private void abrirConexao()
        {
            mysqlConn.Open();
        }
        private void fecharConexao()
        {
            mysqlConn.Close();
        }
        public bool TesteConexao()
        {
            try
            {
                abrirConexao();
                string quere = $@"create table IF NOT EXISTS MQTTBD
                                (
	                                ID int not null auto_increment primary key,
	                                DataRecebimento datetime not null,
	                                Topico varchar(100) not null,
	                                Mensagen varchar(100) not null,
	                                Retida boolean not null
                                );";
                MySqlCommand command = new MySqlCommand(quere, mysqlConn);
                command.ExecuteNonQuery();
                fecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Conectar ao Servidor MySQL " + ex.Message, "ERRO");
                fecharConexao();
                return false;
            }
        }
        public void Gravar(Dado d)
        {
            try
            {
                abrirConexao();
                string quere = $@"INSERT INTO MQTTBD(DataRecebimento,Topico,Mensagen,Retida) VALUES('{d.DataRecebimento.ToString("yyyy-MM-dd HH:mm:ss")}','{d.Topico}','{d.Mensagen}',{d.Retida});";
                MySqlCommand command = new MySqlCommand(quere, mysqlConn);
                command.ExecuteNonQuery();
                fecharConexao();
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Gravar ao Servidor MySQL " + ex.Message, "ERRO");
                fecharConexao();
            }
        }
    }
}
