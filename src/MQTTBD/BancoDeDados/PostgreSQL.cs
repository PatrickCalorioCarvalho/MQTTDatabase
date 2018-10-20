using MQTTBD.Arquivos;
using MQTTBD.Entidades;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTBD.BancoDeDados
{
    public class PostgreSQL
    {
        private string conexao;
        private NpgsqlConnection pgsqlConn;
        public PostgreSQL(BD config)
        {
           
            conexao = $"Server ={ config.Servidor}; Port ={ config.Porta}; User Id = { config.Usuario }; Password ={ config.Senha}; Database ={ config.BaseDados}";
            pgsqlConn = new NpgsqlConnection(conexao);
        }
        private void abrirConexao()
        {
            pgsqlConn.Open();
        }
        private void fecharConexao()
        {
            pgsqlConn.Close();
        }
        public bool TesteConexao()
        {
            try
            {
                abrirConexao();
                string quere = $@"DO
                                    $do$
                                    BEGIN
                                    IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'mqttbd')
                                    THEN
	                                    create table MQTTBD
	                                    (
		                                    ID serial not null primary key,
		                                    DataRecebimento TIMESTAMP not null,
		                                    Topico varchar(100) not null,
		                                    Mensagen varchar(100) not null,
		                                    Retida bool not null
	                                    );
                                    END IF;
                                    END
                                    $do$";
                NpgsqlCommand command = new NpgsqlCommand(quere, pgsqlConn);
                command.ExecuteNonQuery();
                fecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Conectar ao Servidor PostgreSQL " + ex.Message, "ERRO");
                fecharConexao();
                return false;
            }
        }
        public void Gravar(Dado d)
        {
            try
            {
                abrirConexao();
                string quere = $@"INSERT INTO mqttbd(DataRecebimento,Topico,Mensagen,Retida) VALUES('{d.DataRecebimento.ToString("yyyy-MM-dd HH:mm:ss")}','{d.Topico}','{d.Mensagen}','{d.Retida}');";
                NpgsqlCommand command = new NpgsqlCommand(quere, pgsqlConn);
                command.ExecuteNonQuery();
                fecharConexao();
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Gravar ao Servidor PostgreSQL " + ex.Message, "ERRO");
                fecharConexao();
            }
        }
    }
}
