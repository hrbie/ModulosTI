using System;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using ModulosTICapaLogica.Compartido;

namespace ModulosTICapaDatos.Compartido
{
	/// <summary>
	/// Modifica las propiedades de las cuentas de usuario mediante marcadores UserAccountControl. 
	/// La información fue tomada de http://support.microsoft.com/kb/305144
	/// </summary>
	public static class MarcadorUAC
	{
		#region Constantes

		/// <summary>
		/// La secuencia de comandos de inicio de sesión se ejecutará.
		/// </summary>
		public const int SCRIPT = 0x0000001;

		/// <summary>
		/// La cuenta de usuario está desactivada.
		/// </summary>
		public const int ACCOUNT_DISABLE = 0x0000002;

		/// <summary>
		/// Se requiere la carpeta principal.
		/// </summary>
		public const int HOMEDIR_REQUIRED = 0x0000008;

		public const int LOCKOUT = 0x0000010;

		/// <summary>
		/// No se requiere contraseña.
		/// </summary>
		public const int PASSWD_NOTREQD = 0x0000020;

		/// <summary>
		/// el usuario no puede cambiar la contraseña. Éste es un permiso del objeto del usuario. 
		/// Para obtener más información acerca de cómo establecer este permiso mediante programación, visite el siguiente sitio Web de Microsoft: 
		/// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/adsi/adsi/modifying_user_cannot_change_password_ldap_provider.asp
		/// </summary>
		public const int PASSWD_CANT_CHANGE = 0x0000040;

		/// <summary>
		/// El usuario puede enviar una contraseña cifrada.
		/// </summary>
		public const int ENCRYPTED_TEXT_PWD_ALLOWED = 0x0000080;

		/// <summary>
		/// Ésta es una cuenta para usuarios cuya cuenta primaria está en otro dominio. 
		/// Esta cuenta proporciona acceso de usuario a este dominio, pero no a cualquier dominio que confíe en este dominio. 
		/// En ocasiones, se denomina cuenta de usuario local.
		/// </summary>
		public const int TEMP_DUPLICATE_ACCOUNT = 0x0000100;

		/// <summary>
		/// Éste es el tipo de cuenta predeterminado que representa a un usuario normal. 
		/// </summary>
		public const int NORMAL_ACCOUNT = 0x0000200;

		/// <summary>
		/// Éste es un permiso para confiar en una cuenta para un dominio del sistema que confía en otros dominios. 
		/// </summary>
		public const int INTERDOMAIN_TRUST_ACCOUNT = 0x0000800;

		/// <summary>
		/// Ésta es una cuenta de equipo para un equipo que utiliza Microsoft Windows NT 4.0 Workstation, Microsoft Windows NT 4.0 Server, 
		/// Microsoft Windows 2000 Professional o Windows 2000 Server y es miembro de este dominio.
		/// </summary>
		public const int WORKSTATION_TRUST_ACCOUNT = 0x0001000;

		/// <summary>
		/// Ésta es una cuenta de equipo para un controlador de dominio que es miembro de este dominio. 
		/// </summary>
		public const int SERVER_TRUST_ACCOUNT = 0x0002000;

		/// <summary>
		/// Representa la contraseña, que nunca debe caducar en la cuenta. 
		/// </summary>
		public const int DONT_EXPIRE_PASSWORD = 0x0010000;

		/// <summary>
		/// Ésta es una cuenta de inicio de sesión de MNS. 
		/// </summary>
		public const int MNS_LOGON_ACCOUNT = 0x0020000;

		/// <summary>
		/// Cuando este indicador está activado, obliga al usuario a iniciar sesión utilizando una tarjeta inteligente. 
		/// </summary>
		public const int SMARTCARD_REQUIRED = 0x0040000;

		/// <summary>
		/// Cuando este indicador está activado, la cuenta de servicio (la cuenta de usuario o equipo) utilizada por un servicio para 
		/// ejecutarse tiene una relación de confianza para la delegación Kerberos. Cualquier servicio puede suplantar un cliente que 
		/// solicite el servicio. Para habilitar un servicio para la delegación Kerberos, debe establecer este indicador en la propiedad 
		/// userAccountControl de la cuenta de servicio. 
		/// </summary>
		public const int TRUSTED_FOR_DELEGATION = 0x0080000;
		
		/// <summary>
		/// Cuando este indicador está activado, el contexto de seguridad del usuario no se delega a un servicio, aun cuando la cuenta de 
		/// servicio tenga establecida una relación de confianza para la delegación Kerberos.
		/// </summary>
		public const int NOT_DELEGATED = 0x0100000;

		/// <summary>
		/// Restringe el principal para que utilice sólo tipos de cifrado del Estándar de cifrado de datos (DES) para las claves.
		/// </summary>
		public const int USE_DES_KEY_ONLY = 0x0200000;

		/// <summary>
		/// Esta cuenta no requiere autenticación previa Kerberos para iniciar sesión.
		/// </summary>
		public const int DONT_REQ_PREAUTH = 0x0400000;

		/// <summary>
		/// La contraseña de usuario ha caducado. 
		/// </summary>
		public const int PASSWORD_EXPIRED = 0x0800000;

		/// <summary>
		/// La cuenta está habilitada para delegación. 
		/// Esta configuración permite que un servicio que se ejecuta bajo esta cuenta asuma la identidad de un cliente y se autentique 
		/// como ese usuario en otros servidores remotos en la red. 
		/// </summary>
		public const int TRUSTED_TO_AUTH_FOR_DELEGATION = 0x1000000;

		#endregion

		#region Métodos

		public static void activar(DirectoryEntry usuario, int marcadores)
		{
			int estado = (int) usuario.Properties["userAccountControl"].Value;
			usuario.Properties["userAccountControl"].Value = estado | marcadores;
		}

		public static void desactivar(DirectoryEntry usuario, int marcadores)
		{
			int estado = (int) usuario.Properties["userAccountControl"].Value;
			usuario.Properties["userAccountControl"].Value = estado & ~marcadores;
		}

		#endregion
	}
}
