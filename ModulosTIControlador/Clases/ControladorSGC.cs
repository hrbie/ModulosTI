using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaLogica.Compartido;
using ModulosTICapaLogica.ModuloSGC;
using ModulosTICapaDatos.ModuloSGC;
using System.IO;
using System.Net;


namespace ModulosTIControlador.Clases
{
	public class ControladorSGC
	{
		#region Atributos

		private ConexionLDAP _conexionLDAP; // Manejar conexiones con el LDAP
		private ConexionAD _conexionAD; // Manejar conexiones con Active Directory
		private ManejoBD _conexionBD; // Manejar conexiones con la base de datos para datos compartidos
        private ManejoBDSGC _conexionSGC; // Manejo con la base de datos para las tablas directamente relacionadas con SGC
		public static List<List<String>> usuarios = new List<List<String>>();  //Lista de las cuentas a generar.

		#endregion

		#region Contructor

		public ControladorSGC()
		{
			_conexionLDAP = new ConexionLDAP();
            _conexionAD = new ConexionAD();
			_conexionBD = new ManejoBD();
            _conexionSGC = new ManejoBDSGC();
		}

		#endregion

		#region Métodos

		/// <summary>
		/// Método que se encarga de crear un usuario en el sistema
		/// </summary>
		/// <param name="datosUsuario">Lista que contiene los datos del usuario a crear dela forma:
		/// List(Nombre:string, Apellidos:String, Carnet:int, TelefonoCasa:String, TelefonoCelular:String, UID:String,
		/// Contrasena:String, Correo:String)</param>
		/// <param name="tipoUsuario">Indica que tipo de usuario es el se va a crear (profesor o estudiante).
		/// True: estudiante, false: profesor</param>
		/// <returns>Un valor booleano, true en caso de éxito, false en caso contrario</returns>

		public int crearUsuario(List<string> datosUsuario, Boolean tipoUsuario, byte[] foto)
		{
			Notificacion notificacion = new Notificacion();
			Usuario usuario = new Usuario();
			usuario.Nombre = datosUsuario.ElementAt(0);
			usuario.Apellidos = datosUsuario.ElementAt(1) + " " + datosUsuario.ElementAt(2);
			usuario.Carnet = datosUsuario.ElementAt(3);
			usuario.TelefonoCasa = datosUsuario.ElementAt(4);
			usuario.TelefonoCelular = datosUsuario.ElementAt(5);
			usuario.Correo = datosUsuario.ElementAt(6);
            usuario.Carrera = datosUsuario.ElementAt(7);
			usuario.UID = datosUsuario.ElementAt(8);
			usuario.Contrasena = datosUsuario.ElementAt(9);
			
			if (tipoUsuario)
			{

                //_conexionLDAP.crearEstudiante(usuario)

                //debug paso a paso como se crea el usuario (AD, LDAP)
                if (_conexionLDAP.crearEstudiante(usuario)) // Si se pudo crear en el LDAP 
				{
					//Definición del correo de bienvenida para los nuevos estudiantes. 
					
					String ms = "Bienvenido a la Escuela de Ingeniería en Computación, los datos de su cuenta son: \n\nUsuario: " + usuario.UID + "\nContraseña: " + usuario.Contrasena + "\n\n";
					String msDescripcion = "\n\nLa siguiente es información que le será de utilidad:";
                    String msCorreo = "\n\nCorreo de la Oficina de TI: soporte@ic-itcr.ac.cr";
					String msForo = "\n\nForo: https://www.ic-itcr.ac.cr/foro";
					String msWebmail = "\n\nCorreo electrónico: webmail.ic-itcr.ac.cr";
					String msAlianza = "\n\nCorreo de la Alianza con Microsoft: alianzas@ic-itcr.ac.cr";
					String msLinux = "\n\nEncargado de las tecnologías basadas en GNU Linux: Mario Castro (mcastro@ic-itcr.ac.cr)";
					String msWindows = "\n\nEncargado de las tecnologías de Microsoft: Kenneth Díaz (kdiaz@ic-itcr.ac.cr)";
					String asunto = "Creación de cuenta en la Escuela de Ingeniería en Computación";
					ms = ms + msDescripcion + msCorreo + msForo + msWebmail + msAlianza + msLinux + msWindows;

                    //revisar correo a enviar
				    notificacion.enviarCorreo(ms, usuario.Correo, asunto);
                    
                    //_conexionAD.crearEstudiante(usuario)
                    if (_conexionAD.crearEstudiante2(usuario))
                    {
                        if (_conexionSGC.crearUsuario(usuario, foto) == 1) 
                            return 4;  // Se creo con éxito el estudiante en los directorios y en la BD del sistema
                        else
                            return 2; // Exito al crear al estudiante en AD y LDAP pero no en la BD del sistema
                    }
                    else
                        return 3; // Se creo bien en el LDAP pero no en AD
			    }
				else // Si no se pudo crear en el LDAP
					return 1;
			}
			else
			{
                if (_conexionLDAP.crearProfesor(usuario)) // Si se pudo crear en el LDAP
                {
                    if (_conexionAD.crearProfesor2(usuario))
                    {
                        if (_conexionSGC.crearUsuario(usuario, foto) == 1)
                            return 4;  // Se creo con éxito el profesor en los directorios y en la BD del sistema
                        else
                            return 2; // Exito al crear al profesor en AD y LDAP pero no en la BD del sistema
                    }
                    else
                        return 3; // Se creo bien en el LDAP pero no en AD
                }
                else // Si no se pudo crear en el LDAP
                    return 1;
			}
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
        /// Método que se encarga de llamar al método que busca un nombre de usuario disponible según opciones generadas en éste método
        /// </summary>
        /// <param name="nombreUsuario">Nombre de la persona</param>
        /// <param name="primerApellidoUsuario">Primer apellido de la persona</param>
        /// <returns>Null: en caso de error, "" en caso de no haber ninguna combinación posible, String que indica el login del nuevo usuario</returns>

        public String buscarNombreUsuario(string nombreUsuario, string primerApellidoUsuario)
        {
            try
            {
                String[] nombre = nombreUsuario.Split(' '); // Obtener solo un nombre, en caso de que hayan sido especificado mas de uno
                String nuevoLogin = "";
                Boolean encontrado = false;
                for (int i = 0; i < nombre[0].Length; i++) // Itera según el largo del nombre que indica las posibles combinaciones del login
                {
                    nuevoLogin = nombre[0].Substring(0, i + 1) + primerApellidoUsuario; // Se forma la propuesta del posible login
                    encontrado = _conexionLDAP.buscarNombreUsuario(nuevoLogin); // Realizar la consulta en el LDAP de si el login esta disponible
                    if (!encontrado)
                        break; // Si ese login aún no ha sido asignado se termina el ciclo puesto que se asignará dicho login al nuevo usuario
                }
                if (!encontrado)
				{
					nuevoLogin = nuevoLogin.ToLower(); // Cambiar a minúsculas todas las letras que conforman el login
                    return nuevoLogin; // Se retorna el login encontrado para el usuario
				}
                else
                    return ""; // En caso de si ya no hubieran posibilidades para formar el login del nuevo usuario
            }
            catch (Exception e)
            {
                insertarBitacoraError(e.ToString(), ""); // Enviar el error a la bitácora de error del sistema
                return null;
            }
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
        /// Método que se encarga de obtener los datos de un usuario
        /// </summary>
        /// <param name="login">Login de la persona</param>
        /// <param name="tipoBusqueda">Puede ser por login o carnet</param>
        /// <returns>
        /// Todos los datos de un usuario
        /// </returns>

        public Usuario buscarUsuario(string clave, Boolean tipoBusqueda)
        {
            Usuario _user = null;
            if (!tipoBusqueda)
                clave = _conexionLDAP.buscarUsuarioPorCarnet(clave);

            if (clave != String.Empty && _conexionLDAP.buscarNombreUsuario(clave))
            {
                _user = _conexionLDAP.buscarUsuario(clave);
            }
            return _user;
        }


        /// <summary>
        /// Método que se encarga de modificar un usuario en el sistema
        /// </summary>
        /// <param name="datosUsuario">Lista que contiene los datos del usuario a modificar dela forma:
        /// List(Nombre:string, Apellidos:String, Carnet:int, TelefonoCasa:String, TelefonoCelular:String, UID:String,
        /// Contrasena:String, Correo:String)</param>
        /// <param name="tipoUsuario">Indica que tipo de usuario es el se va a modificar
        /// True: estudiante, false: profesor</param>
        /// <returns>Un valor booleano, true en caso de éxito, false en caso contrario</returns>

        public int modificarUsuario(List<string> datosUsuario, Boolean tipoBusqueda, byte[] foto, Boolean changePass)
        {
            Usuario usuario = new Usuario();
            usuario.Nombre = datosUsuario.ElementAt(0);
            usuario.Apellidos = datosUsuario.ElementAt(1)+" "+datosUsuario.ElementAt(2);
            usuario.Carnet = datosUsuario.ElementAt(3);
            usuario.TelefonoCasa = datosUsuario.ElementAt(4);
            usuario.TelefonoCelular = datosUsuario.ElementAt(5);
			usuario.UID = datosUsuario.ElementAt(6);
			usuario.Contrasena = datosUsuario.ElementAt(7);
			usuario.Correo = datosUsuario.ElementAt(8);
			usuario.Carrera = datosUsuario.ElementAt(9);
            usuario.Grupo = datosUsuario.ElementAt(10);
             // El criterio seleccionado es "Login"
            if (!tipoBusqueda)
                usuario.UID = _conexionLDAP.buscarUsuarioPorCarnet(usuario.UID);

            if (_conexionLDAP.modificarUsuario(usuario, changePass)) // Si se pudo modificar en el LDAP
            {
                if (_conexionAD.modificarUsuario2(usuario,changePass))
                {
                    if (_conexionSGC.modificarUsuario(usuario.UID, foto, usuario.Carrera) == 1)
                        return 4;  // Se modifico con éxito el estudiante en los directorios y en la BD del sistema
                    else
                        return 2; // Exito al modificar al estudiante en AD y LDAP pero no en la BD del sistema
                }
                else
                    return 3; // Se modifico bien en el LDAP pero no en AD
            }
            else // Si no se pudo modifico en el LDAP
                return 1;
         
       }

		/// <summary>
		/// Método que se encarga de modificar la contraseña de un usuario en el sistema
		/// </summary>
		/// <param name="datosUsuario">Lista que contiene los datos del usuario a modificar dela forma:
		/// List(Nombre:string, Apellidos:String, Carnet:int, TelefonoCasa:String, TelefonoCelular:String, UID:String,
		/// Contrasena:String, Correo:String)</param>
		/// <param name="tipoUsuario">Indica que tipo de usuario es el se va a modificar
		/// True: estudiante, false: profesor</param>
		/// <returns>Un valor booleano, true en caso de éxito, false en caso contrario</returns>

		public int modificarPassword(List<string> datosUsuario, Boolean tipoBusqueda, Boolean changePass)
		{
			Usuario usuario = new Usuario();
			usuario.Nombre = datosUsuario.ElementAt(0);
			usuario.Apellidos = datosUsuario.ElementAt(1);
			usuario.Carnet = datosUsuario.ElementAt(2);
			usuario.TelefonoCasa = datosUsuario.ElementAt(3);
			usuario.TelefonoCelular = datosUsuario.ElementAt(4);
			usuario.UID = datosUsuario.ElementAt(5);
			usuario.Contrasena = datosUsuario.ElementAt(6);
			usuario.Correo = datosUsuario.ElementAt(7);
			usuario.Carrera = datosUsuario.ElementAt(8);
			usuario.Grupo = datosUsuario.ElementAt(9);
			// El criterio seleccionado es "Login"
			if (!tipoBusqueda)
				usuario.UID = _conexionLDAP.buscarUsuarioPorCarnet(usuario.UID);

			if (_conexionLDAP.modificarUsuario(usuario, changePass)) // Si se pudo modificar en el LDAP
			{
                if (_conexionAD.modificarUsuario2(usuario,changePass)){
					if (_conexionSGC.modificarUsuario(usuario.UID, null, usuario.Carrera) == 1) //null pq no hay imagen.
						return 4;  // Se modifico con éxito el estudiante en los directorios y en la BD del sistema
					else
						return 2; // Exito al modificar al estudiante en AD y LDAP pero no en la BD del sistema
				}
				else
					return 3; // Se modifico bien en el LDAP pero no en AD
			}
			else // Si no se pudo modifico en el LDAP
				return 1;

		}


		/// <summary>
		/// Método que se encarga de autenticar un usuario
		/// </summary>
		/// <param name="login">Login del usuario</param>
		/// <param name="password">Password del usuario</param>
		/// <returns>Retorna true en caso de que la autenticación sea exitosa, false en caso contrario</returns>

		public bool validarUsuario(string login, string password)
		{
			GrupoLDAP grupo = _conexionLDAP.autenticarUsuario(login, password);

			if (grupo != null) // Si es válido
			{
				return true;
			}
			else // Si no es válido
				return false;
		}


		/// <summary>
		/// Método que se encarga de procesar un documento de Excel para generar nuevas cuentas.
		/// </summary>
		/// <param name="savePath">Dirección donde se guardó el documento</param>
		/// <param name="nombreExcel">Nombre del archivo de Excel ya guardado</param>
		/// <param name="strFileType">Extensión del documento</param>
		/// <param name="strNewPath">Dirección completa del nuevo archivo.</param>
		/// <returns></returns>

		public List<ErroresExcel> validarDatosExcel(String savePath, String nombreExcel, String strFileType, String strNewPath)
		{
			OleDbConnection conn = new OleDbConnection();
			OleDbCommand cmd = new OleDbCommand();
			OleDbDataAdapter da = new OleDbDataAdapter();
			DataSet ds = new DataSet();
			List<ErroresExcel> errores = new List<ErroresExcel>();
			ErroresExcel error;
			string query = null;
			string connString = "";
			int linea = 2; //Número de la fila
			bool insertar = true;
			char[] badChars = { 'á', 'é', 'í', 'ó', 'ú', 'ñ' };
			char[] goodChars = { 'a', 'e', 'i', 'o', 'u', 'n' };

			try
			{

				//Conexión al workbook de Excel
				if (strFileType.Trim() == ".xls")
				{
					connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
				}
				else if (strFileType.Trim() == ".xlsx")
				{
					connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strNewPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
				}

				query = "SELECT * FROM [cuentas$]";

				//Crea la conexión al objeto
				conn = new OleDbConnection(connString);
				//Abre la conexión
				if (conn.State == ConnectionState.Closed)
					conn.Open();
				//Crea el objeto de comando
				cmd = new OleDbCommand(query, conn);
				da = new OleDbDataAdapter(cmd);
				ds = new DataSet();
				da.Fill(ds, "cuentas");

				DataTable dt = ds.Tables["cuentas"];

				foreach (DataRow row in dt.Rows)
				{
					List<string> datosUsuario = new List<string>();
					string contra = "";
					string login = "";
					string dato = "";

					//Se recorren las columnas de la fila.
					foreach (DataColumn col in dt.Columns)
					{
						dato = row[col].ToString();

						//Se valida que los campos no estén vacíos
						if (dato == "")
						{
							error = new ErroresExcel();
							error.linea = linea;
							error.dato = dato;
							error.mensaje = "Hay un dato faltante.";
							errores.Add(error);
							datosUsuario.Add(dato);
							insertar = false;
						}
						else if (dato == "fin")
						{
							da.Dispose();
							conn.Close();
							conn.Dispose();
							return errores;
						}
						else
						{
							datosUsuario.Add(dato);
						}							
					}

					
					for (int i = 0; i < badChars.Length; i++) // Limpiar caracteres del nombre de cada estudiante
					{

						datosUsuario[0] = datosUsuario[0].Replace(badChars[i], goodChars[i]); // Quitar tildes u caracteres especiales del nombre
						datosUsuario[1] = datosUsuario[1].Replace(badChars[i], goodChars[i]); // Quitar tildes u caracteres especiales de los apellidos
						datosUsuario[2] = datosUsuario[2].Replace(badChars[i], goodChars[i]); // Quitar tildes u caracteres especiales de los apellidos
					}


					//Validar que el nombre sea correcto.
					if (!Regex.IsMatch(datosUsuario[0], "^[A-Za-z ]+$"))
					{
						error = new ErroresExcel();
						error.linea = linea;
						error.dato = datosUsuario[0];
						error.mensaje = "El nombre solo debe de contener letras.";
						errores.Add(error);
						insertar = false;
					}

					//Validar que el primer apellido sea correcto.
					if (!Regex.IsMatch(datosUsuario[1], "^[A-Za-z ]+$"))
					{
						error = new ErroresExcel();
						error.linea = linea;
						error.dato = datosUsuario[1];
						error.mensaje = "El nombre solo debe de contener letras.";
						errores.Add(error);
						insertar = false;
					}

					//Validar que el segundo apellido sea correcto.
					if (!Regex.IsMatch(datosUsuario[2], "^[A-Za-z ]+$"))
					{
						error = new ErroresExcel();
						error.linea = linea;
						error.dato = datosUsuario[2];
						error.mensaje = "El nombre solo debe de contener letras.";
						errores.Add(error);
						insertar = false;
					}


					//Validar que el carnet sea correcto.
					if (!Regex.IsMatch(datosUsuario[3], "^[0-9]+$"))
					{
						error = new ErroresExcel();
						error.linea = linea;
						error.dato = datosUsuario[3];
						error.mensaje = "El carnet debe de estar formado por nueve dígitos.";
						errores.Add(error);
						insertar = false;
					}


					//Validar que el telefono sea correcto.
					if (!Regex.IsMatch(datosUsuario[4], "^[0-9]+$"))
					{
						error = new ErroresExcel();
						error.linea = linea;
						error.dato = datosUsuario[4];
						error.mensaje = "El número de teléfono debe de estar formado por ocho dígitos";
						errores.Add(error);
						insertar = false;
					}

					//Validar que el celular sea correcto.
					if (!Regex.IsMatch(datosUsuario[5], "^[0-9]+$"))
					{
						error = new ErroresExcel();
						error.linea = linea;
						error.dato = datosUsuario[5];
						error.mensaje = "El número de celular debe de estar formado por ocho dígitos";
						errores.Add(error);
						insertar = false;
					}

					//Validar que el correo sea correcto.
					if (!Regex.IsMatch(datosUsuario[6], @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
					{
						error = new ErroresExcel();
						error.linea = linea;
						error.dato = datosUsuario[6];
						error.mensaje = "El correo electrónico tiene un formato inválido";
						errores.Add(error);
						insertar = false;
					}

					//Validar que la carrera sea correcto.
                    if (!Regex.IsMatch(datosUsuario[7], "^ATI|IC|CE|OT$"))
					{
						error = new ErroresExcel();
						error.linea = linea;
						error.dato = datosUsuario[7];
						error.mensaje = "La carrera no es válida";
						errores.Add(error);
						insertar = false;
					}

					login = buscarNombreUsuario(datosUsuario[0], datosUsuario[1]);
					//Verificar que el login se generó
					if (login != "")
					{
						contra = login + datosUsuario[3] + datosUsuario[7];
						//Se agregan los valores generados a la lista de datos del usuario.
						datosUsuario.Add(login);
						datosUsuario.Add(contra);
					}
					else
					{
						error = new ErroresExcel();
						error.linea = linea;
						error.dato = login;
						error.mensaje = "No fue posible generar un login para el usuario, intente utilizando el segundo nombre.";
						errores.Add(error);
						insertar = false;
					}
                    	
                    //Si los datos del usuario son correctos.
					if (insertar)
					{
						//Se agregan todos los datos del usuario a la lista de usuarios.
						usuarios.Add(datosUsuario);
					}
					
					linea++;
					insertar = true;
				}
				
				da.Dispose();
				conn.Close();
				conn.Dispose();
				return errores;
			}
			catch (Exception ex)
			{
				insertarBitacoraError(ex.ToString(), ""); // Enviar el error a la bitácora de error del sistema
				return errores;
			}
		}

		/// <summary>
		/// Método que se encarga de procesar los datos de las cuentas recuperados del archivo de Excel.
		/// </summary>
		/// <returns>Lista con el estado de creación de cada cuenta</returns>

		public List<string> generarCuentas()
		{
			List<string> estados = new List<string>();
			int resultado;
			int contador = 1;
			string mensaje = "";

			foreach (List<String> usuario in usuarios)
			{
				//Se crea la cuenta.
				resultado = crearUsuario(usuario, true, null);

				switch (resultado)
				{
					case 1:
						mensaje = "Fila " + contador + ": Se ha producido un error al crear la cuenta en el LDAP\n";
                        //enviar correo
						estados.Add(mensaje);
						break;
					case 2:
						mensaje = "Fila " + contador + ": La cuenta fue creada en el LDAP y Active Directory pero no en la base del Sistema\n";
						estados.Add(mensaje);
						break;
					case 3:
						mensaje = "Fila " + contador + ": La cuenta fue creada en el LDAP pero no en Active Directory\n";
						estados.Add(mensaje);
						break;
					case 4:
						mensaje = "Fila " + contador + ": La cuenta ha sido creada con éxito\n";
						estados.Add(mensaje);
						break;
				}
				contador++;
			}

            usuarios = new List<List<String>>();

			return estados;
		}
		
		

        public List<List<string>> crearArchivoUsuariosPorFecha(DateTime _fechaInicial, DateTime _fechaFinal)
        {
            List<List<object>> resultado = _conexionSGC.consultarUsuariosPorFecha(_fechaInicial, _fechaFinal);
            List<List<string>> contenido = new List<List<string>>();
            List<string> fila = new List<string>();

            foreach (List<object> i in resultado)
            {
                foreach (object j in i)
                {
                    fila.Add(j.ToString());
                }
                contenido.Add(fila);
                fila = new List<string>();
            }

            return contenido;     
        }
        #endregion
    }
}
