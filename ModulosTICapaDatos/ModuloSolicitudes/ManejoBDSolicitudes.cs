using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using ModulosTICapaLogica.Compartido;
using ModulosTICapaLogica.ModuloSolicitudes;
using System.Configuration;
using ModulosTICapaDatos.Compartido;

namespace ModulosTICapaDatos.ModuloSolicitudes
{
    public class ManejoBDSolicitudes
    {
        private SqlConnection _conexion; // Manejo de conexión con la BD

		/// <summary>
		/// Constructor de la clase ManejoBDSolicitudes
		/// Este constructor indica los terminos de la conexión con la Base da Datos
		/// </summary>

        public ManejoBDSolicitudes()
        {

            ConnectionStringSettings _settings = ConfigurationManager.ConnectionStrings["ModulosTI"]; // Propiedas del string de conexión almacenado en el .config
            _conexion = new SqlConnection(_settings.ConnectionString);
        }

        /// <summary>
        /// Recupera los posibles estados de una solicitud de la BD
        /// </summary>
        /// <returns>DataView</returns>

        public DataView recuperarEstados()
        {
            //Crea una tabla para guardar los datos del DropDownList _ddlEstado
            DataTable _dtResultado = new DataTable();
            //Define las columnas de la tabla
            _dtResultado.Columns.Add(new DataColumn("id", typeof(Int32)));
            _dtResultado.Columns.Add(new DataColumn("estado", typeof(String)));

            SqlCommand buscar = new SqlCommand("SP_SOL_EstadosSolicitud", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();

            DataRow _drSeleccionar = _dtResultado.NewRow();
            _drSeleccionar[0] = "0";
            _drSeleccionar[1] = "Seleccionar";
            _dtResultado.Rows.Add(_drSeleccionar);

            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
                while (reader.Read())
                {                    
                    //Poblar la tabla resultado
                    _dtResultado.Rows.Add(CrearFila(reader.GetInt32(0), reader.GetString(1), _dtResultado));

                }
                _conexion.Close();
            }
            catch (Exception)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
            }

            //Se crea un DataView a partir del DataTable par que actue como DataSource del DropDownList
            DataView _dv = new DataView(_dtResultado);
            return _dv;
        }

        /// <summary>
        /// Crea una fila para una DataTable
        /// </summary>
        /// <param name="id">ID de la entrada en al BD</param>
        /// <param name="valor">Valor de la entrada que será mostrado en el DropDownList</param>
        /// <param name="dt">Tabla destino</param>
        /// <returns>Una fila de la Tabla</returns>

        DataRow CrearFila(int id, String valor, DataTable dt)
        {

            //Crea una fila en la DataTable pasada como parametro
            DataRow _dr = dt.NewRow();

            //Agrega los datos pasados como parametro en fila 1 y fila 2
            _dr[0] = id;
            _dr[1] = valor;

            return _dr;

        }

        /// <summary>
        /// Recupera de la BD todas las solicitudes en estado "Pendiente"
        /// </summary>
        /// <returns>Lista con los datos de las solicitudes pendientes</returns>

        public List<Solicitud> recuperarSolicitudesPendientes()
        {
            List<Solicitud> resultado = new List<Solicitud>();
            
            SqlCommand buscar = new SqlCommand("SP_SOL_RecuperarSolicitudesPendientes", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();

            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
                while (reader.Read())
                {
                    Solicitud _solicitud = new Solicitud();
                    _solicitud.ID = reader.GetInt32(0);
                    _solicitud.FechaSolicitud = reader.GetDateTime(1);
                    _solicitud.Solicitante = reader.GetString(2);
                    _solicitud.Asunto = reader.GetString(3);
                    _solicitud.Descripcion = reader.GetString(4);
                    _solicitud.PostBy = reader.GetString(5);
                    resultado.Add(_solicitud);
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
        /// Recupera de la BD a los usuarios que tienen solicitudes no pendientes.
        /// </summary>
        /// <returns>Lista de usuarios</returns>

        public List<Usuario> recuperarUsuarios()
        {
            List<Usuario> resultado = new List<Usuario>();
            
            SqlCommand buscar = new SqlCommand("SP_SOL_RecuperarSolicitantes", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();

            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
                while (reader.Read())
                {
                    Usuario _usuario = new Usuario();
                    _usuario.UID = reader.GetString(0);    // Login del Usuario
                    _usuario.Nombre = reader.GetString(1); // Nombre del Usuario
                    resultado.Add(_usuario); 
                    
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
        /// Recupera de la BD todas las solicitudes no pendientes de un usuario
        /// </summary>
        /// <param name="usuario">login del usuario</param>
        /// <returns>Lista de solicitudes</returns>

        public List<Solicitud> recuperarSolicitudesUsuario(String usuario)
        {
            List<Solicitud> resultado = new List<Solicitud>();

            SqlCommand buscar = new SqlCommand("SP_SOL_RecuperarSolicitudesPorUsuario", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@login", SqlDbType.VarChar);
            parametros[0].Value = usuario;
            buscar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();

            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
                while (reader.Read())
                {
                    
                    Solicitud _solicitud = new Solicitud();
                    _solicitud.ID = reader.GetInt32(0);                 //ID de la solicitud
                    _solicitud.Asunto = reader.GetString(1);            //Asunto de la solicitud
                    _solicitud.Descripcion = reader.GetString(2);       //Descripción de la solicitud
                    _solicitud.FechaSolicitud = reader.GetDateTime(3);  //Fecha de la solicitud
                    _solicitud.PostBy = reader.GetString(4);            //Postby de la solicitud
                    _solicitud.Estado = reader.GetString(5);            //Estado de la solicitud
                    _solicitud.Responsable = reader.GetString(6);       //Responsable de la solicitud
                    resultado.Add(_solicitud);
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
        /// Recupera de la BD todas las solicitudes de un usuario
        /// </summary>
        /// <param name="usuario">login del usuario</param>
        /// <returns>Lista de solicitudes</returns>

        public List<Solicitud> recuperarSolicitudesDeUsuario(String usuario)
        {
            List<Solicitud> resultado = new List<Solicitud>();

            SqlCommand buscar = new SqlCommand("SP_SOL_RecuperarSolicitudesUsuario", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@login", SqlDbType.VarChar);
            parametros[0].Value = usuario;
            buscar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();

            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
                while (reader.Read())
                {

                    Solicitud _solicitud = new Solicitud();
                    _solicitud.ID = reader.GetInt32(0);                 //ID de la solicitud
                    _solicitud.Asunto = reader.GetString(1);            //Asunto de la solicitud
                    _solicitud.Descripcion = reader.GetString(2);       //Descripción de la solicitud
                    _solicitud.FechaSolicitud = reader.GetDateTime(3);  //Fecha de la solicitud
                    _solicitud.PostBy = reader.GetString(4);            //Postby de la solicitud  
                    _solicitud.Responsable = reader.GetString(5);       //Responsable de la solicitud
                    _solicitud.FechaFin = reader.GetDateTime(6);        //Fecha de finalización de la solicitud
                    _solicitud.Estado = reader.GetString(7);            //Estado de la solicitud
                    resultado.Add(_solicitud);
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
        /// Método que se encarga de ejecutar el SP que recupera de la BD las solicitudes asignadas
        /// a un miembro de la oficina de TI
        /// </summary>
        /// <param name="soportista">login de un miembro de la oficina de TI</param>
        /// <returns>Lista de solicitudes</returns>

        public List<Solicitud> recuperarSolicitudesSoportista(String soportista)
        {
            List<Solicitud> resultado = new List<Solicitud>();

            SqlCommand buscar = new SqlCommand("SP_SOL_RecuperarSolicitudesSoportista", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@responsable", SqlDbType.VarChar);
            parametros[0].Value = soportista;
            buscar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();

            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
                while (reader.Read())
                {

                    Solicitud _solicitud = new Solicitud();
                    _solicitud.ID = reader.GetInt32(0);                 //ID de la solicitud
                    _solicitud.Asunto = reader.GetString(1);            //Asunto de la solicitud
                    _solicitud.Descripcion = reader.GetString(2);       //Descripción de la solicitud
                    _solicitud.Solicitante = reader.GetString(3);       //Solicitante
                    _solicitud.FechaSolicitud = reader.GetDateTime(4);  //Fecha de la solicitud
                    _solicitud.FechaFin = reader.GetDateTime(5);        //Fecha de finalización
                    _solicitud.PostBy = reader.GetString(6);            //Postby de la solicitud
                    _solicitud.Estado = reader.GetString(7);            //Estado de la solicitud
                    resultado.Add(_solicitud);
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
        /// Método que ejecuta un SP que recupera de la BD los avance de una solicitud
        /// </summary>
        /// <param name="idSolicitud">identificador de la solicitud</param>
        /// <returns>Lista de Avances</returns>
        
        public List<Avance> recuperarAvances(int idSolicitud)
        {
            List<Avance> _resultado = new List<Avance>();

            SqlCommand buscar = new SqlCommand("SP_SOL_RecuperarAvancesPorSolicitud", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@idSolicitud", SqlDbType.Int);
            parametros[0].Value = idSolicitud;
            buscar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();

            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
                while (reader.Read())
                {
                    Avance _avance = new Avance();
                    _avance.ID = reader.GetInt32(0);            //ID del Avance
                    _avance.fecha = reader.GetDateTime(1);      //Fecha del Avance
                    _avance.Descripcion = reader.GetString(2);  //Descripción del Avance
                    _resultado.Add(_avance);
                }
                _conexion.Close();
            }
            catch (Exception)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
            }

            return _resultado;
        }

        /// <summary>
        /// Método que se encarga de ejecutar el SP que agrega un avance a una solicitud
        /// </summary>
        /// <param name="idSolicitud">id de la solicitud</param>
        /// <param name="descripcion">descripción del avance</param>
        /// <param name="postBy">la identificación de la persona que agregó el avance</param>
        /// <returns>Un valor de true si la inserción fue exitosa, false si no se logró</returns>

        public Boolean agregarAvance(Avance avance)
        {
            Boolean resultado = false;

            SqlCommand agregar = new SqlCommand("SP_SOL_AgregarAvance", _conexion);
            agregar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@idSolicitud", SqlDbType.Int);
            parametros[0].Value = avance.ID;
            parametros[1] = new SqlParameter("@descripcion", SqlDbType.NText);
            parametros[1].Value = avance.Descripcion;
            parametros[2] = new SqlParameter("@postBy", SqlDbType.VarChar);
            parametros[2].Value = avance.PostBy;
            agregar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = agregar.ExecuteReader();
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
        /// Método que se encarga de ejecutar el SP que agrega una descripción del estado estado de una solicitud.
        /// </summary>
        /// <param name="descripcion">descripción del estado de la solicitud</param>
        /// <returns>Un valor de true si la inserción fue exitosa, false si no se logró</returns>

        public Boolean agregarEstado(string descripcion)
        {
            Boolean resultado = false;

            SqlCommand buscar = new SqlCommand("SP_SOL_AgregarEstado", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@descripcion", SqlDbType.NText);
            parametros[0].Value = descripcion;
            
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
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

        

        public Boolean asignarSolicitud(int idSolicitud, string responsable, DateTime fechaFin, String avance, String postBy)
        {
            Boolean resultado = false;

            SqlCommand agregar = new SqlCommand("SP_SOL_AsignarSolicitud", _conexion);
            agregar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("@solicitudID", SqlDbType.Int);
            parametros[0].Value = idSolicitud;
            parametros[1] = new SqlParameter("@login", SqlDbType.NText);
            parametros[1].Value = responsable;
            parametros[2] = new SqlParameter("@fechaFin", SqlDbType.Date);
            parametros[2].Value = fechaFin;
            parametros[3] = new SqlParameter("@avance", SqlDbType.NText);
            parametros[3].Value = avance;
            parametros[4] = new SqlParameter("@postBy", SqlDbType.VarChar);
            parametros[4].Value = postBy;
            agregar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = agregar.ExecuteReader();
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
        /// Método que ejecuta el SP que cancela una solicitud
        /// </summary>
        /// <param name="idSolicitud">id de la solicitud a cancelar</param>
        /// <param name="descripcion">motivo de la cancelación de la solicitud</param>
        /// <returns>Un valor de true si la inserción fue exitosa, false si no se logró</returns>

        public Boolean cancelarSolicitud(int idSolicitud, string descripcion, string postBy)
        {
            Boolean resultado = false;

            SqlCommand agregar = new SqlCommand("SP_SOL_CancelarSolicitud", _conexion);
            agregar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@idSolicitud", SqlDbType.Int);
            parametros[0].Value = idSolicitud;
            parametros[1] = new SqlParameter("@descripcion", SqlDbType.NText);
            parametros[1].Value = descripcion;
            parametros[2] = new SqlParameter("@postBy", SqlDbType.NText);
            parametros[2].Value = postBy;
            agregar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = agregar.ExecuteReader();
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
        /// Método que se encarga de ejecutar el SP que da por finalizada una solicitud.
        /// </summary>
        /// <param name="idSolicitud">id de la solicitud a finalizar</param>
        /// <param name="descripcion">descripción de la finalización de la solicitud</param>
        /// <returns>Un valor de true si la inserción fue exitosa, false si no se logró</returns>

        public Boolean finiquitarSolicitud(int idSolicitud, string descripcion)
        {
            Boolean resultado = false;

            SqlCommand buscar = new SqlCommand("SP_SOL_FiniquitarSolicitud", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@idSolicitud", SqlDbType.Int);
            parametros[0].Value = idSolicitud;
            parametros[1] = new SqlParameter("@descripcion", SqlDbType.NText);
            parametros[1].Value = descripcion;


            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
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


        public Boolean suspenderSolicitud(int idSolicitud, string descripcion)
        {
            Boolean resultado = false;

            SqlCommand buscar = new SqlCommand("SP_SOL_SuspenderSolicitud", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@idSolicitud", SqlDbType.Int);
            parametros[0].Value = idSolicitud;
            parametros[1] = new SqlParameter("@descripcion", SqlDbType.NText);
            parametros[1].Value = descripcion;


            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
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
        /// Método que ejecuta el SP que agrega una solicitud a la BD   
        /// </summary>
        /// <param name="solicitud">Objeto solicitud </param>
        /// <returns>boolean que indica si la inserción fue exitosa o no</returns>
        /// 
        public Boolean insertarSolicitud(Solicitud solicitud)
        {
            Boolean resultado = false;

            SqlCommand agregar = new SqlCommand("SP_SOL_InsertarSolicitud", _conexion);
            agregar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("@solicitante", SqlDbType.VarChar);
            parametros[0].Value = solicitud.Solicitante;
            parametros[1] = new SqlParameter("@asunto", SqlDbType.VarChar);
            parametros[1].Value = solicitud.Asunto;
            parametros[2] = new SqlParameter("@descripcion", SqlDbType.NText);
            parametros[2].Value = solicitud.Descripcion;
            parametros[3] = new SqlParameter("@PostBy", SqlDbType.NText);
            parametros[3].Value = solicitud.PostBy;
            parametros[4] = new SqlParameter("@loginSolicitante", SqlDbType.VarChar);
            parametros[4].Value = solicitud.Login;
            agregar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = agregar.ExecuteReader();
                reader.Read();
                resultado = !(reader.HasRows);
                _conexion.Close();
				resultado = true;
            }
            catch (Exception)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
				resultado = false;
            }

            return resultado;
        }

        /// <summary>
        /// Método que ejecuta el SP que cambia el estado de una solicitud en la BD
        /// </summary>
        /// <param name="idSolicitud">Identificador de la solicitud</param>
        /// <param name="idEstado">Identificador del estado</param>
        /// <returns>boolean</returns>

        public Boolean cambiarEstado(int idSolicitud, int idEstado, String postBy, String avance)
        {
            Boolean resultado = false;

            SqlCommand agregar = new SqlCommand("SP_SOL_CambiarEstadoSolicitud", _conexion);
            agregar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("@solicitudID", SqlDbType.Int);
            parametros[0].Value = idSolicitud;
            parametros[1] = new SqlParameter("@estadoID", SqlDbType.Int);
            parametros[1].Value = idEstado;
            parametros[2] = new SqlParameter("@postBy", SqlDbType.VarChar);
            parametros[2].Value = postBy;
            parametros[3] = new SqlParameter("@Avance", SqlDbType.NText);
            parametros[3].Value = avance;
            agregar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = agregar.ExecuteReader();
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
        /// Método que ejecuta el SP que cambia el login del encargado de una solicitud en la BD.
        /// </summary>
        /// <param name="idSolicitud">Identificador de la solicitud</param>
        /// <param name="login">login del nuevo encargado</param>
        /// <returns>boolean</returns>

        public Boolean cambiarEncargado(int idSolicitud, String login, String avance, String postBy)
        {
            Boolean resultado = false;

            SqlCommand agregar = new SqlCommand("SP_SOL_CambiarEncargado", _conexion);
            agregar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("@solicitudID", SqlDbType.Int);
            parametros[0].Value = idSolicitud;
            parametros[1] = new SqlParameter("@login", SqlDbType.VarChar);
            parametros[1].Value = login;
            parametros[2] = new SqlParameter("@avance", SqlDbType.NText);
            parametros[2].Value = avance;
            parametros[3] = new SqlParameter("@postBy", SqlDbType.VarChar);
            parametros[3].Value = postBy;
            agregar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = agregar.ExecuteReader();
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
        /// Método que ejecuta el SP que cambia la descripción de una solicitud en la BD.
        /// </summary>
        /// <param name="idSolicitud">Identificador de la solicitud</param>
        /// <param name="nuevaDescripcion">Descripción con el agregado</param>
        /// <param name="postBy">Identificación de la perosona que hace el cambio</param>
        /// <param name="avance">Avance de modificación de la descripción</param>
        /// <returns>boolean</returns>
        

        public Boolean agregarDescripcion(int idSolicitud, String nuevaDescripcion, String postBy, String avance)
        {
            Boolean resultado = false;

            SqlCommand agregar = new SqlCommand("SP_SOL_AgregarDescripcion", _conexion);
            agregar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("@solicitudID", SqlDbType.Int);
            parametros[0].Value = idSolicitud;
            parametros[1] = new SqlParameter("@nuevaDescripcion", SqlDbType.VarChar);
            parametros[1].Value = nuevaDescripcion;
            parametros[2] = new SqlParameter("@postBy", SqlDbType.VarChar);
            parametros[2].Value = postBy;
            parametros[3] = new SqlParameter("@avance", SqlDbType.NText);
            parametros[3].Value = avance;
            agregar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = agregar.ExecuteReader();
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

    }
}