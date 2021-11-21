using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using ProyectoCore.Persistencia.DapperConexion.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Persistencia.DapperConexion
{
    public class FactoryConnection : IFactoryConnection
    {
        private IDbConnection dbConnection;
        private readonly IOptions<ConexionConfiguracion> configs;

        public FactoryConnection(IOptions<ConexionConfiguracion> configs)
        {
            this.configs = configs;
        }

        public void CloseConnection()
        {
            if (dbConnection != null && dbConnection.State == ConnectionState.Open)
            {
                dbConnection.Close();
            }

        }

        public IDbConnection GetConnection()
        {
            if (dbConnection == null)
            {
                dbConnection = new SqlConnection(configs.Value.DefaultConexion);
            }

            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.Open();
            }

            return dbConnection;
        }
    }
}
