using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ModulosTIControlador.Clases;
using ModulosTICapaDatos.Compartido;

namespace ModulosTICapaDatos.ModuloPEUL
{
	public class ManejoBDPEUL
	{
		#region Atributos

		private SqlConnection _conexion; // Manejo de conexión con la BD
        private ManejoBD _BDCompartido; // Conexion con el ManejoBD que esta en la carpeta Compartido

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor de la clase ManejoBD
		/// Este constructor indica los terminos de la conexión con la Base da Datos
		/// </summary>

		public ManejoBDPEUL()
		{
			ConnectionStringSettings _settings = ConfigurationManager.ConnectionStrings["ModulosTI"]; // Propiedas del string de conexión almacenado en el .config
			_conexion = new SqlConnection(_settings.ConnectionString);
		}

		#endregion

		#region Métodos

		/// <summary>
		/// Método que se encarga de obtener el porcentaje de uso de actual de cada laboratorio y registrar un uso de laboratorio
		/// </summary>
		/// /// <param name="lugar">Objeto Registro que indica los datos del registro</param>
		/// <returns>Lista de objetos que contiene sublistas con la información de uso de cada laboratorio
		///			 (Laboratorio-Operador-Fecha última actualización-Cantidad de usuarios-Cantidad de portatiles-Porcentaje de uso</returns

		public List<List<object>> registrarPeul(Registro registro)
		{
			List<List<object>> resultado = new List<List<object>>();
			List<object> nodo;
			SqlCommand insertar = new SqlCommand("SP_PEUL_IngresarRegistroUtilizacion", _conexion);
			insertar.CommandType = CommandType.StoredProcedure;
			SqlParameter[] parametros = new SqlParameter[6];
			parametros[0] = new SqlParameter("@idLugar", SqlDbType.Int);
			parametros[0].Value = registro.IdLugar;
			parametros[1] = new SqlParameter("@idEstadoLaboratorio", SqlDbType.Int);
			parametros[1].Value = registro.IdEstadoLaboratorio;
			parametros[2] = new SqlParameter("@cantUsuarios", SqlDbType.Int);
			parametros[2].Value = registro.CantidadUsuarios;
			parametros[3] = new SqlParameter("@cantPortatiles", SqlDbType.Int);
			parametros[3].Value = registro.CantidadPortatiles;
			parametros[4] = new SqlParameter("@comentario", SqlDbType.NText);
			parametros[4].Value = registro.Comentario;
			parametros[5] = new SqlParameter("@login", SqlDbType.VarChar);
            parametros[5].Value = registro.Login;
			insertar.Parameters.AddRange(parametros);
			if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
			try
			{
				SqlDataReader reader = insertar.ExecuteReader();
				while (reader.Read())
				{
                    nodo = new List<object>();
                    nodo.Add(reader.GetString(0)); // Nombre del Laboratorio
                    nodo.Add(reader.GetString(1)); // Operador
                    nodo.Add(reader.GetDateTime(2)); // Fecha última actualización
                    nodo.Add(reader.GetString(3)); // Comentario
                    nodo.Add(reader.GetInt32(4)); // Cantidad de usuarios
                    nodo.Add(reader.GetInt32(5)); // Cantidad de portatiles
                    nodo.Add(reader.GetDecimal(6)); // Porcentaje de uso
                    resultado.Add(nodo);
				}
                _conexion.Close();
			}
			catch (Exception ex)
			{
				if (_conexion.State == ConnectionState.Open)
					_conexion.Close();
                resultado = null;
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
			}
			return resultado;
		}

		/// <summary>
		/// Método que se encarga de insertar un nuevo estado para los laboratorios
		/// </summary>
		/// <param name="descripcion">Nuevo estado a insertar</param>
		/// <returns>Retorna un valor booleano que indica true si la inserción fue exitosa o false en caso contrario</returns>

		public bool insertarEstadoLaboratorio(String descripcion)
		{
			Boolean resultado = false;
			SqlCommand insertar = new SqlCommand("SP_PEUL_IngresarEstadoLaboratorio", _conexion);
			insertar.CommandType = CommandType.StoredProcedure;
			SqlParameter[] parametros = new SqlParameter[1];
			parametros[0] = new SqlParameter("@descripcion", SqlDbType.NText);
			parametros[0].Value = descripcion;
			insertar.Parameters.AddRange(parametros);
			if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
			try
			{
				SqlDataReader reader = insertar.ExecuteReader();
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
		/// Método que se encarga de llamar al stored procedure que modifica un estado de laboratorio
		/// </summary>
		/// <param name="idEstado">id del estado a modificar</param>
		/// <param name="descripcion">Descripción del estado</param>
		/// <returns>Retorna un valor booleano que indica true si la inserción fue exitosa o false en caso contrario</returns>

		public bool modificarEstadoLaboratorio(int idEstado, String descripcion)
		{
			Boolean resultado = false;
			SqlCommand modificar = new SqlCommand("SP_PEUL_ModificarEstadoLaboratorio", _conexion);
			modificar.CommandType = CommandType.StoredProcedure;
			SqlParameter[] parametros = new SqlParameter[2];
			parametros[0] = new SqlParameter("idEstadoLaboratorio", SqlDbType.Int);
			parametros[0].Value = idEstado;
			parametros[1] = new SqlParameter("@descripcion", SqlDbType.NText);
			parametros[1].Value = descripcion;
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
        /// Método que se encarga de obtener los estados en los que puede estar un laboratorio
        /// </summary>
        /// <returns>Lista de listas de objetos, la cual posee los PK del estado y la descripción de este</returns>
        
        public List<List<object>> obtenerEstadoLaboratorio()
        {   
            List<List<object>> resultado = new List<List<object>>();
            List<object> nodo;
            SqlCommand obtener = new SqlCommand("SP_PEUL_ObtenerEstadoLaboratorio", _conexion);
            obtener.CommandType = CommandType.StoredProcedure;            
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = obtener.ExecuteReader();
                while (reader.Read())
                {
                    nodo = new List<object>();
                    nodo.Add(reader.GetInt32(0));  // Obtener el ID
                    nodo.Add(reader.GetString(1)); // Obtener la descripción
                    resultado.Add(nodo);
                }
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
        /// Método que se utiliza para obtener los datos del uso de los laboratorios
        /// </summary>
        /// <returns>Una lista de listas de objetos, la cual contiene (nombre del laboratorio, login del operador, fecha de ultima actualización, comentario,
        /// cantidad de usuarios, cantidad de laptops) </returns>
        
        public List<List<object>> calcularUsoActual()
        {
            List<List<object>> resultado = new List<List<object>>();
            List<object> nodo;
            SqlCommand obtener = new SqlCommand("SP_PEUL_CalcularUsoActualEnTodoLaboratorio", _conexion);
            obtener.CommandType = CommandType.StoredProcedure;
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = obtener.ExecuteReader();
                while (reader.Read())
                {
                    nodo = new List<object>();
                    nodo.Add(reader.GetString(0));      // Obtener el nombre del Laboratorio
                    nodo.Add(reader.GetString(1));      // Obtener el login del operador
                    nodo.Add(reader.GetDateTime(2));    // Obtener fecha de ultima actualización
                    nodo.Add(reader.GetString(3));      // Obtener el comentario
                    nodo.Add(reader.GetInt32(4));       // Obtener la cantidad de usuarios
                    nodo.Add(reader.GetInt32(5));       // Obtener la cantidad de laptops
                    nodo.Add(reader.GetDecimal(6));     // Obtener el porcentaje de uso
                    resultado.Add(nodo);
                }
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
        /// Método que se encarga de llamar al stored procedure que retorna los reportes de registros de uso hechos en el laboratorio especificado según el rango
        /// </summary>
        /// <param name="pkLugar">Identificador del lugar sobrel el cual se realizará la búsqueda</param>
        /// <param name="fechaInicio">Fecha de inicio del rango para la búsqueda</param>
        /// <param name="fechaFinal">Fecha final del rango para la búsqueda</param>
        /// <returns>Retorna un DataTable con los reportes de uso encontrados</returns>

        public DataTable consultarRegistroUso(int pkLugar, string fechaInicio, string fechaFinal)
        {
            SqlCommand consultar = new SqlCommand("SP_PEUL_ConsultarReportesUso", _conexion);
            consultar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@fkHistorialLugar", SqlDbType.Int);
            parametros[0].Value = pkLugar;
            parametros[1] = new SqlParameter("@fechaInicio", SqlDbType.Date);
            parametros[1].Value = fechaInicio;
            parametros[2] = new SqlParameter("@fechaFinal", SqlDbType.Date);
            parametros[2].Value = fechaFinal;
            consultar.Parameters.AddRange(parametros);
            DataTable _tablaReportes;
            _tablaReportes = new DataTable("Datos");
            _tablaReportes.Columns.Add(new DataColumn("Fecha"));
            _tablaReportes.Columns.Add(new DataColumn("Estado"));
            _tablaReportes.Columns.Add(new DataColumn("Operador"));
            _tablaReportes.Columns.Add(new DataColumn("Porcentaje"));
            _tablaReportes.Columns.Add(new DataColumn("Usuarios"));
            _tablaReportes.Columns.Add(new DataColumn("Portatiles"));
            _tablaReportes.Columns.Add(new DataColumn("Comentario"));
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = consultar.ExecuteReader();
                while (reader.Read()) // Obtener todos los lugares del sistema
                    _tablaReportes.Rows.Add(reader.GetDateTime(0), reader.GetString(1), reader.GetString(2), Convert.ToInt32(reader.GetDecimal(3)), reader.GetInt32(4), reader.GetInt32(5), reader.GetString(6));
                _conexion.Close();
                return _tablaReportes;
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que retorna el porcentaje de uso de 
        /// un laboratorio en un rango de fechas
        /// </summary>
        /// <param name="pkLugar">Identificador del lugar sobrel el cual se realizará la búsqueda</param>
        /// <param name="fechaInicio">Fecha de inicio del rango para la búsqueda</param>
        /// <param name="fechaFinal">Fecha final del rango para la búsqueda</param>
        /// <returns>Retorna un DataTable con los reportes de uso encontrados</returns>

        public DataTable consultarPorcentajeUsoAnos(int pkLugar, string fechaInicio, string fechaFinal)
        {
            SqlCommand consultar = new SqlCommand("SP_PEUL_CalcularUsoEnAnos", _conexion);
            consultar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[3];
            
            parametros[0] = new SqlParameter("@fechaInicio", SqlDbType.Date);
            parametros[0].Value = fechaInicio;
            parametros[1] = new SqlParameter("@fechaFinal", SqlDbType.Date);
            parametros[1].Value = fechaFinal;
            parametros[2] = new SqlParameter("@lugar", SqlDbType.Int);
            parametros[2].Value = pkLugar;

            consultar.Parameters.AddRange(parametros);
            DataTable _tablaReportes;
            _tablaReportes = new DataTable("Datos");
            _tablaReportes.Columns.Add(new DataColumn("Periodos"));
            _tablaReportes.Columns.Add(new DataColumn("Usos"));
            _tablaReportes.Columns.Add(new DataColumn("Laptops"));
            
            try
            {
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            
                SqlDataReader reader = consultar.ExecuteReader();
                while (reader.Read()) // Obtener todos los lugares del sistema
                {
                    _tablaReportes.Rows.Add(reader.GetInt32(0), reader.GetDecimal(1), reader.GetDecimal(2));
                }
                    
                _conexion.Close();
                return _tablaReportes;
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }


        /// <summary>
        /// Método que se encarga de llamar al stored procedure que retorna el porcentaje de uso de 
        /// un laboratorio en dias en un rango de fechas
        /// </summary>
        /// <param name="pkLugar">Identificador del lugar sobrel el cual se realizará la búsqueda</param>
        /// <param name="fechaInicio">Fecha de inicio del rango para la búsqueda</param>
        /// <param name="fechaFinal">Fecha final del rango para la búsqueda</param>
        /// <returns>Retorna un DataTable con los reportes de uso encontrados</returns>

        public DataTable consultarPorcentajeUsoDias(int pkLugar, string fechaInicio, string fechaFinal)
        {
            SqlCommand consultar = new SqlCommand("SP_PEUL_CalcularUsoEnDias", _conexion);
            consultar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[3];

            parametros[0] = new SqlParameter("@fechaInicio", SqlDbType.Date);
            parametros[0].Value = fechaInicio;
            parametros[1] = new SqlParameter("@fechaFinal", SqlDbType.Date);
            parametros[1].Value = fechaFinal;
            parametros[2] = new SqlParameter("@lugar", SqlDbType.Int);
            parametros[2].Value = pkLugar;

            consultar.Parameters.AddRange(parametros);
            DataTable _tablaReportes;
            _tablaReportes = new DataTable("Datos");
            _tablaReportes.Columns.Add(new DataColumn("Periodos"));
            _tablaReportes.Columns.Add(new DataColumn("Usos"));
            _tablaReportes.Columns.Add(new DataColumn("Laptops"));

            try
            {
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();

                SqlDataReader reader = consultar.ExecuteReader();
                while (reader.Read()) // Obtener todos los lugares del sistema
                {
                    _tablaReportes.Rows.Add(reader.GetString(0), reader.GetDecimal(1), reader.GetDecimal(2));
                }

                _conexion.Close();
                return _tablaReportes;
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }


        /// <summary>
        /// Método que se encarga de llamar al stored procedure que retorna el porcentaje de uso de 
        /// un laboratorio en dias en un rango de fechas
        /// </summary>
        /// <param name="pkLugar">Identificador del lugar sobrel el cual se realizará la búsqueda</param>
        /// <param name="fechaInicio">Fecha de inicio del rango para la búsqueda</param>
        /// <param name="fechaFinal">Fecha final del rango para la búsqueda</param>
        /// <returns>Retorna un DataTable con los reportes de uso encontrados</returns>

        public DataTable consultarPorcentajeUsoMeses(int pkLugar, string fechaInicio, string fechaFinal)
        {
            SqlCommand consultar = new SqlCommand("SP_PEUL_CalcularUsoEnMeses", _conexion);
            consultar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[3];

            parametros[0] = new SqlParameter("@fechaInicio", SqlDbType.Date);
            parametros[0].Value = fechaInicio;
            parametros[1] = new SqlParameter("@fechaFinal", SqlDbType.Date);
            parametros[1].Value = fechaFinal;
            parametros[2] = new SqlParameter("@lugar", SqlDbType.Int);
            parametros[2].Value = pkLugar;

            consultar.Parameters.AddRange(parametros);
            DataTable _tablaReportes;
            _tablaReportes = new DataTable("Datos");
            _tablaReportes.Columns.Add(new DataColumn("Periodos"));
            _tablaReportes.Columns.Add(new DataColumn("Usos"));
            _tablaReportes.Columns.Add(new DataColumn("Laptops"));

            try
            {
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();

                SqlDataReader reader = consultar.ExecuteReader();
                while (reader.Read()) // Obtener todos los lugares del sistema
                {
                    _tablaReportes.Rows.Add(reader.GetString(1), reader.GetDecimal(2), reader.GetDecimal(3));
                }

                _conexion.Close();
                return _tablaReportes;
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }


        /// <summary>
        /// Método que se encarga de llamar al stored procedure que retorna el porcentaje de uso de 
        /// un laboratorio en semestres en un rango de fechas
        /// </summary>
        /// <param name="pkLugar">Identificador del lugar sobrel el cual se realizará la búsqueda</param>
        /// <param name="fechaInicio">Fecha de inicio del rango para la búsqueda</param>
        /// <param name="fechaFinal">Fecha final del rango para la búsqueda</param>
        /// <returns>Retorna un DataTable con los reportes de uso encontrados</returns>

        public DataTable consultarPorcentajeUsoSemestres(int pkLugar, string fechaInicio, string fechaFinal)
        {
            SqlCommand consultar = new SqlCommand("SP_PEUL_CalcularUsoEnSemestre", _conexion);
            consultar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[3];

            parametros[0] = new SqlParameter("@fechaInicio", SqlDbType.Date);
            parametros[0].Value = fechaInicio;
            parametros[1] = new SqlParameter("@fechaFinal", SqlDbType.Date);
            parametros[1].Value = fechaFinal;
            parametros[2] = new SqlParameter("@lugar", SqlDbType.Int);
            parametros[2].Value = pkLugar;

            consultar.Parameters.AddRange(parametros);
            DataTable _tablaReportes;
            _tablaReportes = new DataTable("Datos");
            _tablaReportes.Columns.Add(new DataColumn("Periodos"));
            _tablaReportes.Columns.Add(new DataColumn("Usos"));
            _tablaReportes.Columns.Add(new DataColumn("Laptops"));

            try
            {
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();

                SqlDataReader reader = consultar.ExecuteReader();
                while (reader.Read()) // Obtener todos los lugares del sistema
                {
                    _tablaReportes.Rows.Add(reader.GetString(0), reader.GetDecimal(1), reader.GetDecimal(2));
                }

                _conexion.Close();
                return _tablaReportes;
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }
		#endregion
	}
}
