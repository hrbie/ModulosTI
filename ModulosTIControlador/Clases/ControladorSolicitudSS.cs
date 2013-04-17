using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaDatos.ModuloSolicitudes;
using ModulosTICapaLogica.ModuloSolicitudes;

namespace ModulosTIControlador.Clases
{
    public class ControladorSolicitudSS
    {
        private ManejoBDSolicitudes _manejoBD;
        private ConexionLDAP _ldap;
        private static List<Solicitud> _solicitudes;
        private static List<Avance> _avances;

        public ControladorSolicitudSS(String usuario)
        {
            _manejoBD = new ManejoBDSolicitudes();
            _ldap = new ConexionLDAP();
            _solicitudes = _manejoBD.recuperarSolicitudesSoportista(usuario);
        }

		public ControladorSolicitudSS()
		{
			_manejoBD = new ManejoBDSolicitudes();
			_ldap = new ConexionLDAP();
		}

		/// <summary>
		/// Carga las solicitudes que tiene a cargo un soportista
		/// </summary>
		/// <param name="usuario">login del soportista</param>
		public void cargarSolicitudes(String usuario)
		{
			_solicitudes = _manejoBD.recuperarSolicitudesSoportista(usuario);
		}
		

        /// <summary>
        /// Crea un DataView a partir de las solicitudes del soportista
        /// </summary>
        /// <returns>DataView con el asunto y el id de las solicitudes</returns>

        public DataView crearDataViewSolicitudes()
        {
            //Crea una tabla para guardar los datos del DropDownList _ddlSolicitudes
            DataTable _dtResultado = new DataTable();
            DataView _dvResultado;
            //Define las columnas de la tabla
            _dtResultado.Columns.Add(new DataColumn("id", typeof(String)));
            _dtResultado.Columns.Add(new DataColumn("asunto", typeof(String)));
            try
            {
                DataRow _drSeleccionar = _dtResultado.NewRow();
                _drSeleccionar[0] = "0";
                _drSeleccionar[1] = "Seleccionar";
                _dtResultado.Rows.Add(_drSeleccionar);

                foreach (Solicitud sol in _solicitudes)
                {
                    DataRow _dr = _dtResultado.NewRow();
                    _dr[0] = sol.ID;
                    _dr[1] = sol.Asunto;

                    _dtResultado.Rows.Add(_dr);
                }
                _dvResultado = new DataView(_dtResultado);
                return _dvResultado;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// Crea un DataView de los avances de una solicitud.
        /// </summary>
        /// <param name="idSolicitud">identificador de la solicitud</param>
        /// <returns>DataView con id y fecha de los avance</returns>

        public DataView crearDataViewAvances(int idSolicitud)
        {
            _avances = _manejoBD.recuperarAvances(idSolicitud);

            //Crea una tabla para guardar los datos del DropDownList _ddlSolicitudes
            DataTable _dtResultado = new DataTable();
            DataView _dvResultado;
            //Define las columnas de la tabla
            _dtResultado.Columns.Add(new DataColumn("id", typeof(String)));
            _dtResultado.Columns.Add(new DataColumn("fecha", typeof(String)));
            try
            {
                DataRow _drSeleccionar = _dtResultado.NewRow();
                _drSeleccionar[0] = "0";
                _drSeleccionar[1] = "Seleccionar";
                _dtResultado.Rows.Add(_drSeleccionar);

                foreach (Avance avance in _avances)
                {
                    DataRow _dr = _dtResultado.NewRow();
                    _dr[0] = avance.ID;
                    _dr[1] = avance.fecha;

                    _dtResultado.Rows.Add(_dr);
                }
                _dvResultado = new DataView(_dtResultado);
                return _dvResultado;
            }
            catch (Exception e)
            {
                throw (e);
            }

        }

        /// <summary>
        /// Llama al método recuperarEstados()
        /// </summary>
        /// <returns>DataView con los estados de las solicitudes</returns>

        public DataView crearDataViewEstados()
        {
            return _manejoBD.recuperarEstados();
        }

        /// <summary>
        /// Obtiene la descripción de una solicitud según el ID
        /// </summary>
        /// <param name="id">identificador de la solicitud en la BD</param>
        /// <returns>descripción de la solicitud</returns>

        public String obtenerDescripcion(int id)
        {
            String _descripcion = "";

            foreach (Solicitud sol in _solicitudes)
            {
                if (sol.ID == id)
                    _descripcion = sol.Descripcion;
            }

            return _descripcion;
        }
        
        /// <summary>
        /// Crea un objeto avance y se lo envía el método de la capa de datos que agrega el avance a la BD
        /// </summary>
        /// <param name="idSolicitud">ID de la solicitud a la que se le desea agregar un avance</param>
        /// <param name="descripcion">Descripción del avance</param>

        public void agregarAvance(int idSolicitud, String descripcion, String postBy)
        {
            Avance _avance = new Avance();
            _avance.ID = idSolicitud;
            _avance.Descripcion = descripcion;
            _avance.PostBy = postBy;

            _manejoBD.agregarAvance(_avance);

            _avances.Add(_avance);
        }

        /// <summary>
        /// Obtiene el solicitante de una solicitud según el ID
        /// </summary>
        /// <param name="id">identificador de la solicitud en la BD</param>
        /// <returns>solicitante de la solicitud</returns>

        public String obtenerSolicitante(int id)
        {
            String _solicitante = "";

            foreach (Solicitud sol in _solicitudes)
            {
                if (sol.ID == id)
                    _solicitante = sol.Solicitante;
            }

            return _solicitante;
        }

        /// <summary>
        /// Obtiene el estado de una solicitud según el ID
        /// </summary>
        /// <param name="id">identificador de la solicitud en la BD</param>
        /// <returns>estado de la solicitud</returns>

        public String obtenerEstado(int id)
        {
            String _estado = "";

            foreach (Solicitud sol in _solicitudes)
            {
                if (sol.ID == id)
                    _estado = sol.Estado;
            }

            return _estado;
        }

        /// <summary>
        /// Obtiene la fecha de finalización de una solicitud según el ID
        /// </summary>
        /// <param name="id">identificador de la solicitud en la BD</param>
        /// <returns>fecha de finalización de la solicitud</returns>

        public String obtenerFechaFin(int id)
        {
            String _fecha = "";
            DateTime _dtFecha = new DateTime();

            foreach (Solicitud sol in _solicitudes)
            {
                if (sol.ID == id)
                {
                    _dtFecha = sol.FechaFin;
                    _fecha = _dtFecha.ToString("MM-dd-yyyy");
                }
            }

            return _fecha;
        }

        /// <summary>
        /// Obtiene la fecha de una solicitud según el ID
        /// </summary>
        /// <param name="id">identificador de la solicitud en la BD</param>
        /// <returns>fecha en que la solicitud fue realizada</returns>

        public String obtenerFechaSolicitud(int id)
        {
            String _fecha = "";
            DateTime _dtFecha = new DateTime();

            foreach (Solicitud sol in _solicitudes)
            {
                if (sol.ID == id)
                {
                    _dtFecha = sol.FechaSolicitud;
                    _fecha = _dtFecha.ToString("MM-dd-yyyy");
                }
            }

            return _fecha;
        }

        /// <summary>
        /// Obtiene el PostBy de una solicitud basandose en su ID
        /// </summary>
        /// <param name="id">Identificador de la solicitud</param>
        /// <returns>PostBy de la solicitud</returns>

        public String obtenerPostBy(int id)
        {
            String _postBy = "";
            foreach (Solicitud sol in _solicitudes)
            {
                if (sol.ID == id)
                    _postBy = _ldap.obtenerNombrePersona(sol.PostBy);
            }
            return _postBy;
        }

        /// <summary>
        /// Obtiene la descripción de un avance
        /// </summary>
        /// <param name="id">Identificador de la solicitud</param>
        /// <returns>PostBy de la solicitud</returns>

        public String obtenerDescripcionAvance(int id)
        {
            String _descripcion = "";
            foreach (Avance avance in _avances)
            {
                if (avance.ID == id)
                    _descripcion = avance.Descripcion;  
            }
            return _descripcion;
        }

        /// <summary>
        /// Llama al método que se encarga de ejecutar el SP para el cambio de estado, y le envía los parámetros necesarios.
        /// </summary>
        /// <param name="idSolicitud">Identificador de la solicitud</param>
        /// <param name="idEstado">Identificador del estado de la solicitud</param>
        /// <param name="estado">Estado de la solicitud</param>

        public void cambiarEstado(int idSolicitud, int idEstado, String estado, String postBy)
        {
            String avance = "El estado de la solicitud ha sido cambiado a: " + estado;

            _manejoBD.cambiarEstado(idSolicitud, idEstado, postBy, avance);
        }
        
    }
}
