using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ModulosTICapaDatos.Compartido;
using System.Configuration;
using System.Data;
using ModulosTICapaLogica.ModuloHorario;

namespace ModulosTICapaDatos.ModuloHorario
{
    public class ManejoBDHorario
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

		public ManejoBDHorario()
		{
			ConnectionStringSettings _settings = ConfigurationManager.ConnectionStrings["ModulosTI"]; // Aquí se obtiene el string de conexión a la BD almacenado en el .config
			_conexion = new SqlConnection(_settings.ConnectionString);
		}

		#endregion

        #region Métodos

        /// <summary>
        /// Método que se encarga de ver si existe un horario de disponibiliad habilitado
        /// </summary>
        /// <returns>Retorna el PK del horario o un 0 si no hay ninguno habilitado</returns>
        
        public int ConsultarHorarioDisponibilidad()
        {
            int resultado = 0;
            SqlCommand obtener = new SqlCommand("SP_HOR_ConsultarHorarioDisponibilidad", _conexion);
            obtener.CommandType = CommandType.StoredProcedure;
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = obtener.ExecuteReader();
                while (reader.Read())
                {
                    resultado = reader.GetInt32(0);
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
        /// Método que se encarga de obtener el PK del horario de disponibilidad del semestre actual una vez deshabilitado el mismo
        /// </summary>
        /// <returns>Retorna un valor entero con el valor del PK o -1 en caso de error</returns>

        public int consultarHorarioDisDeshabilitado()
        {
            int resultado = 0;
            SqlCommand obtener = new SqlCommand("SP_HOR_ConsultarHorarioDisponiblidadActual", _conexion);
            obtener.CommandType = CommandType.StoredProcedure;
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = obtener.ExecuteReader();
				reader.Read();
                resultado = reader.GetInt32(0);
                _conexion.Close();
                return resultado;
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
                return -1;
            }
        }

        /// <summary>
        /// Método que se encarga de debolver todos los turnos existen en un horario de disponibilidad
        /// </summary>
        /// <param name="idHorario">PK del horario de disponibilidad</param>
        /// <returns>Lista de listas de objetos, la listas contienen (PKTurnoDisponibilidad, FKHorarioDisponibilidad, Dia, HoraInicio, HorarioFinal, Nombre, Login)</returns>
        public List<List<object>> obtenerTurnosDisponibilidad(int idHorario)
        {
            List<List<object>> resultado = new List<List<object>>();
            List<object> nodo;
            char[] dia;     // Arreglo de chars para obtener el dia en que se encuentra el turno
            SqlCommand obtener = new SqlCommand("SP_HOR_ObtenerTurnosDisponibilidad", _conexion);
            obtener.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idhorario", SqlDbType.Int);
            parametros[0].Value = idHorario;
            obtener.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = obtener.ExecuteReader();
                while (reader.Read())
                {
                    nodo = new List<object>();
                    //nodo.Add(reader.GetInt32(0)); // PK del turno de disponibilidad
                    //nodo.Add(reader.GetInt32(1)); // FK del horario de disponibilidad
                    dia = reader.GetSqlChars(2).Value; // Char del dia
                    nodo.Add(dia[0]);
                    nodo.Add(reader.GetTimeSpan(3)); // Hora inicio
                    //nodo.Add(reader.GetTimeSpan(4)); // Hora final
                    nodo.Add(reader.GetString(5)); // Nombre
                    nodo.Add(reader.GetString(6)); // Login
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
        /// Método que se encarga de insertar un turno de disponibilidad
        /// </summary>
        /// <param name="nuevoTurno">Turno que se va a insertar</param>
        /// <param name="login">Login del operador que registro el turno</param>
        /// <returns>Retorna true si se inserto con exito, de lo contrario retorna false</returns>
        public Boolean insertarTurnoDisponibilidad(List<object> turno)
        {
            Boolean resultado = false;
            SqlCommand insertar = new SqlCommand("SP_HOR_InsertarTurnoDisponibilidad", _conexion);
            insertar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[6];
            parametros[0] = new SqlParameter("@idhorario", SqlDbType.Int);
            parametros[0].Value = Convert.ToInt32(turno[0]);
            parametros[1] = new SqlParameter("@dia", SqlDbType.Char);
            parametros[1].Value = Convert.ToChar(turno[1]);
            parametros[2] = new SqlParameter("@horaInicio", SqlDbType.Time);
            parametros[2].Value = TimeSpan.Parse(turno[2].ToString());
            parametros[3] = new SqlParameter("@horaFinal", SqlDbType.Time);
            parametros[3].Value = TimeSpan.Parse(turno[3].ToString());
            parametros[4] = new SqlParameter("@nombre", SqlDbType.NText);
            parametros[4].Value = turno[4].ToString();
            parametros[5] = new SqlParameter("@login", SqlDbType.VarChar);
            parametros[5].Value = turno[5].ToString();
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
        /// Método que se utiliza para eliminar los turnos que ya tenga un usuario en el horario, esto para que no haya choque con los nuevos que va a ingresar
        /// </summary>
        /// <param name="idHorario">PK del horario de disponibilidad</param>
        /// <param name="login">Login del usuario al que se le van a eliminar los turnos</param>
        public void eliminarTurnosDisponibilidad(int idHorario, string login)
        {
            SqlCommand insertar = new SqlCommand("SP_HOR_EliminarTurnosDisponibilidad", _conexion);
            insertar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@idhorario", SqlDbType.Int);
            parametros[0].Value = idHorario;
            parametros[1] = new SqlParameter("@login", SqlDbType.VarChar);
            parametros[1].Value = login;
            insertar.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = insertar.ExecuteReader();
                reader.Read();
                _conexion.Close();
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que inserta un nuevo turno en el sistema
        /// </summary>
        /// <param name="nuevoTurno">Objeto Turno que contiene la información del turno nuevo</param>
        /// <returns>Retorna un valor booleano indicando si la operación tuvo éxito (true) o no (false)</returns>

        public Boolean insertarTurno(Turno nuevoTurno)
        {
            Boolean resultado;
            SqlCommand insertar = new SqlCommand("SP_HOR_InsertarTurno", _conexion);
            insertar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("@idHorario", SqlDbType.Int);
            parametros[0].Value = nuevoTurno.IdHorario;
            parametros[1] = new SqlParameter("@dia", SqlDbType.Char);
            parametros[1].Value = nuevoTurno.Dia;
            parametros[2] = new SqlParameter("@horaInicio", SqlDbType.Time);
			parametros[2].Value = String.Format("{0:HH:mm:ss}", nuevoTurno.HoraInicio);
            parametros[3] = new SqlParameter("@horaFinal", SqlDbType.Time);
			parametros[3].Value = String.Format("{0:HH:mm:ss}", nuevoTurno.HoraFinal);
            parametros[4] = new SqlParameter("@nombre", SqlDbType.NText);
            parametros[4].Value = nuevoTurno.NombrePersona;
            insertar.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = insertar.ExecuteReader();
                reader.Read();
                resultado = !(reader.HasRows);
                _conexion.Close();
                return resultado;
            }
            catch (Exception ex) // Si algo falla se reporta el error en la Bitácora de Errores del sistema
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
                return false;
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que inserta un nuevo horario de disponibilidad
        /// </summary>
        /// <returns>Retorna un valor booleano indicando si la operación tuvo éxito (true) o no (false)</returns>

        public Boolean insertarHorarioDisponibilidad()
        {
            Boolean resultado;
            SqlCommand insertar = new SqlCommand("SP_HOR_InsertarHorarioDisponibilidad", _conexion);
            insertar.CommandType = CommandType.StoredProcedure;
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = insertar.ExecuteReader();
                reader.Read();
                resultado = !(reader.HasRows);
                _conexion.Close();
                return resultado;
            }
            catch (Exception ex) // Si algo falla se reporta el error en la Bitácora de Errores del sistema
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
                return false;
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que deshabilita un horario de disponibilidad abierto
        /// </summary>
        /// <returns>Retorna un valor booleano indicando si la operación tuvo éxito (true) o no (false)</returns>

        public Boolean deshabilitarHorarioDisponibilidad()
        {
            Boolean resultado;
            SqlCommand actualizar = new SqlCommand("SP_HOR_DeshabilitarHorarioDisponibilidad", _conexion);
            actualizar.CommandType = CommandType.StoredProcedure;
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = actualizar.ExecuteReader();
                reader.Read();
                resultado = !(reader.HasRows);
                _conexion.Close();
                return resultado;
            }
            catch (Exception ex) // Si algo falla se reporta el error en la Bitácora de Errores del sistema
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
                return false;
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que modificar un turno especificado
        /// </summary>
		/// <param name="idTurno">Id del turno a modificar</param>
		/// <param name="textoTurno">Texto del turno</param>
        /// <returns>Retorna un valor booleano indicando si la operación tuvo éxito (true) o no (false)</returns>

        public Boolean modificarTurno(int idTurno, string textoTurno)
        {
            Boolean resultado;
            SqlCommand actualizar = new SqlCommand("SP_HOR_ModificarTurno", _conexion);
            actualizar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@idTurno", SqlDbType.Int);
            parametros[0].Value = idTurno;
            parametros[1] = new SqlParameter("@nombre", SqlDbType.NText);
            parametros[1].Value = textoTurno;
            actualizar.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = actualizar.ExecuteReader();
                reader.Read();
                resultado = !(reader.HasRows);
                _conexion.Close();
                return resultado;
            }
            catch (Exception ex) // Si algo falla se reporta el error en la Bitácora de Errores del sistema
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
                return false;
            }
        }

		/// <summary>
		/// Método que se encarga de llamar al stored procedure que borra un turno
		/// </summary>
		/// <param name="idTurno">Id del turno a borrar</param>
		/// <returns>Retorno una valor booleano indicando el éxito (true) o fracaso (false)</returns>

		public Boolean borrarTurno(int idTurno)
		{
			Boolean resultado;
			SqlCommand borrar = new SqlCommand("SP_HOR_BorrarTurno", _conexion);
			borrar.CommandType = CommandType.StoredProcedure;
			SqlParameter[] parametros = new SqlParameter[1];
			parametros[0] = new SqlParameter("@idTurno", SqlDbType.Int);
			parametros[0].Value = idTurno;
			borrar.Parameters.AddRange(parametros);
			if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
			try
			{
				SqlDataReader reader = borrar.ExecuteReader();
				reader.Read();
				resultado = !(reader.HasRows);
				_conexion.Close();
				return resultado;
			}
			catch (Exception ex) // Si algo falla se reporta el error en la Bitácora de Errores del sistema
			{
				if (_conexion.State == ConnectionState.Open)
					_conexion.Close();
				_BDCompartido = new ManejoBD();
				_BDCompartido.insertarBitacoraError(ex.ToString(), "");
				return false;
			}
		}

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que obtiene los turnos correspondientes a un horario de un semestre dado
        /// </summary>
        /// <param name="idSemestre">Id del semestre a buscar un horario</param>
        /// <param name="idLugar">Id del lugar que se quiere ver su horario</param>
        /// <returns>Retorna una lista de objetos Turno</returns>

        public List<Turno> obtenerTurnosHorarios(int idSemestre, int idLugar)
        {
            List<Turno> resultado = new List<Turno>();
            Turno turno;
            SqlCommand consultar = new SqlCommand("SP_HOR_ConsultarTurnos", _conexion);
            consultar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@idSemestre", SqlDbType.Int);
            parametros[0].Value = idSemestre;
            parametros[1] = new SqlParameter("@idLugar", SqlDbType.Int);
            parametros[1].Value = idLugar;
            consultar.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = consultar.ExecuteReader();
                while (reader.Read())
                {
                    turno = new Turno();
                    turno.IdTurno = reader.GetInt32(0);
                    turno.Dia = reader.GetSqlChars(1).Value[0];
                    turno.HoraInicio = Convert.ToDateTime(reader.GetTimeSpan(2).ToString());
                    turno.HoraFinal = Convert.ToDateTime(reader.GetTimeSpan(3).ToString());
                    turno.NombrePersona = reader.GetString(4);
                    resultado.Add(turno);
                }
                _conexion.Close();
                return resultado;
            }
            catch (Exception ex) // Si algo falla se reporta el error en la Bitácora de Errores del sistema
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que crea un nuevo horario para un lugar y semestre determinado
        /// </summary>
        /// <param name="idLugar">Id del lugar al cual pertence el horario</param>
        /// <param name="idSemestre">Id del semestre al cual pertence el horario</param>
        /// <returns>Retorna un valor booleano indicando si la operación tuvo éxito (true) o no (false)</returns>

        public int insertarHorario(int idLugar, int idSemestre)
        {
            int resultado;
            SqlCommand insertar = new SqlCommand("SP_HOR_InsertarHorario", _conexion);
            insertar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@idLugar", SqlDbType.Int);
            parametros[0].Value = idLugar;
            parametros[1] = new SqlParameter("@idSemestre", SqlDbType.Int);
            parametros[1].Value = idSemestre;
            insertar.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = insertar.ExecuteReader();
                reader.Read();
                resultado = Convert.ToInt32(reader.GetDecimal(0));
                _conexion.Close();
                return resultado;
            }
            catch (Exception ex) // Si algo falla se reporta el error en la Bitácora de Errores del sistema
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _BDCompartido = new ManejoBD();
                _BDCompartido.insertarBitacoraError(ex.ToString(), "");
                return -1;
            }
        }

		/// <summary>
		/// Método que se encarga de consultar el stored procedure que consulta si hay algún horario de disponibilidad habilitado
		/// </summary>
		/// <returns>Retorna un valor numérico de acuerdo al resultado obtenido (-1: en caso de error, 0: Deshabilitados, 1: Habilitado)</returns>

		public int consultarHorarioActivo()
		{
			int resultado = -1;
			SqlCommand consultar = new SqlCommand("SP_HOR_ConsultarHorarioDisActivo", _conexion);
			consultar.CommandType = CommandType.StoredProcedure;
			if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
			try
			{
				SqlDataReader reader = consultar.ExecuteReader();
				reader.Read();
				resultado = reader.GetInt32(0);
				_conexion.Close();
				return resultado;
			}
			catch (Exception ex) // Si algo falla se reporta el error en la Bitácora de Errores del sistema
			{
				if (_conexion.State == ConnectionState.Open)
					_conexion.Close();
				_BDCompartido = new ManejoBD();
				_BDCompartido.insertarBitacoraError(ex.ToString(), "");
				return -1;
			}
		}

		/// <summary>
		/// Método que se encarga de llamar al stored procedure que encuentra si hay un horario dado
		/// </summary>
		/// <param name="idSemestre">Id del semestre a buscar un horario</param>
		/// <param name="idLugar">Id del lugar que se quiere ver su horario</param>
		/// <returns>Retorna un int con el id del Horario o -1 en caso de error</returns>

		public int obtenerHorario(int idSemestre, int idLugar)
		{
			int resultado = -1;
			SqlCommand consultar = new SqlCommand("SP_HOR_ConsultarHorario", _conexion);
			consultar.CommandType = CommandType.StoredProcedure;
			SqlParameter[] parametros = new SqlParameter[2];
			parametros[0] = new SqlParameter("@idLugar", SqlDbType.Int);
			parametros[0].Value = idLugar;
			parametros[1] = new SqlParameter("@idSemestre", SqlDbType.Int);
			parametros[1].Value = idSemestre;
			consultar.Parameters.AddRange(parametros);
			if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
			try
			{
				SqlDataReader reader = consultar.ExecuteReader();
				reader.Read();
				resultado = reader.GetInt32(0);
				_conexion.Close();
				return resultado;
			}
			catch (Exception ex) // Si algo falla se reporta el error en la Bitácora de Errores del sistema
			{
				if (_conexion.State == ConnectionState.Open)
					_conexion.Close();
				_BDCompartido = new ManejoBD();
				_BDCompartido.insertarBitacoraError(ex.ToString(), "");
				return -1;
			}
		}

        #endregion
    }
}
