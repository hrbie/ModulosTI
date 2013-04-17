using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ModulosTICapaLogica.ModuloReservacion;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaLogica.Compartido;


namespace ModulosTICapaDatos.ModuloReservacion
{
	public class ManejoBDReservacion
	{
		#region Atributos

		private SqlConnection _conexion; // Manejo de conexión con la BD
        private ManejoBD _conexionCompartido; // Para manejo de accesos la base de datos para información compartida

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor de la clase ManejoBD
		/// Este constructor indica los terminos de la conexión con la Base da Datos
		/// </summary>

		public ManejoBDReservacion()
		{
			ConnectionStringSettings _settings = ConfigurationManager.ConnectionStrings["ModulosTI"]; // Propiedas del string de conexión almacenado en el .config
			_conexion = new SqlConnection(_settings.ConnectionString);
            _conexionCompartido = new ManejoBD();
		}

		#endregion

		#region Métodos

		/// <summary>
		/// Método que se encarga de llamar al stored procedure que inserta una reservación, una vez verificado que no haya choques
		/// </summary>
		/// <param name="reservacion">Objeto reservación</param>
		/// <param name="usuario">Usuario que esta logueado en el sistema</param>
		/// <param name="horario">Valor booleano que indica si esta reservación también se toma en cuenta para los Horario</param>
		/// <returns>Retorna null si tuvo éxito. Si existen choques retorna una lista con la información de los choques encontrados.
		/// En caso de error en el primer elemento podría tener una de las siguientes cadenas de caracteres: Error 1 (se dio una excepción
		/// al buscar choques con la reservación que se quiere insertar), Error 2 (se dio una excepción al insertar la reservación),
		/// Error 3 (se dio un error al insertar una asignación) o Error 4 (se dio una excepción al insertar una asignación).</returns>

		public List<List<object>> insertarReservacion(Reservacion reservacion, string usuario, Boolean horario)
        {
			int idReservacion = -1; // PK de la reservación que será registrada
			List<List<object>> choques = new List<List<object>>(); // Lista donde se guardan todos los choques encontrados
			List<object> choque; // Sublista que contiene un choque, se guardará en la lista anterior
			SqlCommand comando = new SqlCommand("SP_RES_VerificarChoques", _conexion);
			SqlParameter[] parametros;
			comando.CommandType = CommandType.StoredProcedure;
			for (int i = 0; i < reservacion.Dia.Count; i++) // Verificar que no hay choques en todos los días especificados por el solicitante
			{
				comando.Parameters.Clear();
				parametros = new SqlParameter[6];
				parametros[0] = new SqlParameter("@dia", SqlDbType.Char);
				parametros[0].Value = reservacion.Dia.ElementAt(i);
				parametros[1] = new SqlParameter("@horaInicio", SqlDbType.Time);
				parametros[1].Value = String.Format("{0:HH:mm:ss}", reservacion.HoraInicio);
				parametros[2] = new SqlParameter("@horaFinal", SqlDbType.Time);
				parametros[2].Value = String.Format("{0:HH:mm:ss}", reservacion.HoraFinal);
				parametros[3] = new SqlParameter("@fechaInicio", SqlDbType.Date);
				parametros[3].Value = reservacion.FechaInicio;
				parametros[4] = new SqlParameter("@fechaFinal", SqlDbType.Date);
				parametros[4].Value = reservacion.FechaFinal;
				parametros[5] = new SqlParameter("@idLugar", SqlDbType.Int);
				parametros[5].Value = reservacion.IdLugar;
				comando.Parameters.AddRange(parametros);
				if (_conexion.State == ConnectionState.Closed)
					_conexion.Open();
				try
				{
					SqlDataReader reader = comando.ExecuteReader();
					int reservacionActual = 0; // Obtener el id de la reservación que se esta buscando actualmente (para choques)
					bool reservacionExiste; // Para saber si ya ha sido insertada
					while (reader.Read())
					{
						if (reader.GetInt32(0) != 1) // Si encuentra choques los guarda en la lista
						{
							choque = new List<object>();
							reservacionActual = reader.GetInt32(1); // Identificador de la reservación (para no llenar con repetidos)
							reservacionExiste = true;
							for (int j = 0; j < choques.Count; j++) // Verificar que el la reservación ya no haya sido reportada
							{
								if (Convert.ToInt32(choques.ElementAt(j).ElementAt(5)) == reservacionActual)
								{
									reservacionExiste = false;
									break;
								}
							}
							if (reservacionExiste)
							{
								choque.Add(reader.GetDateTime(2)); // Fecha Inicio
								choque.Add(reader.GetDateTime(3)); // Fecha Final
								choque.Add(reader.GetTimeSpan(4)); // Hora Inicio
								choque.Add(reader.GetTimeSpan(5)); // Hora Final
								choque.Add(reader.GetString(6)); // Nombre del solicitante con el cual presenta choque
								choque.Add(reservacionActual); // PK de la reservación con la que tiene choque
								choques.Add(choque); // Agregar el choque a la lista de choques encontrados
							}
						}
						else
							break;
					}
					_conexion.Close();
				}
				catch (Exception e)
				{
					if (_conexion.State == ConnectionState.Open)
						_conexion.Close();
					choques = new List<List<object>>();
					choque = new List<object>();
					choque.Add("Error 1");
					choques.Add(choque);
                    _conexionCompartido.insertarBitacoraError(e.ToString(), "");
					return choques; // Reportar que hubo una excepción al buscar choques de la reservación
				}
			}
			if (choques.Count < 1) // Insertar la reservacíón una vez verificado que no hay choques
			{
				comando = new SqlCommand("SP_RES_InsertarReservacion", _conexion);
				comando.CommandType = CommandType.StoredProcedure;
				parametros = new SqlParameter[8];
				parametros[0] = new SqlParameter("@fechaInicio", SqlDbType.Date);
				parametros[0].Value = reservacion.FechaInicio;
				parametros[1] = new SqlParameter("@fechaFinal", SqlDbType.Date);
				parametros[1].Value = reservacion.FechaFinal;
				parametros[2] = new SqlParameter("@idLugar", SqlDbType.Int);
				parametros[2].Value = reservacion.IdLugar;
				parametros[3] = new SqlParameter("@idCurso", SqlDbType.Int);
				parametros[3].Value = reservacion.IdCurso;
				parametros[4] = new SqlParameter("@solicitante", SqlDbType.NVarChar);
				parametros[4].Value = reservacion.Solicitante;
				parametros[5] = new SqlParameter("@solicitanteID", SqlDbType.VarChar);
				parametros[5].Value = reservacion.LoginSolicitante;
				parametros[6] = new SqlParameter("@descripcion", SqlDbType.NText);
				parametros[6].Value = reservacion.Descripcion;
				parametros[7] = new SqlParameter("@postBy", SqlDbType.VarChar);
				parametros[7].Value = usuario; // Usuario que esta logueado en el sistema y realiza la reservación
				comando.Parameters.AddRange(parametros);
				if (_conexion.State == ConnectionState.Closed)
					_conexion.Open();
				try
				{
					SqlDataReader reader = comando.ExecuteReader();
					reader.Read();
					idReservacion = Convert.ToInt32(reader.GetDecimal(0)); // Obtiene el PK de la reservación recién insertada
					_conexion.Close();
				}
				catch (Exception e)
				{
					if (_conexion.State == ConnectionState.Open)
						_conexion.Close();
					choques = new List<List<object>>();
					choque = new List<object>();
					choque.Add("Error 2");
					choques.Add(choque);
                    _conexionCompartido.insertarBitacoraError(e.ToString(), "");
					return choques; // Reportar que hubo una excepción al insertar la reservación
				}
				if (idReservacion != -1) // Verificar que se haya obtenido el PK de la reservación
				{
					// Insertar las asignaciones
					comando = new SqlCommand("SP_RES_InsertarAsignacion", _conexion);
					comando.CommandType = CommandType.StoredProcedure;
					for (int i = 0; i < reservacion.Dia.Count; i++) // Insertar las asignaciones para todos los días especificados
					{
						comando.Parameters.Clear();
						parametros = new SqlParameter[5];
						parametros[0] = new SqlParameter("@dia", SqlDbType.Char);
						parametros[0].Value = reservacion.Dia.ElementAt(i);
						parametros[1] = new SqlParameter("@idReservacion", SqlDbType.Int);
						parametros[1].Value = idReservacion;
						parametros[2] = new SqlParameter("@horaInicio", SqlDbType.Time);
						parametros[2].Value = String.Format("{0:HH:mm:ss}", reservacion.HoraInicio);
						parametros[3] = new SqlParameter("@horaFinal", SqlDbType.Time);
						parametros[3].Value = String.Format("{0:HH:mm:ss}", reservacion.HoraFinal);
						parametros[4] = new SqlParameter("@guardarHorario", SqlDbType.Bit);
						parametros[4].Value = horario;
						comando.Parameters.AddRange(parametros);
						if (_conexion.State == ConnectionState.Closed)
							_conexion.Open();
						try
						{
							SqlDataReader reader = comando.ExecuteReader();
							reader.Read();
							if (reader.HasRows) // No se inserto la asignación
							{
								choques = new List<List<object>>();
								choque = new List<object>();
								choque.Add("Error 3");
								choques.Add(choque);
								return choques; // Reportar que hubo un error al insertar una asignación
							}
							_conexion.Close();
						}
						catch (Exception e)
						{
							if (_conexion.State == ConnectionState.Open)
								_conexion.Close();
							choques = new List<List<object>>();
							choque = new List<object>();
							choque.Add("Error 4");
							choques.Add(choque);
                            _conexionCompartido.insertarBitacoraError(e.ToString(), "");
							return choques; // Retornar que hubo una excepción al insertar una asignación
						}
					}
				}
				return null; // Si tuvo éxito
			}
			else // Retorna los choques encontrados
				return choques;
		}

		/// <summary>
		/// Método que se encarga de retornar los días que pertenecen a una reservación
		/// </summary>
		/// <param name="pkReservacion">Id de la reservación que se quiere consultar sus días</param>
		/// <returns>Retorna un arreglo de char con los días</returns>

		public List<char> obtenerDiasReservacion(int pkReservacion)
		{
			List<char> diasChoque = new List<char>();
			SqlCommand comando = new SqlCommand("SP_RES_ConsultarDiasReservacion", _conexion);
			SqlParameter[] parametros = new SqlParameter[1];
			comando.CommandType = CommandType.StoredProcedure;
			parametros[0] = new SqlParameter("@pkReservacion", SqlDbType.Int);
			parametros[0].Value = pkReservacion;
			comando.Parameters.AddRange(parametros);
			if (_conexion.State == ConnectionState.Closed)
				_conexion.Open();
			try
			{
				SqlDataReader reader = comando.ExecuteReader();
				char[] diasEncontrados;
				while (reader.Read())
				{
					diasEncontrados = reader.GetSqlChars(0).Value;
					diasChoque.Add(diasEncontrados[0]);
				}
				_conexion.Close();
				return diasChoque;
			}
			catch (Exception e)
			{
				if (_conexion.State == ConnectionState.Open)
					_conexion.Close();
                _conexionCompartido.insertarBitacoraError(e.ToString(), "");
				return null;
			}
		}

        /***********************************/
        /// <summary>
        /// Método que se encarga de buscar las reservaciones en un periodo especifico
        /// </summary>
        /// <param name="idLugar">Id del laboratorio que se desea consultar</param>
        /// <param name="fechaInicio">Fecha de Inicio del periodo a consultar</param>
        /// <param name="fechaFin">Fecha Fin del periodo a consultar</param>
        /// <returns>Retorna un arreglo de arreglos de string con las reservaciones realizadas en ese periodo</returns>

        public DataTable consultarReservacion(string idLugar, string fechaInicio, string fechaFin)
        {

            SqlCommand consultar = new SqlCommand("SP_RES_ConsultarHorarioReservacion", _conexion);
            consultar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@idLugar", SqlDbType.Int);
            parametros[0].Value = idLugar;
            parametros[1] = new SqlParameter("@fechaInicio", SqlDbType.Date);
            parametros[1].Value = fechaInicio;
            parametros[2] = new SqlParameter("@fechaFin", SqlDbType.Date);
            parametros[2].Value = fechaFin;
            consultar.Parameters.AddRange(parametros);
            DataTable _tablaReportes;
            _tablaReportes = new DataTable("Datos");
            _tablaReportes.Columns.Add(new DataColumn("Dias"));
            _tablaReportes.Columns.Add(new DataColumn("Hora Inicio"));
            _tablaReportes.Columns.Add(new DataColumn("Hora Final"));
            _tablaReportes.Columns.Add(new DataColumn("Fecha Inicio"));
            _tablaReportes.Columns.Add(new DataColumn("Fecha Final"));
            _tablaReportes.Columns.Add(new DataColumn("Solicitante"));
            _tablaReportes.Columns.Add(new DataColumn("Curso"));
            _tablaReportes.Columns.Add(new DataColumn("Descripcion"));
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = consultar.ExecuteReader();
                string dias = "";
                int idReserv = 0; //guarda el id de la reservacion
                List<List<object>> reservaciones = new List<List<object>>();
                List<object> reserva;
                while (reader.Read()) // Obtener todos los lugares del sistema
                {
                    reserva = new List<object>();
                    reserva.Add(reader.GetInt32(0));
                    reserva.Add(reader.GetString(1));
                    reserva.Add(reader.GetString(2));
                    reserva.Add(reader.GetString(3));
                    reserva.Add(reader.GetString(4));
                    reserva.Add(reader.GetString(5));
                    reserva.Add(reader.GetString(6));
                    reserva.Add(reader.GetString(7));
                    reserva.Add(reader.GetString(8));
                    reservaciones.Add(reserva);
               }
                idReserv=(int)reservaciones.ElementAt(0).ElementAt(0);
                int i = 0;
                for (i=0; i < reservaciones.Count; i++)
                {
                    
                    if ((!(idReserv == (int)reservaciones.ElementAt(i).ElementAt(0))))
                    {
                        
                        _tablaReportes.Rows.Add(dias, (string)reservaciones.ElementAt(i-1).ElementAt(1), (string)reservaciones.ElementAt(i-1).ElementAt(2), (string)reservaciones.ElementAt(i-1).ElementAt(3), (string)reservaciones.ElementAt(i-1).ElementAt(4), (string)reservaciones.ElementAt(i-1).ElementAt(5), (string)reservaciones.ElementAt(i-1).ElementAt(6), (string)reservaciones.ElementAt(i-1).ElementAt(7));//, dias);
                        dias = "";
                        dias += (String)reservaciones.ElementAt(i).ElementAt(8)+",";
                    }
                    else 
                    {
                        dias += (String)reservaciones.ElementAt(i).ElementAt(8) + ",";                    
                    }
                    idReserv = (int)reservaciones.ElementAt(i).ElementAt(0);
                    
                }
                i = reservaciones.Count-1;
                _tablaReportes.Rows.Add(dias, (string)reservaciones.ElementAt(i).ElementAt(1), (string)reservaciones.ElementAt(i).ElementAt(2), (string)reservaciones.ElementAt(i).ElementAt(3), (string)reservaciones.ElementAt(i).ElementAt(4), (string)reservaciones.ElementAt(i).ElementAt(5), (string)reservaciones.ElementAt(i).ElementAt(6), (string)reservaciones.ElementAt(i).ElementAt(7));//, dias);




                    _conexion.Close();
                return _tablaReportes;
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _conexionCompartido = new ManejoBD();
                _conexionCompartido.insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }

        /// <summary>
		/// Método que se encarga de llamar al stored procedure que devuelve una consulta de todas las reservaciones actuales
		/// </summary>
		/// <returns>Retorna la tabla que se va a mostrar con todas las reservaciones actuales </returns>
        public DataTable consultarTodasReservaciones()
        {
            SqlCommand consultar = new SqlCommand("SP_RES_ConsultarTodasReservaciones", _conexion);
            consultar.CommandType = CommandType.StoredProcedure;
            DataTable _tablaReportes;
            _tablaReportes = new DataTable("Datos");
            _tablaReportes.Columns.Add(new DataColumn("PKReservacion"));
            _tablaReportes.Columns.Add(new DataColumn("Dias"));
            _tablaReportes.Columns.Add(new DataColumn("Hora_Inicio"));
            _tablaReportes.Columns.Add(new DataColumn("Hora_Final"));
            _tablaReportes.Columns.Add(new DataColumn("Fecha_Inicio"));
            _tablaReportes.Columns.Add(new DataColumn("Fecha_Final"));
            _tablaReportes.Columns.Add(new DataColumn("Solicitante"));
            _tablaReportes.Columns.Add(new DataColumn("Curso"));
            _tablaReportes.Columns.Add(new DataColumn("Descripcion"));
            _tablaReportes.Columns.Add(new DataColumn("Lugar"));
            _tablaReportes.Columns.Add(new DataColumn("Carrera"));
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            
                {
                SqlDataReader reader = consultar.ExecuteReader();
                //byte cont = 0; //Para guardar los dias de la reservacion
                string dias = "";
                int idReserv = 0;
                List<List<object>> reservaciones = new List<List<object>>();
                List<object> reserva;
                while (reader.Read()) // Obtener todos los lugares del sistema
                {
                    reserva = new List<object>();
                    reserva.Add(reader.GetInt32(0));
                    reserva.Add(reader.GetString(1));
                    reserva.Add(reader.GetString(2));
                    reserva.Add(reader.GetString(3));
                    reserva.Add(reader.GetString(4));
                    reserva.Add(reader.GetString(5));
                    reserva.Add(reader.GetString(6));
                    reserva.Add(reader.GetString(7));
                    reserva.Add(reader.GetString(8));
                    reserva.Add(reader.GetString(9));
                    reserva.Add(reader.GetString(10));
                    reservaciones.Add(reserva);
               }
                idReserv=(int)reservaciones.ElementAt(0).ElementAt(0);
                int i = 0;
                for (i=0; i < reservaciones.Count; i++)
                {
                    
                    if ((!(idReserv == (int)reservaciones.ElementAt(i).ElementAt(0))))
                    {
                        //_tablaReportes.Rows.Add(reader.GetInt32(0), reader.GetString(8), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetString(6), reader.GetString(7), reader.GetString(9), reader.GetString(10));
                        _tablaReportes.Rows.Add((int)reservaciones.ElementAt(i-1).ElementAt(0), dias, (string)reservaciones.ElementAt(i-1).ElementAt(1), (string)reservaciones.ElementAt(i-1).ElementAt(2), (string)reservaciones.ElementAt(i-1).ElementAt(3), (string)reservaciones.ElementAt(i-1).ElementAt(4), (string)reservaciones.ElementAt(i-1).ElementAt(5), (string)reservaciones.ElementAt(i-1).ElementAt(6), (string)reservaciones.ElementAt(i-1).ElementAt(7),(string)reservaciones.ElementAt(i-1).ElementAt(9),(string)reservaciones.ElementAt(i-1).ElementAt(10));//, dias);
                        dias = "";
                        dias += (String)reservaciones.ElementAt(i).ElementAt(8)+",";
                    }
                    else 
                    {
                        dias += (String)reservaciones.ElementAt(i).ElementAt(8) + ",";                    
                    }
                    idReserv = (int)reservaciones.ElementAt(i).ElementAt(0);
                    
                }
                i = reservaciones.Count-1;
                _tablaReportes.Rows.Add((int)reservaciones.ElementAt(i-1).ElementAt(0),dias, (string)reservaciones.ElementAt(i).ElementAt(1), (string)reservaciones.ElementAt(i).ElementAt(2), (string)reservaciones.ElementAt(i).ElementAt(3), (string)reservaciones.ElementAt(i).ElementAt(4), (string)reservaciones.ElementAt(i).ElementAt(5), (string)reservaciones.ElementAt(i).ElementAt(6), (string)reservaciones.ElementAt(i).ElementAt(7),(string)reservaciones.ElementAt(i-1).ElementAt(9),(string)reservaciones.ElementAt(i-1).ElementAt(10));//, dias);




                    _conexion.Close();
                return _tablaReportes;
            }
            catch (Exception ex)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _conexionCompartido.insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que modifica una reservación, una vez verificado que no haya choques
        /// </summary>
        /// <param name="pkReservacion">id de la reservacion</param>
        /// <param name="reservacion">Objeto reservación</param>
        /// <param name="usuario">Usuario que esta logueado en el sistema</param>
        /// <param name="horario">Valor booleano que indica si esta reservación también se toma en cuenta para los Horario</param>
        /// <returns>Retorna null si tuvo éxito. Si existen choques retorna una lista con la información de los choques encontrados.
        /// En caso de error en el primer elemento podría tener una de las siguientes cadenas de caracteres: Error 1 (se dio una excepción
        /// al buscar choques con la reservación que se quiere insertar), Error 2 (se dio una excepción al modificar la reservación),
        /// Error 3 (se dio un error al insertar una asignación) o Error 4 (se dio una excepción al insertar una asignación).</returns>

        public List<List<object>> modificaReservacion(List<List<string>>excepciones,String pkReservacion,Reservacion reservacion, string usuario, Boolean horario)
        {
            int idReservacion = -1; // PK de la reservación que será registrada
            List<List<object>> choques = new List<List<object>>(); // Lista donde se guardan todos los choques encontrados
            List<object> choque; // Sublista que contiene un choque, se guardará en la lista anterior
            SqlCommand comando = new SqlCommand("SP_RES_VerificarChoquesModificar", _conexion);
            SqlParameter[] parametros;
            comando.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < reservacion.Dia.Count; i++) // Verificar que no hay choques en todos los días especificados por el solicitante
            {
                comando.Parameters.Clear();
                parametros = new SqlParameter[10];
                parametros[0] = new SqlParameter("@dia", SqlDbType.Char);
                parametros[0].Value = reservacion.Dia.ElementAt(i);
                parametros[1] = new SqlParameter("@horaInicio", SqlDbType.Time);
                parametros[1].Value = String.Format("{0:HH:mm:ss}", reservacion.HoraInicio);
                parametros[2] = new SqlParameter("@horaFinal", SqlDbType.Time);
                parametros[2].Value = String.Format("{0:HH:mm:ss}", reservacion.HoraFinal);
                parametros[3] = new SqlParameter("@fechaInicio", SqlDbType.Date);
                parametros[3].Value = reservacion.FechaInicio;
                parametros[4] = new SqlParameter("@fechaFinal", SqlDbType.Date);
                parametros[4].Value = reservacion.FechaFinal;
                parametros[5] = new SqlParameter("@idLugar", SqlDbType.Int);
                parametros[5].Value = reservacion.IdLugar;
                parametros[6] = new SqlParameter("@pkReservacion", SqlDbType.Int);
                parametros[6].Value = pkReservacion;


                DateTime d = new DateTime();
                parametros[7] = new SqlParameter("@fechaExcepcion", SqlDbType.Date);
                parametros[7].Value = d;


                parametros[8] = new SqlParameter("@diaExcepcion", SqlDbType.VarChar);
                parametros[8].Value = excepciones.ElementAt(0).ElementAt(0);

                parametros[9] = new SqlParameter("@Motivo", SqlDbType.VarChar);
                parametros[9].Value = excepciones.ElementAt(0).ElementAt(0);


                comando.Parameters.AddRange(parametros);
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();
                try
                {
                    SqlDataReader reader = comando.ExecuteReader();
                    int reservacionActual = 0; // Obtener el id de la reservación que se esta buscando actualmente (para choques)
                    bool reservacionExiste; // Para saber si ya ha sido insertada
                    while (reader.Read())
                    {
                        if (reader.GetInt32(0) != 1) // Si encuentra choques los guarda en la lista
                        {
                            choque = new List<object>();
                            reservacionActual = reader.GetInt32(1); // Identificador de la reservación (para no llenar con repetidos)
                            reservacionExiste = true;
                            for (int j = 0; j < choques.Count; j++) // Verificar que el la reservación ya no haya sido reportada
                            {
                                if (Convert.ToInt32(choques.ElementAt(j).ElementAt(5)) == reservacionActual)
                                {
                                    reservacionExiste = false;
                                    break;
                                }
                            }
                            if (reservacionExiste)
                            {
                                choque.Add(reader.GetDateTime(2)); // Fecha Inicio
                                choque.Add(reader.GetDateTime(3)); // Fecha Final
                                choque.Add(reader.GetTimeSpan(4)); // Hora Inicio
                                choque.Add(reader.GetTimeSpan(5)); // Hora Final
                                choque.Add(reader.GetString(6)); // Nombre del solicitante con el cual presenta choque
                                choque.Add(reservacionActual); // PK de la reservación con la que tiene choque
                                choques.Add(choque); // Agregar el choque a la lista de choques encontrados
                            }
                        }
                        else
                            break;
                    }
                    _conexion.Close();
                }
                catch (Exception e)
                {
                    if (_conexion.State == ConnectionState.Open)
                        _conexion.Close();
                    choques = new List<List<object>>();
                    choque = new List<object>();
                    choque.Add("Error 1");
                    choques.Add(choque);
                    _conexionCompartido.insertarBitacoraError(e.ToString(), "");
                    return choques; // Reportar que hubo una excepción al buscar choques de la reservación
                }
            }
            if (choques.Count < 1) // Insertar la reservacíón una vez verificado que no hay choques
            {
                comando = new SqlCommand("SP_RES_ModificarReservacion", _conexion);
                comando.CommandType = CommandType.StoredProcedure;
                parametros = new SqlParameter[11];
                parametros[0] = new SqlParameter("@fechaInicio", SqlDbType.Date);
                parametros[0].Value = reservacion.FechaInicio;
                parametros[1] = new SqlParameter("@fechaFinal", SqlDbType.Date);
                parametros[1].Value = reservacion.FechaFinal;
                parametros[2] = new SqlParameter("@idLugar", SqlDbType.Int);
                parametros[2].Value = reservacion.IdLugar;
                parametros[3] = new SqlParameter("@idCurso", SqlDbType.Int);
                parametros[3].Value = reservacion.IdCurso;
                parametros[4] = new SqlParameter("@solicitante", SqlDbType.NVarChar);
                parametros[4].Value = reservacion.Solicitante;
                parametros[5] = new SqlParameter("@solicitanteID", SqlDbType.VarChar);
                parametros[5].Value = reservacion.LoginSolicitante;
                parametros[6] = new SqlParameter("@descripcion", SqlDbType.NText);
                parametros[6].Value = reservacion.Descripcion;
                parametros[7] = new SqlParameter("@horaInicio", SqlDbType.Time);
                parametros[7].Value = String.Format("{0:HH:mm:ss}",reservacion.HoraInicio);
                parametros[8] = new SqlParameter("@horaFinal", SqlDbType.Time);
                parametros[8].Value = String.Format("{0:HH:mm:ss}",reservacion.HoraFinal); 
                parametros[9] = new SqlParameter("@postby", SqlDbType.VarChar);
                parametros[9].Value = usuario; // Usuario que esta logueado en el sistema y realiza la reservación
                parametros[10] = new SqlParameter("@idReservacion", SqlDbType.Int);
                parametros[10].Value = pkReservacion; 
                comando.Parameters.AddRange(parametros);
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();
                try
                {
                    SqlDataReader reader = comando.ExecuteReader();
                    idReservacion = reader.RecordsAffected;
                    //idReservacion = Convert.ToInt32(reader.GetDecimal(0)); // Obtiene el PK de la reservación recién insertada
                    _conexion.Close();
                }
                catch (Exception e)
                {
                    if (_conexion.State == ConnectionState.Open)
                        _conexion.Close();
                    choques = new List<List<object>>();
                    choque = new List<object>();
                    choque.Add("Error 2");
                    choques.Add(choque);
                    _conexionCompartido.insertarBitacoraError(e.ToString(), "");
                    return choques; // Reportar que hubo una excepción al insertar la reservación
                }
                if (idReservacion != 0) // Verificar que se haya modificado la reservacion
                {
                    // Insertar las asignaciones
                    comando = new SqlCommand("SP_RES_InsertarAsignacion", _conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < reservacion.Dia.Count; i++) // Insertar las asignaciones para todos los días especificados
                    {
                        comando.Parameters.Clear();
                        parametros = new SqlParameter[5];
                        parametros[0] = new SqlParameter("@dia", SqlDbType.Char);
                        parametros[0].Value = reservacion.Dia.ElementAt(i);
                        parametros[1] = new SqlParameter("@idReservacion", SqlDbType.Int);
                        parametros[1].Value = pkReservacion;
                        parametros[2] = new SqlParameter("@horaInicio", SqlDbType.Time);
                        parametros[2].Value = String.Format("{0:HH:mm:ss}", reservacion.HoraInicio);
                        parametros[3] = new SqlParameter("@horaFinal", SqlDbType.Time);
                        parametros[3].Value = String.Format("{0:HH:mm:ss}", reservacion.HoraFinal);
                        parametros[4] = new SqlParameter("@guardarHorario", SqlDbType.Bit);
                        parametros[4].Value = horario;
                        comando.Parameters.AddRange(parametros);
                        if (_conexion.State == ConnectionState.Closed)
                            _conexion.Open();
                        try
                        {
                            SqlDataReader reader = comando.ExecuteReader();
                            reader.Read();
                            if (reader.HasRows) // No se inserto la asignación
                            {
                                choques = new List<List<object>>();
                                choque = new List<object>();
                                choque.Add("Error 3");
                                choques.Add(choque);
                                return choques; // Reportar que hubo un error al insertar una asignación
                            }
                            _conexion.Close();
                        }
                        catch (Exception e)
                        {
                            if (_conexion.State == ConnectionState.Open)
                                _conexion.Close();
                            choques = new List<List<object>>();
                            choque = new List<object>();
                            choque.Add("Error 4");
                            choques.Add(choque);
                            _conexionCompartido.insertarBitacoraError(e.ToString(), "");
                            return choques; // Retornar que hubo una excepción al insertar una asignación
                        }
                    }


                    //Insertar Excepcion, solo en caso de que exista

                    comando = new SqlCommand("SP_RES_InsertarExcepcion", _conexion);
                    comando.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < excepciones.Count - 1; i++) // Insertar las asignaciones para todos los días especificados
                    {
                        comando.Parameters.Clear();
                        parametros = new SqlParameter[3];
                      
                        parametros[0] = new SqlParameter("@fecha", SqlDbType.Date);
                        DateTime dt = Convert.ToDateTime(excepciones.ElementAt(i).ElementAt(0)); 
                        parametros[0].Value = dt;
                        parametros[1] = new SqlParameter("@motivo", SqlDbType.NVarChar);
                        parametros[1].Value = excepciones.ElementAt(i).ElementAt(2);
                        parametros[2] = new SqlParameter("@pkReservacion", SqlDbType.Int);
                        parametros[2].Value = pkReservacion;
                        comando.Parameters.AddRange(parametros);
                        if (_conexion.State == ConnectionState.Closed)
                            _conexion.Open();
                        try
                        {
                            SqlDataReader reader = comando.ExecuteReader();
                            reader.Read();
                            return null;
                        }
                        catch (Exception e)
                        {
                            if (_conexion.State == ConnectionState.Open)
                                _conexion.Close();
                            List<List<object>> retorno = new List<List<object>>();
                            List<object> dev = new List<object>();
                            dev.Add("Error al insertar excepcion");
                            retorno.Add(dev);
                            _conexionCompartido.insertarBitacoraError(e.ToString(), "");
                            return retorno; // Retornar que hubo una excepción al insertar una excepcion
                        }
                    }
                }
                return null; // Si tuvo éxito
            }
            else // Retorna los choques encontrados
                return choques;
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que cancela una reservación
        /// </summary>
        /// <param name="pkReservacion">id de la reservacion</param>
        /// <returns>Retorna null si tuvo éxito. Si existen choques retorna una lista con la información de los choques encontrados.
        
        public String cancelarReservacion(String pkReservacion,String Usuario)
        {
            int idReservacion = -1; // PK de la reservación que será registrada
            
            SqlCommand comando = new SqlCommand("SP_RES_CancelarReservacion", _conexion);
            comando.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@pkReservacion", SqlDbType.Int);
            parametros[0].Value = pkReservacion;
            parametros[1] = new SqlParameter("@postby", SqlDbType.NVarChar);
            parametros[1].Value = Usuario;
            comando.Parameters.AddRange(parametros);
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();
                try
                {
                    SqlDataReader reader = comando.ExecuteReader();
                    idReservacion = reader.RecordsAffected;
                    //idReservacion = Convert.ToInt32(reader.GetDecimal(0)); // Obtiene el PK de la reservación recién insertada
                    _conexion.Close();
                    return null; // Si tuvo éxito
                }
                catch (Exception e)
                {
                    if (_conexion.State == ConnectionState.Open)
                        _conexion.Close();
                    _conexionCompartido.insertarBitacoraError(e.ToString(), "");
                    return "Error";
                }
                
        }



		#endregion
	}
}
