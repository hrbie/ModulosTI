using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using System.Data;
using ModulosTICapaGUI.Compartido;
using ModulosTICapaLogica.Compartido;

namespace ModulosTICapaGUI.ModuloPEUL
{
	public partial class ConsultarReporteUso : System.Web.UI.Page
    {
        #region Atributos

        private ControladorPEUL _controladorPEUL;
        private static List<List<object>> _listaLaboratorios; // Lista con la informacíón de los laboratorios para llenar el _ddlLaboratorios
		private Sesion _sesion;
        private DataTable _tablaReportes;
        private HttpCookie _cookieActual;

        #endregion

        #region Métodos

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
                _controladorPEUL = new ControladorPEUL();
				String grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
				if ((grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")) ||
						(grupoUsuario.Equals("operadores"))) // Reportar si un usuario autenticado intenta hacer ingreso a una página que no tiene permiso
				{
					Notificacion notificacion = new Notificacion();
					notificacion.enviarCorreo("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(_cookieActual) + " a la página de ConsultarReporteUso.aspx", "soporte@ic-itcr.ac.cr", "Violación de Seguridad");
					Response.Redirect("../Compartido/AccesoDenegado.aspx");
				}
                _listaLaboratorios = _controladorPEUL.obtenerLaboratorios(); // Obtener laboratorios
                if (_listaLaboratorios != null)
                {
                    _ddlLaboratorios.Items.Add("Seleccionar");
                    for (int i = 0; i < _listaLaboratorios.Count; i++)
                        _ddlLaboratorios.Items.Add(_listaLaboratorios.ElementAt(i).ElementAt(1).ToString()); // Llenar el _ddlLaboratorios con los nombres de los labs
                }
                else
                {
                    _ddlLaboratorios.Items.Clear();
                    _ddlLaboratorios.Items.Add("No hay laboratorios disponibles");
                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Se ha producido un error al obtener los laboratorios.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }
        }

        /// <summary>
        /// Método que se encarga de manejar el evento del botón de consultar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _btnConsultar_Click(object sender, EventArgs e)
        {
			_controladorPEUL = new ControladorPEUL();
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx"); // 
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
            if ((_ddlLaboratorios.SelectedIndex == 0) || (_txtFechaInicio.Text.Equals("")) || (_txtFechaFinal.Text.Equals("")))
            {
                _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                _lblMensaje.Text = "Debe especificar todos los campos del formulario.";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }
            else // Buscar los reportes
            {
                _tablaReportes = _controladorPEUL.consultarReportesUso(_listaLaboratorios.ElementAt(_ddlLaboratorios.SelectedIndex-1).ElementAt(0).ToString(), _txtFechaInicio.Text, _txtFechaFinal.Text);
                if (_tablaReportes == null)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Ha habido un error al encontrar los reportes de uso de laboratorio.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
                else if (_tablaReportes.Rows.Count == 0)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                    _lblMensaje.Text = "No hay reportes disponibles.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _gvwRegistro.Visible = false;
                }
                else // Llenar la tabla
                {
                    _imgMensaje.Visible = false;
                    _lblMensaje.Visible = false;
                    _gvwRegistro.Visible = true;
                    _gvwRegistro.DataSource = _tablaReportes;
                    _gvwRegistro.DataBind();
                }
            }
        }

        #endregion
    }
}