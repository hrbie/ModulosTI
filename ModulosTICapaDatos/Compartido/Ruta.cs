using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTICapaDatos.Compartido
{
	public class Ruta
	{
		// Rutas en el LDAP

		//public const String RUTAESTUDIANTE = @"\\JORDAN\est\";
		//public const String RUTAPROFESOR = @"\\JORDAN\prof\";

        public const String RUTAESTUDIANTE = @"/home/est/";
        public const String RUTAPROFESOR = @"/home/prof/";

		// Rutas en Active Directory
        //  
		public const String RUTAUSUARIO = @"\\JORDAN\home\est\"; // QUITAR CON AVISO DE KENNETH PORQUE EL SERVER LO HACE SOLO
        public const String RUTAPROFESOR_AD = @"\\JORDAN\home\prof\";
	}
}
