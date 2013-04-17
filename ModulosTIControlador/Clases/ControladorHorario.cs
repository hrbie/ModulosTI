using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModulosTICapaDatos.ModuloHorario;
using System.Data;
using ModulosTICapaLogica.ModuloHorario;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaLogica.Compartido;

namespace ModulosTIControlador.Clases
{
    public class ControladorHorario
    {
        #region Atributos
        private ManejoBDHorario _conexionBDHorario;
        private ManejoBD _conexionBD;
        #endregion

        #region Contructor

        public ControladorHorario()
        {
            _conexionBDHorario = new ManejoBDHorario();
            _conexionBD = new ManejoBD();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que se encarga de ver si existe un horario de disponibiliad habilitado
        /// </summary>
        /// <returns>Retorna el PK del horario o un 0 si no hay ninguno habilitado</returns>
        public int consultarHorarioDisponibilidad()
        {
            return _conexionBDHorario.ConsultarHorarioDisponibilidad();
        }

        /// <summary>
        /// Método que se encarga de obtener el PK del horario de disponibilidad del semestre actual una vez deshabilitado el mismo
        /// </summary>
        /// <returns>Retorna un valor entero con el valor del PK o -1 en caso de error</returns>

        public int consultarHorarioDisDeshabilitado()
        {
            return _conexionBDHorario.consultarHorarioDisDeshabilitado();
        }

        /// <summary>
        /// Método que se encarga de debolver todos los turnos existen en un horario de disponibilidad
        /// </summary>
        /// <param name="idHorario">PK del horario de disponibilidad</param>
        /// <returns>Lista de listas de objetos, las listas contienen (Dia, HoraInicio, Nombre, Login)</returns>
        public List<List<object>> obtenerTurnosDisponibilidad(int idHorario)
        {
            return _conexionBDHorario.obtenerTurnosDisponibilidad(idHorario);
        }

        /// <summary>
        /// Método que se encarga de guardar los turnos de disponibilidad
        /// </summary>
        /// <param name="turnos">Lista de listas de objetos que contiene los turnos que se han ingresado</param>
        /// <returns>Ture si se guardaron con exito, de lo contrario false</returns>
        public Boolean insertarTurnosDisponibilidad(List<List<object>> turnos)
        {
            bool resultado = true;
            try
            {
                // Eliminar los turnos de disponibilidad que el usuario pudiera tener antes
                _conexionBDHorario.eliminarTurnosDisponibilidad(Convert.ToInt32(turnos[0][0]), Convert.ToString(turnos[0][5]));
                //Insertar los turnos
                foreach (List<object> turno in turnos)
                    resultado = resultado && _conexionBDHorario.insertarTurnoDisponibilidad(turno);    //Inserta el turno y va verificando que se inserto bien
            }
            catch (Exception ex)
            {
                _conexionBD.insertarBitacoraError(ex.ToString(), "");
            }
            return resultado;
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
		/// Método que se encarga de obtener todos los lugares
		/// </summary>
		/// <returns>Una lista de listas de Strings, con el formato ((IdLugar, NombreLugar, NombreTipoLugar))</returns>

		public List<List<String>> consultarLugares()
		{
			List<List<String>> listaLugares = new List<List<string>>();
			List<String> nodo;
			List<Lugar> listaObtenida = _conexionBD.consultarLugares();
			for (int i = 0; i < listaObtenida.Count; i++)
			{
				nodo = new List<String>();
				nodo.Add(listaObtenida.ElementAt(i).IdLugar.ToString());
				nodo.Add(listaObtenida.ElementAt(i).NombreLugar);
				nodo.Add(listaObtenida.ElementAt(i).NombreTipoLugar);
				listaLugares.Add(nodo);
			}
			return listaLugares;
		}

		/// <summary>
		/// Método que se encarga de consultar todos los semestre que existan en el sistema
		/// </summary>
		/// <returns>Una lista de listas de Strings, con el formato ((IdSemestre, NombreSemestre))</returns>

		public List<List<String>> consultarSemestres()
		{
			List<List<String>> listaSemestre = new List<List<string>>();
			List<String> nodo;
			List<Semestre> listaObtenida = _conexionBD.consultarSemestres();
			for (int i = 0; i < listaObtenida.Count; i++)
			{
				nodo = new List<String>();
				nodo.Add(listaObtenida.ElementAt(i).IdSemestre.ToString());
				nodo.Add(listaObtenida.ElementAt(i).NombreSemestre);
				listaSemestre.Add(nodo);
			}
			return listaSemestre;
		}

        /// <summary>
        /// Método que se encarga de crear un nuevo horario de disponibilidad
        /// </summary>
        /// <returns>Retorna un valor booleano, true en caso de éxito, false en caso contrario</returns>

        public Boolean crearHorarioDisponibilidad()
        {
            return _conexionBDHorario.insertarHorarioDisponibilidad();
        }

        /// <summary>
        /// Método que se encarga de deshabilitar el horario de disponibilidad que se encuentre activo
        /// </summary>
        /// <returns>Retorna un valor booleano, true en caso de éxito, false en caso contrario</returns>

        public Boolean deshabilitarHorarioDisponibilidad()
        {
            return _conexionBDHorario.deshabilitarHorarioDisponibilidad();
        }

       /// <summary>
       /// Método que se encarga de enviar los datos para que se cree un nuevo horario
       /// </summary>
       /// <param name="idLugar">Id del lugar al cual pertence el horario</param>
       /// <param name="idSemestre">Id del semestre al cual pertence el horario</param>
       /// <returns>Retorna un valor booleano indicando si la operación tuvo éxito (true) o no (false)</returns>

        public int crearHorario(int idLugar, int idSemestre)
        {
            return _conexionBDHorario.insertarHorario(idLugar, idSemestre);
        }

		/// <summary>
		/// Método que se encarga de consultar si hay horarios de disponibilidad habilitados
		/// </summary>
		/// <returns>Retorna un valor numérico de acuerdo al resultado obtenido (-1: en caso de error, 0: Deshabilitados, 1: Habilitado)</returns>

		public int consultarHorarioActivo()
		{
			return _conexionBDHorario.consultarHorarioActivo();
		}

		/// <summary>
		/// Método que se encarga de consultar los turnos de un horario en específico
		/// </summary>
		/// <param name="idLugar">Id del lugar del que se quiere buscar el semestre</param>
		/// <param name="idSemestre">Id del semestre que se quiere obtener el horario</param>
		/// <returns>Retorna un lista de listas de objetos con la forma ((IdTurno:int, Dia:char, HoraInicio:DateTime, HoraFinal:DateTime, NombrePersona:String))</returns>

		public List<List<Object>> consultarTurnosHorario(int idLugar, int idSemestre)
		{
			List<Turno> listaTurno = _conexionBDHorario.obtenerTurnosHorarios(idSemestre, idLugar);
			List<List<Object>> resultado = new List<List<Object>>();
			List<Object> nodo;
			if (listaTurno != null)
			{
				for (int i = 0; i < listaTurno.Count; i++)
				{
					nodo = new List<Object>();
					nodo.Add(listaTurno.ElementAt(i).IdTurno);
					nodo.Add(listaTurno.ElementAt(i).Dia);
					nodo.Add(listaTurno.ElementAt(i).HoraInicio);
					nodo.Add(listaTurno.ElementAt(i).HoraFinal);
					nodo.Add(listaTurno.ElementAt(i).NombrePersona);
					resultado.Add(nodo);
				}
				return resultado;
			}
			else
				return null;
		}

		/// <summary>
		/// Método que se encarga de modificar un turno dado
		/// </summary>
		/// <param name="idTurno">Id del turno a modificar</param>
		/// <param name="textoTurno">Texto del turno</param>
		/// <returns>Retorna un valor booleano indicando si la operación tuvo éxito (true) o no (false)</returns>

		public Boolean modificarTurno(int idTurno, string textoTurno)
		{
			return _conexionBDHorario.modificarTurno(idTurno, textoTurno);
		}

		/// <summary>
		/// Método que se encarga de borrar un turno
		/// </summary>
		/// <param name="idTurno">Id del turno a borrar</param>
		/// <returns>Retorno una valor booleano indicando el éxito (true) o fracaso (false)</returns>

		public Boolean borrarTurno(int idTurno)
		{
			return _conexionBDHorario.borrarTurno(idTurno);
		}

		/// <summary>
		/// Método que se encarga de enviar la solcitud para la creación de un nuevo turno
		/// </summary>
		/// <param name="dia">Dia del turno</param>
		/// <param name="horaInicio">Hora Inicio del turno</param>
		/// <param name="horaFinal">Hora Final del turno</param>
		/// <param name="textoTurno">Texto del turno</param>
		/// <returns>Retorno una valor booleano indicando el éxito (true) o fracaso (false)</returns>

		public Boolean insertarTurno(char dia, string horaInicio, string horaFinal, string textoTurno, int idHorario)
		{
			String[] horaInicioDividida = horaInicio.Split(':');
			String[] horaFinalDividida = horaFinal.Split(':');
			DateTime horaInicioNueva = new DateTime(2011, 12, 24, Convert.ToInt32(horaInicioDividida[0]), Convert.ToInt32(horaInicioDividida[1]), 0);
			DateTime horaFinalNueva = new DateTime(2011, 12, 24, Convert.ToInt32(horaFinalDividida[0]), Convert.ToInt32(horaFinalDividida[1]), 0);
			Turno nuevoTurno = new Turno();
			nuevoTurno.Dia = dia;
			nuevoTurno.HoraInicio = horaInicioNueva;
			nuevoTurno.HoraFinal = horaFinalNueva;
			nuevoTurno.NombrePersona = textoTurno;
			nuevoTurno.IdHorario = idHorario;
			return _conexionBDHorario.insertarTurno(nuevoTurno);
		}

		/// <summary>
		/// Método que se encarga de encontrar si hay un horario dado
		/// </summary>
		/// <param name="idSemestre">Id del semestre a buscar un horario</param>
		/// <param name="idLugar">Id del lugar que se quiere ver su horario</param>
		/// <returns>Retorna un int con el id del Horario o -1 en caso de error</returns>

		public int consultarHorario(int idLugar, int idSemestre)
		{
			return _conexionBDHorario.obtenerHorario(idSemestre, idLugar);
		}

        /// <summary>
        /// Método que se encarga de obtener el semestre que se encuentre activo (el actual)
        /// </summary>
        /// <returns>Debuelve el PK del semestre activo</returns>
        public int consultarSemestreActivo()
        {
            return _conexionBD.consultarSemestreActivo();
        }

        #endregion
    }
}
