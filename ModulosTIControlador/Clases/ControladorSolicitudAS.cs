using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaDatos.ModuloSolicitudes;
using ModulosTICapaLogica.Compartido;
using ModulosTICapaLogica.ModuloSolicitudes;

namespace ModulosTIControlador.Clases
{
    public class ControladorSolicitudAS
    {
        private ManejoBDSolicitudes _manejoBD;
        private ConexionLDAP _ldap;
        private List<Solicitud> _solicitudesPendientes;
        private List<Usuario> _soportistas;

        public ControladorSolicitudAS()
        {
            _manejoBD = new ManejoBDSolicitudes();
            _ldap = new ConexionLDAP();
            recuperarSolicitudesPendientes();
        }

        /// <summary>
        /// Recupera las solicitudes pendientes en la lista _solicitudesPendientes
        /// </summary>
        /// <returns>lista de solicitudes con estado pendiente</returns>
        /// 
        public Boolean recuperarSolicitudesPendientes()
        {
            try
            {
                _solicitudesPendientes = _manejoBD.recuperarSolicitudesPendientes();
                return true;
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        /// <summary>
        /// Crea un Data view a partir de los datos de _solicitudesPendientes
        /// </summary>
        /// <returns>DataView con las solicitudes pendientes y sus ID</returns>
        public DataView crearDataViewPendientes()
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

                foreach (Solicitud sol in _solicitudesPendientes)
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
                throw(e);
            }
            
        }

        /// <summary>
        /// Obtiene el solicitante de una solicitud según el ID
        /// </summary>
        /// <param name="id">identificador de la solicitud en la BD</param>
        /// <returns>solicitante</returns>

        public String obtenerSolicitante(int id)
        {
            String _solicitante = "";
          
            foreach (Solicitud sol in _solicitudesPendientes)
            {
                if (sol.ID == id)
                    _solicitante = sol.Solicitante;
            }

            return _solicitante;
        }

        /// <summary>
        /// Obtiene la descripción de una solicitud según el ID
        /// </summary>
        /// <param name="id">identificador de la solicitud en la BD</param>
        /// <returns>descripción de la solicitud</returns>

        public String obtenerDescripcion(int id)
        {
            String _descripcion = "";

            foreach (Solicitud sol in _solicitudesPendientes)
            {
                if (sol.ID == id)
                    _descripcion = sol.Descripcion;
            }

            return _descripcion;
        }

        /// <summary>
        /// Obtiene el postby de una solicitud según el ID
        /// </summary>
        /// <param name="id">identificador de la solicitud en la BD</param>
        /// <returns>postby de la solicitud</returns>

        public String obtenerPostBy(int id)
        {
            String _postBy = "";

            foreach (Solicitud sol in _solicitudesPendientes)
            {
                if (sol.ID == id)
                    _postBy = _ldap.obtenerNombrePersona(sol.PostBy);
            }

            return _postBy;
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

            foreach (Solicitud sol in _solicitudesPendientes)
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
        /// Crea un DataView con el nombre y el login de los soportistas
        /// </summary>
        /// <returns>DataView</returns>

        public DataView crearDataViewSoporte()
        {
            DataTable _dtResultado = new DataTable();
            DataView _dvResultado;
            _soportistas = _ldap.obtenerListaSoporte();
            //Define las columnas de la tabla
            _dtResultado.Columns.Add(new DataColumn("login", typeof(String)));
            _dtResultado.Columns.Add(new DataColumn("soportista", typeof(String)));
            try
            {
                DataRow _drSeleccionar = _dtResultado.NewRow();
                _drSeleccionar[1] = "Seleccionar";
                _dtResultado.Rows.Add(_drSeleccionar);

                foreach (Usuario soportista in _soportistas)
                {
                    DataRow _dr = _dtResultado.NewRow();
                    _dr[0] = soportista.UID;
                    _dr[1] = soportista.Nombre;

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
        /// Llama al método que envía a la BD los datos para la asignación y aprobación de una solicitud.
        /// </summary>
        /// <param name="idSolicitud">Identificador de la solicitud</param>
        /// <param name="responsable">Login del responsable de la solicitud</param>
        /// <param name="fechaFin">Fecha de finalización de la solicitud</param>

        public void asignar(int idSolicitud, String responsable, DateTime fechaFin, String postBy)
        {
            String _avance = "La solicitud ha sido aceptada y puesta a cargo de: " + responsable;

            _manejoBD.asignarSolicitud(idSolicitud, responsable, fechaFin, _avance, postBy);
        }

        public void cancelar(int idSolicitud, String postBy)
        {
            String _avance = "La solicitud ha sido cancelada por el administrador";
            _manejoBD.cancelarSolicitud(idSolicitud, _avance, postBy);
        }


        /// <summary>
        /// Llama al método que se encarga de ejecutar al SP para agregar texto a la descripción de una solicitud en la BD.
        /// </summary>
        /// <param name="idSolicitud">Identificador de la solicitud</param>
        /// <param name="nuevaDescripcion">Nueva descripción</param>
        /// <param name="postBy">Login de la persona que hace el cambio</param>
        /// <param name="avance">Mensaje de avance que se debe de asociar a la solicitud</param>

        public void agregarDescripcion(int idSolicitud, String descripcion, String nuevaDescripcion, String postBy)
        {
            String _postBy = _ldap.obtenerNombrePersona(postBy);
            String _descripcion = descripcion + "\n\nAgregado por " + _postBy + ":\n\n" + nuevaDescripcion;
            String _avance = _postBy + " agregó texto a la descripción de la solicitud.";

            _manejoBD.agregarDescripcion(idSolicitud, _descripcion, postBy, _avance);
        }

    }
}
