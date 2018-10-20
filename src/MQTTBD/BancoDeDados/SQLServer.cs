using MQTTBD.Arquivos;
using MQTTBD.Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace MQTTBD.BancoDeDados
{
    public class SQLServer
    {
        private string conexao;
        private SqlConnection sqlConn;
        public SQLServer(BD config)
        {
            conexao = $"Server={config.Servidor};Database={config.BaseDados};User Id={config.Usuario};Password={config.Senha}; ";
            sqlConn = new SqlConnection(conexao);
        }
        private void abrirConexao()
        {
            sqlConn.Open();
        }
        private void fecharConexao()
        {
            sqlConn.Close();
        }
        public bool TesteConexao()
        {
            try
            {
                abrirConexao();
                string quere = $@"IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'MQTTBD')
                                    BEGIN
                                        create table MQTTBD
                                        (
                                            ID int not null identity(1, 1) primary key,
                                            DataRecebimento datetime not null,
		                                    Topico varchar(100) not null,
		                                    Mensagen varchar(100) not null,
		                                    Retida bit not null
	                                    )
                                    END";
                SqlCommand sqlComm = new SqlCommand(quere, sqlConn);
                sqlComm.ExecuteNonQuery();
                fecharConexao();
                return true;
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Conectar ao Servidor SQL Server " + ex.Message, "ERRO");
                fecharConexao();
                return false;
            }
        }
        public void Gravar(Dado d)
        {
            try
            {
                abrirConexao();
                string quere = $@"INSERT INTO MQTTBD(DataRecebimento,Topico,Mensagen,Retida) VALUES('{d.DataRecebimento.ToString("yyyy-MM-dd HH:mm:ss.fff")}','{d.Topico}','{d.Mensagen}','{d.Retida}');";
                SqlCommand sqlComm = new SqlCommand(quere, sqlConn);
                sqlComm.ExecuteNonQuery();
                fecharConexao();
            }
            catch (Exception ex)
            {
                Log.GravarLog("Não foi possivel Gravar ao Servidor SQL Server " + ex.Message, "ERRO");
                fecharConexao();
            }
        }
    }
}
