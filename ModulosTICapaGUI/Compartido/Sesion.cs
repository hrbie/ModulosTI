using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Web.UI;


namespace ModulosTICapaGUI.Compartido
{
	public class Sesion
	{
		#region Atributos

		private static byte[] llave = ASCIIEncoding.ASCII.GetBytes("PlaformaServiciosTI2011."); // Llave que se usará para encriptar con TripleDES
	
		#endregion

		#region Constructor

		public Sesion()
		{
		}

		#endregion

		#region Métodos

        /// <summary>
        /// Método que se encarga de crear una nueva cookie
        /// </summary>
        /// <returns>Retorna una nueva cookie</returns>

		public HttpCookie crearCookie()
		{
			HttpCookie cookieNueva = new HttpCookie("PS"); // PS es el nombre de la cookie
			cookieNueva.Secure = true; // Para que sea transimitida con SSL (si se usa HTTPS)
			return cookieNueva;
		}

		/// <summary>
		/// Método que se encarga de encriptar un texto usando el algoritmo TripleDES
		/// </summary>
		/// <param name="texto">Texto a encriptar</param>
		/// <returns>Retorna el texto encriptado</returns>

		private String encriptarTexto(string texto)
		{
			TripleDESCryptoServiceProvider proveedorEncriptador = new TripleDESCryptoServiceProvider();
			MemoryStream salvarEnMemoria = new MemoryStream(); // Donde se mantendra los datos encriptados temporalmente
			// Atributos para realizar la encriptación (llave, donde guardarlo, etc)
			CryptoStream flujoCriptografico = new CryptoStream(salvarEnMemoria, proveedorEncriptador.CreateEncryptor(llave, llave), CryptoStreamMode.Write);
			StreamWriter escritor = new StreamWriter(flujoCriptografico);
			escritor.Write(texto);
			escritor.Flush();
			flujoCriptografico.FlushFinalBlock();
			escritor.Flush();
			return Convert.ToBase64String(salvarEnMemoria.GetBuffer(), 0, (int)salvarEnMemoria.Length); // Retornarlo en texto plano
		}

		/// <summary>
		/// Método que se encarga de desencriptar un texto
		/// </summary>
		/// <param name="textoEncriptado">Texto encriptado que desea desencriptar</param>
		/// <returns>Retorna el texto desencriptado</returns>

		private String desencriptarTexto(string textoEncriptado)
		{
			TripleDESCryptoServiceProvider proveedorDesencriptador = new TripleDESCryptoServiceProvider(); // Inicializa el proveedor
			MemoryStream salvarEnMemoria = new MemoryStream(Convert.FromBase64String(textoEncriptado)); // Traer a memoria los bytes del texto encriptado
			CryptoStream flujoCriptografico = new CryptoStream(salvarEnMemoria, proveedorDesencriptador.CreateDecryptor(llave, llave), CryptoStreamMode.Read);
			StreamReader lector = new StreamReader(flujoCriptografico);
			return lector.ReadToEnd();
		}

		/// <summary>
		/// Método que se encarga de encriptar una cookie
		/// </summary>
		/// <param name="cookie">Cookie que se desea encriptar</param>
		/// <returns>Retorna la cookie encriptada</returns>

		public HttpCookie encriptarCookie(HttpCookie cookie)
		{
			if ((cookie == null) || (cookie.Values.Count == 0)) // Si la cookie no ha sido seteada o no tiene valores
				return null;
			else
			{
				HttpCookie cookieEncriptada = new HttpCookie(cookie.Name);
				String valor;
				String nombre;
				for (int i = 0; i < cookie.Values.Count; i++)
				{
					valor = HttpContext.Current.Server.UrlEncode(encriptarTexto(cookie.Values[i]));
					nombre = HttpContext.Current.Server.UrlEncode(encriptarTexto(cookie.Values.GetKey(i)));
					cookieEncriptada.Values.Set(nombre, valor);
				}
				return cookieEncriptada;
			}
		}

		/// <summary>
		/// Método que se encarga de desencriptar una cookie
		/// </summary>
		/// <param name="cookieEncriptada">Cookie que se desea desencriptar</param>
		/// <returns>Retorna la cookie desencriptada</returns>

		public HttpCookie desencriptarCookie(HttpCookie cookieEncriptada)
		{
			if ((cookieEncriptada == null) || (cookieEncriptada.Values.Count == 0)) // Si la cookie no ha sido seteada o no tiene valores
				return null;
			else
			{
				HttpCookie cookie = new HttpCookie(cookieEncriptada.Name);
				String valor;
				String nombre;
				for (int i = 0; i < cookieEncriptada.Values.Count; i++)
				{
					valor = desencriptarTexto(HttpContext.Current.Server.UrlDecode(cookieEncriptada.Values[i]));
					nombre = desencriptarTexto(HttpContext.Current.Server.UrlDecode(cookieEncriptada.Values.GetKey(i)));
					cookie.Values.Set(nombre, valor);
				}
				return cookie;
			}
		}

		/// <summary>
		/// Método que se encarga de verificar si una cookie aún es válida y actualizar su expiración
		/// </summary>
		/// <returns>Un valor booleano, true si la cookie aún es válida, false en caso contrario</returns>

		public HttpCookie verificarValidez(HttpCookie _cookieActual)
		{
			_cookieActual = desencriptarCookie(_cookieActual); // Desencriptar la cookie para poder trabajar con ella
			if ((_cookieActual != null) && (Convert.ToDateTime(_cookieActual["Expira"]).CompareTo(DateTime.Now) >= 0)) // Refrescar expiración de la cookie
			{
				_cookieActual["Expira"] = DateTime.Now.AddMinutes(20.0).ToString(); // Aumentar el tiempo de expiración de la cookie
				_cookieActual = encriptarCookie(_cookieActual); // Volverla a encriptar
				return _cookieActual;
			}
			else // Si la cookie expiró
				return null;
		}

		/// <summary>
		/// Método que se encarga de obtener el nombre de usuario en la cookie
		/// </summary>
		/// <param name="cookieEncriptada">Cookie guardada en el cliente</param>
		/// <returns>Retorna una cadena con el valor del login del cliente</returns>

		public String obtenerLoginUsuario(HttpCookie cookieEncriptada)
		{	
			try
			{
				HttpCookie cookie = desencriptarCookie(cookieEncriptada);
				return cookie["Login"];
			}
			catch (Exception )
			{
				HttpContext.Current.Response.Redirect("../Autentificacion/Login.aspx");
				return "";
			}

		}

		/// <summary>
		/// Método que se encarga de obtener el grupo al que pertenece el usuario guardado en la cookie
		/// </summary>
		/// <param name="cookieEncriptada">Cookie guardada en el cliente</param>
		/// <returns>Retorna una cadena con el valor del grupo del cliente</returns>

		public String obtenerGrupoUsuario(HttpCookie cookieEncriptada)
		{
			HttpCookie cookie = desencriptarCookie(cookieEncriptada);
			return cookie["Grupo"];
		}

		#endregion
	}
}