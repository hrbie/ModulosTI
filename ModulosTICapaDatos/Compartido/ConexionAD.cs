using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using ModulosTICapaLogica.Compartido;

namespace ModulosTICapaDatos.Compartido
{
	public class ConexionAD
	{
		#region Atributos

		private ListadoGruposLDAP _listadaGrupos;
		private ManejoBD _conexionBD; // Manejar conexión con la BD

		#endregion
          
		#region Constructor

		public ConexionAD()
		{
			_listadaGrupos = new ListadoGruposLDAP();
		}

		#endregion

		#region Métodos

		/// <summary>
		/// Método que crea un DirectoryEntry con configuraciones por defecto para poder conectarse con el servidor de Active Directory.
		/// </summary>
		/// <returns>Un DirectoryEntry con el que se puede realizar consultas a Active Directory</returns>
		
		private DirectoryEntry crearConexionActiveDirectory()
		{
			DirectoryEntry conexion = new DirectoryEntry(Constantes.AD_BASEDN);
			conexion.AuthenticationType = AuthenticationTypes.Secure;
			conexion.Username = Constantes.AD_USER;
			conexion.Password = Constantes.AD_PASS;
			return conexion;
		}

        //BASARSE EN ESTO PARA ARREGLAR TODO LO QUE SEA CON EL AD
        //Una mejor manera de hacerlo http://johnbarquin.wordpress.com/2008/06/12/servicios-de-directorio-en-net-35/

        /// <summary>
        /// Método que se encarga de crear un usuario estudiante en Active Directory
        /// </summary>
        /// <param name="estudiante">
        /// Los datos del estudiante (en un tipo Usuario) por ingresar a Active Directory
        /// </param>

        public Boolean crearEstudiante2(Usuario estudiante) {
            String nombre_completo = estudiante.Carnet + " " + estudiante.Nombre + " " + estudiante.Apellidos + " " + estudiante.Carrera;
            try	{
            
            PrincipalContext contextoDominio = new PrincipalContext(ContextType.Domain, Constantes.DOM, Constantes.AD_USER, Constantes.AD_PASS);
            UserPrincipal usuario = new UserPrincipal(contextoDominio, estudiante.UID, estudiante.Contrasena, true);
            usuario.SamAccountName = estudiante.UID;// LEGACY: Cuenta de estudiante Pre-Win2000
            usuario.UserPrincipalName = estudiante.UID + Constantes.DOMINIO;//Debe de contener el dominio
            usuario.GivenName = estudiante.Nombre;
            usuario.Surname = estudiante.Apellidos;
            usuario.DisplayName = nombre_completo;
            usuario.Description = "Estudiante";
            usuario.HomeDirectory = getHomeDirectoryAD(estudiante);
            usuario.EmailAddress = estudiante.Correo;
            usuario.HomeDrive = "M";
            usuario.PasswordNeverExpires = true;
            usuario.Save();
            usuario.SetPassword(estudiante.Contrasena);
            usuario.Save();
            return true;
            }
            catch (Exception e)
            {
                _conexionBD = new ManejoBD();
                _conexionBD.insertarBitacoraError(e.ToString(), "");
                return false;
            }
        }
        
		/// <summary>
		/// Método que se encarga de crear un usuario profesor en Active Directory
		/// </summary>
		/// <param name="estudiante">
		/// Los datos del profesor por ingresar a Active Directory
		/// </param>

        public Boolean crearProfesor2(Usuario profesor)
        {
            String nombre_completo = profesor.Carnet + " " + profesor.Nombre + " " + profesor.Apellidos;
            try
            {

                PrincipalContext contextoDominio = new PrincipalContext(ContextType.Domain, Constantes.DOM, Constantes.AD_USER, Constantes.AD_PASS);
                UserPrincipal usuario = new UserPrincipal(contextoDominio, profesor.UID, profesor.Contrasena, true);
                usuario.SamAccountName = profesor.UID;// LEGACY: Cuenta de estudiante Pre-Win2000
                usuario.UserPrincipalName = profesor.UID + Constantes.DOMINIO;//Debe de contener el dominio
                usuario.GivenName = profesor.Nombre;
                usuario.Surname = profesor.Apellidos;
                usuario.DisplayName = nombre_completo;
                usuario.Description = "Profesor";
                usuario.HomeDirectory = getHomeDirectoryAD(profesor);
                usuario.EmailAddress = profesor.Correo;
                usuario.HomeDrive = "M";
                usuario.PasswordNeverExpires = true;
                usuario.Save();
                usuario.SetPassword(profesor.Contrasena);
                usuario.Save();
                return true;
            }
            catch (Exception e)
            {
                _conexionBD = new ManejoBD();
                _conexionBD.insertarBitacoraError(e.ToString(), "");
                return false;
            }
        }
		/*public Boolean crearProfesor(Usuario profesor)
		{
			try
			{
				DirectoryEntry conexion = crearConexionActiveDirectory();
				DirectoryEntry nuevo_usuario = conexion.Children.Add("cn=" + profesor.UID, Constantes.AD_SCHEMA_USER);
				String nombre_completo = profesor.Carnet + " " + profesor.Nombre + " " + profesor.Apellidos;

				nuevo_usuario.Properties["sAMAccountName"].Add(profesor.UID); // LEGACY: Cuenta de estudiante Pre-Win2000
				nuevo_usuario.Properties["userPrincipalName"].Add(profesor.UID);
				nuevo_usuario.Properties["givenName"].Add(profesor.Nombre);
				nuevo_usuario.Properties["sn"].Add(profesor.Apellidos);
				nuevo_usuario.Properties["displayName"].Add(nombre_completo);
				nuevo_usuario.Properties["homePhone"].Value = profesor.TelefonoCasa;
				nuevo_usuario.Properties["mobile"].Value = profesor.TelefonoCelular;
				nuevo_usuario.Properties["mail"].Value = profesor.Correo;
				
				nuevo_usuario.Properties["info"].Value = (_listadaGrupos.Profesor.NombreGrupo);
				nuevo_usuario.Properties["description"].Add("Profesor");

                cambiarContraseña(nuevo_usuario, profesor.Contrasena);
				nuevo_usuario.CommitChanges();

				nuevo_usuario.Properties["homeDirectory"].Value = (Ruta.RUTAPROFESOR + profesor.UID);
				nuevo_usuario.Properties["homeDirectory"].Value = "M";
				nuevo_usuario.CommitChanges();

				nuevo_usuario.Close();
				nuevo_usuario.Dispose();
				conexion.Close();
				conexion.Dispose();
				return true;
			}
			catch (Exception e)
			{
				_conexionBD = new ManejoBD();
				_conexionBD.insertarBitacoraError(e.ToString(), "");
				return false;
			}
		}*/

		/// <summary>
		/// Devuelve la ruta del perfil del estudiante con base en su carné.
		/// </summary>
		/// <param name="estudiante">datos del estudiante</param>
		/// <returns>un string de la forma \\jordan\home\est*año del estudiante*\*login* </returns>
		
		private string getHomeDirectory(Usuario estudiante)
		{
			if (estudiante.Carnet.StartsWith("2"))
				return Ruta.RUTAESTUDIANTE + estudiante.Carnet.Substring(0, 4) + @"\" + estudiante.UID;
			else
				return Ruta.RUTAESTUDIANTE + "19" + estudiante.Carnet.Substring(0, 2) + @"\" + estudiante.UID;
		}

        private string getHomeDirectoryAD(Usuario estudiante)
        {
            if (estudiante.Grupo == "Profesor")
                return Ruta.RUTAPROFESOR_AD + estudiante.UID;
            if (estudiante.Carnet.StartsWith("2"))
                return Ruta.RUTAUSUARIO + estudiante.Carnet.Substring(0, 4) + @"\" + estudiante.UID;
            else
                return Ruta.RUTAUSUARIO + "19" + estudiante.Carnet.Substring(0, 2) + @"\" + estudiante.UID;
        }

		/// <summary>
		/// Comprueba la existencia de un UID cualquiera en Active Directory
		/// </summary>
		/// <param name="usuario">Nombre del estudiante TODO solo estudiantes?</param>
		/// <returns>1 si el estudiante existe, 0 sino existe, -1 si hubo un error</returns>
		
		public int existe(string usuario)
		{
			try
			{
				DirectoryEntry conexion = crearConexionActiveDirectory();
				DirectorySearcher search = new DirectorySearcher(conexion, "(sAMAccountName=" + usuario + ")"); // LEGACY: usa sAMAccountName
				SearchResult result = search.FindOne();

				search.Dispose();

				conexion.Close();
				conexion.Dispose();
				if (result != null)
					return 1; // Si el usuario existe
				else
					return 0; // Sino existe
			}
			catch (Exception e) // En caso de error
			{
				_conexionBD = new ManejoBD();
				_conexionBD.insertarBitacoraError(e.ToString(), "");
				return -1;
			}
		}


        /// <summary>
        /// Modifca los datos de un usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        /// 
        /*
        public Boolean modificarUsuario(Usuario usuario, Boolean passChance)
        {
            try
            {
                DirectoryEntry conexion = crearConexionActiveDirectory();
                DirectoryEntry nuevo_usuario = conexion.Children.Add("cn=" + usuario.UID, Constantes.AD_SCHEMA_USER);
                String nombre_completo = usuario.Carnet + " " + usuario.Nombre + " " + usuario.Apellidos + " " + usuario.Carrera;
                
                cambiarDatoUsuario(conexion, "sAMAccountName", usuario.UID);
                cambiarDatoUsuario(conexion, "userPrincipalName", usuario.UID);
                cambiarDatoUsuario(conexion, "givenName", usuario.Nombre);
                cambiarDatoUsuario(conexion, "sn", usuario.Apellidos);
                cambiarDatoUsuario(conexion, "displayName", nombre_completo);
                cambiarDatoUsuario(conexion, "homePhone", usuario.TelefonoCasa);
                cambiarDatoUsuario(conexion, "mobile", usuario.TelefonoCelular);
                cambiarDatoUsuario(conexion, "mail", usuario.Correo);

                if(usuario.Grupo == "Estudiante") {
                    cambiarDatoUsuario(conexion, "homeDirectory", Ruta.RUTAPROFESOR + usuario.UID);
                    cambiarDatoUsuario(conexion, "info", nombre_completo +_listadaGrupos.Estudiante.NombreGrupo);
                }
                else {
                    cambiarDatoUsuario(conexion, "homeDirectory", Ruta.RUTAESTUDIANTE + usuario.UID);
                    cambiarDatoUsuario(conexion, "info", _listadaGrupos.Profesor.NombreGrupo);
                }
                cambiarDatoUsuario(conexion, "description", usuario.Grupo);
                if(passChance)
                    cambiarContraseña(nuevo_usuario, usuario.Contrasena);
                conexion.Close();
                conexion.Dispose();
                return true;
            }
            catch (Exception e)
            {
                _conexionBD = new ManejoBD();
                _conexionBD.insertarBitacoraError(e.ToString(), "");
                return false;
            }
        }
        */
        public Boolean modificarUsuario2(Usuario usuario, Boolean passChance)
        {
            String nombre_completo = usuario.Carnet + " " + usuario.Nombre + " " + usuario.Apellidos + " " + usuario.Carrera;
            try
            {
                PrincipalContext contextoDominio = new PrincipalContext(ContextType.Domain, Constantes.DOM, Constantes.AD_USER, Constantes.AD_PASS);
                UserPrincipal usuarioAD = UserPrincipal.FindByIdentity(contextoDominio, usuario.UID);
                
                usuarioAD.SamAccountName = usuario.UID;// LEGACY: Cuenta de estudiante Pre-Win2000
                usuarioAD.UserPrincipalName = usuario.UID + Constantes.DOMINIO;//Debe de contener el dominio
                usuarioAD.GivenName = usuario.Nombre;
                usuarioAD.Surname = usuario.Apellidos;
                usuarioAD.DisplayName = nombre_completo;
                usuarioAD.Description = "usuario";
                usuarioAD.HomeDirectory = getHomeDirectoryAD(usuario);
                usuarioAD.EmailAddress = usuario.Correo;
                usuarioAD.HomeDrive = "H";
                usuarioAD.PasswordNeverExpires = true;
                
                usuarioAD.Save();
                usuarioAD.SetPassword(usuario.Contrasena);
                usuarioAD.Save();
                return true;
            }
            catch (Exception e)
            {
                _conexionBD = new ManejoBD();
                _conexionBD.insertarBitacoraError(e.ToString(), "");
                return false;
            }
        }
      
		/// <summary>
		/// Método que se encarga de cambiar la contraseña de un usuario
		/// </summary>
		/// <param name="userName">estudiante</param>
		/// <param name="contraseña">contraseña nueva</param>
		
		public void cambiarContraseña(DirectoryEntry conexion, String password)
		{
            try
            {
                conexion.Invoke("SetPassword", new object[] { password });
                conexion.Properties["LockOutTime"].Value = 0;
                MarcadorUAC.desactivar(conexion, MarcadorUAC.ACCOUNT_DISABLE); // Habilitar una cuenta
                //MarcadorUAC.activar(conexion, MarcadorUAC.NORMAL_ACCOUNT);
            }
            catch (Exception e)
            {
                _conexionBD = new ManejoBD();
                _conexionBD.insertarBitacoraError(e.ToString(), "");
            }
		}
        /// <summary>
        /// Cambia los datos de un usuario
        /// </summary>
        /// <param name="conexion">Conexión con el AD</param>
        /// <param name="propiedad">Parametro a modifica</param>
        /// <param name="valor">Valor del parametro</param>
        public void cambiarDatoUsuario(DirectoryEntry conexion, String propiedad, String valor) {
            conexion.Invoke(propiedad, new object[] { valor });
			conexion.Properties["LockOutTime"].Value = 0;
        }

		#endregion
	}
}
