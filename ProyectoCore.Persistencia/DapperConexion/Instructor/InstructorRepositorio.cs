using Dapper;
using ProyectoCore.Persistencia.DapperConexion.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCore.Persistencia.DapperConexion.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConnection factoryConnection;

        public InstructorRepositorio(IFactoryConnection factoryConnection)
        {
            this.factoryConnection = factoryConnection;
        }
        public async Task<int> EditarInstructor(Guid InstructorId, string Nombre, string Apellidos, string Titulos)
        {
            string StoredProcedureName = "usp_instructor_editar";
            int response=0;

            try
            {
                var Connection  = factoryConnection.GetConnection();
                response = await Connection.ExecuteAsync(
                    StoredProcedureName,
                    new {
                        InstructorId = InstructorId,
                        Nombre = Nombre,
                        Apellidos = Apellidos,
                        Titulo = Titulos},
                    commandType:CommandType.StoredProcedure);

                response = Math.Abs(response);

            }

            catch (Exception ex)
            {
                throw new Exception("No se pudo editar la data del instructor", ex);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }

            return response;
        }

        public async Task<int> EliminarInstructor(Guid InstructorId)
        {
            string StoredProcedureName = "usp_instructor_eliminar";
            int resultado = 0;

            try
            {
                var Connection = factoryConnection.GetConnection();
                var response = await Connection.ExecuteAsync(StoredProcedureName,new { InstructorId},commandType:CommandType.StoredProcedure);
                resultado = Math.Abs(response);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar instructor", ex);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
            return resultado;

        }

        public async Task<int> NuevoInstructor(InstructorModel instructor)
        {
            string StoredProcedureName = "usp_instructor_nuevo";
            int response = 0;
            try
            {
                var Connection = factoryConnection.GetConnection();
                
                response = await Connection.ExecuteAsync(StoredProcedureName, new
                {
                    InstructorID = Guid.NewGuid(),
                    Apellidos = instructor.Apellidos,
                    Grado = instructor.Grado,
                    Nombre = instructor.Nombre
                }, commandType: CommandType.StoredProcedure);

                response = Math.Abs(response);

            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo guardar el nuevo instructor",ex);
            }
            finally 
            { 
                factoryConnection.CloseConnection();
            }

            return response;
        }

        public async Task<InstructorModel> ObtenerInstructor(Guid InstructorId)
        {
            string StoredProcedureName = "usp_instructor_by_id";
            InstructorModel instructor = null;

            try
            {
                var Connection = factoryConnection.GetConnection();
                var response = await Connection.QueryAsync<InstructorModel>(StoredProcedureName, new { InstructorId },commandType: CommandType.StoredProcedure);
                instructor = response.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consultar por id", ex);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }
            return instructor;
        }

        public async Task<IList<InstructorModel>> ObtenerLista()
        {
            var InstructorList = new List<InstructorModel>();
            string StoreProcedure = "usp_Obtener_Instructores";

            try
            {
                var connection = factoryConnection.GetConnection();
                var response = await connection.QueryAsync<InstructorModel>(StoreProcedure, null, commandType: CommandType.StoredProcedure);
                InstructorList = response.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la consulta de datos",ex);
            }
            finally
            {
                factoryConnection.CloseConnection();
            }

            return InstructorList;
        }
    }
}
