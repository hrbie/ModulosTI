using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaLogica.ModuloActivo;

namespace ModulosTICapaDatos.ModuloActivo
{
    public class ManejoBDActivo
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

		public ManejoBDActivo()
		{
			ConnectionStringSettings _settings = ConfigurationManager.ConnectionStrings["ModulosTI"]; // Propiedas del string de conexión almacenado en el .config
			_conexion = new SqlConnection(_settings.ConnectionString);
		}

		#endregion

        #region Métodos

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que inserta un activo en el sistema
        /// </summary>
        /// <param name="activo">Objeto activo</param>
        /// <returns>Retorna un número entero (1: fue exitoso, 0: codigo ya existe, -1: error al intentar crearlo)</returns>

        public int insertarActivo(Activo activo,String login)
        {
            SqlCommand insertar = new SqlCommand("SP_ACT_InsertarActivo", _conexion);
            insertar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[4];
            parametros[0] = new SqlParameter("@FK_TipoActivo", SqlDbType.Int);
            parametros[0].Value = activo.IdTipoActivo;
            parametros[1] = new SqlParameter("@codigo", SqlDbType.VarChar);
            parametros[1].Value = activo.Codigo;
            parametros[2] = new SqlParameter("@descripcion", SqlDbType.VarChar);
            parametros[2].Value = activo.Descripcion;
            parametros[3] = new SqlParameter("@login", SqlDbType.VarChar);
            parametros[3].Value = login;
            insertar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {

                SqlDataReader reader = insertar.ExecuteReader();
                reader.Read();
                int resultado = reader.GetInt32(0);
                if (resultado == -1)
                {
                    _conexion.Close();
                    return 0;
                }
                else
                {
                    _conexion.Close();
                    return 1;
                }
            }
            catch (Exception)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                return -1;
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que modifica un activo en el sistema
        /// </summary>
        /// <param name="activo">Objeto activo</param>
        /// <returns>Retorna una valor booleano indicando si tuvo éxito (true) o no (false)</returns>

        public bool modificarActivo(Activo activo)
        {
            Boolean resultado = false;
            SqlCommand modificar = new SqlCommand("SP_ACT_ModificarActivo", _conexion);
            modificar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@PK_Activo", SqlDbType.Int);
            parametros[0].Value = activo.IdActivo;
            parametros[1] = new SqlParameter("@descripcion", SqlDbType.VarChar);
            parametros[1].Value = activo.Descripcion;
            parametros[2] = new SqlParameter("@estado", SqlDbType.Bit);
            parametros[2].Value = activo.EstadoActivo;
            modificar.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = modificar.ExecuteReader();
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
        /// Método que llama al stored procedure que retorna todos los tipos activos del sistema
        /// </summary>
        /// <returns></returns>

        public List<List<object>> consultarTipoActivo()
        {
            List<List<object>> resultado = new List<List<object>>();
            List<object> nodo;
            SqlCommand buscar = new SqlCommand("SP_ACT_ObtenerTipoActivo", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
                while (reader.Read())
                {
                    nodo = new List<object>();
                    nodo.Add(reader.GetInt32(0)); // Obtener el PK del tipo de activo
                    nodo.Add(reader.GetString(1)); // Obtener el nombre del tipo de activo
                    resultado.Add(nodo);
                }
                _conexion.Close();
                return resultado;
            }
            catch (Exception)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                //insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }

        /// <summary>
        /// Método que se encarga de llamar al stored procedure que inserta un movimineto de un activo en el sistema
        /// </summary>
        /// <param name="movimiento">Objeto movimiento</param>
        /// <returns>Retorna un número entero (1: fue exitoso, 0: fallo, -1: error al intentar crearlo)</returns>

        public int insertarMovimientoActivo(Movimiento movimiento)
        {
            SqlCommand insertar = new SqlCommand("SP_ACT_InsertarMovimientoActivo", _conexion);
            insertar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[5];
            parametros[0] = new SqlParameter("@FK_Activo", SqlDbType.Int);
            parametros[0].Value = movimiento.IdActivo;
            parametros[1] = new SqlParameter("@FK_TipoMovimiento", SqlDbType.Int);
            parametros[1].Value = movimiento.IdTipoMovimiento;
            parametros[2] = new SqlParameter("@login", SqlDbType.VarChar);
            parametros[2].Value = movimiento.Solicitante;
            parametros[3] = new SqlParameter("@postby", SqlDbType.VarChar);
            parametros[3].Value = movimiento.PostBy;
            parametros[4] = new SqlParameter("@comentario", SqlDbType.VarChar);
            parametros[4].Value = movimiento.Comentario;
            insertar.Parameters.AddRange(parametros);

            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {

                SqlDataReader reader = insertar.ExecuteReader();
                Boolean resul = !(reader.HasRows);
                if (resul)
                {
                    _conexion.Close();
                    return 1;
                }
                else
                {
                    _conexion.Close();
                    return 0;
                }
            }
            catch (Exception)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                return -1;
            }
        }


        /// <summary>
        /// Método que llama al stored procedure que retorna todos los tipos de movimiento del sistema
        /// </summary>
        /// <returns></returns>

        public List<List<object>> consultarTipoMovimiento()
        {
            List<List<object>> resultado = new List<List<object>>();
            List<object> nodo;
            SqlCommand buscar = new SqlCommand("SP_ACT_ObtenerTipoMovimiento", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
                while (reader.Read())
                {
                    nodo = new List<object>();
                    nodo.Add(reader.GetInt32(0)); // Obtener el PK del tipo de movimiento
                    nodo.Add(reader.GetString(1)); // Obtener el nombre del tipo de movimiento
                    resultado.Add(nodo);
                }
                _conexion.Close();
                return resultado;
            }
            catch (Exception )
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                //insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }


        /// <summary>
        /// Método que llama al stored procedure que retorna el id del activo de acuerdo al codigo del activo
        /// </summary>
        /// <param name="codigo">String del codigo del activo a consultar</param>
        /// <returns>Retorna un int, el id del activo si se encuentra, -1 si hubo un error, 
        /// -2 si no se encuentra el codigo</returns>

        public int consultarIdActivo(string codigo)
        {   
            int idActivo;
            SqlCommand buscar = new SqlCommand("SP_ACT_ObtenerIDActivo", _conexion);
            buscar.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@Codigo", SqlDbType.VarChar);
            parametros[0].Value = codigo;
            buscar.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = buscar.ExecuteReader();
                if (reader.Read())
                    idActivo = reader.GetInt32(0); // Obtener el PK del tipo de movimiento
                else
                    idActivo = -2;
                _conexion.Close();
                return idActivo;
            }
            catch (Exception )
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                //insertarBitacoraError(ex.ToString(), "");
                return -1;
            }
        }

        /// <summary>
        /// Método que se utiliza para consultar movimiento de activos por estado (prestado, devuelto)
        /// </summary>
        /// <param name="estado">int del estado a consultar</param>
        /// <returns>Una lista de listas de objetos, la cual contiene (tipo de movimiento, id del activo, codigo del activo, 
        /// nombre del activo, fecha del movimiento, solicitante del movimiento, postby del movimiento) </returns>

        public DataTable consultarActivoPorEstado(int estado)
        {
            //List<List<object>> resultado = new List<List<object>>();
            //List<object> nodo;
            SqlCommand obtener = new SqlCommand("SP_ACT_ConsultarActivoPorEstado", _conexion);
            obtener.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@estado", SqlDbType.Int);
            parametros[0].Value = estado;
            obtener.Parameters.AddRange(parametros);
            DataTable _tablaReportes;
            _tablaReportes = new DataTable("Datos");
            //_tablaReportes.Columns.Add(new DataColumn("Id Activo"));
            _tablaReportes.Columns.Add(new DataColumn("Tipo Movimiento"));
            _tablaReportes.Columns.Add(new DataColumn("Codigo activo"));
            _tablaReportes.Columns.Add(new DataColumn("Descripcion"));
            _tablaReportes.Columns.Add(new DataColumn("Fecha"));
            _tablaReportes.Columns.Add(new DataColumn("Solicitante"));
            _tablaReportes.Columns.Add(new DataColumn("PostBy"));
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = obtener.ExecuteReader();
                while (reader.Read())
                {
                    //nodo = new List<object>();
                    //nodo.Add(reader.GetString(0));      // Obtener el tipo de movimiento
                    //nodo.Add(reader.GetInt16(1));      // Obtener el id de activo
                    //nodo.Add(reader.GetString(2));    // Obtener el codigo de activo
                    //nodo.Add(reader.GetString(3));      // Obtener la descripcion del activo
                    //nodo.Add(reader.GetDateTime(4));       // Obtener el fecha del movimiento
                    //nodo.Add(reader.GetString(5));       // Obtener el solicitante del movimiento
                    //nodo.Add(reader.GetString(6));      // Obtener el postBy del movimiento
                    //resultado.Add(nodo);
                    _tablaReportes.Rows.Add(reader.GetSqlString(0), reader.GetSqlString(1), reader.GetSqlString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
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
            return _tablaReportes;
        }

        /// <summary>
        /// Método que se utiliza para consultar movimiento de activos por descripcion
        /// </summary>
        /// <param name="estado">string de descripcion a consultar</param>
        /// <returns>Una lista de listas de objetos, la cual contiene (id del activo, codigo del activo, nombre del activo, 
        /// postby del movimiento, solicitante del movimiento, comentario del movimiento, fecha del movimiento) </returns>

        public DataTable consultarActivoPorDescripcion(string descrip)
        {
            //List<List<object>> resultado = new List<List<object>>();
            //List<object> nodo;
            SqlCommand obtener = new SqlCommand("SP_ACT_ConsultarActivoPorDescripcion", _conexion);
            obtener.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@descripcion", SqlDbType.VarChar);
            parametros[0].Value = descrip;
            obtener.Parameters.AddRange(parametros);
            DataTable _tablaReportes;
            _tablaReportes = new DataTable("Datos");
            //_tablaReportes.Columns.Add(new DataColumn("Id Activo"));
            _tablaReportes.Columns.Add(new DataColumn("Codigo"));
            _tablaReportes.Columns.Add(new DataColumn("Nombre"));
            _tablaReportes.Columns.Add(new DataColumn("PostBy"));
            _tablaReportes.Columns.Add(new DataColumn("Solicitante"));
            _tablaReportes.Columns.Add(new DataColumn("Comentario"));
            _tablaReportes.Columns.Add(new DataColumn("Fecha"));
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = obtener.ExecuteReader();
                while (reader.Read())
                {
                    /*nodo = new List<object>();
                    nodo.Add(reader.GetInt16(0));      // Obtener el id de activo
                    nodo.Add(reader.GetString(1));      // Obtener el codigo del activo
                    nodo.Add(reader.GetString(2));    // Obtener el nombre del activo
                    nodo.Add(reader.GetString(3));      // Obtener el postby del movimiento
                    nodo.Add(reader.GetString(4));       // Obtener el solicitante del movimiento
                    nodo.Add(reader.GetString(5));       // Obtener el comentario del movimiento
                    nodo.Add(reader.GetDateTime(6));      // Obtener la fecha del movimiento
                    resultado.Add(nodo);*/
                    _tablaReportes.Rows.Add(reader.GetSqlString(0), reader.GetSqlString(1), reader.GetSqlString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5));
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
            return _tablaReportes;
        }

        /// <summary>
        /// Método que se utiliza para consultar movimiento de activos por codigo y fecha
        /// </summary>
        /// <param name="codigo">string de codigo a consultar</param>
        /// <param name="fecha">DateTime de fecha a consultar</param>
        /// <returns>Una lista de listas de objetos, la cual contiene (codigo de activo, nombre de activo, tipo de movimiento
        /// postby del movimiento, solicitante del movimiento, comentario del movimiento y fecha del movimiento) </returns>

        public DataTable consultarActivoPorCodigo(string codigo)
        {
            //List<List<object>> resultado = new List<List<object>>();
            //List<object> nodo;
            SqlCommand obtener = new SqlCommand("SP_ACT_ConsultarActivoPorCodigo", _conexion);
            obtener.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@codigo", SqlDbType.VarChar);
            parametros[0].Value = codigo;
            obtener.Parameters.AddRange(parametros);
            DataTable _tablaReportes;
            _tablaReportes = new DataTable("Datos");
            //_tablaReportes.Columns.Add(new DataColumn("Id Activo"));
            _tablaReportes.Columns.Add(new DataColumn("Codigo"));
            _tablaReportes.Columns.Add(new DataColumn("Nombre"));
            _tablaReportes.Columns.Add(new DataColumn("Tipo movimiento"));
            _tablaReportes.Columns.Add(new DataColumn("PostBy"));
            _tablaReportes.Columns.Add(new DataColumn("Solicitante"));
            _tablaReportes.Columns.Add(new DataColumn("Comentario"));
            _tablaReportes.Columns.Add(new DataColumn("Fecha"));
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = obtener.ExecuteReader();
                while (reader.Read())
                {
                    //nodo = new List<object>();
                    //nodo.Add(reader.GetInt16(0));      // Obtener el codigo de activo
                    //nodo.Add(reader.GetString(1));      // Obtener el nombre del activo
                    //nodo.Add(reader.GetString(2));    // Obtener el tipo de movimiento del activo
                    //nodo.Add(reader.GetString(3));      // Obtener el postby del movimiento
                    //nodo.Add(reader.GetString(4));       // Obtener el solicitante del movimiento
                    //nodo.Add(reader.GetString(5));       // Obtener el comentario del movimiento
                    //nodo.Add(reader.GetDateTime(6));      // Obtener la fecha del movimiento
                    //resultado.Add(nodo);
                    _tablaReportes.Rows.Add(reader.GetSqlString(0), reader.GetSqlString(1), reader.GetSqlString(2), reader.GetSqlString(3), reader.GetSqlString(4), reader.GetString(5), reader.GetString(6));
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
            return _tablaReportes;
        }

        /// <summary>
        /// Método que se utiliza para consultar movimiento de activos por codigo y fecha
        /// </summary>
        /// <param name="codigo">string de codigo a consultar</param>
        /// <param name="fecha">DateTime de fecha a consultar</param>
        /// <returns>Una lista de listas de objetos, la cual contiene (codigo de activo, nombre de activo, tipo de movimiento
        /// postby del movimiento, solicitante del movimiento, comentario del movimiento y fecha del movimiento) </returns>

        public List<List<object>> consultarActivoPorCodigo2(string codigo)
        {
            List<List<object>> resultado = new List<List<object>>();
            List<object> nodo;
            SqlCommand obtener = new SqlCommand("SP_ACT_ConsultarActivoPorCodigo", _conexion);
            obtener.CommandType = CommandType.StoredProcedure;
            SqlParameter[] parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@codigo", SqlDbType.VarChar);
            parametros[0].Value = codigo;
            obtener.Parameters.AddRange(parametros);
            //DataTable _tablaReportes;
            //_tablaReportes = new DataTable("Datos");
            ////_tablaReportes.Columns.Add(new DataColumn("Id Activo"));
            //_tablaReportes.Columns.Add(new DataColumn("Codigo"));
            //_tablaReportes.Columns.Add(new DataColumn("Nombre"));
            //_tablaReportes.Columns.Add(new DataColumn("Tipo movimiento"));
            //_tablaReportes.Columns.Add(new DataColumn("PostBy"));
            //_tablaReportes.Columns.Add(new DataColumn("Solicitante"));
            //_tablaReportes.Columns.Add(new DataColumn("Comentario"));
            //_tablaReportes.Columns.Add(new DataColumn("Fecha"));
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = obtener.ExecuteReader();
                while (reader.Read())
                {
                    nodo = new List<object>();
                    nodo.Add(reader.GetString(0));      // Obtener el codigo de activo
                    nodo.Add(reader.GetString(1));      // Obtener el nombre del activo
                    nodo.Add(reader.GetString(2));    // Obtener el tipo de movimiento del activo
                    nodo.Add(reader.GetString(3));      // Obtener el postby del movimiento
                    nodo.Add(reader.GetString(4));       // Obtener el solicitante del movimiento
                    nodo.Add(reader.GetString(5));       // Obtener el comentario del movimiento
                    nodo.Add(reader.GetString(6));      // Obtener la fecha del movimiento
                    nodo.Add(reader.GetInt32(7));       // Obtener id de activo
                    nodo.Add(reader.GetInt32(8));       //Obtener estado de activo
                    resultado.Add(nodo);
                    //_tablaReportes.Rows.Add(reader.GetSqlString(0), reader.GetSqlString(1), reader.GetSqlString(2), reader.GetSqlString(3), reader.GetSqlString(4), reader.GetString(5), reader.GetString(6));
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

        #endregion
    }
}
