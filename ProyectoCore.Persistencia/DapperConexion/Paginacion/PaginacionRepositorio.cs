using Dapper;
using ProyectoCore.Persistencia.DapperConexion.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Persistencia.DapperConexion.Paginacion
{
    public class PaginacionRepositorio : IPaginacion
    {
        private readonly IFactoryConnection factoryConnection;

        public PaginacionRepositorio(IFactoryConnection factoryConnection)
        {
            this.factoryConnection = factoryConnection;
        }

        public async Task<PaginacionModel> DevolverPaginacion(string NombreProcedimiento, int NumeroPagina, int CantidadElemento, IDictionary<string, object> parametrosFiltro, string ordenamientoColumna)
        {
            PaginacionModel paginacionModel = new();
            int totalRecords = 0;
            int totalPaginas = 0;

            List<IDictionary<string,object>> ListaReporte = null;

            try
            {
                var connection = factoryConnection.GetConnection();

                DynamicParameters parameters = new();

                foreach (var param in parametrosFiltro)
                {
                    parameters.Add("@"+param.Key, param.Value);
                }

                //INPUT:
                parameters.Add("@NumeroPagina", NumeroPagina);
                parameters.Add("@CantidadPagina", CantidadElemento);
                parameters.Add("@ordenamientoColumna", ordenamientoColumna);

                //OUTPUT:
                parameters.Add("@TotalRecords", totalRecords, DbType.Int32, ParameterDirection.Output);
                parameters.Add("@TotalPaginas", totalPaginas, DbType.Int32, ParameterDirection.Output);

                var result = await connection.QueryAsync(NombreProcedimiento, parameters, commandType: CommandType.StoredProcedure);
                ListaReporte = result.Select(x => (IDictionary<string, object>)x).ToList();
                paginacionModel.ListaRecords = ListaReporte;

                paginacionModel.NumeroPagina = parameters.Get<int?>("@TotalPaginas");
                paginacionModel.TotalRecords = parameters.Get<int?>("@TotalRecords");

            }
            catch (Exception)
            {
                throw new Exception("No se pudo ejecutar el procedimiento almacenado");
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
            return paginacionModel;
        }
    }
}
