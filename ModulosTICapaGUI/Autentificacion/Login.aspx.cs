using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using ModulosTICapaGUI.Compartido;
using ModulosTICapaLogica.Compartido;


namespace ModulosTICapaGUI.Autentificacion
{
    public partial class Login : System.Web.UI.Page
	{
		#region Atributos

		private ControladorLogin _controladorLogin;
		private Sesion _nuevaSesion;

		#endregion

		#region Métodos

		protected void Page_Load(object sender, EventArgs e)
        {
            _ibtnLogin.Attributes.Add("onmouseover", "this.src='../Imagenes/botonLoginPulsado.png'");
            _ibtnLogin.Attributes.Add("onmouseout", "this.src='../Imagenes/botonLogin.png'");
            _controladorLogin = new ControladorLogin();
			_nuevaSesion = new Sesion();
        }

        protected void ibtnLogin_Click(object sender, EventArgs e)
        {
          if ((_txtUsuario.Equals("")) || (_txtPassword.Equals(""))) // Sino se indicaron todos los datos
			{
				_imgError.ImageUrl = "../Imagenes/Advertencia.png";
				_lblError.Text = "Debe llenar todos los campos del formulario";
				_imgError.Visible = true;
				_lblError.Visible = true;
			}
			else // Sino faltan llenar datos en el formulario de Login
			{
				Usuario usuario = new Usuario();
				usuario = _controladorLogin.usuarioValido(_txtUsuario.Text, _txtPassword.Text); // Verificar el usuario
				if (usuario != null) // Si es un usuario válido
				{
					HttpCookie cookie = _nuevaSesion.crearCookie();
					// Establecer parámetros que tendrá la cookie
					cookie.Values.Add("Login", usuario.UID);
					cookie.Values.Add("Grupo", usuario.Grupo);
					cookie.Values.Add("Expira", DateTime.Now.AddMinutes(15.0).ToString());
					cookie = _nuevaSesion.encriptarCookie(cookie); // Encriptar la cookie
					cookie.Expires = DateTime.Now.AddMinutes(15.0); // Tiempo en que expira la cookie
					Response.Cookies.Add(cookie); // Crear cookie
					Response.Redirect("../Compartido/Inicio.aspx");
				}
				else // Si no es usuario válido
				{
					_imgError.ImageUrl = "../Imagenes/errorLogin.png";
					_lblError.Text = "Datos incorrectos";
					_imgError.Visible = true;
					_lblError.Visible = true;
				}
			}
        }

		#endregion
    }
}