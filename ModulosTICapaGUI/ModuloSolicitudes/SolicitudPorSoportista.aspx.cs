using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using ModulosTICapaLogica.Compartido;
using ModulosTICapaGUI.Compartido;

namespace ModulosTICapaGUI.ModuloSolicitudes
{
	public partial class SolicitudPorSoportista : System.Web.UI.Page
	{

		#region Atributos

		private HttpCookie _cookieActual;
		private Sesion _sesion;
		private static ControladorSolicitudCSE _controlador;
		private static ControladorSolicitudSS _auxControlador;

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				_sesion = new Sesion();
				_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
				if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
					Response.Redirect("../Autentificacion/Login.aspx"); // 
				else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
					Response.SetCookie(_cookieActual);

				String grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
				if ((grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")) || (grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("operadores")) || (grupoUsuario.Equals("soporte")) || (grupoUsuario.Equals("jefes"))) // En caso de que usuarios que no puedan ingresar a ésta área intenten hacerlo
				{
					Notificacion notificacion = new Notificacion();
					notificacion.enviarCorreo("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(_cookieActual) + " a la página de CrearReservacion.aspx", "soporte@ic-itcr.ac.cr", "Violación de Seguridad");
					Response.Redirect("../Compartido/AccesoDenegado.aspx");
				}

				_controlador = new ControladorSolicitudCSE();
				_auxControlador = new ControladorSolicitudSS();

				//Se agregan los datos recuperados al DropDownList _ddlUsuario
				_ddlSoportista.DataSource = _controlador.crearDataViewSoporte();
				_ddlSoportista.DataTextField = "soportista";
				_ddlSoportista.DataValueField = "login";
				_ddlSoportista.DataBind();
				_ddlSoportista.SelectedIndex = 0;

				//Se agregan los datos recuperados al DropDownList _ddlEstado
				_ddlEstado.DataSource = _controlador.crearDataViewEstados();
				_ddlEstado.DataTextField = "estado";
				_ddlEstado.DataValueField = "id";
				_ddlEstado.DataBind();  //Enlazar los datos con el control
				_ddlEstado.SelectedIndex = 0;
				_upEstado.Update();

			}
		}


		public void ddlSoportistaSelection_Change(Object sender, EventArgs e)
		{
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx"); // 
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);

			if (_ddlSoportista.SelectedIndex != 0)
			{
				_auxControlador.cargarSolicitudes(_ddlSoportista.SelectedValue);

				_ddlSolicitud.Items.Clear();
				_ddlSolicitud.DataSource = _auxControlador.crearDataViewSolicitudes();
				_ddlSolicitud.DataTextField = "asunto";
				_ddlSolicitud.DataValueField = "id";
				_ddlSolicitud.DataBind();
				_ddlSolicitud.SelectedIndex = 0;
				//Se le indica al panel que se actualice
				_upSolicitud.Update();

				//Se limpian los campos
				_taDescripcion.InnerText = "";
				_taAvance.InnerText = "";
				_lblPostBy.Text = "";
				_lblFechaSolicitud.Text = "";
				_ddlFechaAvance.SelectedIndex = 0;
				_ddlEstado.SelectedIndex = 0;

				//Se actualizan los paneles
				_upDescripcion.Update();
				_upAvance.Update();
				_upPostBy.Update();
				_upFechaSolicitud.Update();
				_upFechaAvance.Update();
				_upEstado.Update();
			}
		}

		public void ddlSolicitudSelection_Change(Object sender, EventArgs e)
		{
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx"); // 
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);

			if (_ddlSolicitud.SelectedIndex != 0)
			{
				int id = int.Parse(_ddlSolicitud.SelectedValue);
				//Actualiza _taDescripcion con la descripción de la solicitud seleccionada
				_taDescripcion.InnerText = _auxControlador.obtenerDescripcion(id);
				_upDescripcion.Update();

				//Selecciona el estado de la solicitud en el DropDownList _ddlEstado
				seleccionarEstado(_auxControlador.obtenerEstado(id));

				//Actualiza _lblPostBy con el postby de la solicitud
				_lblPostBy.Text = _auxControlador.obtenerPostBy(id);
				_upPostBy.Update();

				//Actualiza _lblFechaSolicitud con la fecha de solicitud
				_lblFechaSolicitud.Text = _auxControlador.obtenerFechaSolicitud(id);
				_upFechaSolicitud.Update();



				//Se carga el DropDownList de los avances
				_ddlFechaAvance.DataSource = _auxControlador.crearDataViewAvances(id);
				_ddlFechaAvance.DataTextField = "fecha";
				_ddlFechaAvance.DataValueField = "id";

				//Se limpia el campo de texto de los avances.
				_taAvance.InnerText = "";

				//Se enlazan los datos al control
				_ddlFechaAvance.DataBind();
				_ddlFechaAvance.SelectedIndex = 0;
				_upFechaAvance.Update();
				_upAvance.Update();
			}
		}

		/// <summary>
		/// Carga la descripción del avance cuando se selecciona la fecha del DropDownList
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void ddlFechaAvanceSelection_Change(Object sender, EventArgs e)
		{
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx"); // 
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);

			if (_ddlFechaAvance.SelectedIndex != 0)
			{
				int id = int.Parse(_ddlFechaAvance.SelectedValue);

				_taAvance.InnerText = _auxControlador.obtenerDescripcionAvance(id);
				_upAvance.Update();
			}
		}

		/// <summary>
		/// Selecciona del DropDownList _ddlEstado el estado de la solicitud actual
		/// </summary>
		/// <param name="estado">Estado de la solicitud actual</param>

		private void seleccionarEstado(String estado)
		{
			int contador = 0;

			foreach (ListItem li in _ddlEstado.Items)
			{
				if (li.Text == estado)
				{
					_ddlEstado.SelectedIndex = contador;
					_upEstado.Update();
				}
				else
					contador++;
			}
		}



		/// <summary>
		/// Método OnClick del botón _btnCambiar
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		public void btnCambiar_Click(Object sender, EventArgs e)
		{
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx"); // 
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);

			int _idSolicitud = int.Parse(_ddlSolicitud.SelectedValue);
			int _idEstado = int.Parse(_ddlEstado.SelectedValue);
			_auxControlador.cambiarEstado(_idSolicitud, _idEstado, _ddlEstado.SelectedItem.Text, _sesion.obtenerLoginUsuario(_cookieActual));
		}

	}
}