using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using ModulosTICapaLogica.Compartido;
using System.Data;
using ModulosTICapaDatos.Compartido;

namespace ModulosTICapaDatos.ModuloSGC
{
	public class ManejoBDSGC
	{
		#region Atributos

		private SqlConnection _conexion; // Manejo de conexión con la BD
        private ManejoBD _conexionBD;
        
		#endregion

		#region Constructor

		/// <summary>
		/// Constructor de la clase ManejoBD
		/// Este constructor indica los terminos de la conexión con la Base da Datos
		/// </summary>

		public ManejoBDSGC()
		{
			ConnectionStringSettings _settings = ConfigurationManager.ConnectionStrings["ModulosTI"]; // Propiedas del string de conexión almacenado en el .config
			_conexion = new SqlConnection(_settings.ConnectionString);
            _conexionBD = new ManejoBD();
		}

		#endregion

		#region Métodos

        /// <summary>
        /// Método que se encarga de crear un nuevo usuario en la base de datos
        /// </summary>
        /// <param name="usuario">Objeto usuario</param>
        /// <param name="imagen">Foto del usuario si es proporcionada</param>
        /// <returns>Retorna un número entero (1: fue exitoso, 0: fallo, -1: error al intentar crearlo)</returns>

        public int crearUsuario(Usuario usuario, byte[] imagen)
        {
            SqlCommand comando = new SqlCommand("SP_COM_InsertarUsuario", _conexion);
            SqlParameter[] parametros = new SqlParameter[3];
            comando.CommandType = CommandType.StoredProcedure;
            parametros[0] = new SqlParameter("@login", SqlDbType.VarChar);
            parametros[0].Value = usuario.UID;
            parametros[1] = new SqlParameter("@Carrera", SqlDbType.VarChar);
            parametros[1].Value = usuario.Carrera;
            parametros[2] = new SqlParameter("@foto", SqlDbType.Image);
            if (imagen != null)
                parametros[2].Value = imagen;
            else
                parametros[2].Value = DBNull.Value;
            comando.Parameters.AddRange(parametros);
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = comando.ExecuteReader();
                Boolean resultado = !(reader.HasRows);
                if (resultado)  
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
            catch (Exception e)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _conexionBD.insertarBitacoraError(e.ToString(), "");
                return -1;
            }
        }


        public int modificarUsuario(String clave, byte[] imagen, String Carrera)
        {
            int res = 1;
            var modificar = new SqlCommand("SP_COM_ModificarUsuario", _conexion) { CommandType = CommandType.StoredProcedure };
            var parametros = new SqlParameter[3];
            parametros[0] = new SqlParameter("@login", SqlDbType.VarChar);
            parametros[0].Value = clave;
            parametros[1] = new SqlParameter("@carrera", SqlDbType.VarChar);
            parametros[1].Value = Carrera;
            parametros[2] = new SqlParameter("@foto", SqlDbType.Image);
            if (imagen != null)
                parametros[2].Value = imagen;
            else
                parametros[2].Value = DBNull.Value;
          
			
            modificar.Parameters.AddRange(parametros);
            try
            {
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();

                SqlDataReader reader = modificar.ExecuteReader(); 
                _conexion.Close();

                return res;
            }
            catch (Exception e)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _conexionBD.insertarBitacoraError(e.ToString(), "");
                return 0;
            };   
        }


        public int consultarUsuario(String clave) {
               
            var consultar = new SqlCommand("SP_COM_ConsultarUsuario", _conexion) { CommandType = CommandType.StoredProcedure };
            var parametros = new SqlParameter[1];
            parametros[0] = new SqlParameter("@login", SqlDbType.VarChar) { Value = clave };

            int res = 1;
            consultar.Parameters.AddRange(parametros);
            try
            {
                if (_conexion.State == ConnectionState.Closed)
                    _conexion.Open();

                SqlDataReader reader = consultar.ExecuteReader();
           
                while (reader.Read())
                {
                    res = reader.GetInt32(2);
                    
                }
                _conexion.Close();

                return res;
            }
            catch (Exception e)
            {
                if (_conexion.State == ConnectionState.Open)
                    _conexion.Close();
                _conexionBD.insertarBitacoraError(e.ToString(), "");
                return -1;
            };   
        }

        public List<List<object>> consultarUsuariosPorFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            //revisar
            List<List<object>> resultado = new List<List<object>>();
            List<object> nodo;
            var consultar = new SqlCommand("SP_COM_ConsultarUsuariosPorFecha", _conexion) { CommandType = CommandType.StoredProcedure };
            var parametros = new SqlParameter[2];
            parametros[0] = new SqlParameter("@FechaInicial", SqlDbType.Date) { Value = fechaInicial };
            parametros[1] = new SqlParameter("@FechaFinal", SqlDbType.Date) { Value = fechaFinal };
                        
            consultar.Parameters.AddRange(parametros);
                                    
            if (_conexion.State == ConnectionState.Closed)
                _conexion.Open();
            try
            {
                SqlDataReader reader = consultar.ExecuteReader();
                while (reader.Read())
                {
                    nodo = new List<object>();
                    nodo.Add(reader.GetString(0)); // Obtener el Login del usuario
                    nodo.Add(reader.GetString(1)); // Obtener el acrónimo de la Carrera
                    nodo.Add(reader.GetDateTime(2).Day.ToString() + "/" + reader.GetDateTime(2).Month.ToString() + "/" + reader.GetDateTime(2).Year.ToString()); //Obtener la fecha de creacion
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

        #endregion
	}
}
