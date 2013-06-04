using System;

namespace ModulosTICapaDatos.Compartido
{
	public static class Constantes
	{
		#region OpenLDAP

		#region User Attributes

		public static string SHELL_POR_DEFECTO = "/bin/bash";

		public static string GID_ESTUDIANTE = "102";
		public static string GID_PROFESOR   = "101";

		public static string[] GROUPS =  {"audio", "cdrom", "floppy", "plugdev", "power",
                                            "netdev", "users"}; // Grupos para permisos de las cuentas sobre las máquinas

		public static string[] OBJECTCLASSES = { "inetOrgPerson", "top", "posixAccount", 
                                                 "shadowAccount", "inetLocalMailRecipient" };
        public static string DOM = "ic-itcr.ac.cr";
		public static string DOMINIO = "@ic-itcr.ac.cr";

		#endregion

		#region Connection

		public static string LDAP_SERVER = "mnemosine.ic-itcr.ac.cr";
		public static string LDAP_UIDDN  = "LDAP://mnemosine.ic-itcr.ac.cr/uid=";
		public static string LDAP_BASEDN = "LDAP://mnemosine.ic-itcr.ac.cr/ou=people,dc=ic-itcr,dc=ac,dc=cr";
		public static string LDAP_USER   = "cn=admin,dc=ic-itcr,dc=ac,dc=cr";
        //public static string LDAP_PASS   = "Solaris2012";
        public static string LDAP_PASS   = "Y0uH4v3N0w4514nFlu";

		#endregion

		#endregion

		#region Active Directory

		public static string AD_BASEDN      = "LDAP://jordan.ic-itcr.ac.cr/CN=users,DC=ic-itcr,DC=ac,DC=cr";
		public static string AD_USER        = "jefe";
        public static string AD_PASS        = "addPCDominio";
		public static string AD_SCHEMA_USER = "users";
		#endregion

		#region Otros
		public static Random RANDOMNUMBERS = new Random();
		#endregion
	}
}
