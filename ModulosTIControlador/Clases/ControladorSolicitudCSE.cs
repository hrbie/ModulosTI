using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ModulosTICapaLogica.Compartido;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaLogica.ModuloSolicitudes;
using ModulosTICapaDatos.ModuloSolicitudes;


namespace ModulosTIControlador.Clases
{
    public class ControladorSolicitudCSE
    {

        private ManejoBDSolicitudes _manejoBD;
        private ConexionLDAP _ldap;
        private static List<Solicitud> _solicitudes;
        private static List<Usuario> _soportistas;
        private static List<Avance> _avances;
        
        public ControladorSolicitudCSE()
        {
            _manejoBD = new ManejoBDSolicitudes();
            _ldap = new ConexionLDAP();
        }

        /// <summary>
        /// Recupera de la BD los posibles estados para una solicitud
        /// </summary>
        /// <returns>DataView de los estados</returns>

        public DataView crearDataViewEstados()
        {
            DataView _dvEstados;
            try
            {
                _dvEstados = _manejoBD.recuperarEstados();
                return _dvEstados;
            }
            catch(Exception e)
            {
                throw(e);
            }
            

        }

        /// <summary>
        /// Crea un DataView con los usuarios que tienen solicitudes no pendientes en la BD
        /// </summary>
        /// <returns>DataView con los usuarios</returns>

        public DataView crearDataViewUsuarios()
        {
            DataTable _dtResultado = new DataTable();
            DataView _dvResultado;
            List<Usuario> _usuarios = _manejoBD.recuperarUsuarios();
            //Define las columnas de la tabla
            _dtResultado.Columns.Add(new DataColumn("login", typeof(String)));
            _dtResultado.Columns.Add(new DataColumn("usuario", typeof(String)));
            try
            {
                DataRow _drSeleccionar = _dtResultado.NewRow();
                _drSeleccionar[1] = "Seleccionar";
                _dtResultado.Rows.Add(_drSeleccionar);

                foreach (Usuario us in _usuarios)
                {
                    DataRow _dr = _dtResultado.NewRow();
                    _dr[0] = us.UID;
                    _dr[1] = us.Nombre;
                    
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
        /// Crea un DataView con las solicitudes no pendientes de un usuario
        /// </summary>
        /// <param name="usuario">un usuario</param>
        /// <returns>DataView de solicitudes</returns>

        public DataView crearDataViewSolicitudes(String usuario)
        {
            DataTable _dtResultado = new DataTable();
            DataView _dvResultado;
            //Se inicializa la lista global _solicitudes de la clase.
            _solicitudes = _manejoBD.recuperarSolicitudesUsuario(usuario);

            _dtResultado.Columns.Add(new DataColumn("id", typeof(Int32)));
            _dtResultado.Columns.Add(new DataColumn("asunto", typeof(String)));

            try
            {
                DataRow _drSeleccionar = _dtResultado.NewRow();
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
        /// Obtiene la descripción de una solicitud basandose en su ID
        /// </summary>
        /// <param name="id">Identificador de la solicitud</param>
        /// <returns>Descripción de la solicitud</returns>

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
        /// Obtiene el estado de una solicitud basandose en su ID
        /// </summary>
        /// <param name="id">Identificador de la solicitud</param>
        /// <returns>Estado de la solicitud</returns>

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
        /// Obtiene el nombre de la persona a cargo de una solicitud basandose en su ID
        /// </summary>
        /// <param name="id">Identificador de la solicitud</param>
        /// <returns>Encargado de la solicitud</returns>

        public String obtenerSoportista(int id)
        {
            String _soportista = "";
            foreach (Solicitud sol in _solicitudes)
            {
                if (sol.ID == id)
                    _soportista = sol.Responsable;
            }
            return _soportista;
        }


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

        /// <summary>
        /// Llama al método que se encarga de ejecutar el SP para el cambio de engarado de la solicitud, y le envía los parametros
        /// que sean necesarios.
        /// </summary>
        /// <param name="idSolicitud">Identificador de la solicitud</param>
        /// <param name="login">login del nuevo encargado</param>

        public void cambiarEncargado(int idSolicitud, String login, String postBy)
        {
            String avance = "El encargado de la solicitud ha sido cambiado por " + _ldap.obtenerNombrePersona(login);

            _manejoBD.cambiarEncargado(idSolicitud, login, avance, postBy);

            foreach (Solicitud sol in _solicitudes)
            {
                if (sol.ID == idSolicitud)
                    sol.Responsable = login;
            }
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

			//Actualizar la descripción de la solicitud.
			foreach (Solicitud sol in _solicitudes)
			{
				if (sol.ID == idSolicitud)
					sol.Descripcion = _descripcion;
			}
        }

    }
}
