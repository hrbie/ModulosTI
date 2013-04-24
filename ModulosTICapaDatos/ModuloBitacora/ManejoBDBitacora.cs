using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaLogica.ModuloBitacora;

namespace ModulosTICapaDatos.ModuloBitacora
{
	public class ManejoBDBitacora
	{
		#region Atributos

		private SqlConnection _conexion; // Manejo de conexión con la BD
        private ManejoBD _BDCompartido;
		#endregion

		#region Constructor

		/// <summary>
		/// Constructor de la clase ManejoBD
		/// Este constructor indica los terminos de la conexión con la Base da Datos
		/// </summary>

		public ManejoBDBitacora()
		{
			ConnectionStringSettings _settings = ConfigurationManager.ConnectionStrings["ModulosTI"]; // Propiedas del string de conexión almacenado en el .config
			_conexion = new SqlConnection(_settings.ConnectionString);
		}

		#endregion

		#region Métodos

		/// <summary>
		/// Este método se encarga de ejecutar el stored procedure que cierra la sesión de una bitácora
		/// </summary>
		/// <param name="idSesion">El id de la sesión de bitácora actual</param>
		/// <param name="login">Login del usuario conectado</param>
		/// <returns>Un valor booleano que indica el resultado de la acción. True si logró cerrar sesión, false en caso contrario</returns>

		public Boolean cerrarSesion(int idSesion, string login)
		{
			Boolean resultado = false;
			SqlCommand cerrar = new SqlCommand("SP_BIT_CerrarSesion", _conexion);
			cerrar.CommandType = CommandType.StoredProcedure;
			SqlParameter[] parametros = new SqlParameter[2];
			parametros[0] = new SqlParameter("@idSesion", SqlDbType.Int);
			parametros[0].Value = idSesion;
			parametros[1] = new SqlParameter("@login", SqlDbType.VarChar);
			parametros[1].Value = login;
			cerrar.Parameters.AddRange(parametros);
			
			try
			{
                if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
				SqlDataReader reader = cerrar.ExecuteReader();
                while (reader.Read())
                {
                    resultado = !(reader.HasRows);
                }
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
		/// Este método se encarga de llamar al stored procedure que se encarga de insertar un nuevo evento en la bitácora
		/// </summary>
		/// <param name="idSesion">El id de la sesión de bitácora actual</param>
		/// <param name="evento">El texto del evento a insertar</param>
		/// <returns>Retorna el id del evento insertado</returns>

		public int insertarEventoBitacora(Evento evento)
		{
			int resultado = 0;
			SqlCommand insertar = new SqlCommand("SP_BIT_InsertarEntrada", _conexion);
			insertar.CommandType = CommandType.StoredProcedure;
			SqlParameter[] parametros = new SqlParameter[4];
			parametros[0] = new SqlParameter("@idSesion", SqlDbType.Int);
			parametros[0].Value = evento.IdSesion;
            parametros[1] = new SqlParameter("@idLugar", SqlDbType.Int);
            parametros[1].Value = evento.IdLugar;
			parametros[2] = new SqlParameter("@evento", SqlDbType.NText);
			parametros[2].Value = evento.Descripcion;
            parametros[3] = new SqlParameter("@login", SqlDbType.VarChar);
            parametros[3].Value =evento.Operador;
			insertar.Parameters.AddRange(parametros);
			
			try
			{
                if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
				SqlDataReader reader = insertar.ExecuteReader();
                while (reader.Read())
                {
                    resultado = reader.GetInt32(0); // Obtiene el id del evento insertado
                }
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
		/// Método que se encarga de llamar al stored procedure que crea una nueva sesión en la bitácora
		/// </summary>
		/// <param name="idLugar">Lugar al cual se le quiere crear una sesión de bitácora nueva</param>
		/// <param name="login">Nombre de usuario que inicia sesión</param>
		/// <param name="fechaInicio">Fecha y hora actual</param>
		/// <returns>Retorna el id de la sesión actual</returns>

		public int insertarSesionBitacora(string login)
		{
			var resultado = 0;
			var insertar = new SqlCommand("SP_BIT_InsertarSesion", _conexion) {CommandType = CommandType.StoredProcedure};
		    var parametros = new SqlParameter[1];
			parametros[0] = new SqlParameter("@login", SqlDbType.VarChar) {Value = login};
		    insertar.Parameters.AddRange(parametros);
			
			try
			{
                if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();

				SqlDataReader reader = insertar.ExecuteReader();
				while(reader.Read())
                {
				    resultado = reader.GetInt32(0); // Obtiene el id de la sesión creada
                }
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
        /// Método que se encarga de llamar al stored procedure que consulta los eventos de la bitácora en una fecha
        /// </summary>
        /// <param name="idLugar">Lugar en donde se registraron los eventos</param>
        /// <param name="fecha">Fecha que se desea consultar</param>
        /// <returns>Retorna una DataTable con las entradas registradas en la bitácora</returns>
        public DataTable ConsultarEntradaPorDia(int idLugar, DateTime fecha)
        {
            var tablaEventos = new DataTable();
            tablaEventos.Columns.Add("PK_Entrada");
            tablaEventos.Columns.Add("Fecha");
            tablaEventos.Columns.Add("Operador");
            tablaEventos.Columns.Add("Evento");
            var consultar = new SqlCommand("SP_BIT_ConsultarEntradaPorDia", _conexion)
                                {CommandType = CommandType.StoredProcedure};
	        var parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@idLugar", SqlDbType.Int) {Value = idLugar};
	        parametros[1] = new SqlParameter("@fecha", SqlDbType.DateTime) {Value = fecha};
	        consultar.Parameters.AddRange(parametros);
            try
            {
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();
                
                    SqlDataReader reader = consultar.ExecuteReader();
                    while (reader.Read()) //Obtiene todos lo eventos, resultado de la busqueda
                    {
                        tablaEventos.Rows.Add(reader.GetInt32(0), reader.GetDateTime(1), reader.GetString(2), reader.GetString(3));
                    }
                    _conexion.Close();
                    return tablaEventos;
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
        /// Método que se encarga de llamar al stored procedure que consulta los eventos de la bitácora en una fecha
        /// </summary>
        /// <param name="idSesion">Sesion en donde se registraron los eventos</param>
        /// <returns>Retorna una DataTable con las entradas registradas en la bitácora en una sesion activa</returns>
        public DataTable ConsultarEntradaPorSesion(int idSesion, int idLugar)
        {
            var tablaEventos = new DataTable();
            tablaEventos.Columns.Add("PK_Entrada");
            tablaEventos.Columns.Add("Fecha");
            tablaEventos.Columns.Add("Operador");
            tablaEventos.Columns.Add("Evento");
            var consultar = new SqlCommand("SP_BIT_ConsultarEntradaPorSesion", _conexion)
                                {CommandType = CommandType.StoredProcedure};
            var parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@idSesion", SqlDbType.Int) {Value = idSesion};
            parametros[1] = new SqlParameter("@idLugar", SqlDbType.Int) { Value = idLugar };
            consultar.Parameters.AddRange(parametros);
            try
            {
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();

                SqlDataReader reader = consultar.ExecuteReader();
                while (reader.Read()) //Obtiene todos lo eventos, resultado de la busqueda
                {
                    tablaEventos.Rows.Add(reader.GetInt32(0), reader.GetDateTime(1), reader.GetString(2), reader.GetString(3));
                }
                _conexion.Close();
                return tablaEventos;
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
	    /// Método que se encarga de llamar al stored procedure que modifica un evento durante una sesion
	    /// </summary>
	    /// <param name="login">Nombre de usuario que inicia sesión</param>
	    /// <param name="idSesion">PK de la sesioni actual</param>
	    /// <param name="idEntrada">PK de la entrada que será modificada</param>
	    /// <param name="evento">Nuevo evento</param>
	    /// <returns>Retorna 0 para error, cualquier otro número como correcto</returns>
	    public int ModificarEvento(string login, int idSesion, int idEntrada, string evento)
        {
            int resultado = 0;
            var insertar = new SqlCommand("SP_BIT_ModificarEvento", _conexion)
                               {CommandType = CommandType.StoredProcedure};
	        var parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("@login", SqlDbType.VarChar) {Value = login};
            parametros[1] = new SqlParameter("@idSesion", SqlDbType.VarChar) { Value = idSesion };
            parametros[2] = new SqlParameter("@idEntrada", SqlDbType.VarChar) { Value = idEntrada };
            parametros[3] = new SqlParameter("@evento", SqlDbType.VarChar) { Value = evento };
	        insertar.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = insertar.ExecuteReader();
                while (reader.Read())
                {
                    resultado = reader.GetInt32(0); // Obtiene el id de la entrada modificada
                }
                _conexion.Close();
            }
            catch (Exception)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
            }
            return resultado;
        }


		#endregion
	}
}
