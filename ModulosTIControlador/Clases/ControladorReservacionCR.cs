using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaDatos.ModuloReservacion;
using ModulosTICapaLogica.Compartido;
using ModulosTICapaLogica.ModuloReservacion;
using System.Data;

namespace ModulosTIControlador.Clases
{
	public class ControladorReservacionCR
	{
		#region Atributos

		private ConexionLDAP _conexionLDAP;
		private ManejoBD _conexionBD;
		private ManejoBDReservacion _reservacionBD;

		#endregion

		#region Contructor

		public ControladorReservacionCR()
		{
			_conexionBD = new ManejoBD();
			_reservacionBD = new ManejoBDReservacion();
			_conexionLDAP = new ConexionLDAP();
		}

		#endregion

		#region Métodos

		/// <summary>
		/// Método que se encarga de obtener todos los lugares existentes
		/// </summary>S
		/// <returns>Retorna una lista de listas de objetos, en donde cada sublista es un lugar de la forma List(List(IdLugar:int, NombreLugar:String
		/// Capacidad:int, Descripcion:String, Encargado:String, Tipo:Boolean)). En caso de fallo retorna null</returns>

		public List<List<object>> obtenerLugares()
		{
			List<Lugar> lugar = _conexionBD.consultarLugares();
			if ((lugar != null) && (lugar.Count != 0))
			{
				List<object> nodo = new List<object>();
				List<List<object>> lugares = new List<List<object>>();
				for (int i = 0; i < lugar.Count; i++)
				{
                    if (!lugar.ElementAt(i).NombreTipoLugar.Equals("Oficina")) // Obtener todos los lugares menos las Oficinas, en un futuro
                    {                                                             // podrían haber más excepciones
                        nodo.Add(lugar.ElementAt(i).IdLugar);
                        nodo.Add(lugar.ElementAt(i).NombreLugar);
                        nodo.Add(lugar.ElementAt(i).Capacidad);
                        nodo.Add(lugar.ElementAt(i).Descripcion);
                        nodo.Add(lugar.ElementAt(i).Encargado);
                        nodo.Add(lugar.ElementAt(i).NombreTipoLugar);
                        nodo.Add(lugar.ElementAt(i).IdTipoLugar);
                        lugares.Add(nodo);
                        nodo = new List<object>();
                    }
				}
				return lugares;
			} // Luego agregar reporte de error sino se encuentran lugares
			else
				return null;
		}

		/// <summary>
		/// Método que se encarga de obtener todas las carreras del sistema
		/// </summary>
		/// <returns>Retorna una lista de listas de objetos, en donde cada sublista es una carrera de la forma List(List(IdCarrera:int,
		/// Acronimo:String)). En caso de fallo retorna null</returns>

		public List<List<object>> obtenerCarreras()
		{
			List<List<object>> resultado = _conexionBD.consultarCarreras();
			return resultado;
		}

		/// <summary>
		/// Método que se encarga de obtener los cursos para una carrera específica
		/// </summary>
		/// <param name="acronimoCarrera">Acrónimo de la carrera sobre la cual se quieren buscar sus cursos</param>
		/// <returns>Retorna una lista de listas de objetos, en donde cada sublista es una carrera de la forma List(List(IdCurso:int,
		/// Código:String, Nombre:String)). En caso de fallo retorna null</returns>

		public List<List<object>> obtenerCursos(string acronimoCarrera)
		{
			List<List<object>> resultado = _conexionBD.consultarCurso(acronimoCarrera);
			return resultado;
		}

		/// <summary>
		/// Método que se encarga de crear una reservación según los datos obtenidos de la interfaz
		/// </summary>
		/// <param name="datosGenerales">Se esperan los datos generales de la reservación en el siguiente orden:
		///								 IdLugar, IdCurso, LoginSolicitante, Descripcion, FechaInicio, FechaFinal,
		///								 HoraInicio, HoraFinal</param>
		/// <param name="dias">Lista de días que se requiere la reservación, únicamente se esperan chars</param>
		/// <param name="login">Login del usuario que esta realizando la operación</param>
		/// <param name="horario">Valor booleano que indica si esta reservación también se toma en cuenta para los Horario</param>
		/// <returns>Retorna null si tuvo éxito. Si existen choques retorna una lista con la información de los choques encontrados.
		/// En caso de error en el primer elemento podría tener una de las siguientes cadenas de caracteres: Error 1 (se dio una excepción
		/// al buscar choques con la reservación que se quiere insertar), Error 2 (se dio una excepción al insertar la reservación),
		/// Error 3 (se dio un error al insertar una asignación) o Error 4 (se dio una excepción al insertar una asignación).</returns>

		public List<List<object>> crearReservacion(List<string> datosGenerales, List<char> dias, string login, Boolean horario)
		{
			Reservacion reservacion = new Reservacion();
			reservacion.IdLugar = Convert.ToInt32(datosGenerales.ElementAt(0));
			reservacion.IdCurso = Convert.ToInt32(datosGenerales.ElementAt(1));
			reservacion.LoginSolicitante = datosGenerales.ElementAt(2);
            reservacion.Solicitante = _conexionLDAP.obtenerNombrePersona(datosGenerales.ElementAt(2));//"Pao";//
			if (reservacion.Solicitante != null)
			{
				reservacion.Descripcion = datosGenerales.ElementAt(3);
				reservacion.FechaInicio = Convert.ToDateTime(datosGenerales.ElementAt(4));
				reservacion.FechaFinal = Convert.ToDateTime(datosGenerales.ElementAt(5));
				if (Convert.ToInt32(datosGenerales.ElementAt(6)) < 12)
					reservacion.HoraInicio = Convert.ToDateTime(datosGenerales.ElementAt(6) + ':' + datosGenerales.ElementAt(7));
				else
					reservacion.HoraInicio = Convert.ToDateTime(datosGenerales.ElementAt(6) + ':' + datosGenerales.ElementAt(7) + ":00 PM");
				if (Convert.ToInt32(datosGenerales.ElementAt(8)) < 12)
					reservacion.HoraFinal = Convert.ToDateTime(datosGenerales.ElementAt(8) + ':' + datosGenerales.ElementAt(9) + ":00 AM");
				else
					reservacion.HoraFinal = Convert.ToDateTime(datosGenerales.ElementAt(8) + ':' + datosGenerales.ElementAt(9) + ":00 PM");
				reservacion.Dia = dias;
				List<List<object>> resultadoBD = _reservacionBD.insertarReservacion(reservacion, login, horario);
				if (resultadoBD == null)
					return null; // Tuvo éxito
				else
					return resultadoBD; // Existen choques con la reservación especificada o si hubo una excepción retorna el error
			}
			else // No pudo encontrar 
			{
				List<List<object>> errorLdap = new List<List<object>>();
				List<object> detalle = new List<object>();
				detalle.Add("Error LDAP");
				errorLdap.Add(detalle);
				return null;
			}
		}

        
        /// <summary>
        /// Método que se encarga de modificar una reservacion según los datos obtenidos de la interfaz
        /// </summary>
        /// <param name="datosGenerales">Se esperan los datos generales de la reservación en el siguiente orden:
        ///								 IdLugar, IdCurso, LoginSolicitante, Descripcion, FechaInicio, FechaFinal,
        ///								 HoraInicio, HoraFinal</param>
        /// <param name="dias">Lista de días que se requiere la reservación, únicamente se esperan chars</param>
        /// <param name="login">Login del usuario que esta realizando la operación</param>
        /// <param name="horario">Valor booleano que indica si esta reservación también se toma en cuenta para los Horario</param>
        /// <returns>Retorna null si tuvo éxito. Si existen choques retorna una lista con la información de los choques encontrados.
        /// En caso de error en el primer elemento podría tener una de las siguientes cadenas de caracteres: Error 1 (se dio una excepción
        /// al buscar choques con la reservación que se quiere insertar), Error 2 (se dio una excepción al insertar la reservación),
        /// Error 3 (se dio un error al insertar una asignación) o Error 4 (se dio una excepción al insertar una asignación).</returns>

        public List<List<object>> modificaReservacion(List<List<string>> excepciones,String pkReservacion,List<string> datosGenerales, List<char> dias, string login, Boolean horario)
        {
            Reservacion reservacion = new Reservacion();
            reservacion.IdLugar = Convert.ToInt32(datosGenerales.ElementAt(0));
            reservacion.IdCurso = Convert.ToInt32(datosGenerales.ElementAt(1));
            reservacion.LoginSolicitante = datosGenerales.ElementAt(2);
            reservacion.Solicitante = "Pao";//_conexionLDAP.obtenerNombrePersona(datosGenerales.ElementAt(2));//"Pao";//
            if (reservacion.Solicitante != null)
            {
                reservacion.Descripcion = datosGenerales.ElementAt(3);
                reservacion.FechaInicio = Convert.ToDateTime(datosGenerales.ElementAt(4));
                reservacion.FechaFinal = Convert.ToDateTime(datosGenerales.ElementAt(5));
                if (Convert.ToInt32(datosGenerales.ElementAt(6)) < 12)
                    reservacion.HoraInicio = Convert.ToDateTime(datosGenerales.ElementAt(6) + ':' + datosGenerales.ElementAt(7));
                else
                    reservacion.HoraInicio = Convert.ToDateTime(datosGenerales.ElementAt(6) + ':' + datosGenerales.ElementAt(7) + ":00 PM");
                if (Convert.ToInt32(datosGenerales.ElementAt(8)) < 12)
                    reservacion.HoraFinal = Convert.ToDateTime(datosGenerales.ElementAt(8) + ':' + datosGenerales.ElementAt(9) + ":00 AM");
                else
                    reservacion.HoraFinal = Convert.ToDateTime(datosGenerales.ElementAt(8) + ':' + datosGenerales.ElementAt(9) + ":00 PM");
                reservacion.Dia = dias;
                List<List<object>> resultadoBD = _reservacionBD.modificaReservacion(excepciones,pkReservacion,reservacion, login, horario);
                if (resultadoBD == null)
                    return null; // Tuvo éxito
                else
                    return resultadoBD; // Existen choques con la reservación especificada o si hubo una excepción retorna el error
            }
            else // No pudo encontrar 
            {
                List<List<object>> errorLdap = new List<List<object>>();
                List<object> detalle = new List<object>();
                detalle.Add("Error LDAP");
                errorLdap.Add(detalle);
                return null;
            }
        }

		/// <summary>
		/// Método que se encarga de retornar los días que pertenecen a una reservación
		/// </summary>
		/// <param name="pkReservacion">Id de la reservación que se quiere consultar sus días</param>
		/// <returns>Retorna un arreglo de char con los días</returns>

		public List<char> obtenerDiasReservacion(int pkReservacion)
		{
			List<char> diasChoque = _reservacionBD.obtenerDiasReservacion(pkReservacion);
			return diasChoque;
		}

		/// <summary>
		/// Método que se encarga de insertar los errores que se produscan en el sistema en la tabla BitacoraError
		/// </summary>
		/// <param name="descripcionSis">Descripción del error provista por el sistema</param>
		/// <param name="descripcionUser">Descripción del error provista por el usuario</param>

		public void insertarBitacoraError(string descripcionSis, string descripcionUser)
		{
			_conexionBD.insertarBitacoraError(descripcionSis, descripcionUser);
		}

        /// <summary>
        /// Método que se encarga de consultar las reservaciones en un período específico
        /// </summary>
        /// <param name="fechaInicio">Fecha de Inicio a consultar</param>
        /// <param name="fechaFin">Fecha de Fin a consultar</param>
        /// <param name="lugar">Id del lugar que se desea verificar la reservación</param>


        public DataTable consultarReservacion(string lugar, string fechaInicio, string fechaFin)
        {
            return _reservacionBD.consultarReservacion(lugar, fechaInicio, fechaFin); ;
        }



        /// <summary>
        /// Método que se encarga de cancelar una reservacion específica
        /// </summary>
        /// <param name="pkReservacion">Id de la reservacion a cancelar</param>
        /// <returns>Retorna una lista de lista de objetos

        public String cancelarReservacion(String pkReservacion,String Usuario)
        {
            return _reservacionBD.cancelarReservacion(pkReservacion, Usuario);
        }

        /// <summary>
        /// Método que se encarga de obtener los datos de todas las reservaciones en el sistema
        /// </summary>
        /// <returns>Retorna un DataTable con los datos o null en caso de error</returns>

        public DataTable consultarTodasReservaciones()
        {
            return _reservacionBD.consultarTodasReservaciones();
        }
		#endregion
	}
}
