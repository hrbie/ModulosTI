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

namespace ModulosTICapaGUI.ModuloReservacion
{
	public partial class ModificaReservacion : System.Web.UI.Page
	{
		#region Atributos

		private Sesion _sesion;
		private HttpCookie _cookieActual;
		private ControladorReservacionCR _controlador; // Controlador de la Interfaz
		private DataTable _tablaDatosReservacion;
        private static List<List<object>> _lugares; // Lista de lugares que será cargada en el _ddlLugar
        private static List<List<object>> _carreras; // Lista de carreras que será cargada en el _ddlCarrera
        private static List<List<object>> _cursos; // Lista de cursos que será cargada en el _ddlCurso
        private int _errorCarga; // Para manejo de errores en el tiempo de carga de la página
        // 0: No se cargaron lugares
        // 1: No se cargaron los cursos
        // 2: No se cargaron tanto lugares como cursos
        private DataTable _tablaReportes= new DataTable("Excepciones"); //para cargar los valores de las excepciones
                
                

		#endregion

		#region Métodos

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack) // Solo cargar los datos la primera vez que la página ha sido cargada
            {
                _sesion = new Sesion();
                _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
                if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                    Response.Redirect("../Autentificacion/Login.aspx"); // 
                else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                    Response.SetCookie(_cookieActual);
				_controlador = new ControladorReservacionCR();
                String grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
                if ((grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests"))) // En caso de que usuarios que no puedan ingresar a ésta área intenten hacerlo
                {
                    Notificacion notificacion = new Notificacion();
                    notificacion.enviarCorreo("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(_cookieActual) + " a la página de CrearReservacion.aspx", "soporte@ic-itcr.ac.cr", "Violación de Seguridad");
                    Response.Redirect("../Compartido/AccesoDenegado.aspx");
                }
                switch (grupoUsuario) // Deshabilitar opción de marcar como horario permanente a ciertos usuarios
                {
                    case "operadores":
                        _cbHorario.Enabled = false;
                        break;
                    case "prof":
                        _cbHorario.Enabled = false;
                        break;
                    default: // Para Jefes, Profesor encargado de la oficina de TI y Soporte
                        _cbHorario.Enabled = true;
                        break;
                }
                _lugares = _controlador.obtenerLugares();
                _errorCarga = -1;
                if (_lugares != null) // Llenar _ddlLugar si se encontraton datos 
                {
                    for (int i = 0; i < _lugares.Count; i++)
                        _ddlLugar.Items.Add(_lugares.ElementAt(i).ElementAt(1).ToString()); // Obtiene los nombres de los lugares encontrados
                }
                else // No se encontraron lugares
                    _errorCarga = 0;
                _carreras = _controlador.obtenerCarreras();
                if ((_carreras != null) && (_carreras.Count != 0)) // Llenar _ddlCarrera si se encontraton datos 
                {
                    for (int i = 0; i < _carreras.Count; i++)
                        _ddlCarrera.Items.Add(_carreras.ElementAt(i).ElementAt(1).ToString()); // Obtiene los nombres de las carreras encontradas
                }
                else // No se encontraron carreras
                {
                    if (_errorCarga == 0)
                        _errorCarga = 2;
                    else
                        _errorCarga = 1;
                }
                switch (_errorCarga) // Detectar errores
                {
                    case 0:
                        _ddlLugar.Items.Clear();
                        _ddlLugar.Items.Add("No hay lugares disponibles");
                        _imgMensajeMod.ImageUrl = "../Imagenes/Error.png";
                        _lblMensajeMod.Text = "Se ha producido un error al obtener los lugares.";
                        _imgMensajeMod.Visible = true;
                        _lblMensajeMod.Visible = true;
                        break;
                    case 1:
                        _ddlCarrera.Items.Clear();
                        _ddlCarrera.Items.Add("No hay carreras disponibles");
                        _imgMensajeMod.ImageUrl = "../Imagenes/Error.png";
                        _lblMensajeMod.Text = "Se ha producido un error al obtener las carreras.";
                        _imgMensajeMod.Visible = true;
                        _lblMensajeMod.Visible = true;
                        break;
                    case 2:
                        _ddlCarrera.Items.Clear();
                        _ddlCarrera.Items.Add("No hay carreras disponibles");
                        _ddlLugar.Items.Clear();
                        _ddlLugar.Items.Add("No hay lugares disponibles");
                        _imgMensajeMod.ImageUrl = "../Imagenes/Error.png";
                        _lblMensajeMod.Text = "Ha habido un problema al cargar información en la página.";
                        _imgMensajeMod.Visible = true;
                        _lblMensajeMod.Visible = true;
                        break;
                }
                llenarTablaReservaciones();
			}
		}



        /// <summary>
        /// Método encargado de permitir edición de los datos de los lugares
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _gridReservaciones_RowEditing(object sender, GridViewEditEventArgs e)
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
            _gridReservaciones.EditIndex = e.NewEditIndex;
            _treservacion.Visible = true;
            
            _gridReservaciones.Visible = false;
            ViewState["filaSeleccionada"] = e.NewEditIndex;
            _lblMensajeMod.Visible = false;
            _imgMensajeMod.Visible = false;
            _ddlLugar.SelectedValue =  ((Label)_gridReservaciones.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblLugar")).Text;
            actualizaLugar();
            _controlador = new ControladorReservacionCR();
            _ddlCarrera.SelectedValue = ((Label)_gridReservaciones.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblCarreraReserv")).Text;
            actualizaCursos();
            _txtDescripcionReservacion.Text = ((Label)_gridReservaciones.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblDescripcion")).Text;
            _lblPKReserv.Text = ((Label)_gridReservaciones.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblPKReservacion")).Text;
            _txtLogin.Text = ((Label)_gridReservaciones.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblSolicitante")).Text;
            _txtFechaFinal.Text = ((Label)_gridReservaciones.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblFechaFinal")).Text;
            _txtFechaInicio.Text = ((Label)_gridReservaciones.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblFechaInicio")).Text;
            actualizaFecha();
            string dias = ((Label)_gridReservaciones.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblDia")).Text;
            for (int i = 0; i < dias.Length; i++)
            {
                switch (dias[i])
                {
                    case 'L':
                        _cbLunes.Checked = true;
                        break;
                    case 'K':
                        _cbMartes.Checked = true;
                        break;
                    case 'M':
                        _cbMiercoles.Checked = true;
                        break;
                    case 'J':
                        _cbJueves.Checked = true;
                        break;
                    case 'V':
                        _cbViernes.Checked = true;
                        break;
                    case 'S':
                        _cbSabado.Checked = true;
                        break;
                    case 'D':
                        _cbDomingo.Checked = true;
                        break;
                }
            }
            string horaInicio = ((Label)_gridReservaciones.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblHoraInicio")).Text;
            string horaFinal = ((Label)_gridReservaciones.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblHoraFinal")).Text;
            _ddlHoraInicio.SelectedValue = horaInicio.Substring(0,2);
            _ddlMinutoInicio.SelectedValue = horaInicio.Substring(3, 2);
            _ddlHoraFinal.SelectedValue = horaFinal.Substring(0, 2);
            _ddlMinutoFinal.SelectedValue = horaFinal.Substring(3, 2);
        }


        /// <summary>
        /// Método que actualiza la informacion respecto al lugar seleccionado
        /// </summary>

        private void actualizaCursos()
        {
            _cursos = _controlador.obtenerCursos(_ddlCarrera.SelectedValue);
            _ddlCurso.Enabled = true;
            //_ddlCurso.Enabled = false;
            _upCurso.Update();
            _ddlCurso.Items.Clear();
            _ddlCurso.Items.Add("Seleccionar");
            for (int i = 0; i < _cursos.Count; i++) // Obtiene los nombres y codigos de los cursos encontrados
                _ddlCurso.Items.Add(_cursos.ElementAt(i).ElementAt(2).ToString());
            _ddlCurso.SelectedValue = ((Label)_gridReservaciones.Rows[(int)ViewState["filaSeleccionada"]].FindControl("_lblCurso")).Text;

        }
        /// <summary>
        /// Método que actualiza la informacion respecto al lugar seleccionado
        /// </summary>

        private void actualizaLugar()
        {
            _lblCantidad.Text = _lugares.ElementAt(_ddlLugar.SelectedIndex - 1).ElementAt(2).ToString();
            _txtDescripcion.Text = _lugares.ElementAt(_ddlLugar.SelectedIndex - 1).ElementAt(3).ToString();
            _lblNombreEncargado.Text = _lugares.ElementAt(_ddlLugar.SelectedIndex - 1).ElementAt(4).ToString();
            _lblNombreTipo.Text = _lugares.ElementAt(_ddlLugar.SelectedIndex - 1).ElementAt(5).ToString();
            _upEncargado.Update();
            _upDescripcion.Update();
            _upPanelCantidad.Update();
            _upTipo.Update();
        }

        /// <summary>
		/// Método que se encarga de llenar el grid de reservaciones
		/// </summary>

        private void llenarTablaReservaciones()
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            _controlador = new ControladorReservacionCR();
            _tablaDatosReservacion = _controlador.consultarTodasReservaciones();
            if (_tablaDatosReservacion == null)
            {
                _imgMensajeMod.ImageUrl = "~/Imagenes/Error.png";
                _lblMensajeMod.Text = "La informacion no pudo ser cargada";
                _imgMensajeMod.Visible = true;
                _lblMensajeMod.Visible = true;
            }
            else if (_tablaDatosReservacion.Rows.Count == 0)
            {
                _imgMensajeMod.ImageUrl = "~/Imagenes/Advertencia.png";
                _lblMensajeMod.Text = "No hay lugares registrados en el sistema.";
                _imgMensajeMod.Visible = true;
                _lblMensajeMod.Visible = true;
            }
            else
            {
                _gridReservaciones.DataSource = _tablaDatosReservacion;
                _gridReservaciones.DataBind();
            }
        }

        /// <summary>
        /// Método que se encarga del manejo de la selección en el _ddlLugar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _ddlLugar_SelectedIndexChanged(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            if (_ddlLugar.SelectedIndex != 0)
            {
                actualizaLugar();
            }
        }


        /// <summary>
		/// Método para el control del _ddlCarrera y su selección
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		protected void _ddlCarrera_SelectedIndexChanged(object sender, EventArgs e)
		{
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
			if (_ddlCarrera.SelectedIndex != 0)
			{
				_controlador = new ControladorReservacionCR();
				_cursos = _controlador.obtenerCursos(_ddlCarrera.SelectedValue);
				_ddlCurso.Enabled = true;
				if ((_cursos != null) && (_cursos.Count != 0)) // Llenar _ddlCurso si se encontraton datos 
				{
					_ddlCurso.Items.Clear();
					_ddlCurso.Items.Add("Seleccionar");
					for (int i = 0; i < _cursos.Count; i++) // Obtiene los nombres y codigos de los cursos encontrados
						_ddlCurso.Items.Add(_cursos.ElementAt(i).ElementAt(1).ToString() + " " + _cursos.ElementAt(i).ElementAt(2).ToString());
				}
				else // No se encontraron cursos
				{
					_ddlCurso.Items.Clear();
					_ddlCurso.Items.Add("No hay cursos disponibles");
					_imgMensajeMod.ImageUrl = "~/Imagenes/Error.png";
					_lblMensajeMod.Text = "Se ha producido un error al obtener los cursos.";
					_imgMensajeMod.Visible = true;
					_lblMensajeMod.Visible = true;
				}
			}
			else
				_ddlCurso.Enabled = false;
			_upCurso.Update();
		}

        /// <summary>
        /// Método que se encarga de manejar la fecha de inicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _txtFechaInicio_TextChanged(object sender, EventArgs e)
        {
            DateTime fechaInicio = Convert.ToDateTime(_txtFechaInicio.Text);
            _cbLunes.Checked = false;
            _cbMartes.Checked = false;
            _cbMiercoles.Checked = false;
            _cbJueves.Checked = false;
            _cbViernes.Checked = false;
            _cbSabado.Checked = false;
            _cbDomingo.Checked = false;
            switch (fechaInicio.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    _cbLunes.Checked = true;
                    break;
                case DayOfWeek.Tuesday:
                    _cbMartes.Checked = true;
                    break;
                case DayOfWeek.Wednesday:
                    _cbMiercoles.Checked = true;
                    break;
                case DayOfWeek.Thursday:
                    _cbJueves.Checked = true;
                    break;
                case DayOfWeek.Friday:
                    _cbViernes.Checked = true;
                    break;
                case DayOfWeek.Saturday:
                    _cbSabado.Checked = true;
                    break;
                case DayOfWeek.Sunday:
                    _cbDomingo.Checked = true;
                    break;
            }
            if (!_txtFechaFinal.Text.Equals(""))
            {
                DateTime fechaFinal = Convert.ToDateTime(_txtFechaFinal.Text);
                switch (fechaFinal.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        _cbLunes.Checked = true;
                        break;
                    case DayOfWeek.Tuesday:
                        _cbMartes.Checked = true;
                        break;
                    case DayOfWeek.Wednesday:
                        _cbMiercoles.Checked = true;
                        break;
                    case DayOfWeek.Thursday:
                        _cbJueves.Checked = true;
                        break;
                    case DayOfWeek.Friday:
                        _cbViernes.Checked = true;
                        break;
                    case DayOfWeek.Saturday:
                        _cbSabado.Checked = true;
                        break;
                    case DayOfWeek.Sunday:
                        _cbDomingo.Checked = true;
                        break;
                }
            }
            _upDias.Update();
        }


        /// <summary>
        /// Método que se encarga de actualizar dias de la fecha
        /// </summary>

        protected void actualizaFecha()
        {
            DateTime fechaInicio = Convert.ToDateTime(_txtFechaInicio.Text);
            DateTime fechaFinal = Convert.ToDateTime(_txtFechaFinal.Text);
            _cbLunes.Checked = false;
            _cbMartes.Checked = false;
            _cbMiercoles.Checked = false;
            _cbJueves.Checked = false;
            _cbViernes.Checked = false;
            _cbSabado.Checked = false;
            _cbDomingo.Checked = false;
            switch (fechaInicio.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    _cbLunes.Checked = true;
                    break;
                case DayOfWeek.Tuesday:
                    _cbMartes.Checked = true;
                    break;
                case DayOfWeek.Wednesday:
                    _cbMiercoles.Checked = true;
                    break;
                case DayOfWeek.Thursday:
                    _cbJueves.Checked = true;
                    break;
                case DayOfWeek.Friday:
                    _cbViernes.Checked = true;
                    break;
                case DayOfWeek.Saturday:
                    _cbSabado.Checked = true;
                    break;
                case DayOfWeek.Sunday:
                    _cbDomingo.Checked = true;
                    break;
            }
            if (!_txtFechaFinal.Text.Equals(""))
            {
                switch (fechaFinal.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        _cbLunes.Checked = true;
                        break;
                    case DayOfWeek.Tuesday:
                        _cbMartes.Checked = true;
                        break;
                    case DayOfWeek.Wednesday:
                        _cbMiercoles.Checked = true;
                        break;
                    case DayOfWeek.Thursday:
                        _cbJueves.Checked = true;
                        break;
                    case DayOfWeek.Friday:
                        _cbViernes.Checked = true;
                        break;
                    case DayOfWeek.Saturday:
                        _cbSabado.Checked = true;
                        break;
                    case DayOfWeek.Sunday:
                        _cbDomingo.Checked = true;
                        break;
                }
            }
            switch (fechaFinal.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    _cbLunes.Checked = true;
                    break;
                case DayOfWeek.Tuesday:
                    _cbMartes.Checked = true;
                    break;
                case DayOfWeek.Wednesday:
                    _cbMiercoles.Checked = true;
                    break;
                case DayOfWeek.Thursday:
                    _cbJueves.Checked = true;
                    break;
                case DayOfWeek.Friday:
                    _cbViernes.Checked = true;
                    break;
                case DayOfWeek.Saturday:
                    _cbSabado.Checked = true;
                    break;
                case DayOfWeek.Sunday:
                    _cbDomingo.Checked = true;
                    break;
            }
            if (!_txtFechaInicio.Text.Equals(""))
            {
                switch (fechaInicio.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        _cbLunes.Checked = true;
                        break;
                    case DayOfWeek.Tuesday:
                        _cbMartes.Checked = true;
                        break;
                    case DayOfWeek.Wednesday:
                        _cbMiercoles.Checked = true;
                        break;
                    case DayOfWeek.Thursday:
                        _cbJueves.Checked = true;
                        break;
                    case DayOfWeek.Friday:
                        _cbViernes.Checked = true;
                        break;
                    case DayOfWeek.Saturday:
                        _cbSabado.Checked = true;
                        break;
                    case DayOfWeek.Sunday:
                        _cbDomingo.Checked = true;
                        break;
                }
            }
            _upDias.Update();
        }
        

        /// <summary>
        /// Método que se encarga de manejar la fecha final
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _txtFechaFinal_TextChanged(object sender, EventArgs e)
        {
            DateTime fechaFinal = Convert.ToDateTime(_txtFechaFinal.Text);
            _cbLunes.Checked = false;
            _cbMartes.Checked = false;
            _cbMiercoles.Checked = false;
            _cbJueves.Checked = false;
            _cbViernes.Checked = false;
            _cbSabado.Checked = false;
            _cbDomingo.Checked = false;
            switch (fechaFinal.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    _cbLunes.Checked = true;
                    break;
                case DayOfWeek.Tuesday:
                    _cbMartes.Checked = true;
                    break;
                case DayOfWeek.Wednesday:
                    _cbMiercoles.Checked = true;
                    break;
                case DayOfWeek.Thursday:
                    _cbJueves.Checked = true;
                    break;
                case DayOfWeek.Friday:
                    _cbViernes.Checked = true;
                    break;
                case DayOfWeek.Saturday:
                    _cbSabado.Checked = true;
                    break;
                case DayOfWeek.Sunday:
                    _cbDomingo.Checked = true;
                    break;
            }
            if (!_txtFechaInicio.Text.Equals(""))
            {
                DateTime fechaInicio = Convert.ToDateTime(_txtFechaInicio.Text);
                switch (fechaInicio.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        _cbLunes.Checked = true;
                        break;
                    case DayOfWeek.Tuesday:
                        _cbMartes.Checked = true;
                        break;
                    case DayOfWeek.Wednesday:
                        _cbMiercoles.Checked = true;
                        break;
                    case DayOfWeek.Thursday:
                        _cbJueves.Checked = true;
                        break;
                    case DayOfWeek.Friday:
                        _cbViernes.Checked = true;
                        break;
                    case DayOfWeek.Saturday:
                        _cbSabado.Checked = true;
                        break;
                    case DayOfWeek.Sunday:
                        _cbDomingo.Checked = true;
                        break;
                }
            }
            _upDias.Update();
        }

       
         /// <summary>
        /// Método que controla el evento de cancelar la reservacion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _btnCancelaReservacion_Click(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            _controlador = new ControladorReservacionCR();
            String resultado = _controlador.cancelarReservacion(_lblPKReserv.Text, "Pao");//_sesion.obtenerLoginUsuario(_cookieActual));
            if (resultado == null) // Si tuvo éxito
                {
                    _treservacion.Visible = false;
                    _gridExcepciones.Visible = false;
                    _gridReservaciones.Visible = true;
                    limpiaFormulario();
                    _gridReservaciones.EditIndex = -1;
                    llenarTablaReservaciones();
                    _imgMensajeMod.ImageUrl = "../Imagenes/ok.png";
                    _lblMensajeMod.Text = "La reservación se ha cancelado con éxito";
                    _imgMensajeMod.Visible = true;
                    _lblMensajeMod.Visible = true;
                    limpiaFormulario();
                }
                else 
                {
                    _lblMensajeMod.Text = "Ha habido un error al cancelar la reservacion";
                    _imgMensajeMod.ImageUrl = "../Imagenes/Error.png";
                    _imgMensajeMod.Visible = true;
                    _lblMensajeMod.Visible = true;
                }
        }

        /// <summary>
        /// Método que controla el evento de volver a la consulta de reservaciones
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            _treservacion.Visible = false;
            _gridExcepciones.Visible = false;
            _gridReservaciones.Visible = true;
            limpiaFormulario();
            _gridReservaciones.EditIndex = -1;
            llenarTablaReservaciones();
            _lblMensajeMod.Visible = false;
            _imgMensajeMod.Visible = false;
        }
       
        /// <summary>
        /// Método que limpia el formulario de la reservacion
        /// </summary>
        /// 
        private void limpiaFormulario()
        {
            _ddlCurso.Enabled = false;
            _lblChoques.Visible = false;
            _txtChoques.Visible = false;
            _lblCantidad.Visible = false;
            _lblNombreEncargado.Visible = false;
            _lblNombreTipo.Visible = false;
            _cbLunes.Checked = false;
            _cbMartes.Checked = false;
            _cbMiercoles.Checked = false;
            _cbJueves.Checked = false;
            _cbViernes.Checked = false;
            _cbSabado.Checked = false;
            _cbDomingo.Checked = false;
            _cbHorario.Checked = false;
            _ddlCurso.SelectedIndex = 0;
            _ddlHoraInicio.SelectedIndex = 0;
            _ddlHoraFinal.SelectedIndex = 0;
            _ddlMinutoInicio.SelectedIndex = 0;
            _ddlMinutoFinal.SelectedIndex = 0;
            _ddlCarrera.SelectedIndex = 0;
            _ddlLugar.SelectedIndex = 0;
            _ddlCurso.Items.Clear();
            _ddlCurso.Items.Add("Seleccionar");
            _txtDescripcion.Text = "";
            _txtDescripcionReservacion.Text = "";
            _txtLogin.Text = "";
            _txtFechaInicio.Text = "";
            _txtFechaFinal.Text = "";
            _txtFechaExcepcion.Text = "";
        
        }

        /// <summary>
        /// Método que controla el evento de modificar una reservación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _btnModificar_Click(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            bool reservar = true; // Estas verificaciones se pueden optimizar si se pasa las fechas y horas dadas por el usuario a un DateTime ******
             //Verificar que todos los campos del formulario han sido especificados por el usuario
            if ((_txtFechaInicio.Text.Equals("")) || (_txtFechaFinal.Text.Equals("")) || (_txtLogin.Text.Equals("")) || (_ddlCarrera.SelectedIndex == 0)
                || (_ddlCurso.SelectedIndex == 0) || (_ddlHoraFinal.SelectedIndex == 0) || (_ddlHoraInicio.SelectedIndex == 0)
                || (_ddlLugar.SelectedIndex == 0) || (_ddlMinutoFinal.SelectedIndex == 0) || (_ddlMinutoInicio.SelectedIndex == 0))
            {
                _imgMensajeMod.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensajeMod.Text = "Debe especificar todos los campos del formulario";
                _imgMensajeMod.Visible = true;
                _lblMensajeMod.Visible = true;
                reservar = false;
            } // Al menos un día haya sido especificado
            else if ((!_cbLunes.Checked) && (!_cbMartes.Checked) && (!_cbMiercoles.Checked) && (!_cbJueves.Checked) && (!_cbViernes.Checked)
                    && (!_cbSabado.Checked) && (!_cbDomingo.Checked))
            {
                _imgMensajeMod.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensajeMod.Text = "Debe seleccionar al menos un día para la reservación";
                _imgMensajeMod.Visible = true;
                _lblMensajeMod.Visible = true;
                reservar = false;
            } // La fechaInicio no sea menor a la de hoy
            else if (Convert.ToDateTime(_txtFechaInicio.Text) < DateTime.Today)
            {
                _imgMensajeMod.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensajeMod.Text = "La fecha de inicio no puede ser menor a la del día de hoy";
                _imgMensajeMod.Visible = true;
                _lblMensajeMod.Visible = true;
                reservar = false;
            } // La fechaFinal no sea menor a la fechaInicio
            else if ((Convert.ToDateTime(_txtFechaInicio.Text) > Convert.ToDateTime(_txtFechaFinal.Text)))
            {
                _imgMensajeMod.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensajeMod.Text = "La fecha final no puede ser menor a la inicial";
                _imgMensajeMod.Visible = true;
                _lblMensajeMod.Visible = true;
                reservar = false;
            } // La horaFinal no sea menor o igual a la horaInicio
            else if ((Convert.ToInt32(_ddlHoraInicio.Text) > Convert.ToInt32(_ddlHoraFinal.Text))
                        || ((Convert.ToInt32(_ddlMinutoInicio.Text) >= Convert.ToInt32(_ddlMinutoFinal.Text)) &&
                            (Convert.ToInt32(_ddlHoraInicio.Text) == Convert.ToInt32(_ddlHoraFinal.Text))))
            {
                _imgMensajeMod.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensajeMod.Text = "La hora de inicio no puede ser mayor o igual a la hora final";
                _imgMensajeMod.Visible = true;
                _lblMensajeMod.Visible = true;
                reservar = false;
            } // La horaInicio no sea menor a la actual en caso de elegir el día de hoy
            else if ((Convert.ToDateTime(_txtFechaInicio.Text) == Convert.ToDateTime(_txtFechaFinal.Text)) &&
                        ((Convert.ToInt32(_ddlHoraInicio.Text) < DateTime.Now.Hour) || ((Convert.ToInt32(_ddlMinutoInicio.Text) < DateTime.Now.Minute)
                        && (Convert.ToInt32(_ddlHoraInicio.Text) == DateTime.Now.Hour))))
            {
                _imgMensajeMod.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensajeMod.Text = "La hora de inicio no puede ser menor a la actual";
                _imgMensajeMod.Visible = true;
                _lblMensajeMod.Visible = true;
                reservar = false;
            }
            else if ((Convert.ToDateTime(_txtFechaFinal.Text).Subtract(Convert.ToDateTime(_txtFechaInicio.Text))).TotalDays < 7.0)
            {
                DateTime inicio = Convert.ToDateTime(_txtFechaInicio.Text);
                DateTime temp = Convert.ToDateTime(_txtFechaInicio.Text); // Para no realizar varios Convert más adelante
                DateTime final = Convert.ToDateTime(_txtFechaFinal.Text);
                int check = 0;
                if (_cbLunes.Checked)
                {
                    while (inicio <= final)
                    {
                        if (DayOfWeek.Monday == inicio.DayOfWeek)
                        {
                            check = 1;
                            break;
                        }
                        inicio = inicio.AddDays(1);
                    }
                }
                else
                    check = 1;
                if ((check != 0) && (_cbMartes.Checked))
                {
                    inicio = temp;
                    check = 0;
                    while (inicio <= final)
                    {
                        if (DayOfWeek.Tuesday == inicio.DayOfWeek)
                        {
                            check = 1;
                            break;
                        }
                        inicio = inicio.AddDays(1);
                    }
                }
                if ((check != 0) && (_cbMiercoles.Checked))
                {
                    inicio = temp;
                    check = 0;
                    while (inicio <= final)
                    {
                        if (DayOfWeek.Wednesday == inicio.DayOfWeek)
                        {
                            check = 1;
                            break;
                        }
                        inicio = inicio.AddDays(1);
                    }
                }
                if ((check != 0) && (_cbJueves.Checked))
                {
                    inicio = temp;
                    check = 0;
                    while (inicio <= final)
                    {
                        if (DayOfWeek.Thursday == inicio.DayOfWeek)
                        {
                            check = 1;
                            break;
                        }
                        inicio = inicio.AddDays(1);
                    }
                }
                if ((check != 0) && (_cbViernes.Checked))
                {
                    inicio = temp;
                    check = 0;
                    while (inicio <= final)
                    {
                        if (DayOfWeek.Friday == inicio.DayOfWeek)
                        {
                            check = 1;
                            break;
                        }
                        inicio = inicio.AddDays(1);
                    }
                }
                if ((check != 0) && (_cbSabado.Checked))
                {
                    inicio = temp;
                    check = 0;
                    while (inicio <= final)
                    {
                        if (DayOfWeek.Saturday == inicio.DayOfWeek)
                        {
                            check = 1;
                            break;
                        }
                        inicio = inicio.AddDays(1);
                    }
                }
                if ((check != 0) && (_cbDomingo.Checked))
                {
                    inicio = temp;
                    check = 0;
                    while (inicio <= final)
                    {
                        if (DayOfWeek.Sunday == inicio.DayOfWeek)
                        {
                            check = 1;
                            break;
                        }
                        inicio = inicio.AddDays(1);
                    }
                }
                if (check == 0) // Si se encontró algún día marcado que no se encontraba dentro del rango de fecha se informa
                {
                    _imgMensajeMod.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensajeMod.Text = "Uno o más días que fueron marcados no se encuentran dentro del rango de fechas especificado";
                    _imgMensajeMod.Visible = true;
                    _lblMensajeMod.Visible = true;
                    reservar = false;
                }
            }
            if (reservar) // Enviar al controlador de Reservación la información para que pueda ser creada
            {
                List<string> datosGenerales = new List<string>();
                datosGenerales.Add(_lugares.ElementAt(_ddlLugar.SelectedIndex - 1).ElementAt(0).ToString());
                datosGenerales.Add(_cursos.ElementAt(_ddlCurso.SelectedIndex - 1).ElementAt(0).ToString());
                datosGenerales.Add(_txtLogin.Text);
                datosGenerales.Add(_txtDescripcionReservacion.Text);
                datosGenerales.Add(_txtFechaInicio.Text);
                datosGenerales.Add(_txtFechaFinal.Text);
                datosGenerales.Add(_ddlHoraInicio.SelectedValue);
                datosGenerales.Add(_ddlMinutoInicio.SelectedValue);
                datosGenerales.Add(_ddlHoraFinal.SelectedValue);
                datosGenerales.Add(_ddlMinutoFinal.SelectedValue);


                //para obtener las excepciones que se agregaron
                List<List<string>> excepciones = new List<List<string>>();

                for (int i = 0; i < _gridExcepciones.Rows.Count; i++)
                {
                    List<string> excepcion = new List<string>();
                    excepcion.Add(((Label)_gridExcepciones.Rows[i].FindControl("_lblFechaExcepcion")).Text);
                    DateTime fecha=Convert.ToDateTime(((Label)_gridExcepciones.Rows[i].FindControl("_lblFechaExcepcion")).Text);
                    if (fecha.DayOfWeek==DayOfWeek.Monday)
                        excepcion.Add("L");
                    if (fecha.DayOfWeek==DayOfWeek.Tuesday)
                        excepcion.Add("K");
                    if (fecha.DayOfWeek==DayOfWeek.Wednesday)
                        excepcion.Add("M");
                    if (fecha.DayOfWeek==DayOfWeek.Thursday)
                        excepcion.Add("J");
                    if (fecha.DayOfWeek==DayOfWeek.Friday)
                        excepcion.Add("V");
                    if (fecha.DayOfWeek==DayOfWeek.Saturday)
                        excepcion.Add("S");
                    if (fecha.DayOfWeek==DayOfWeek.Sunday)
                        excepcion.Add("D");
                    excepcion.Add(((Label)_gridExcepciones.Rows[i].FindControl("_lblMotivo")).Text);
                    excepciones.Add(excepcion);
                }
                List<string> exc = new List<string>();//para que siempre tenga un campo y no de error de indice
                exc.Add("");
                exc.Add("");
                exc.Add("");
                excepciones.Add(exc);
                List<char> dias = new List<char>();
                if (_cbLunes.Checked)
                    dias.Add('L');
                if (_cbMartes.Checked)
                    dias.Add('K');
                if (_cbMiercoles.Checked)
                    dias.Add('M');
                if (_cbJueves.Checked)
                    dias.Add('J');
                if (_cbViernes.Checked)
                    dias.Add('V');
                if (_cbSabado.Checked)
                    dias.Add('S');
                if (_cbDomingo.Checked)
                    dias.Add('D');
                _controlador = new ControladorReservacionCR();
                List<List<object>> resultado = _controlador.modificaReservacion(excepciones,_lblPKReserv.Text,datosGenerales, dias,_txtLogin.Text, _cbHorario.Checked);
                if (resultado == null) // Si tuvo éxito
                {
                    _treservacion.Visible = false;
                    _gridExcepciones.Visible = false;
                    _gridReservaciones.Visible = true;
                    _gridReservaciones.EditIndex = -1;
                    llenarTablaReservaciones();
                    _imgMensajeMod.ImageUrl = "../Imagenes/ok.png";
                    _lblMensajeMod.Text = "La reservación se ha modificado con éxito";
                    _imgMensajeMod.Visible = true;
                    _lblMensajeMod.Visible = true;
                    limpiaFormulario();
                }
                else if ((resultado.Count > 0) && (resultado.ElementAt(0).ElementAt(0).Equals("Error LDAP"))) // Si hay error con el usuario en el LDAP
                {
                    _lblMensajeMod.Text = "Ha habido un error al verificar el usuario en el sistema";
                    _imgMensajeMod.ImageUrl = "../Imagenes/Error.png";
                    _imgMensajeMod.Visible = true;
                    _lblMensajeMod.Visible = true;
                }
                else if ((resultado.Count > 0) && (resultado.ElementAt(0).Count > 1)) // Existen choques con la reservación especificada
                {
                    String choques = "";
                    String[] fechaCompleta;
                    List<char> diasChoque = new List<char>();
                    for (int i = 0; i < resultado.Count; i++)
                    {
                        fechaCompleta = resultado.ElementAt(i).ElementAt(0).ToString().Split();
                        choques += "Fecha Inicio: " + fechaCompleta[0] + '\n';
                        fechaCompleta = resultado.ElementAt(i).ElementAt(1).ToString().Split();
                        choques += "Fecha Final: " + fechaCompleta[0] + '\n';
                        fechaCompleta = resultado.ElementAt(i).ElementAt(2).ToString().Split();
                        choques += "Hora Inicio: " + fechaCompleta[0] + '\n';
                        fechaCompleta = resultado.ElementAt(i).ElementAt(3).ToString().Split();
                        choques += "Hora Final: " + fechaCompleta[0] + '\n';
                        diasChoque = _controlador.obtenerDiasReservacion(Convert.ToInt32(resultado.ElementAt(i).ElementAt(5)));
                        choques += "Dias solicitados: ";
                        for (int j = 0; j < diasChoque.Count - 1; j++)
                            choques += diasChoque.ElementAt(j).ToString() + ", ";
                        choques += diasChoque.ElementAt(diasChoque.Count - 1).ToString() + '\n';
                        choques += "Solicitante: " + resultado.ElementAt(i).ElementAt(4).ToString() + '\n';
                        choques += "----------------------------";
                    }
                    _txtChoques.Text = choques;
                    _txtChoques.Visible = true;
                    _lblChoques.Visible = true;
                    _lblMensajeMod.Text = "Existen choques con la reservación que desea realizar";
                    _imgMensajeMod.ImageUrl = "../Imagenes/Advertencia.png";
                    _imgMensajeMod.Visible = true;
                    _lblMensajeMod.Visible = true;
                }
                else // Si hubo una excepción retorna el error
                {
                    switch (resultado.ElementAt(0).ElementAt(0).ToString())
                    {
                        case "Error 1":
                            _lblMensajeMod.Text = "Ha habido un error al buscar choques con la reservación que ha sido especificada";
                            break;
                        case "Error 2":
                            _lblMensajeMod.Text = "Ha habido un error al tratar de modificar la reservación";
                            break;
                        case "Error 3":
                            _lblMensajeMod.Text = "Ha habido un error al insertar uno o más días que se han asignado a la reservación";
                            break;
                        case "Error 4":
                            _lblMensajeMod.Text = "Ha habido un error al insertar los días de la reservación";
                            break;
                    }
                    _imgMensajeMod.ImageUrl = "../Imagenes/Error.png";
                    _imgMensajeMod.Visible = true;
                    _lblMensajeMod.Visible = true;
                }
            }
        }


        /// <summary>
        /// Método que controla el evento de agrega una excepcion a una reservación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _btnAgregarExcepcion_Click(object sender, EventArgs e)
        {
            _gridExcepciones.Visible = true;
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            bool reservar = true; // Estas verificaciones se pueden optimizar si se pasa las fechas y horas dadas por el usuario a un DateTime ******
            // Verificar que todos los campos del formulario han sido especificados por el usuario
            if (_txtFechaExcepcion.Text.Equals("")) 
            {
                _imgMensajeMod.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensajeMod.Text = "Debe especificar la fecha de la excepcion";
                _imgMensajeMod.Visible = true;
                _lblMensajeMod.Visible = true;
                reservar = false;
            } // La fechaExcepcion no sea menor a la de hoy
            else if (Convert.ToDateTime(_txtFechaExcepcion.Text) < DateTime.Today)
            {
                _imgMensajeMod.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensajeMod.Text = "La fecha de la excepcion no puede ser menor a la del día de hoy";
                _imgMensajeMod.Visible = true;
                _lblMensajeMod.Visible = true;
                reservar = false;
            } // La fechaExcepcion no sea menor a la fechaInicio ni mayor a la fecha final
            else if ((Convert.ToDateTime(_txtFechaInicio.Text) > Convert.ToDateTime(_txtFechaExcepcion.Text)) ||
                     (Convert.ToDateTime(_txtFechaFinal.Text) < Convert.ToDateTime(_txtFechaExcepcion.Text)))
            {
                    _imgMensajeMod.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensajeMod.Text = "La fecha excepcion no puede ser menor a la inicial ni mayor a la final";
                    _imgMensajeMod.Visible = true;
                    _lblMensajeMod.Visible = true;
                    reservar = false;
            }// La fechaExcepcion que se cancela no pertenece a los dias marcados
            else if ( ((((Convert.ToDateTime(_txtFechaExcepcion.Text)).DayOfWeek == DayOfWeek.Monday) && (_cbLunes.Checked ==false))
                          || (((Convert.ToDateTime(_txtFechaExcepcion.Text)).DayOfWeek == DayOfWeek.Tuesday) && (_cbMartes.Checked == false))
                          || (((Convert.ToDateTime(_txtFechaExcepcion.Text)).DayOfWeek == DayOfWeek.Wednesday) && (_cbMiercoles.Checked == false))
                          || (((Convert.ToDateTime(_txtFechaExcepcion.Text)).DayOfWeek == DayOfWeek.Thursday) && (_cbJueves.Checked == false))
                          || (((Convert.ToDateTime(_txtFechaExcepcion.Text)).DayOfWeek == DayOfWeek.Friday) && (_cbViernes.Checked == false))
                          || (((Convert.ToDateTime(_txtFechaExcepcion.Text)).DayOfWeek == DayOfWeek.Saturday) && (_cbSabado.Checked == false))
                          || (((Convert.ToDateTime(_txtFechaExcepcion.Text)).DayOfWeek == DayOfWeek.Sunday) && (_cbDomingo.Checked == false)))
                      && (_ddlExcepcion.SelectedValue=="Cancelar Dia")
                )
            {
                _imgMensajeMod.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensajeMod.Text = "La fecha de la exepcción no corresponde a ninguno de la reservación";
                _imgMensajeMod.Visible = true;
                _lblMensajeMod.Visible = true;
                reservar = false;
            }
            if (reservar) // Enviar al controlador de Reservación la información para que pueda ser creada
            {
                _tablaReportes.Columns.Add(new DataColumn("Motivo"));
                _tablaReportes.Columns.Add(new DataColumn("Fecha"));
                _tablaReportes.Rows.Add(_ddlExcepcion.SelectedValue, _txtFechaExcepcion.Text);

                _gridExcepciones.DataSource = _tablaReportes;
                _gridExcepciones.DataBind();
                _imgMensajeMod.ImageUrl = "../Imagenes/ok.png";
                _lblMensajeMod.Text = "Se agrego la Excepcion";
                _imgMensajeMod.Visible = true;
                _lblMensajeMod.Visible = true;
                reservar = false;
            }
        }

        #endregion
    }
}