using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using ModulosTICapaGUI.Compartido;
using ModulosTICapaLogica.Compartido;

namespace ModulosTICapaGUI.ModuloReservacion
{
	public partial class CrearReservacion : System.Web.UI.Page
	{
		#region Atributos

		private Sesion _sesion;
		private HttpCookie _cookieActual;
		private ControladorReservacionCR _controlador; // Controlador de la Interfaz
		private static List<List<object>> _lugares; // Lista de lugares que será cargada en el _ddlLugar
		private static List<List<object>> _carreras; // Lista de carreras que será cargada en el _ddlCarrera
		private static List<List<object>> _cursos; // Lista de cursos que será cargada en el _ddlCurso
		private int _errorCarga; // Para manejo de errores en el tiempo de carga de la página
										// 0: No se cargaron lugares
										// 1: No se cargaron los cursos
										// 2: No se cargaron tanto lugares como cursos
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
						_imgMensaje.ImageUrl = "../Imagenes/Error.png";
						_lblMensaje.Text = "Se ha producido un error al obtener los lugares.";
						_imgMensaje.Visible = true;
						_lblMensaje.Visible = true;
						break;
					case 1:
						_ddlCarrera.Items.Clear();
						_ddlCarrera.Items.Add("No hay carreras disponibles");
						_imgMensaje.ImageUrl = "../Imagenes/Error.png";
						_lblMensaje.Text = "Se ha producido un error al obtener las carreras.";
						_imgMensaje.Visible = true;
						_lblMensaje.Visible = true;
						break;
					case 2:
						_ddlCarrera.Items.Clear();
						_ddlCarrera.Items.Add("No hay carreras disponibles");
						_ddlLugar.Items.Clear();
						_ddlLugar.Items.Add("No hay lugares disponibles");
						_imgMensaje.ImageUrl = "../Imagenes/Error.png";
						_lblMensaje.Text = "Ha habido un problema al cargar información en la página.";
						_imgMensaje.Visible = true;
						_lblMensaje.Visible = true;
						break;
				}
			}
		}

		/// <summary>
		/// Método que controla el evento de crear una reservación
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		protected void _btnReservar_Click(object sender, EventArgs e)
		{
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
			bool reservar = true; // Estas verificaciones se pueden optimizar si se pasa las fechas y horas dadas por el usuario a un DateTime ******
			// Verificar que todos los campos del formulario han sido especificados por el usuario
            if ((_txtFechaInicio.Text.Equals("")) || (_txtFechaFinal.Text.Equals("")) || (_txtLogin.Text.Equals("")) || (_ddlCarrera.SelectedIndex == 0)
                || (_ddlCurso.SelectedIndex == 0) || (_ddlHoraFinal.SelectedIndex == 0) || (_ddlHoraInicio.SelectedIndex == 0)
				|| (_ddlLugar.SelectedIndex == 0) || (_ddlMinutoFinal.SelectedIndex == 0) || (_ddlMinutoInicio.SelectedIndex == 0))
			{
				_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
				_lblMensaje.Text = "Debe especificar todos los campos del formulario";
				_imgMensaje.Visible = true;
				_lblMensaje.Visible = true;
				reservar = false;
			} // Al menos un día haya sido especificado
			else if ((!_cbLunes.Checked) && (!_cbMartes.Checked) && (!_cbMiercoles.Checked) && (!_cbJueves.Checked) && (!_cbViernes.Checked)
					&& (!_cbSabado.Checked) && (!_cbDomingo.Checked))
			{
				_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
				_lblMensaje.Text = "Debe seleccionar al menos un día para la reservación";
				_imgMensaje.Visible = true;
				_lblMensaje.Visible = true;
				reservar = false;
			} // La fechaInicio no sea menor a la de hoy
			else if (Convert.ToDateTime(_txtFechaInicio.Text) < DateTime.Today)
			{
				_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
				_lblMensaje.Text = "La fecha de inicio no puede ser menor a la del día de hoy";
				_imgMensaje.Visible = true;
				_lblMensaje.Visible = true;
				reservar = false;
			} // La fechaFinal no sea menor a la fechaInicio
			else if ((Convert.ToDateTime(_txtFechaInicio.Text) > Convert.ToDateTime(_txtFechaFinal.Text)))
			{
				_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
				_lblMensaje.Text = "La fecha final no puede ser menor a la inicial";
				_imgMensaje.Visible = true;
				_lblMensaje.Visible = true;
				reservar = false;
			} // La horaFinal no sea menor o igual a la horaInicio
			else if ((Convert.ToInt32(_ddlHoraInicio.Text) > Convert.ToInt32(_ddlHoraFinal.Text))
						|| ((Convert.ToInt32(_ddlMinutoInicio.Text) >= Convert.ToInt32(_ddlMinutoFinal.Text)) &&
							(Convert.ToInt32(_ddlHoraInicio.Text) == Convert.ToInt32(_ddlHoraFinal.Text))))
			{
				_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
				_lblMensaje.Text = "La hora de inicio no puede ser mayor o igual a la hora final";
				_imgMensaje.Visible = true;
				_lblMensaje.Visible = true;
				reservar = false;
			} // La horaInicio no sea menor a la actual en caso de elegir el día de hoy
			else if ((Convert.ToDateTime(_txtFechaInicio.Text) == Convert.ToDateTime(_txtFechaFinal.Text)) &&
						((Convert.ToInt32(_ddlHoraInicio.Text) < DateTime.Now.Hour) || ((Convert.ToInt32(_ddlMinutoInicio.Text) < DateTime.Now.Minute)
						&& (Convert.ToInt32(_ddlHoraInicio.Text) == DateTime.Now.Hour))))
			{
				_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
				_lblMensaje.Text = "La hora de inicio no puede ser menor a la actual";
				_imgMensaje.Visible = true;
				_lblMensaje.Visible = true;
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
					_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
					_lblMensaje.Text = "Uno o más días que fueron marcados no se encuentran dentro del rango de fechas especificado";
					_imgMensaje.Visible = true;
					_lblMensaje.Visible = true;
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
				List<List<object>> resultado = _controlador.crearReservacion(datosGenerales, dias, _sesion.obtenerLoginUsuario(_cookieActual), _cbHorario.Checked);
				if (resultado == null) // Si tuvo éxito
				{
					_imgMensaje.ImageUrl = "../Imagenes/ok.png";
					_lblMensaje.Text = "La reservación se ha realizado con éxito";
					_imgMensaje.Visible = true;
					_lblMensaje.Visible = true;
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
				}
				else if ((resultado.Count > 0) && (resultado.ElementAt(0).ElementAt(0).Equals("Error LDAP"))) // Si hay error con el usuario en el LDAP
				{
					_lblMensaje.Text = "Ha habido un error al verificar el usuario en el sistema";
					_imgMensaje.ImageUrl = "../Imagenes/Error.png";
					_imgMensaje.Visible = true;
					_lblMensaje.Visible = true;
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
						choques += diasChoque.ElementAt(diasChoque.Count-1).ToString() + '\n';
						choques += "Solicitante: " + resultado.ElementAt(i).ElementAt(4).ToString() + '\n';
						choques += "----------------------------";
					}
					_txtChoques.Text = choques;
					_txtChoques.Visible = true;
					_lblChoques.Visible = true;
					_lblMensaje.Text = "Existen choques con la reservación que desea realizar";
					_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
					_imgMensaje.Visible = true;
					_lblMensaje.Visible = true;
				}
				else // Si hubo una excepción retorna el error
				{
					switch (resultado.ElementAt(0).ElementAt(0).ToString())
					{
						case "Error 1":
							_lblMensaje.Text = "Ha habido un error al buscar choques con la reservación que ha sido especificada";
							break;
						case "Error 2":
							_lblMensaje.Text = "Ha habido un error al tratar de insertar la reservación al sistema";
							break;
						case "Error 3":
							_lblMensaje.Text = "Ha habido un error al insertar uno o más días que se han asignado a la reservación";
							break;
						case "Error 4":
							_lblMensaje.Text = "Ha habido un error al insertar los días de la reservación";
							break;
					}
					_imgMensaje.ImageUrl = "../Imagenes/Error.png";
					_imgMensaje.Visible = true;
					_lblMensaje.Visible = true;
				}
			}
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
				_lblCantidad.Text = _lugares.ElementAt(_ddlLugar.SelectedIndex-1).ElementAt(2).ToString();
				_txtDescripcion.Text = _lugares.ElementAt(_ddlLugar.SelectedIndex-1).ElementAt(3).ToString();				
				_lblNombreEncargado.Text = _lugares.ElementAt(_ddlLugar.SelectedIndex-1).ElementAt(4).ToString();
				_lblNombreTipo.Text = _lugares.ElementAt(_ddlLugar.SelectedIndex - 1).ElementAt(5).ToString();
				_upEncargado.Update();
				_upDescripcion.Update();
				_upPanelCantidad.Update();
				_upTipo.Update();
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
					_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
					_lblMensaje.Text = "Se ha producido un error al obtener los cursos.";
					_imgMensaje.Visible = true;
					_lblMensaje.Visible = true;
				}
			}
			else
				_ddlCurso.Enabled = false;
			_upCurso.Update();
		}

		#endregion
	}
}