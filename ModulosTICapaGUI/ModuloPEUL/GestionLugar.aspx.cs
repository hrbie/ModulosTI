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
	public partial class GestionLugar : System.Web.UI.Page
	{
		#region Atributos

		private ControladorPEUL _controladorPEUL;
		private Sesion _sesion;
		private DataTable _tablaDatosLugares;
		private HttpCookie _cookieActual;
		private static List<List<Object>> _listaTipoLugar;

		#endregion

		#region Métodos

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
				_controladorPEUL = new ControladorPEUL(); // Inicializar el controlador
				String grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
				if ((grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")) ||
						(grupoUsuario.Equals("operadores")))
				{
					Notificacion notificacion = new Notificacion();
					notificacion.enviarCorreo("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(_cookieActual) + " a la página de GestionLugar.aspx", "soporte@ic-itcr.ac.cr", "Violación de Seguridad");
					Response.Redirect("../Compartido/AccesoDenegado.aspx");
				}
				_listaTipoLugar = _controladorPEUL.consultarTipoLugar();
				for (int i = 0; i < _listaTipoLugar.Count; i++) // Llenar el drop down de tipos de lugar
					_ddlTipoLugar.Items.Add(_listaTipoLugar.ElementAt(i).ElementAt(1).ToString());
				llenarTablaLugares(); // Llenar el grid de lugares
			}
		}

		/// <summary>
		/// Método encargado de permitir edición de los datos de los lugares
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		protected void _gvwLugares_RowEditing(object sender, GridViewEditEventArgs e)
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
			_gvwLugares.EditIndex = e.NewEditIndex;
			ViewState["filaSeleccionada"] = e.NewEditIndex;
			((TextBox)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_txtLugares")).Text = ((Label)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblLugares")).Text;
			_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblLugares").Visible = false;
			_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_txtLugares").Visible = true;
			((TextBox)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_txtCapacidad")).Text = ((Label)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblCapacidad")).Text;
			_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblCapacidad").Visible = false;
			_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_txtCapacidad").Visible = true;
			((TextBox)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_txtDescripcion")).Text = ((Label)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblDescripcion")).Text;
			_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblDescripcion").Visible = false;
			_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_txtDescripcion").Visible = true;
			((TextBox)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_txtLogin")).Text = ((Label)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblLogin")).Text;
			_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblLogin").Visible = false;
			_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_txtLogin").Visible = true;
			((DropDownList)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_ddlTipo")).Items.Add("Seleccionar");
			for (int i = 0; i < _listaTipoLugar.Count; i++) // Llenar el drop down de tipos de lugar
				((DropDownList)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_ddlTipo")).Items.Add(_listaTipoLugar.ElementAt(i).ElementAt(1).ToString());
			_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblTipo").Visible = false;
			_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_ddlTipo").Visible = true;
			_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblEstado").Visible = false;
			_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_ddlEstado").Visible = true;
			_gvwLugares.Columns[7].Visible = false;
			_btnCancelar.Enabled = true;
			_btnGuardar.Enabled = true;
		}

		/// <summary>
		/// Método que se encarga de llenar el grid de lugares
		/// </summary>

		private void llenarTablaLugares()
		{
			_controladorPEUL = new ControladorPEUL();
			_tablaDatosLugares = _controladorPEUL.obtenerDatosLugares();
			if (_tablaDatosLugares == null)
			{
                _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                _lblMensaje.Text = "Los datos de lugares no han podido ser cargados";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
			}
			else if (_tablaDatosLugares.Rows.Count == 0)
			{
				_imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                _lblMensaje.Text = "No hay lugares registrados en el sistema.";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
			}
			else
			{
				_gvwLugares.DataSource = _tablaDatosLugares;
				_gvwLugares.DataBind();
			}
		}

		/// <summary>
		/// Método que se encarga de manejar el evento al crear un nuevo lugar
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		protected void _btnCrearLugar_Click(object sender, EventArgs e)
		{
			_sesion = new Sesion();
			_controladorPEUL = new ControladorPEUL();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx"); // 
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
            if ((_txtCapacidad.Text.Equals("")) || (_txtDescripcion.Text.Equals("")) || (_txtLoginEncargado.Text.Equals("")) ||
				(_txtNombreLugar.Text.Equals("")) || (_ddlTipoLugar.SelectedIndex == 0)) // Verificar que todos los campos han sido especificados
			{
				_imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
				_lblMensaje.Text = "Debe especificar todos los campos del formulario";
				_imgMensaje.Visible = true;
				_lblMensaje.Visible = true;
			}
			else // Intentar crea el nuevo lugar
			{
				int resultadoCreacion = _controladorPEUL.crearLugar(_txtNombreLugar.Text, _txtLoginEncargado.Text, _txtCapacidad.Text, _txtDescripcion.Text, Convert.ToInt32(_listaTipoLugar.ElementAt(_ddlTipoLugar.SelectedIndex - 1).ElementAt(0)), _sesion.obtenerLoginUsuario(_cookieActual));
				if (resultadoCreacion == 1) // Si se pudo crear el lugar
				{
					_imgMensaje.ImageUrl = "~/Imagenes/ok.png";
					_lblMensaje.Text = "Se ha creado con éxito el lugar";
					_imgMensaje.Visible = true;
					_lblMensaje.Visible = true;
					_txtDescripcion.Text = "";
					_txtCapacidad.Text = "";
					_txtLoginEncargado.Text = "";
					_txtNombreLugar.Text = "";
					_ddlTipoLugar.SelectedIndex = 0;
					llenarTablaLugares();
				}
                else if (resultadoCreacion == 0) // Hubo error al crear el lugar
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Ha habido un error al crear el lugar";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
                else // Si el login que se especifica no es encontrado en el LDAP
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                    _lblMensaje.Text = "No hay ningún usuario registrado con el login proporcionado";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
			}
		}

		protected void _btnGuardar_Click(object sender, EventArgs e)
		{
			_sesion = new Sesion();
			_controladorPEUL = new ControladorPEUL();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx");
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
			String pkLugar = ((Label)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblPKLugar")).Text; // PK del lugar que se está editando puesto en una columna oculta
			String nombreLugar = ((TextBox)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].Cells[1].FindControl("_txtLugares")).Text;
			String capacidad = ((TextBox)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_txtCapacidad")).Text;
			String descripcion = ((TextBox)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_txtDescripcion")).Text;
            String loginEncargado = ((TextBox)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_txtLogin")).Text;
			DropDownList tipo = (DropDownList)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_ddlTipo");
			DropDownList activo = (DropDownList)_gvwLugares.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_ddlestado");
			Boolean activoLugar; // Indica si el lugar esta habilitado o no
            if ((tipo.SelectedIndex != 0) && (!nombreLugar.Equals("")) && (!capacidad.Equals("")) && (!descripcion.Equals("")) &&
                (!loginEncargado.Equals("")) && (activo.SelectedIndex != 0))
            {
                if (activo.SelectedIndex == 1)
                    activoLugar = true; // El lugar esta habilitado
                else
                    activoLugar = false; // El lugar esta inhabilitado
                int resultado = _controladorPEUL.modificarLugar(pkLugar, nombreLugar, loginEncargado, capacidad, descripcion, activoLugar, Convert.ToInt32(_listaTipoLugar.ElementAt(tipo.SelectedIndex - 1).ElementAt(0)), _sesion.obtenerLoginUsuario(_cookieActual));
                if (resultado == 1)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/ok.png";
                    _lblMensaje.Text = "Se han modificado los datos del lugar con éxito";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _gvwLugares.EditIndex = -1;
                    llenarTablaLugares();
                    _btnCancelar.Enabled = false;
                    _btnGuardar.Enabled = false;
                    _gvwLugares.Columns[7].Visible = true;
                }
                else if (resultado == 0)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Ha habido un error al intentar modificar los datos del lugar";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
                else
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                    _lblMensaje.Text = "El login proporcionado no corresponde a ningún usuario del sistema";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }
            else
            {
                _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                _lblMensaje.Text = "Debe especificar todos los campos del formulario";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }
		}

		protected void _btnCancelar_Click(object sender, EventArgs e)
		{
			_gvwLugares.EditIndex = -1;
			llenarTablaLugares();
			_btnCancelar.Enabled = false;
			_btnGuardar.Enabled = false;
			_gvwLugares.Columns[7].Visible = true;
		}

		#endregion
	}
}