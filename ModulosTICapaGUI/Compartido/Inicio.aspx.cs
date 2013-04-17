using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;

namespace ModulosTICapaGUI.Compartido
{
    public partial class Inicio : System.Web.UI.Page
    {

		#region Atributos

		private HttpCookie _cookieActual;
		private static Sesion _sesion;

		#endregion

		#region Métodos

		protected void Page_Load(object sender, EventArgs e)
        {
			if (!IsPostBack)
			{
				_sesion = new Sesion();
				_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
				if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				{
					//Global._usuario = null; HACER CLASE EN CONTROLADOR PARA QUE LO LIMPIE
					Response.Redirect("../Autentificacion/Login.aspx"); // 
				}
				else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
					Response.SetCookie(_cookieActual);
			}
        }

		#endregion
	}
}