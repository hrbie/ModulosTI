using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ModulosTICapaLogica.Compartido;
using ModulosTIControlador.Clases;

namespace ModulosTICapaDatos.Compartido
{
	public class ManejoBD
	{
		#region Atrbutos

		private SqlConnection _conexion; // Manejo de conexión con la BD

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor de la clase ManejoBD
		/// Este constructor indica los terminos de la conexión con la Base da Datos
		/// </summary>

		public ManejoBD()
		{
			ConnectionStringSettings _settings = ConfigurationManager.ConnectionStrings["ModulosTI"]; // Propiedas del string de conexión almacenado en el .config
			_conexion = new SqlConnection(_settings.ConnectionString);
		}

		#endregion  

		#region Métodos

		/// <summary>
		/// Método que se encarga de llamar al stored procedure que crea un nuevo lugar en el sistema
		/// </summary>
		/// <param name="nuevoLugar">Tipo Lugar que contiene los datos del lugar a ser creado</param>
		/// <returns>Retorna un valor booleano que indica true si la inserción fue exitosa o false en caso contrario</returns>

		public bool insertarLugar(Lugar nuevoLugar)
		{
			Boolean resultado = false;
			SqlCommand insertar = new SqlCommand("SP_COM_InsertarLugar", _conexion);
			insertar.CommandType = CommandType.StoredProcedure;
			SqlParameter[] parametros = new SqlParameter[7];
			parametros[0] = new SqlParameter("@nombre", SqlDbType.VarChar);
			parametros[0].Value = nuevoLugar.NombreLugar;
			parametros[1] = new SqlParameter("@capacidad", SqlDbType.Int);
			parametros[1].Value = nuevoLugar.Capacidad;
			parametros[2] = new SqlParameter("@login", SqlDbType.VarChar);
			parametros[2].Value = nuevoLugar.Login;
			parametros[3] = new SqlParameter("@descripcion", SqlDbType.NText);
			parametros[3].Value = nuevoLugar.Descripcion;
			parametros[4] = new SqlParameter("@encargado", SqlDbType.NVarChar);
			parametros[4].Value = nuevoLugar.Encargado;
            parametros[5] = new SqlParameter("@loginEncargado", SqlDbType.VarChar);
            parametros[5].Value = nuevoLugar.LoginEncargado;
			parametros[6] = new SqlParameter("@idTipo", SqlDbType.Int); // El tipo del Lugar es 0 si es Laboratorio, 1 si es Aula
			parametros[6].Value = nuevoLugar.IdTipoLugar;
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
                insertarBitacoraError(ex.ToString(), "");
			}
			return resultado;
		}

		/// <summary>
		/// Método que se encarga de llamar al stored procedure que borra un lugar del sistema
		/// </summary>
		/// <param name="idLugar">id del lugar que se quiere borrar</param>
		/// <returns>Retorna un valor booleano que indica true si la inserción fue exitosa o false en caso contrario</returns>

		public bool borrarLugar(Lugar lugar)
		{
			Boolean resultado = false;
			SqlCommand borrar = new SqlCommand("SP_COM_BorrarLugar", _conexion);
			borrar.CommandType = CommandType.StoredProcedure;
			SqlParameter[] parametros = new SqlParameter[2];
			parametros[0] = new SqlParameter("@idLugar", SqlDbType.Int);
			parametros[0].Value = lugar.IdLugar;
			parametros[1] = new SqlParameter("@login", SqlDbType.VarChar);
			parametros[1].Value = lugar.Login;
			borrar.Parameters.AddRange(parametros);
			if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
			try
			{
				SqlDataReader reader = borrar.ExecuteReader();
				reader.Read();
				resultado = !(reader.HasRows);
				_conexion.Close();
			}
			catch (Exception ex)
			{
				if (_conexion.State == ConnectionState.Open)
					_conexion.Close();
                insertarBitacoraError(ex.ToString(), "");
			}
			return resultado;
		}

		/// <summary>
		/// Método que se encarga de llamar al stored procedure que modifca un Lugar
		/// </summary>
		/// <param name="lugar">Objeto Lugar sobre el cual se quiere hacer el cambio</param>
		/// <returns>Retorna un valor booleano que indica true si la inserción fue exitosa o false en caso contrario</returns>

		public bool modificarLugar(Lugar lugar)
		{
			Boolean resultado = false;
			SqlCommand modificar = new SqlCommand("SP_COM_ModificarLugar", _conexion);
			modificar.CommandType = CommandType.StoredProcedure;
			SqlParameter[] parametros = new SqlParameter[9];
			parametros[0] = new SqlParameter("@idLugar", SqlDbType.Int);
			parametros[0].Value = lugar.IdLugar;
			parametros[1] = new SqlParameter("@login", SqlDbType.VarChar);
			parametros[1].Value = lugar.Login;
			parametros[2] = new SqlParameter("@nombre", SqlDbType.VarChar);
			parametros[2].Value = lugar.NombreLugar;
			parametros[3] = new SqlParameter("@capacidad", SqlDbType.Int);
			parametros[3].Value = lugar.Capacidad;
			parametros[4] = new SqlParameter("@descripcion", SqlDbType.NText);
			parametros[4].Value = lugar.Descripcion;
			parametros[5] = new SqlParameter("@encargado", SqlDbType.NVarChar);
			parametros[5].Value = lugar.Encargado;
            parametros[6] = new SqlParameter("@loginEncargado", SqlDbType.VarChar);
            parametros[6].Value = lugar.LoginEncargado;
			parametros[7] = new SqlParameter("@idTipoLugar", SqlDbType.Int);
			parametros[7].Value = lugar.IdTipoLugar;
            parametros[8] = new SqlParameter("@activo", SqlDbType.Bit);
            parametros[8].Value = lugar.Activo;
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
                insertarBitacoraError(ex.ToString(), "");
			}
			return resultado;
		}

		/// <summary>
		/// Método que se encarga de obtener todos los lugares existentes (sean aulas o laboratorios)
		/// </summary>
		/// <returns>Retorna una lista de tipo Lugar, con todos los lugares encontrados (que estén activos). Null si falla</returns>

		public List<Lugar> consultarLugares()
		{
			List<Lugar> resultado = new List<Lugar>();
			Lugar lugar;
			SqlCommand buscar = new SqlCommand("SP_COM_ConsultarLugares", _conexion);
			buscar.CommandType = CommandType.StoredProcedure;
			if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
			try
			{
				SqlDataReader reader = buscar.ExecuteReader();
				while (reader.Read())
				{
					lugar = new Lugar();
					lugar.IdLugar = reader.GetInt32(0); // Obtener el PK del Lugar
					lugar.NombreLugar = reader.GetString(1); // Obtener nombre del Lugar
					lugar.Capacidad = reader.GetInt32(2); // Obtener capacidad del Lugar
					lugar.Descripcion = reader.GetString(3); // Obtener la descripción de Lugar
					lugar.Encargado = reader.GetString(4); // Obtener el nombre del encargado del Lugar
					lugar.NombreTipoLugar = reader.GetString(5); // Obtener el nombre de tipo del Lugar
					lugar.Activo = reader.GetBoolean(6); // Si el lugar esta habilitado o no
					lugar.IdTipoLugar = reader.GetInt32(7); // Obtiene el id del tipoLugar
					resultado.Add(lugar);
				}
				_conexion.Close();
				return resultado;
			}
			catch (Exception ex)
			{
				if (_conexion.State == ConnectionState.Open)
					_conexion.Close();
                insertarBitacoraError(ex.ToString(), "");
				return null;
			}
		}

		/// <summary>
		/// Método que llama al stored procedure que retorna todas las carreras del sistema
		/// </summary>
		/// <returns></returns>

		public List<List<object>> consultarCarreras()
		{
			List<List<object>> resultado = new List<List<object>>();
			List<object> nodo;
			SqlCommand buscar = new SqlCommand("SP_COM_ConsultarCarreras", _conexion);
			buscar.CommandType = CommandType.StoredProcedure;
			if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
			try
			{
				SqlDataReader reader = buscar.ExecuteReader();
				while (reader.Read())
				{
					nodo = new List<object>();
					nodo.Add(reader.GetInt32(0)); // Obtener el PK de la Carrera
					nodo.Add(reader.GetString(1)); // Obtener el acrónimo de la Carrera
					resultado.Add(nodo);
				}
				_conexion.Close();
				return resultado;
			}
			catch (Exception ex)
			{
				if (_conexion.State == ConnectionState.Open)
					_conexion.Close();
                insertarBitacoraError(ex.ToString(), "");
				return null;
			}
		}

		/// <summary>
		/// Método que se encarga de llamar al stored procedure que obtiene los cursos del sistema de acuerdo a una carrera específica
		/// </summary>
		/// <param name="acronimoCarrera">Acrónimo de la carrera sobre la cual se quieren buscar los cursos</param>
		/// <returns>Retorna una lista de objetos con los cursos (PK_Curso, Código, Nombre)</returns>

		public List<List<object>> consultarCurso(string acronimoCarrera)
		{
			List<List<object>> resultado = new List<List<object>>();
			List<object> nodo;
			SqlCommand buscar = new SqlCommand("SP_COM_ConsultarCursosPorCarrera", _conexion);
			buscar.CommandType = CommandType.StoredProcedure;
			SqlParameter[] parametros = new SqlParameter[1];
			parametros[0] = new SqlParameter("@acronimoCarrera", SqlDbType.VarChar);
			parametros[0].Value = acronimoCarrera;
			buscar.Parameters.AddRange(parametros);
			if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
			try
			{
				SqlDataReader reader = buscar.ExecuteReader();
				while (reader.Read())
				{
					nodo = new List<object>();
					nodo.Add(reader.GetInt32(0)); // Obtener el PK del Curso
					nodo.Add(reader.GetString(1)); // Obtener el código cel Curso
					nodo.Add(reader.GetString(2)); // Obtener el nombre del Curso
					resultado.Add(nodo);
				}
				_conexion.Close();
				return resultado;
			}
			catch (Exception ex)
			{
				if (_conexion.State == ConnectionState.Open)
					_conexion.Close();
                insertarBitacoraError(ex.ToString(), "");
				return null;
			}
		}

		/// <summary>
		/// Método que se encarga de obtener todos los lugares existentes en el sistema
		/// </summary>
		/// <returns>Retorna un DataTable con la información de los lugares, o null en caso de error</returns>

		public DataTable obtenerDatosLugares()
		{
			SqlCommand consultar = new SqlCommand("SP_COM_ConsultarTodosLosLugares", _conexion);
			consultar.CommandType = CommandType.StoredProcedure;
			DataTable _tablaLugares;
            String activo; // Si el lugar esta activo o no
			_tablaLugares = new DataTable("Datos");
			_tablaLugares.Columns.Add(new DataColumn("PKLugar"));
			_tablaLugares.Columns.Add(new DataColumn("Lugar"));
			_tablaLugares.Columns.Add(new DataColumn("Capacidad"));
			_tablaLugares.Columns.Add(new DataColumn("Descripción"));
			_tablaLugares.Columns.Add(new DataColumn("Encargado"));
            _tablaLugares.Columns.Add(new DataColumn("Login"));
			_tablaLugares.Columns.Add(new DataColumn("Tipo"));
            _tablaLugares.Columns.Add(new DataColumn("Estado"));
			if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
			try
			{
				SqlDataReader reader = consultar.ExecuteReader();
				while (reader.Read()) // Obtener todos los lugares del sistema
				{
                    if (reader.GetBoolean(7)) // Si el lugar esta habilitado
                        activo = "Habilitado";
                    else
                        activo = "Inhabilitado"; // Si el lugar esta inhabilitado
                    _tablaLugares.Rows.Add(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), activo);
				}
				_conexion.Close();
				return _tablaLugares;
			}
			catch (Exception ex)
			{
				if (_conexion.State == ConnectionState.Open)
					_conexion.Close();
                insertarBitacoraError(ex.ToString(), "");
				return null;
			}
		}

		/// <summary>
		/// Método que se encarga de llamar al stored procedure que consulta los tipos de lugar del sistema
		/// </summary>
		/// <returns>Lista con los nombres de tipos de lugares con la forma ((PK_TipoLugar, Nombre))</returns>

		public List<List<Object>> consultarTipoLugar()
		{
			List<List<Object>> resultado = new List<List<Object>>();
			List<Object> nodo;
			SqlCommand buscar = new SqlCommand("SP_COM_ConsultarTipoLugar", _conexion);
			buscar.CommandType = CommandType.StoredProcedure;

			try
			{
				if (_conexion.State == ConnectionState.Closed)
					_conexion.Open();
				SqlDataReader reader = buscar.ExecuteReader();
				while (reader.Read())
				{
					nodo = new List<object>();
					nodo.Add(reader.GetInt32(0));
					nodo.Add(reader.GetString(1));
					resultado.Add(nodo);
				}
				_conexion.Close();
				return resultado;
			}
			catch (Exception ex)
			{
				if (_conexion.State == ConnectionState.Open)
					_conexion.Close();
				insertarBitacoraError(ex.ToString(), "");
				return null;
			}
		}

        /// <summary>
        /// Método que se encarga de obtener todos los semestres registrados
        /// </summary>
        /// <returns>Retorna una lista de tipo semestre, con todos los semestres encontrados. Null si falla</returns>

        public List<Semestre> consultarSemestres()
        {
            List<Semestre> resultado = new List<Semestre>();
            Semestre semestre;
            SqlCommand buscar = new SqlCommand("SP_COM_ConsultarSemestres", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            
            try
            {
                if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
                SqlDataReader reader = buscar.ExecuteReader();
                while (reader.Read())
                {
                    semestre = new Semestre();
                    semestre.IdSemestre = reader.GetInt32(0); // Obtener el PK del semestre
                    semestre.NombreSemestre = reader.GetString(1); // Obtener nombre del semestre
                    semestre.FechaInicio = reader.GetDateTime(2); // Obtener fecha de inicio del semestre
                    semestre.FechaFinal = reader.GetDateTime(3); // Obtener fecha de fin del semestre
                    semestre.Activo = reader.GetInt32(4); // Obtener el estadod el semestre
                    resultado.Add(semestre);
                }
                _conexion.Close();
                return resultado;
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }

        /// <summary>
        /// Método que se encarga de insertar los errores que se produscan en el sistema en la tabla BitacoraError
        /// </summary>
        /// <param name="descripcionSis">Descripción del error provista por el sistema</param>
        /// <param name="descripcionUser">Descripción del error provista por el usuario</param>
        /// <returns>Retorna true si se puedo realizar la inserción, de lo contrario debuelve false</returns>
        
        public bool insertarBitacoraError(string descripcionSis, string descripcionUser)
        {            
            Boolean resultado = false;
            SqlCommand modificar = new SqlCommand("SP_COM_InsertarBitacoraError", _conexion);
            modificar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@descripcionSistema", SqlDbType.NText);
            parametros[0].Value = descripcionSis;
            parametros[1] = new SqlParameter("@descripcionUsuario", SqlDbType.NText);
            parametros[1].Value = descripcionUser;
            modificar.Parameters.AddRange(parametros);
            try
            {
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();
                SqlDataReader reader = modificar.ExecuteReader();
                reader.Read();
                resultado = !(reader.HasRows);
                _conexion.Close();
            }
            catch (Exception)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
            }
            return resultado;
        }

        /// <summary>
        /// Método que se encarga de obtener el semestre que se encuentre activo (el actual)
        /// </summary>
        /// <returns>Debuelve el PK del semestre activo</returns>
        public int consultarSemestreActivo()
        {
            int resultado = -1;
            SqlCommand consultar = new SqlCommand("SP_COM_ConsultarSemestreActivo", _conexion);
            consultar.CommandType = CommandType.StoredProcedure;
            try
            {
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();
                SqlDataReader reader = consultar.ExecuteReader();
                if (reader.Read())
                {
                    resultado = reader.GetInt32(0); 
                }
                _conexion.Close();
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                insertarBitacoraError(ex.ToString(), "");
            }
            return resultado;
        }
		#endregion
	}
}
