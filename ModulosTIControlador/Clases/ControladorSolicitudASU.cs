using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ModulosTICapaDatos.ModuloSolicitudes;
using ModulosTICapaLogica.ModuloSolicitudes;
using ModulosTICapaDatos.Compartido;

namespace ModulosTIControlador.Clases
{
    public class ControladorSolicitudASU
    {
        private ManejoBDSolicitudes _manejoBD;
        public static List<Solicitud> _solicitudes;
        private static List<Avance> _avances;
        private ConexionLDAP _ldap;

        public ControladorSolicitudASU(String usuario)
        {
            _manejoBD = new ManejoBDSolicitudes();
            _ldap = new ConexionLDAP();
            _solicitudes = _manejoBD.recuperarSolicitudesDeUsuario(usuario);
        }

        /// <summary>
        ///  Crea un DataView a partir de los datos de _solicitudes
        /// </summary>
        /// <returns>DataView con los id y asuntos</returns>

        public DataView crearDataViewSolicitudes()
        {
            //Crea una tabla para guardar los datos del DropDownList _ddlSolicitudes
            DataTable _dtResultado = new DataTable();
            DataView _dvResultado;
            //Define las columnas de la tabla
            _dtResultado.Columns.Add(new DataColumn("id", typeof(String)));
            _dtResultado.Columns.Add(new DataColumn("solicitud", typeof(String)));
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
        /// Obtiene el responsable de una solicitud según el ID
        /// </summary>
        /// <param name="id">identificador de la solicitud en la BD</param>
        /// <returns>responsable de la solicitud</returns>

        public String obtenerResponsable(int id)
        {
            String _responsable = "";

            foreach (Solicitud sol in _solicitudes)
            {
                if (sol.ID == id)
                    _responsable = _ldap.obtenerNombrePersona(sol.Responsable);
            }

            return _responsable;
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
    }
}
