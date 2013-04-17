using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using ModulosTICapaGUI.Compartido;
using ModulosTICapaLogica.Compartido;
using System.Data;

namespace ModulosTICapaGUI.ModuloSolicitudes
{
    public partial class CrearSolicitud : System.Web.UI.Page
    {
        #region Atributos

        private static ControladorSolicitudCS _controlador;
        private Sesion _sesion;
        private HttpCookie _cookieActual;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {
                _sesion = new Sesion();
                _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
                if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                    Response.Redirect("../Autentificacion/Login.aspx");
                else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                    Response.SetCookie(_cookieActual);

                _controlador = new ControladorSolicitudCS();
            }
        }

        public void btnEnviar_Click(Object sender, EventArgs e)
        {

            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            
            _controlador = new ControladorSolicitudCS();

            String asunto = _txtAsunto.Text;
            String descripcion = _taDescripcion.InnerText;
            String login = _txtSolicitante.Text;

			if (_controlador.crearSolicitud(asunto, descripcion, login, _sesion.obtenerLoginUsuario(_cookieActual)))
			{
				_txtAsunto.Text = "";
				_taDescripcion.InnerText = "";
				_txtSolicitante.Text = "";
				_imgMensaje.ImageUrl = "../Imagenes/ok.png";
				_lblEnvio.Text = "Solicitud enviada";
				_imgMensaje.Visible = true;
				_lblEnvio.Visible = true;
			}
			else
			{
				_imgMensaje.ImageUrl = "../Imagenes/Error.png";
				_lblEnvio.Text = "Error al enviar la solicitud";
				_imgMensaje.Visible = true;
				_lblEnvio.Visible = true;
			}
        }
    }
}