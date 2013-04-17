using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.DirectoryServices.Protocols;
using System.DirectoryServices;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaLogica.Compartido;

namespace ModulosTICapaDatos.Compartido
{
	public class ConexionLDAP
	{
		#region Atributos

		private ListadoGruposLDAP _listadaGrupos;
        private ManejoBD _conexionBD;   // Manejador para conectarse a la base de datos
		#endregion

		#region Constructor

		public ConexionLDAP() 
		{
			_listadaGrupos = new ListadoGruposLDAP();
		}

		#endregion

		#region Métodos

		/// <summary>
		/// Crea un DirectoryEntry para poder conectar con el servidor openLDAP.
		/// </summary>
		/// <param name="LDAP_Addr">
		/// Direccion del servidor openLDAP. Formato: LDAP://<ip servidor>/<DN base>
		/// </param>
		/// <param name="Username">
		/// Usuario con el que se va a autenticar en el servidor Formato: "cn=admin,dc=ic-itcr,dc=ac,dc=cr" (DN Completo)
		/// </param>
		/// <param name="Password">
		/// Contraseña de dicho usuario
		/// </param>
		/// <returns>
		/// Retorna un DirectoryEntry con el cual se pueden realizar las consultas necesarias
		/// </returns>  

		public DirectoryEntry crearConexion(string ldapAddr, string username, string password)
		{
			DirectoryEntry connection = new DirectoryEntry(ldapAddr);
			connection.AuthenticationType = AuthenticationTypes.None;
			connection.Username = username;
			connection.Password = password;
			return connection;
		}

		/// <summary>
		/// Método que se encarga de obtener el nombre de una persona a partir de su nombre de usuario (login)
		/// </summary>
		/// <param name="nombreUsuario">Nombre de usuario (login)</param>
		/// <returns>Nombre de la persona</returns>

		public String obtenerNombrePersona(string nombreUsuario)
		{
			LdapDirectoryIdentifier serverInfo = new LdapDirectoryIdentifier(Constantes.LDAP_SERVER);
			LdapConnection openLdap = new LdapConnection(Constantes.LDAP_SERVER);
			try
			{
				String nombrePersona;
				// Crear conexion con LDAP
				openLdap.Credential = new System.Net.NetworkCredential(Constantes.LDAP_USER, Constantes.LDAP_PASS);
				openLdap.AuthType = AuthType.Basic;
				openLdap.SessionOptions.ProtocolVersion = 3; // Hay que usar LDAPv3
				openLdap.Bind(); //Conectar
				string[] attributesToReturn = new string[] { "displayName" }; // Atributos a retornar
				// Buscar al usuario por su login
				SearchRequest searchRequest = new SearchRequest("ou=people,dc=ic-itcr,dc=ac,dc=cr", "(uid=" + nombreUsuario + "*)",
					System.DirectoryServices.Protocols.SearchScope.Subtree, attributesToReturn);
				SearchResponse searchResponse = (SearchResponse)openLdap.SendRequest(searchRequest); // Respuesta del servidor
				DirectoryAttribute atributo = searchResponse.Entries[0].Attributes["displayName"];
				object[] objeto = atributo.GetValues(Type.GetType("System.Byte[]"));
				nombrePersona = Encoding.ASCII.GetString((byte[])objeto[0]);
				openLdap.Dispose(); // Liberar recursos
				return nombrePersona;
			}
			catch (Exception e)
			{
				openLdap.Dispose();
				_conexionBD = new ManejoBD();
				_conexionBD.insertarBitacoraError(e.ToString(), "");
				return null;
			}
		}

        /// <summary>
        /// Método que recupera del LDAP el nombre, login y correo de los miembros de la Oficina de TI.
        /// </summary>
        /// <returns>Lista de Usuarios</returns>

        public List<Usuario> obtenerListaSoporte()
        {
            List<Usuario> _resultado = new List<Usuario>();     //Lista de usuarios con los datos de los miembros de la Oficina de TI
            List<String> _logins = new List<String>();          //Lista con los logins de los miembros de la Oficina de TI

            #region buscar login

            //Busca los memberUid del grupo soporte
            int _conta = 0;
            // Crear conexion con LDAP
            LdapDirectoryIdentifier serverInfo = new LdapDirectoryIdentifier(Constantes.LDAP_SERVER);
            LdapConnection openLdap = new LdapConnection(Constantes.LDAP_SERVER);
            openLdap.Credential = new System.Net.NetworkCredential(Constantes.LDAP_USER, Constantes.LDAP_PASS);
            openLdap.AuthType = AuthType.Basic;
            openLdap.SessionOptions.ProtocolVersion = 3; // Hay que usar LDAPv3
            openLdap.Bind(); //Conectar
            string[] attributesToReturn = new string[] { "memberUid" }; // Atributos a retornar
            SearchRequest searchRequest = new SearchRequest("ou=group,dc=ic-itcr,dc=ac,dc=cr", "(cn=soporte)",
                System.DirectoryServices.Protocols.SearchScope.Subtree, attributesToReturn);     //Filtro de busqueda.
            SearchResponse searchResponse = (SearchResponse)openLdap.SendRequest(searchRequest); // Respuesta del servidor 
            DirectoryAttribute atributo = searchResponse.Entries[0].Attributes["memberUid"];
            object[] objeto = atributo.GetValues(Type.GetType("System.Byte[]"));

            foreach (object ob in objeto)
            {

                String _login = Encoding.ASCII.GetString((byte[])objeto[_conta]);
                _logins.Add(_login);
                _conta++;
            }

            #endregion

            #region buscar nombre y correo

            //Busca el displayName y el mail de cada soportista según el login en el LDAP
            foreach (String login in _logins)
            {

                try
                {
                    Usuario _usuario = new Usuario();
                    _usuario.UID = login;

                    string[] _datos = new string[] { "displayName", "mail" }; // Atributos a retornar
                    // Buscar al usuario por su login
                    SearchRequest _buqueda = new SearchRequest("ou=people,dc=ic-itcr,dc=ac,dc=cr", "(uid=" + login + "*)",
                        System.DirectoryServices.Protocols.SearchScope.Subtree, _datos);
                    SearchResponse _respuesta = (SearchResponse)openLdap.SendRequest(_buqueda); // Respuesta del servidor
                    DirectoryAttribute _atributo = _respuesta.Entries[0].Attributes["displayName"];
                    object[] _objeto = _atributo.GetValues(Type.GetType("System.Byte[]"));
                    _usuario.Nombre = Encoding.ASCII.GetString((byte[])_objeto[0]);

                    _atributo = _respuesta.Entries[0].Attributes["mail"];
                    _objeto = _atributo.GetValues(Type.GetType("System.Byte[]"));
                    _usuario.Correo = Encoding.ASCII.GetString((byte[])_objeto[0]);

                    _resultado.Add(_usuario);
                }
                catch (Exception e)
                {
                    _conexionBD = new ManejoBD();
                    _conexionBD.insertarBitacoraError(e.ToString(), "");
                    //throw e;
                }

            }

            #endregion

            openLdap.Dispose(); // Liberar recursos
            return _resultado;
        }


        /// <summary>
        /// Autentica a un usuario contra openLDAP y verifica su membresia en alguno de los grupos
        /// </summary>
		/// <param name="nombreUsuario">Nombre de usuario</param>
        /// <param name="password">Contraseña del usuario</param>
        /// <returns>El grupo al que pertenece el usuario o null en caso que no esté registrado.</returns>
		
        public GrupoLDAP autenticarUsuario(string nombreUsuario, string password)
        {
            // Valida usuario y contraseña correctos
            LdapDirectoryIdentifier serverInfo = new LdapDirectoryIdentifier(Constantes.LDAP_SERVER);
            LdapConnection openLdap = new LdapConnection(Constantes.LDAP_SERVER);
			openLdap.Credential = new System.Net.NetworkCredential("uid=" + nombreUsuario + ",ou=people,dc=ic-itcr,dc=ac,dc=cr", password);
            openLdap.AuthType = AuthType.Basic;
            openLdap.SessionOptions.ProtocolVersion = 3;
            try
            {
                openLdap.Bind();				
            }
            catch (Exception e)
            {
                openLdap.Dispose();
                _conexionBD = new ManejoBD();
                _conexionBD.insertarBitacoraError(e.ToString(), "");
                return null;
            }

            // Buscar grupo al que pertenezca el usuario    
            foreach (GrupoLDAP grupo in _listadaGrupos.obtenerGruposLDAP())
            {
				SearchRequest searchRequest = new SearchRequest("cn=" + grupo.NombreGrupo + ",ou=group,dc=ic-itcr,dc=ac,dc=cr", "(memberUid=" + nombreUsuario + ")", System.DirectoryServices.Protocols.SearchScope.Subtree);
                try
                {
                    SearchResponse searchResponse = (SearchResponse)openLdap.SendRequest(searchRequest);
                    if (searchResponse.Entries.Count != 0)
                    {
                        openLdap.Dispose();
                        return grupo;
                    }
                }
                catch (Exception e)// En caso que algún grupo registrado en ListadoGruposLDAP.getGroupList() no exista.
                {
                    _conexionBD = new ManejoBD();
                    _conexionBD.insertarBitacoraError(e.ToString(), "Algún grupo registrado en ListadoGruposLDAP.getGroupList() no existe.");
                    continue;
                }
            }
            openLdap.Dispose();
            return null;
        }

		/// <summary>
		/// Método que se encarga de genera un hash SHA de la contraseña del usuario
		/// </summary>
		/// <param name="password">Contraseña del usuario en texto plano</param>
		/// <returns>Un string con el hash codificado en base64, conforme a lo requerido por OpenLDAP</returns>

		private String generarClaveSha(string password)
		{
			SHA1 hasher = SHA1CryptoServiceProvider.Create();
			return "{SHA}" + Convert.ToBase64String(hasher.ComputeHash(Encoding.ASCII.GetBytes(password)));
		}

		/// <summary>
		/// Método que se encarga de crear un usuario estudiante en el LDAP
		/// </summary>
		/// <param name="estudiante">
		/// Los datos del estudiante (en un tipo Usuario) por ingresar al LDAP
		/// </param>
		
		public Boolean crearEstudiante(Usuario estudiante)
		{
			try
			{
				DirectoryEntry conexion = crearConexion(Constantes.LDAP_BASEDN, Constantes.LDAP_USER, Constantes.LDAP_PASS);
				DirectoryEntry nuevoUsuario = conexion.Children.Add("uid=" + estudiante.UID, "uidObject");

				conexion.Close(); // Cerrar la conexion
				conexion.Dispose(); // Liberar los recursos

				// Limpiar caracteres ESTO HAY QUE CAMBIARLO POR VALIDACIONES EN INTERFAZ
				char[] badChars = { 'á', 'é', 'í', 'ó', 'ú', 'ñ' };
				char[] goodChars = { 'a', 'e', 'i', 'o', 'u', 'n' };

				for (int i = 0; i < badChars.Length; i++) // Limpiar caracteres
				{

					estudiante.Nombre = estudiante.Nombre.Replace(badChars[i], goodChars[i]); // Quitar tildes u caracteres especiales del nombre
					estudiante.Apellidos = estudiante.Apellidos.Replace(badChars[i], goodChars[i]); // Quitar tildes u caracteres especiales de los apellidos
				}

				// Agregar los atributos del usuario para LDAP

				nuevoUsuario.Properties["cn"].Add(estudiante.Nombre); // Nombre de la persona
				nuevoUsuario.Properties["objectClass"].Value = Constantes.OBJECTCLASSES;
				nuevoUsuario.Properties["uid"].Add(estudiante.UID); // Nombre de usuario
				nuevoUsuario.Properties["sn"].Add(estudiante.Apellidos); // Apellidos
				nuevoUsuario.Properties["gecos"].Add(estudiante.Carnet + ' ' + estudiante.Nombre + ' ' + estudiante.Apellidos + ' ' + estudiante.Carrera);

				if (estudiante.TelefonoCasa.Replace(" ", "") != "") // Teléfono de la casa en caso de que haya
					nuevoUsuario.Properties["homePhone"].Add(estudiante.TelefonoCasa);
				if (estudiante.TelefonoCelular.Replace(" ", "") != "") // Teléfono celular en caso de que haya
					nuevoUsuario.Properties["mobile"].Add(estudiante.TelefonoCelular);
				if (estudiante.Correo.Replace(" ", "") != "") // Correo electrónico del usuario
					nuevoUsuario.Properties["mail"].Add(estudiante.Correo);

				nuevoUsuario.Properties["displayName"].Add(estudiante.Nombre + ' ' + estudiante.Apellidos);
				nuevoUsuario.Properties["loginShell"].Add(Constantes.SHELL_POR_DEFECTO); // /bin/bash
				nuevoUsuario.Properties["mailLocalAddress"].Add(estudiante.UID + "@ic-itcr.ac.cr"); // Correo con ic-itcr
				nuevoUsuario.Properties["uidNumber"].Add(obtenerNumeroUid()); // Número de UID
				nuevoUsuario.Properties["userPassword"].Add(generarClaveSha(estudiante.Contrasena)); // Contraseña del usuario
				nuevoUsuario.Properties["homeDirectory"].Add(getHomeDirectory(estudiante));
				nuevoUsuario.Properties["gidNumber"].Add(Constantes.GID_ESTUDIANTE);
				nuevoUsuario.Properties["description"].Add("Estudiante");

				nuevoUsuario.CommitChanges(); // Guardar cambios
				nuevoUsuario.Close();
				nuevoUsuario.Dispose();
				agregarGruposGenerales(estudiante.UID); // Agrega a los grupos generales de LDAP
				agregarGrupo(estudiante.UID, _listadaGrupos.Estudiante.NombreGrupo); // Agregar el usuario al grupo de ests
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
		/// Método que se encarga de crear un usuario profesor en el LDAP
		/// </summary>
		/// <param name="estudiante">
		/// Los datos del profesor (en un tipo Usuario) por ingresar al LDAP
		/// </param>

		public Boolean crearProfesor(Usuario profesor)
		{
			try
			{
				DirectoryEntry conexion = crearConexion(Constantes.LDAP_BASEDN, Constantes.LDAP_USER, Constantes.LDAP_PASS);
				DirectoryEntry nuevoUsuario = conexion.Children.Add("uid=" + profesor.UID, "uidObject");

				conexion.Close(); // Cerrar la conexion
				conexion.Dispose(); // Liberar 

				// Limpiar caracteres ESTO HAY QUE CAMBIARLO POR VALIDACIONES EN INTERFAZ
				char[] badChars = { 'á', 'é', 'í', 'ó', 'ú', 'ñ' };
				char[] goodChars = { 'a', 'e', 'i', 'o', 'u', 'n' };

				for (int i = 0; i < badChars.Length; i++) // Limpiar caracteres
				{

					profesor.Nombre = profesor.Nombre.Replace(badChars[i], goodChars[i]); // Quitar tildes u caracteres especiales del nombre
					profesor.Apellidos = profesor.Apellidos.Replace(badChars[i], goodChars[i]); // Quitar tildes u caracteres especiales de los apellidos
				}
				// Agregar las atributos del usuario para LDAP
				nuevoUsuario.Properties["cn"].Add(profesor.Nombre); // Nombre de la persona
				nuevoUsuario.Properties["objectClass"].Value = Constantes.OBJECTCLASSES;
				nuevoUsuario.Properties["uid"].Add(profesor.UID); // Nombre de usuario
				nuevoUsuario.Properties["sn"].Add(profesor.Apellidos); // Apellidos
				nuevoUsuario.Properties["gecos"].Add(profesor.Carnet + ' ' + profesor.Nombre + ' ' + profesor.Apellidos); // El carnet corresponde a la cédula si el profesor no fue estudiante de la Escuela

				if (profesor.TelefonoCasa.Replace(" ", "") != "") // Teléfono de la casa en caso de que haya
					nuevoUsuario.Properties["homePhone"].Add(profesor.TelefonoCasa);
				if (profesor.TelefonoCelular.Replace(" ", "") != "") // Teléfono celular en caso de que haya
					nuevoUsuario.Properties["mobile"].Add(profesor.TelefonoCelular);
				if (profesor.Correo.Replace(" ", "") != "") // Correo electrónico del usuario
					nuevoUsuario.Properties["mail"].Add(profesor.Correo);

				nuevoUsuario.Properties["displayName"].Add(profesor.Nombre + ' ' + profesor.Apellidos);
				nuevoUsuario.Properties["loginShell"].Add(Constantes.SHELL_POR_DEFECTO); // /bin/bash
				nuevoUsuario.Properties["mailLocalAddress"].Add(profesor.UID + "@ic-itcr.ac.cr"); // Correo con ic-itcr
				nuevoUsuario.Properties["uidNumber"].Add(obtenerNumeroUid()); // Número de UID
				nuevoUsuario.Properties["userPassword"].Add(generarClaveSha(profesor.Contrasena));//Contraseña
				nuevoUsuario.Properties["homeDirectory"].Add(Ruta.RUTAPROFESOR + profesor.UID);
				nuevoUsuario.Properties["gidNumber"].Add(Constantes.GID_PROFESOR);
				nuevoUsuario.Properties["description"].Add("Profesor");

				nuevoUsuario.CommitChanges(); // Guardar cambios
				nuevoUsuario.Close();
				//nuevoUsuario.Dispose(); // No hacer esto   
				agregarGruposGenerales(profesor.UID); // Agrega a los grupos generales de LDAP
				agregarGrupo(profesor.UID, _listadaGrupos.Profesor.NombreGrupo); // Agregar el usuario al grupo de profesores
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
		/// Método que retorna la ruta del perfil del estudiante con base en su carné
		/// </summary>
		/// <param name="estudiante">Objeto estudiante</param>
		/// <returns>Cadena de la forma \\jordan\home\est*año del estudiante*\*login* </returns>

		private String getHomeDirectory(Usuario estudiante)
		{
			if (estudiante.Carnet.StartsWith("2"))
				return Ruta.RUTAESTUDIANTE + estudiante.Carnet.Substring(0, 4) + @"/" + estudiante.UID;
			else
				return Ruta.RUTAESTUDIANTE + "19" + estudiante.Carnet.Substring(0, 2) + @"/" + estudiante.UID;
		}

		/// <summary>
		/// Método que retorna el proximo identificador unico libre
		/// </summary>
		/// <returns>Identificador único libre</returns>
		
		private String obtenerNumeroUid()
		{
			string uid = "";
			LdapDirectoryIdentifier serverInfo = new LdapDirectoryIdentifier(Constantes.LDAP_SERVER);
			LdapConnection openLdap = new LdapConnection(Constantes.LDAP_SERVER);
			openLdap.Credential = new System.Net.NetworkCredential(Constantes.LDAP_USER, Constantes.LDAP_PASS);
			openLdap.AuthType = AuthType.Basic;
			openLdap.SessionOptions.ProtocolVersion = 3; // Hay que usar LDAPv3
			openLdap.Bind(); // Conectar
			string[] attributesToReturn = new string[] { "uidNumber" }; // Retornar solamente el uid number
			SearchRequest searchRequest = new SearchRequest("dc=ic-itcr,dc=ac,dc=cr", "(cn=NextFreeUnixId)",
				System.DirectoryServices.Protocols.SearchScope.Subtree, attributesToReturn); // Buscar al objeto NextFreeUnixId
			SearchResponse searchResponse = (SearchResponse)openLdap.SendRequest(searchRequest); // Respuesta del servidor
			// Manejar la respuesta
			DirectoryAttribute atributo = searchResponse.Entries[0].Attributes["uidNumber"];
			object[] objeto = atributo.GetValues(Type.GetType("System.Byte[]"));
			uid = Encoding.ASCII.GetString((byte[])objeto[0]);
			int siguienteuid = Int32.Parse(uid) + 1; // Actualizar el Unix Id libre
			ModifyRequest incremento = new ModifyRequest("cn=NextFreeUnixId,dc=ic-itcr,dc=ac,dc=cr"
				, DirectoryAttributeOperation.Replace, "uidNumber", siguienteuid.ToString()); // Modificar el NextFreeUnixId en el servidor
			openLdap.SendRequest(incremento);
			openLdap.Dispose();
			return uid; // Retornar el uid
		}

		/// <summary>
		/// Método que comprueba la existencia de un login cualquiera en openLdap
		/// </summary>
		/// <param name="nombreUsuario">Nombre de usuario</param>
		/// <returns>Verdadero si el usuario existe, falso en caso contrario</returns>
		
		public Boolean buscarNombreUsuario(string nombreUsuario)
		{
			DirectoryEntry connection = crearConexion(Constantes.LDAP_BASEDN, Constantes.LDAP_USER, Constantes.LDAP_PASS);
			DirectorySearcher search = new DirectorySearcher(connection, "(uid=" + nombreUsuario + ")");
			SearchResult result = search.FindOne();
			connection.Close();
			connection.Dispose();
			return result != null;
		}

		/// <summary>
		/// Método que busca el login de un usuario basado en su numero de carné
		/// </summary>
		/// <param name="carne">Numero de carné</param>
		/// <returns>Login del usuario correspondiente al carné</returns>
		
		public String buscarUsuarioPorCarnet(string carne)
		{
			string uid = "";
			LdapDirectoryIdentifier serverInfo = new LdapDirectoryIdentifier(Constantes.LDAP_SERVER);
			LdapConnection openLdap = new LdapConnection(Constantes.LDAP_SERVER);
			openLdap.Credential = new System.Net.NetworkCredential(Constantes.LDAP_USER, Constantes.LDAP_PASS);
			openLdap.AuthType = AuthType.Basic;
			openLdap.SessionOptions.ProtocolVersion = 3; // Hay que usar LDAPv3
			openLdap.Bind(); // Conectar

			string[] attributesToReturn = new string[] { "uid" }; // Retornar solamente el login
			SearchRequest searchRequest = new SearchRequest("ou=people,dc=ic-itcr,dc=ac,dc=cr", "(gecos=" + carne + "*)",
				System.DirectoryServices.Protocols.SearchScope.Subtree, attributesToReturn); // Buscar por carnet
			SearchResponse searchResponse = (SearchResponse)openLdap.SendRequest(searchRequest); // Respuesta del servidor
            if (searchResponse.Entries.Count != 0)
            {
                DirectoryAttribute atributo = searchResponse.Entries[0].Attributes["uid"];
                object[] objeto = atributo.GetValues(Type.GetType("System.Byte[]"));
                uid = Encoding.ASCII.GetString((byte[])objeto[0]);
            }
			openLdap.Dispose(); // Liberar recursos
			return uid;
		}

		/// <summary>
		/// Método que agrega a un usuario a los grupos generales del LDAP
		/// </summary>
		/// <param name="nombreUsuario">Usuario al cual se tiene que agregar a los grupos</param>
		
		private void agregarGruposGenerales(string nombreUsuario)
		{
			LdapDirectoryIdentifier serverInfo = new LdapDirectoryIdentifier(Constantes.LDAP_SERVER);
			LdapConnection openLdap = new LdapConnection(Constantes.LDAP_SERVER);//Conexion
			openLdap.Credential = new System.Net.NetworkCredential(Constantes.LDAP_USER, Constantes.LDAP_PASS);
			openLdap.AuthType = AuthType.Basic;
			openLdap.SessionOptions.ProtocolVersion = 3; // Hay que usar LDAPv3
			openLdap.Bind(); // Conectar

			// Agregar a cada uno de los grupos generales
			foreach (string grupo in Constantes.GROUPS)
			{
				openLdap.SendRequest(new ModifyRequest("cn=" + grupo + ",ou=group,dc=ic-itcr,dc=ac,dc=cr", DirectoryAttributeOperation.Add, "memberUid", nombreUsuario));
			}
			openLdap.Dispose();
		}

		/// <summary>
		/// Método que se encarga de agregar un usuario a un grupo especifico
		/// </summary>
		/// <param name="nombreUsuario">Nombre de usuario</param>
		/// <param name="grupo">Nombre del grupo al cual se desea agregar el usuario</param>

		private void agregarGrupo(string nombreUsuario, string grupo)
		{ // CONSULTAR ESTO ANTES DE PONERLO EN EJECUCIÓN
			LdapDirectoryIdentifier serverInfo = new LdapDirectoryIdentifier(Constantes.LDAP_SERVER);
			LdapConnection openLdap = new LdapConnection(Constantes.LDAP_SERVER);//Conexion
			openLdap.Credential = new System.Net.NetworkCredential(Constantes.LDAP_USER, Constantes.LDAP_PASS);
			openLdap.AuthType = AuthType.Basic;
			openLdap.SessionOptions.ProtocolVersion = 3; // Hay que usar LDAPv3
			openLdap.Bind(); // Conectar
			// Agregar el usuario al grupo especificado
			openLdap.SendRequest(new ModifyRequest("cn=" + grupo + ",ou=group,dc=ic-itcr,dc=ac,dc=cr", DirectoryAttributeOperation.Add, "memberUid", nombreUsuario));
			openLdap.Dispose();
		}

		/// <summary>
		/// Método que cambia la contraseña de un usuario
		/// </summary>
		/// <param name="nombreUsuario">Nombre de usuario</param>
		/// <param name="password">Contraseña nueva</param>
		
		public void cambiarContrasena(string nombreUsuario, string password)
		{
			LdapDirectoryIdentifier serverInfo = new LdapDirectoryIdentifier(Constantes.LDAP_SERVER);
			LdapConnection openLdap = new LdapConnection(Constantes.LDAP_SERVER);
			openLdap.Credential = new System.Net.NetworkCredential(Constantes.LDAP_USER, Constantes.LDAP_PASS);
			openLdap.AuthType = AuthType.Basic;
			openLdap.SessionOptions.ProtocolVersion = 3; // Hay que usar LDAPv3
			openLdap.Bind(); // Conectar
            
			ModifyRequest increment = new ModifyRequest("uid=" + nombreUsuario + ",ou=people,dc=ic-itcr,dc=ac,dc=cr"
				, DirectoryAttributeOperation.Replace, "userPassword", generarClaveSha(password));
			openLdap.SendRequest(increment);
			openLdap.Dispose();
		}


        public Usuario buscarUsuario(string clave) {


            Usuario user = new Usuario();
            List<String> datos = new List<String>();
            LdapDirectoryIdentifier serverInfo = new LdapDirectoryIdentifier(Constantes.LDAP_SERVER);
            LdapConnection openLdap = new LdapConnection(Constantes.LDAP_SERVER);
            openLdap.Credential = new System.Net.NetworkCredential(Constantes.LDAP_USER, Constantes.LDAP_PASS);
            openLdap.AuthType = AuthType.Basic;
            openLdap.SessionOptions.ProtocolVersion = 3; // Hay que usar LDAPv3
            openLdap.Bind(); // Conectar
            

            string[] attributesToReturn = new string[] { "gecos", "cn", "sn", "homePhone", "mobile", "mail", "description" }; // Retornar solamente el login
            SearchRequest searchRequest = new SearchRequest("ou=people,dc=ic-itcr,dc=ac,dc=cr", "(uid=" + clave + "*)",
                System.DirectoryServices.Protocols.SearchScope.Subtree, attributesToReturn); // Buscar por carnet
            SearchResponse searchResponse = (SearchResponse)openLdap.SendRequest(searchRequest); // Respuesta del servidor
            if (searchResponse.Entries.Count == 0)
                return null;
            //Cambiar a String cada atributo del usuario
            for (int i = 0; i < attributesToReturn.Length; i++) {
                DirectoryAttribute atributo = searchResponse.Entries[0].Attributes[attributesToReturn[i]];
                if (atributo != null)
                {
                    object[] objeto = atributo.GetValues(Type.GetType("System.Byte[]"));
                    datos.Add(Encoding.ASCII.GetString((byte[])objeto[0]));
                }
                else {
                    datos.Add("Atributo Nulo!");
                }
			}
            user.Carnet = datos.ElementAt(0).Split(' ')[0];			// Carnet
            user.Nombre = datos.ElementAt(1);						// Nombre
            user.Apellidos = datos.ElementAt(2);					// Apellidos
            user.TelefonoCasa = datos.ElementAt(3);					// Teléfono Fijo
            user.TelefonoCelular = datos.ElementAt(4);				// Teléfono Celular
            user.Correo = datos.ElementAt(5);						// Correo
            user.Grupo = datos.ElementAt(6);						// Descripcion 
			user.UID = clave;										// Login

			if (datos.ElementAt(0).Split(' ').Length > 4)
				user.Carrera = datos.ElementAt(0).Split(' ')[4];	// Carrera
			else
				user.Carrera = "IC";

            openLdap.Dispose();                        //Liberar recursos	

            return user;
        
        }



        /// <summary>
        /// Método que se encarga de modificar un usuario profesor en el LDAP
        /// </summary>
        /// <param name="estudiante">
        /// Los datos del profesor (en un tipo Usuario) por modificar en el LDAP
        /// </param>
        public Boolean modificarUsuario(Usuario usuario, Boolean changePass)
        {
			String udi = usuario.UID;
			LdapDirectoryIdentifier serverInfo = new LdapDirectoryIdentifier(Constantes.LDAP_SERVER);
			LdapConnection openLdap = new LdapConnection(Constantes.LDAP_SERVER);
			openLdap.Credential = new System.Net.NetworkCredential(Constantes.LDAP_USER, Constantes.LDAP_PASS);
			openLdap.AuthType = AuthType.Basic;
			openLdap.SessionOptions.ProtocolVersion = 3; // Hay que usar LDAPv3
			openLdap.Bind(); // Conectar

			// Limpiar caracteres ESTO HAY QUE CAMBIARLO POR VALIDACIONES EN INTERFAZ
			char[] badChars = { 'á', 'é', 'í', 'ó', 'ú', 'ñ' };
			char[] goodChars = { 'a', 'e', 'i', 'o', 'u', 'n' };

			for (int i = 0; i < badChars.Length; i++) // Limpiar caracteres
			{

				usuario.Nombre = usuario.Nombre.Replace(badChars[i], goodChars[i]); // Quitar tildes u caracteres especiales del nombre
				usuario.Apellidos = usuario.Apellidos.Replace(badChars[i], goodChars[i]); // Quitar tildes u caracteres especiales de los apellidos
			}

            try
            {
                
                // Agregar las atributos del usuario para LDAP

				ModifyRequest increment1 = new ModifyRequest("uid=" + usuario.UID + ",ou=people,dc=ic-itcr,dc=ac,dc=cr"
                , DirectoryAttributeOperation.Replace, "cn", usuario.Nombre );
                openLdap.SendRequest(increment1);

				ModifyRequest increment2 = new ModifyRequest("uid=" + usuario.UID + ",ou=people,dc=ic-itcr,dc=ac,dc=cr"
                , DirectoryAttributeOperation.Replace, "sn", usuario.Apellidos);
                openLdap.SendRequest(increment2);

				ModifyRequest increment3 = new ModifyRequest("uid=" + usuario.UID + ",ou=people,dc=ic-itcr,dc=ac,dc=cr"
                , DirectoryAttributeOperation.Replace, "gecos", usuario.Carnet + ' ' + usuario.Nombre + ' ' + usuario.Apellidos + ' ' + usuario.Carrera);
                openLdap.SendRequest(increment3);


                if (usuario.TelefonoCasa.Replace(" ", "") != "")
                { // Teléfono de la casa en caso de que haya
					ModifyRequest increment4 = new ModifyRequest("uid=" + usuario.UID + ",ou=people,dc=ic-itcr,dc=ac,dc=cr"
                   , DirectoryAttributeOperation.Replace, "homePhone",usuario.TelefonoCasa);
                    openLdap.SendRequest(increment4);
                }
                if (usuario.TelefonoCelular.Replace(" ", "") != "")
                {// Teléfono celular en caso de que haya
					ModifyRequest increment5 = new ModifyRequest("uid=" + usuario.UID + ",ou=people,dc=ic-itcr,dc=ac,dc=cr"
                   , DirectoryAttributeOperation.Replace, "mobile", usuario.TelefonoCelular);
                    openLdap.SendRequest(increment5);
                   
                }
                if (usuario.Correo.Replace(" ", "") != "")
                {// Correo electrónico del usuario
					ModifyRequest increment5 = new ModifyRequest("uid=" + usuario.UID + ",ou=people,dc=ic-itcr,dc=ac,dc=cr"
                  , DirectoryAttributeOperation.Replace, "mail", usuario.Correo);
                    openLdap.SendRequest(increment5);
                   
                }

				ModifyRequest increment6 = new ModifyRequest("uid=" + usuario.UID + ",ou=people,dc=ic-itcr,dc=ac,dc=cr"
              , DirectoryAttributeOperation.Replace, "displayName", usuario.Nombre + ' ' + usuario.Apellidos);
                openLdap.SendRequest(increment6);

                ModifyRequest increment7 = new ModifyRequest("uid=" + usuario.UID + ",ou=people,dc=ic-itcr,dc=ac,dc=cr"
              , DirectoryAttributeOperation.Replace, "description", usuario.Grupo);
                openLdap.SendRequest(increment7);
                
                
                
                //nuevoUsuario.Dispose(); // No hacer esto
                /*
                if(usuario.Grupo == "Estudiante") {
                    agregarGruposGenerales(usuario.UID); // Agrega a los grupos generales de LDAP
                    agregarGrupo(usuario.UID, _listadaGrupos.Estudiante.NombreGrupo); // Agregar el usuario al grupo de estudiantes
                }
                else {
                agregarGruposGenerales(usuario.UID); // Agrega a los grupos generales de LDAP
                    agregarGrupo(usuario.UID, _listadaGrupos.Profesor.NombreGrupo); // Agregar el usuario al grupo de profesores
                }
                */
                if (changePass)
                {
                    ModifyRequest increment9 = new ModifyRequest("uid=" + udi + ",ou=people,dc=ic-itcr,dc=ac,dc=cr"
              , DirectoryAttributeOperation.Replace, "userPassword", usuario.Contrasena);
                    openLdap.SendRequest(increment9);
                }

                return true;
            
            }
            catch (Exception e)
            {
                _conexionBD = new ManejoBD();
                _conexionBD.insertarBitacoraError(e.ToString(), "");
                return false;
            }

        }

        public Boolean verificarProfesor(string clave)
        {
            String descripcion = String.Empty;
            LdapDirectoryIdentifier serverInfo = new LdapDirectoryIdentifier(Constantes.LDAP_SERVER);
            LdapConnection openLdap = new LdapConnection(Constantes.LDAP_SERVER);
            openLdap.Credential = new System.Net.NetworkCredential(Constantes.LDAP_USER, Constantes.LDAP_PASS);  
            openLdap.AuthType = AuthType.Basic;
            openLdap.SessionOptions.ProtocolVersion = 3; // Hay que usar LDAPv3
            openLdap.Bind(); // Conectar

            // El criterio seleccionado es "Login" true
            //  if (!tipoBusqueda)
            //   clave = buscarUsuarioPorCarnet(clave);


            Boolean res = false;
            string[] attributesToReturn = new string[] { "description" }; // Retornar solamente el login
            SearchRequest searchRequest = new SearchRequest("ou=people,dc=ic-itcr,dc=ac,dc=cr", "(uid=" + clave + "*)",
                System.DirectoryServices.Protocols.SearchScope.Subtree, attributesToReturn); // Buscar por carnet
            SearchResponse searchResponse = (SearchResponse)openLdap.SendRequest(searchRequest); // Respuesta del servidor
            if (searchResponse.Entries.Count == 0)
                return res;
            //Cambiar a String cada atributo del usuario
            if (attributesToReturn.Length > 0)
            {
                DirectoryAttribute atributo = searchResponse.Entries[0].Attributes["description"];
                if (atributo != null)
                {
                    object[] objeto = atributo.GetValues(Type.GetType("System.Byte[]"));
                    descripcion = Encoding.ASCII.GetString((byte[])objeto[0]);
                }
                else
                {
                    return res;
                }
            }

            if (descripcion == "Profesor")
                res = true;
            
            
            openLdap.Dispose();                        //Liberar recursos	

            return res;

        }




		#endregion
    }
}