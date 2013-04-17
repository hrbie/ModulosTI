using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaLogica.Compartido;
using System.Data;

namespace ModulosTICapaDatos.ModuloSistema
{
    public class ManejoBDSistema
    {
        #region Atributos

		private SqlConnection _conexion; // Manejo de conexión con la BD
        private ManejoBD _conexionBD;
        private ManejoBD _BDCompartido;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor de la clase ManejoBD
		/// Este constructor indica los terminos de la conexión con la Base da Datos
		/// </summary>

		public ManejoBDSistema()
		{
			ConnectionStringSettings _settings = ConfigurationManager.ConnectionStrings["ModulosTI"]; // Propiedas del string de conexión almacenado en el .config
			_conexion = new SqlConnection(_settings.ConnectionString);
            _conexionBD = new ManejoBD();
		}

		#endregion

        #region Métodos

        /// <summary>
        /// Método que se encarga de obtener las entradas de errores que se han dado en la plataforma, pueden o no usarse filtros
        /// </summary>
        /// <param name="fechaInicio">Fecha inicio para buscar eventos</param>
        /// <param name="fechaFinal">Fecha Final para buscar eventos</param>
        /// <param name="estado">Estados de los filtros a buscar</param>
        /// <returns>Retorna una lista de listas de objetos de la forma (PK_Error:int, Fecha:DateTime, DescripciónSis:string, DescripciónUs:string, 
        ///          Estado:int)</returns>

        public List<List<object>> obtenerEntradasBitError(string fechaInicio, string fechaFinal, int estado)
        {
            List<List<object>> resultado = new List<List<object>>();
            List<object> nodo;
            SqlCommand consulta = new SqlCommand("SP_SIS_ConsultarBitacoraError", _conexion);
            consulta.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@fechaInicio", SqlDbType.Date);
            parametros[1] = new SqlParameter("@fechaFinal", SqlDbType.Date);
            parametros[2] = new SqlParameter("@estado", SqlDbType.Int);
            if (!fechaInicio.Equals(""))
            {
                parametros[0].Value = fechaInicio;
                parametros[1].Value = fechaFinal;
            }
            else
            {
                parametros[0].Value = DBNull.Value;
                parametros[1].Value = DBNull.Value;
            }
            if (estado != -1)
                parametros[2].Value = estado;
            else
                parametros[2].Value = DBNull.Value;
            consulta.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = consulta.ExecuteReader();
                while (reader.Read())
                {
                    nodo = new List<object>();
                    nodo.Add(reader.GetInt32(0));  // Obtener el PK
                    nodo.Add(reader.GetDateTime(1)); // Obtener fecha del evento
                    nodo.Add(reader.GetString(2)); // Obtener descripción del sistema del error
                    nodo.Add(reader.GetString(3)); // Obtener descripción del usuario del error
                    nodo.Add(Convert.ToInt32(reader.GetByte(4)));  // Obtener el estado del error
                    resultado.Add(nodo);
                }
                _conexion.Close();
                return resultado;
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _conexionBD = new ManejoBD();
                _conexionBD.insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que modifica una entrada de la tabla BitacoraError
        /// </summary>
        /// <param name="idBitError">PK de la entrada de BitacoraError que se quiere modificar</param>
        /// <param name="estado">Estado al que se quiere actualizar</param>
        /// <returns>Retorna una valor booleano indicando si tuvo éxito (true) o no (false)</returns>

        public Boolean modificarBitacoraError(int idBitError, int estado)
        {
            Boolean resultado = false;
            SqlCommand modificar = new SqlCommand("SP_SIS_ModificarBitError", _conexion);
            modificar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@idBitError", SqlDbType.Int);
            parametros[0].Value = idBitError;
            parametros[1] = new SqlParameter("@estado", SqlDbType.TinyInt);
            parametros[1].Value = estado;
            modificar.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = modificar.ExecuteReader();
                reader.Read();
                resultado = !(reader.HasRows);
                _conexion.Close();
                return resultado;
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _conexionBD = new ManejoBD();
                _conexionBD.insertarBitacoraError(ex.ToString(), "");
                return false;
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que elimina una entrada en la tabla BitacoraError
        /// </summary>
        /// <param name="idBitError">PK de la entrada que se quiere borrar</param>
        /// <returns>Retorna una valor booleano indicando si tuvo éxito (true) o no (false)</returns>

        public Boolean eliminarBitacoraError(int idBitError)
        {
            Boolean resultado = false;
            SqlCommand eliminar = new SqlCommand("SP_SIS_EliminarBitError", _conexion);
            eliminar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idBitacoraError", SqlDbType.Int);
            parametros[0].Value = idBitError;
            eliminar.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = eliminar.ExecuteReader();
                reader.Read();
                resultado = !(reader.HasRows);
                _conexion.Close();
                return resultado;
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _conexionBD = new ManejoBD();
                _conexionBD.insertarBitacoraError(ex.ToString(), "");
                return false;
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que crea un periodo lectivo en el sistema
        /// </summary>
        /// <param name="semestre">Objeto semestre</param>
        /// <returns>Retorna un número entero (1: fue exitoso, 0: fallo, -1: error al intentar crearlo)</returns>

        public int crearSemestre(Semestre semestre)
        {
            SqlCommand insertar = new SqlCommand("SP_SIS_CrearSemestre", _conexion);
            insertar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@nombre", SqlDbType.VarChar);
            parametros[0].Value = semestre.NombreSemestre;
            parametros[1] = new SqlParameter("@fechaInicio", SqlDbType.Date);
            parametros[1].Value = semestre.FechaInicio;
            parametros[2] = new SqlParameter("@fechaFinal", SqlDbType.Date);
            parametros[2].Value = semestre.FechaFinal;
            insertar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();

            try
            {
                
                SqlDataReader reader = insertar.ExecuteReader();
                Boolean resul = !(reader.HasRows);
                if (resul)
                {
                    _conexion.Close();
                    return 1;
                }
                else
                {
                    _conexion.Close();
                    return 0;
                }
            }
            catch (Exception)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                return -1;
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que modifica un periodo lectivo en el sistema
        /// </summary>
        /// <param name="semestre">Objeto semestre</param>
        /// <returns>Retorna una valor booleano indicando si tuvo éxito (true) o no (false)</returns>

        public bool modificarSemestre(Semestre semestre)
        {
            Boolean resultado = false;
            SqlCommand modificar = new SqlCommand("SP_SIS_ModificarSemestre", _conexion);
            modificar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("@id", SqlDbType.Int);
            parametros[0].Value = semestre.IdSemestre;
            parametros[1] = new SqlParameter("@nombre", SqlDbType.VarChar);
            parametros[1].Value = semestre.NombreSemestre;
            parametros[2] = new SqlParameter("@fechaInicio", SqlDbType.Date);
            parametros[2].Value = semestre.FechaInicio;
            parametros[3] = new SqlParameter("@fechaFinal", SqlDbType.Date);
            parametros[3].Value = semestre.FechaFinal;
            parametros[4] = new SqlParameter("@activo", SqlDbType.Bit);
            parametros[4].Value = semestre.Activo;
            modificar.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = modificar.ExecuteReader();
                reader.Read();
                resultado = !(reader.HasRows);
                _conexion.Close();
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
            }
            return resultado;
        }

        /// <summary>
        /// Método que se encarga de verificar la existencia de un periodo lectivo en el sistema
        /// </summary>
        /// <param name="nombre">Nombre del periodo lectivo a verificar</param>
        /// <returns>Retorna una lista de tipo semestre, con los semestres encontrados. Null si falla</returns>

        public List<Semestre> verificarNombreSemestres(string nombre)
        {
            List<Semestre> resultado = new List<Semestre>();
            Semestre semestre;
            SqlCommand buscar = new SqlCommand("SP_SIS_VerificarNombreSemestre", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@nombre", SqlDbType.VarChar);
            parametros[0].Value = nombre;
            buscar.Parameters.AddRange(parametros);
            try
            {
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();
                SqlDataReader reader = buscar.ExecuteReader();
                while (reader.Read())
                {
                    semestre = new Semestre();
                    semestre.IdSemestre = reader.GetInt32(0); // Obtener el PK del semestre
                    resultado.Add(semestre);
                }
                _conexion.Close();
                return resultado;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
