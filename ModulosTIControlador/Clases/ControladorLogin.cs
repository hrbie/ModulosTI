using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModulosTICapaDatos.Compartido;
using ModulosTIControlador.Clases;
using ModulosTICapaLogica.Compartido;

namespace ModulosTIControlador.Clases
{
    public class ControladorLogin
	{
		#region Atributos

		private static ConexionLDAP _ldap = new ConexionLDAP();

		#endregion

		#region Constructor

		public ControladorLogin()
        {
            
        }

		#endregion

		#region Métodos

		/// <summary>
        /// Método que se encarga de autenticar un usuario
        /// </summary>
        /// <param name="login">Login del usuario</param>
        /// <param name="password">Password del usuario</param>
        /// <returns>Retorna los datos del usuario en caso de que la autenticación sea exitosa, null en caso contrario</returns>

        public Usuario usuarioValido(string login, string password)
        {
			GrupoLDAP grupo = _ldap.autenticarUsuario(login, password);
            
            if (grupo != null) // Si es válido
            {
				// Se guardan los datos del usuario actualmente logueado
				Usuario usuario = new Usuario();
				usuario.UID = login;
				usuario.Grupo = grupo.NombreGrupo;
                return usuario;
            }
            else // Si no es válido
                return null;
		}

		#endregion
	}
}
