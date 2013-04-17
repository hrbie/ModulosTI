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

namespace ModulosTICapaGUI.ModuloHorario
{
    public partial class CrearHorario : System.Web.UI.Page
	{
		#region Atributos

		private ControladorHorario _controladorHorario; // Controlador para el manejo del horario
		private static List<List<String>> _listaLugares; // Lista de lugares que se cargarán
		private static List<List<String>> _listaSemestres; // Lista de semestres que se cargarán
		private static List<List<Object>> _listaTurnos; // Lista de los turnos del horario
		private DataTable _tablaTurnos; // Tabla de turnos
		private Boolean _realizoCambio; // Valor booleano para indicar si los cambios fueron exitosos
		private Sesion _sesion; // Para manejar la cookie de la página
		private HttpCookie _cookieActual; // Variable donde se guarda la cookie

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
				_controladorHorario = new ControladorHorario();
				String grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
				if ((grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")) ||
						(grupoUsuario.Equals("operadores"))) // Reportar si un usuario autenticado intenta hacer ingreso a una página que no tiene permiso
				{
					Notificacion notificacion = new Notificacion(); // Enviar correo informando de la intrusión
					notificacion.enviarCorreo("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(_cookieActual) + " a la página de CrearHorario.aspx", "soporte@ic-itcr.ac.cr", "Violación de Seguridad");
					Response.Redirect("../Compartido/AccesoDenegado.aspx");
				}
				ViewState["filaEditada"] = -1;
				ViewState["columnaEditada"] = -1;
				ViewState["idHorario"] = -1;
				_tablaTurnos = new DataTable();
				_listaLugares = _controladorHorario.consultarLugares(); // Obtener los lugares
                if (_listaLugares != null)
                {
                    if (_listaLugares.Count == 0)
                    {
                        _lblMensaje.Text = "No hay lugares registrados en el sistema";
						_imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                        _lblMensaje.Visible = true;
                        _imgMensaje.Visible = true;
                    }
                    else
                    {
                        _ddlLugar.Items.Add("Seleccionar");
                        for (int i = 0; i < _listaLugares.Count; i++) // Cargar los lugares
                            _ddlLugar.Items.Add(_listaLugares.ElementAt(i).ElementAt(1));
                    }
                }
                else
                {
                    _lblMensaje.Text = "Se ha presentado un error al cargar la información de los lugares";
					_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Visible = true;
                    _imgMensaje.Visible = true;
                }
				_listaSemestres = _controladorHorario.consultarSemestres(); // Obtener los semestres
                if (_listaSemestres != null)
                {
                    if (_listaSemestres.Count == 0)
                    {
                        _lblMensaje.Text = "No hay semestres registrados en el sistema";
                        _lblMensaje.Visible = true;
                        _imgMensaje.Visible = true;
                    }
                    else
                    {
                        _ddlSemestre.Items.Add("Seleccionar");
                        for (int i = 0; i < _listaSemestres.Count; i++) // Cargar los semestres
                            _ddlSemestre.Items.Add(_listaSemestres.ElementAt(i).ElementAt(1));
                    }
                }
                else
                {
                    _lblMensaje.Text = "Se ha presentado un error al cargar la información de los semestres";
					_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Visible = true;
                    _imgMensaje.Visible = true;
                }
				int horarioHabilitado = _controladorHorario.consultarHorarioActivo();
				if (horarioHabilitado != -1)
				{
					if (horarioHabilitado == 0) // No hay horarios habilitados
					{
						_btnHorarioDisponibilidad.Enabled = true;
						_btnDeshabilitarHorarioDisponibilidad.Enabled = false;
					}
					else // Hay horarios habilitados
					{
						_btnHorarioDisponibilidad.Enabled = false;
						_btnDeshabilitarHorarioDisponibilidad.Enabled = true;
					}
				}
				else // Si hubo un error al encontrar un horario de disponibilidad habilitado
				{
					_lblMensaje.Text = "Error al determinar si hay horarios de disponibilidad habilitados";
					_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
					_lblMensaje.Visible = true;
					_imgMensaje.Visible = true;
				}
			}
        }

		protected void _gridHorario_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			Control controlDesplegable; // Label que se encuentra en cada celda del grid, se utiliza para lo que se va a desplegar en la celda
			Control controlEditable;    // Textbox que se enceuntra en cada celda del grid, se utiliza para poder editar la celda            
			_btnGuardarHorario.Enabled = true;
			_controladorHorario = new ControladorHorario();
			if ((int)ViewState["filaEditada"] > -1) // Verificar si hay alguna celda editada para guardar lo que hay en esta
			{
				String textoTurnoViejo =  ((Label)_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[1]).Text;
				String textoTurnoNuevo = ((TextBox)_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[3]).Text;
				int idTurno = -1;
				if ((textoTurnoViejo.Equals("")) && (!textoTurnoNuevo.Equals(""))) // Un turno nuevo
				{
					char dia = ' ';
					switch ((int)ViewState["columnaEditada"])
					{
						case 2:   // Lunes
							dia = 'L';
							break;
						case 3:   // Martes
							dia = 'K';
							break;
						case 4:   // Miércoles
							dia = 'M';
							break;
						case 5:   // Jueves
							dia = 'J';
							break;
						case 6:   // Viernes
							dia = 'V';
							break;
						case 7:   // Sábado
							dia = 'S';
							break;
						case 8:   // Domingo
							dia = 'D';
							break;
					}
					String hora = ((Label)_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[1].Controls[1]).Text;
					String[] horas = hora.Split(' ');
					_realizoCambio = _controladorHorario.insertarTurno(dia, horas[0], horas[2], textoTurnoNuevo, (int)ViewState["idHorario"]);
					if (_realizoCambio)
					{
						_lblMensaje.Text = "Se ha insertado con éxito el turno";
						_imgMensaje.ImageUrl = "~/Imagenes/ok.png";
						_lblMensaje.Visible = true;
						_imgMensaje.Visible = true;
					}
					else
					{
						_lblMensaje.Text = "Error al crear el nuevo turno";
						_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
						_lblMensaje.Visible = true;
						_imgMensaje.Visible = true;
					}
				}
				else if ((!textoTurnoNuevo.Equals(textoTurnoViejo)) && (!textoTurnoNuevo.Equals(""))) // Si cambio el dato del turno
				{
					String hola = ((Label)_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[5]).Text;
					idTurno = Convert.ToInt32(hola);
					_realizoCambio = _controladorHorario.modificarTurno(idTurno, textoTurnoNuevo);
					if (_realizoCambio)
					{
						_lblMensaje.Text = "Se ha cambiado el turno con éxito";
						_imgMensaje.ImageUrl = "~/Imagenes/ok.png";
						_lblMensaje.Visible = true;
						_imgMensaje.Visible = true;
					}
					else
					{
						_lblMensaje.Text = "Error al modificar el turno";
						_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
						_lblMensaje.Visible = true;
						_imgMensaje.Visible = true;
					}
				}
				else if ((!textoTurnoViejo.Equals("")) && (textoTurnoNuevo.Equals(""))) // Si elminó el dato del turno
				{
					idTurno = Convert.ToInt32(((Label)_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[5]).Text);
					_realizoCambio = _controladorHorario.borrarTurno(idTurno);
					if (_realizoCambio)
					{
						_lblMensaje.Text = "Se ha borrado el turno con éxito";
						_imgMensaje.ImageUrl = "~/Imagenes/ok.png";
						_lblMensaje.Visible = true;
						_imgMensaje.Visible = true;
					}
					else
					{
						_lblMensaje.Text = "Error al intentar borrar el turno";
						_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
						_lblMensaje.Visible = true;
						_imgMensaje.Visible = true;
					}
				}
				((Label)_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[1]).Text = textoTurnoNuevo;
				_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[1].Visible = true;
				_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[3].Visible = false;
			}
			// Obtener la fila y columna de la celda
			int fila = int.Parse(e.CommandArgument.ToString());
			int columna = int.Parse(Request.Form["__EVENTARGUMENT"]);

			// Obtener el label de la celda (control desplegable) y ocultarlo
			controlDesplegable = _gridHorario.Rows[fila].Cells[columna].Controls[1];
			controlDesplegable.Visible = false;
			// Obtener el textbox de la celda (control editable) y hacelo visible, para que se pueda utilizar
			controlEditable = _gridHorario.Rows[fila].Cells[columna].Controls[3];
			((TextBox)controlEditable).Text = ((Label)controlDesplegable).Text.Replace("<br/>", " ");
			controlEditable.Visible = true;
			// Setear el foco al control editable
			ClientScript.RegisterStartupScript(GetType(), "SetFocus", "<script>document.getElementById('" + controlEditable.ClientID + "').focus();</script>");
			// Seleccionar el texto del control desplegable, esto para que este en foco
			if (controlEditable is TextBox)
			{
				((TextBox)controlEditable).Attributes.Add("onfocus", "this.select()");
			}
			ViewState["filaEditada"] = fila; // Guardas la fila de la celda que se editó
			ViewState["columnaEditada"] = columna; // Guardar la columna de la celda que se editó
		}

		protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				// Obtener el contro de LinkButton en la primera celda
				LinkButton _singleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
				// Obtener el javascript con el que es asignado éste LinkButton
				string _jsSingle = ClientScript.GetPostBackClientHyperlink(_singleClickButton, "");

				// Agrega los eventos a cada celda editable
				for (int columnIndex = 2; columnIndex < e.Row.Cells.Count; columnIndex++)
				{
					// Añade el índice de la columna como parámetro para el evento de javascript
					string js = _jsSingle.Insert(_jsSingle.Length - 2, columnIndex.ToString());
					// Agrega éste javascript al atributo de onclick de la celda
					e.Row.Cells[columnIndex].Attributes["onclick"] = js;
					// Agrega un estilo al cursor de la celda
					e.Row.Cells[columnIndex].Attributes["style"] += "cursor:pointer;cursor:hand;";
				}
			}
		}

		// Se utiliza para que se habilite lo que se hizo en el metodo RowDataBound

		protected override void Render(HtmlTextWriter writer)
		{
			foreach (GridViewRow r in _gridHorario.Rows)
			{
				if (r.RowType == DataControlRowType.DataRow)
				{
					int primeraCeldaEditar = 2; //desde cual celda empiesan las celdas que son editables
					for (int columnIndex = primeraCeldaEditar; columnIndex < r.Cells.Count; columnIndex++)
					{
						Page.ClientScript.RegisterForEventValidation(r.UniqueID + "$ctl00", columnIndex.ToString());
					}
				}
			}
			base.Render(writer);
		}

        /// <summary>
        /// Método para manejar el evento del botón _btnHorarioDisponibilidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _btnHorarioDisponibilidad_Click(object sender, EventArgs e)
        {
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx"); // 
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
			_controladorHorario = new ControladorHorario();
            Boolean resultado = _controladorHorario.crearHorarioDisponibilidad(); // Enviar al controlador la petición
            if (resultado)
            {
                _lblMensaje.Text = "Se ha creado con éxito el horario";
				_imgMensaje.ImageUrl = "~/Imagenes/ok.png";
                _lblMensaje.Visible = true;
                _imgMensaje.Visible = true;
				_btnHorarioDisponibilidad.Enabled = false;
				_btnDeshabilitarHorarioDisponibilidad.Enabled = true;
            }
            else
            {
                _lblMensaje.Text = "Se ha presentado un error al crear el horario";
                _lblMensaje.Visible = true;
                _imgMensaje.Visible = true;
            }
        }

        /// <summary>
        /// Método que se encarga de manejar el evento del botón _btnDeshabilitarHorarioDisponibilidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _btnDeshabilitarHorarioDisponibilidad_Click(object sender, EventArgs e)
		{
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx"); // 
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
			_controladorHorario = new ControladorHorario();
            Boolean resultado = _controladorHorario.deshabilitarHorarioDisponibilidad(); // Enviar al controlador la petición
            if (resultado)
            {
                _lblMensaje.Text = "Se ha deshabilitado con éxito el horario";
				_imgMensaje.ImageUrl = "~/Imagenes/ok.png";
                _lblMensaje.Visible = true;
                _imgMensaje.Visible = true;
				_btnHorarioDisponibilidad.Enabled = true;
				_btnDeshabilitarHorarioDisponibilidad.Enabled = false;
            }
            else
            {
                _lblMensaje.Text = "Se ha presentado un error al deshabilitar el horario";
				_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                _lblMensaje.Visible = true;
                _imgMensaje.Visible = true;
            }
        }

        /// <summary>
        /// Método que se encarga de manejar el evento del botón _btnCrearHorario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _btnCrearHorario_Click(object sender, EventArgs e)
        {
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx"); // 
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
            if ((_ddlSemestre.SelectedIndex != 0) && (_ddlLugar.SelectedIndex != 0))
            {
				_controladorHorario = new ControladorHorario();
                ViewState["idHorario"] = _controladorHorario.crearHorario(Convert.ToInt32(_listaLugares[_ddlLugar.SelectedIndex-1][0]), Convert.ToInt32(_listaSemestres[_ddlSemestre.SelectedIndex-1][0]));
                if ((int)ViewState["idHorario"] != -1)
                {
                    _lblMensaje.Text = "Se ha creado el horario con éxito";
					_imgMensaje.ImageUrl = "~/Imagenes/ok.png";
                    _lblMensaje.Visible = true;
                    _imgMensaje.Visible = true;
					_btnCrearHorario.Enabled = false;
					llenarTabla(0);
					_gridHorario.Visible = true;
                }
                else
                {
                    _lblMensaje.Text = "Ha habido un error al crear el horario";
					_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Visible = true;
                    _imgMensaje.Visible = true;
                }
            }
        }

		/// <summary>
		/// Método que se encarga de cargar el horario de acuerdo a un lugar escogido
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		protected void _ddlLugar_SelectedIndexChanged(object sender, EventArgs e)
		{
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx"); // 
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
			ViewState["filaEditada"] = -1;
			ViewState["columnaEditada"] = -1;
			_btnCrearHorario.Enabled = true;
			_controladorHorario = new ControladorHorario();
			if (_ddlLugar.SelectedIndex != 0)
			{
				ViewState["idHorario"] = _controladorHorario.consultarHorario(Convert.ToInt32(_listaLugares[_ddlLugar.SelectedIndex - 1][0]), Convert.ToInt32(_listaSemestres[_ddlSemestre.SelectedIndex - 1][0]));
				if ((int)ViewState["idHorario"] != -1)
				{
					if ((int)ViewState["idHorario"] != 0)
					{
						_listaTurnos = _controladorHorario.consultarTurnosHorario(Convert.ToInt32(_listaLugares[_ddlLugar.SelectedIndex - 1][0]), Convert.ToInt32(_listaSemestres[_ddlSemestre.SelectedIndex - 1][0]));
						if (_listaTurnos == null)
						{
							_lblMensaje.Text = "Ha habido un error al obtener los turnos del horario";
							_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
							_lblMensaje.Visible = true;
							_imgMensaje.Visible = true;
						}
						else if (_listaTurnos.Count != 0) // Hay turno
							llenarTabla(1); // Llenar las tablas
						else // No hay turnos
							llenarTabla(0); // Llenar las tablas
						_btnCrearHorario.Enabled = false;
						_gridHorario.Visible = true;
						_lblMensaje.Visible = false;
						_imgMensaje.Visible = false;
					}
					else
					{
						_btnCrearHorario.Enabled = true;
						_gridHorario.Visible = false;
					}
				}
				else if ((int)ViewState["idHorario"] == -1)
				{
					_lblMensaje.Text = "Ha habido un error al obtener el horario";
					_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
					_lblMensaje.Visible = true;
					_imgMensaje.Visible = true;
				}
			}
			else
				_btnCrearHorario.Enabled = false;
		}

		/// <summary>
		/// Método que se encarga de llenar el gridview del horario
		/// </summary>
		/// <param name="tipo">Indica si al horario hay que cargarle los turnos o no. 0:NO 1:SI</param>

		public void llenarTabla(int tipo)
		{
			_tablaTurnos = new DataTable();
			String nombreTipoLugar = _listaLugares[_ddlLugar.SelectedIndex - 1][2]; // Obtener el nombre del tipo lugar
			// Crear las columnas de la tabla
			_tablaTurnos.Columns.Add(new DataColumn("Turno"));
			_tablaTurnos.Columns.Add(new DataColumn("Lunes"));
			_tablaTurnos.Columns.Add(new DataColumn("Martes"));
			_tablaTurnos.Columns.Add(new DataColumn("Miércoles"));
			_tablaTurnos.Columns.Add(new DataColumn("Jueves"));
			_tablaTurnos.Columns.Add(new DataColumn("Viernes"));
			_tablaTurnos.Columns.Add(new DataColumn("Sábado"));
			_tablaTurnos.Columns.Add(new DataColumn("Domingo"));
			if (nombreTipoLugar.Equals("Oficina"))
			{
				// Llenar la tabla, solo con la columna de turno con valor
				_tablaTurnos.Rows.Add("7:30 - 8:30", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("8:30 - 9:30", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("9:30 - 10:30", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("10:30 - 11:30", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("11:30 - 12:00", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("13:00 - 14:00", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("14:00 - 15:00", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("15:00 - 16:00", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("16:00 - 17:00", "", "", "", "", "", "");
				_gridHorario.DataSource = _tablaTurnos;
				_gridHorario.DataBind();
				if (tipo == 1)
					llenarTurnos(0);
			}
			else if (nombreTipoLugar.Equals("Aula"))
			{
				// Llenar la tabla, solo con la columna de turno con valor
				_tablaTurnos.Rows.Add("7:30 - 9:20", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("9:30 - 11:20", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("11:30 - 12:20", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("13:00 - 14:50", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("15:00 - 16:50", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("17:00 - 18:50", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("19:00 - 21:00", "", "", "", "", "", "");
				_gridHorario.DataSource = _tablaTurnos;
				_gridHorario.DataBind();
				if (tipo == 1)
					llenarTurnos(1);
			}
			else if (nombreTipoLugar.Equals("Laboratorio"))
			{
				// Llenar la tabla, solo con la columna de turno con valor
				_tablaTurnos.Rows.Add("7:30 - 11:30", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("11:30 - 15:30", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("15:30 - 19:30", "", "", "", "", "", "");
				_tablaTurnos.Rows.Add("19:30 - 22:30", "", "", "", "", "", "");
				_gridHorario.DataSource = _tablaTurnos;
				_gridHorario.DataBind();
				if (tipo == 1)
					llenarTurnos(2);
			}
		}

		/// <summary>
		/// Método que se encarga de llenar los turnos del horario
		/// </summary>
		/// <param name="tipo">Indica el tipo de horario (de acuerdo al lugar) que se va a llenar</param>

		public void llenarTurnos(int tipo)
		{
			int columna = -1;
			if (tipo == 0) // Llenar horario para la Oficina de TI
			{
				for (int i = 0; i < _listaTurnos.Count; i++)
				{
					switch (Convert.ToChar(_listaTurnos[i][1]))
					{
						case 'L':   // Lunes
							columna = 2;
							break;
						case 'K':   // Martes
							columna = 3;
							break;
						case 'M':   // Miércoles
							columna = 4;
							break;
						case 'J':   // Jueves
							columna = 5;
							break;
						case 'V':   // Viernes
							columna = 6;
							break;
						case 'S':   // Sábado
							columna = 7;
							break;
						case 'D':   // Domingo
							columna = 8;
							break;
					}
					switch (String.Format("{0:HH:mm:ss}", _listaTurnos[i][2]))
					{
						case "07:30:00":
							((Label)_gridHorario.Rows[0].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[0].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[0].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "08:30:00":
							((Label)_gridHorario.Rows[1].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[1].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[1].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "09:30:00":
							((Label)_gridHorario.Rows[2].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[2].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[2].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "10:30:00":
							((Label)_gridHorario.Rows[3].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[3].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[3].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "11:30:00":
							((Label)_gridHorario.Rows[4].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[4].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[4].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "13:00:00":
							((Label)_gridHorario.Rows[5].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[5].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[5].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "14:00:00":
							((Label)_gridHorario.Rows[6].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[6].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[6].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "15:00:00":
							((Label)_gridHorario.Rows[7].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[7].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[7].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "16:00:00":
							((Label)_gridHorario.Rows[8].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[8].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[8].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
					}
				}
			} // Fin llenar para Oficina de TI
			else if (tipo == 1) // Llenar horario para un aula
			{
				for (int i = 0; i < _listaTurnos.Count; i++)
				{
					switch (Convert.ToChar(_listaTurnos[i][1]))
					{
						case 'L':   // Lunes
							columna = 2;
							break;
						case 'K':   // Martes
							columna = 3;
							break;
						case 'M':   // Miércoles
							columna = 4;
							break;
						case 'J':   // Jueves
							columna = 5;
							break;
						case 'V':   // Viernes
							columna = 6;
							break;
						case 'S':   // Sábado
							columna = 7;
							break;
						case 'D':   // Domingo
							columna = 8;
							break;
					}
					switch (String.Format("{0:HH:mm:ss}", _listaTurnos[i][2]))
					{
						case "07:30:00":
							((Label)_gridHorario.Rows[0].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[0].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[0].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "09:30:00":
							((Label)_gridHorario.Rows[1].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[1].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[1].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "11:30:00":
							((Label)_gridHorario.Rows[2].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[2].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[2].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "13:00:00":
							((Label)_gridHorario.Rows[3].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[3].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[3].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "15:00:00":
							((Label)_gridHorario.Rows[4].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[4].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[4].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "17:00:00":
							((Label)_gridHorario.Rows[5].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[5].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[5].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "19:00:00":
							((Label)_gridHorario.Rows[6].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[6].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[6].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
					}
				}
			} // Fin de llenar aula
			else // Llenar horario para un laboratorio
			{
				for (int i = 0; i < _listaTurnos.Count; i++)
				{
					switch (Convert.ToChar(_listaTurnos[i][1]))
					{
						case 'L':   // Lunes
							columna = 2;
							break;
						case 'K':   // Martes
							columna = 3;
							break;
						case 'M':   // Miércoles
							columna = 4;
							break;
						case 'J':   // Jueves
							columna = 5;
							break;
						case 'V':   // Viernes
							columna = 6;
							break;
						case 'S':   // Sábado
							columna = 7;
							break;
						case 'D':   // Domingo
							columna = 8;
							break;
					}
					switch (String.Format("{0:HH:mm:ss}", _listaTurnos[i][2]))
					{
						case "07:30:00":
							((Label)_gridHorario.Rows[0].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[0].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[0].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "11:30:00":
							((Label)_gridHorario.Rows[1].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[1].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[1].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "15:30:00":
							((Label)_gridHorario.Rows[2].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[2].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[2].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
						case "19:30:00":
							((Label)_gridHorario.Rows[3].Cells[columna].Controls[1]).Text = ((Label)_gridHorario.Rows[3].Cells[columna].Controls[1]).Text + _listaTurnos[i][4].ToString();
							((Label)_gridHorario.Rows[3].Cells[columna].Controls[5]).Text = _listaTurnos[i][0].ToString();
							break;
					}
				}
			}
		}

		protected void _btnGuardarHorario_Click(object sender, EventArgs e)
		{
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx"); // 
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
			_controladorHorario = new ControladorHorario();
			String textoTurnoViejo = ((Label)_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[1]).Text;
			String textoTurnoNuevo = ((TextBox)_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[3]).Text;
			int idTurno = -1;
			if ((textoTurnoViejo.Equals("")) && (!textoTurnoNuevo.Equals(""))) // Un turno nuevo
			{
				char dia = ' ';
				switch ((int)ViewState["columnaEditada"])
				{
					case 2:   // Lunes
						dia = 'L';
						break;
					case 3:   // Martes
						dia = 'K';
						break;
					case 4:   // Miércoles
						dia = 'M';
						break;
					case 5:   // Jueves
						dia = 'J';
						break;
					case 6:   // Viernes
						dia = 'V';
						break;
					case 7:   // Sábado
						dia = 'S';
						break;
					case 8:   // Domingo
						dia = 'D';
						break;
				}
				String hora = ((Label)_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[1].Controls[1]).Text;
				String[] horas = hora.Split(' ');
				_realizoCambio = _controladorHorario.insertarTurno(dia, horas[0], horas[2], textoTurnoNuevo, (int)ViewState["idHorario"]);
				if (_realizoCambio)
				{
					_lblMensaje.Text = "Se ha insertado con éxito el turno";
					_imgMensaje.ImageUrl = "~/Imagenes/ok.png";
					_lblMensaje.Visible = true;
					_imgMensaje.Visible = true;
				}
				else
				{
					_lblMensaje.Text = "Error al crear el nuevo turno";
					_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
					_lblMensaje.Visible = true;
					_imgMensaje.Visible = true;
				}
			}
			else if ((!textoTurnoNuevo.Equals(textoTurnoViejo)) && (!textoTurnoNuevo.Equals(""))) // Si cambio el dato del turno
			{
				idTurno = Convert.ToInt32(((Label)_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[5]).Text);
				_realizoCambio = _controladorHorario.modificarTurno(idTurno, textoTurnoNuevo);
				if (_realizoCambio)
				{
					_lblMensaje.Text = "Se ha cambiado el turno con éxito";
					_imgMensaje.ImageUrl = "~/Imagenes/ok.png";
					_lblMensaje.Visible = true;
					_imgMensaje.Visible = true;
				}
				else
				{
					_lblMensaje.Text = "Error al modificar el turno";
					_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
					_lblMensaje.Visible = true;
					_imgMensaje.Visible = true;
				}
			}
			else if ((!textoTurnoViejo.Equals("")) && (textoTurnoNuevo.Equals(""))) // Si elminó el dato del turno
			{
				idTurno = Convert.ToInt32(((Label)_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[5]).Text);
				_realizoCambio = _controladorHorario.borrarTurno(idTurno);
				if (_realizoCambio)
				{
					_lblMensaje.Text = "Se ha borrado el turno con éxito";
					_imgMensaje.ImageUrl = "~/Imagenes/ok.png";
					_lblMensaje.Visible = true;
					_imgMensaje.Visible = true;
				}
				else
				{
					_lblMensaje.Text = "Error al intentar borrar el turno";
					_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
					_lblMensaje.Visible = true;
					_imgMensaje.Visible = true;
				}
			}
			((Label)_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[1]).Text = textoTurnoNuevo;
			_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[1].Visible = true;
			_gridHorario.Rows[(int)ViewState["filaEditada"]].Cells[(int)ViewState["columnaEditada"]].Controls[3].Visible = false;
			_btnGuardarHorario.Enabled = false;
		}

		protected void _ddlSemestre_SelectedIndexChanged(object sender, EventArgs e)
		{
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx"); // 
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
			if (_ddlSemestre.SelectedIndex != 0)
				_ddlLugar.Enabled = true;
			else
				_ddlLugar.Enabled = false;			
			_btnCrearHorario.Enabled = false;
			_gridHorario.Visible = false;
		}

        protected void _btnExportar_Click(object sender, EventArgs e)
        {
            _controladorHorario = new ControladorHorario();
            int idHorario = _controladorHorario.consultarHorarioDisDeshabilitado();
            if (idHorario != -1)
            {
                DataTable tabla = new DataTable();
                // Crear las columnas de la tabla
                tabla.Columns.Add(new DataColumn("Turno"));
                tabla.Columns.Add(new DataColumn("Lunes"));
                tabla.Columns.Add(new DataColumn("Martes"));
                tabla.Columns.Add(new DataColumn("Miércoles"));
                tabla.Columns.Add(new DataColumn("Jueves"));
                tabla.Columns.Add(new DataColumn("Viernes"));
                tabla.Columns.Add(new DataColumn("Sábado"));
				tabla.Columns.Add(new DataColumn("Domingo"));
                // Llenar la tabla, solo con la columna de turno con valor
                tabla.Rows.Add("7:30 a.m - 11:30 a.m", "", "", "", "", "", "");
                tabla.Rows.Add("11:30 a.m - 3:30 p.m", "", "", "", "", "", "");
                tabla.Rows.Add("3:30 p.m - 7:30 p.m", "", "", "", "", "", "");
                tabla.Rows.Add("7:30 p.m - 10:30 p.m", "", "", "", "", "", "");
                List<List<object>> turnos = _controladorHorario.obtenerTurnosDisponibilidad(idHorario);
                if (turnos.Count > 0)
                    ordenarTurnos(tabla, turnos);
                DataTable dtTemp = new DataTable(); // Crear una tabla temporal
                // Creando los encabezados de las filas
                dtTemp.Columns.Add("<b>Turno</b>");
                dtTemp.Columns.Add("<b>Lunes</b>");
                dtTemp.Columns.Add("<b>Martes</b>");
                dtTemp.Columns.Add("<b>Miercoles</b>");
                dtTemp.Columns.Add("<b>Jueves</b>");
                dtTemp.Columns.Add("<b>Viernes</b>");
                dtTemp.Columns.Add("<b>Sabado</b>");
                dtTemp.Columns.Add("<b>Domingo</b>");
                DataRow fila;
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    fila = dtTemp.NewRow();
                    fila[0] = tabla.Rows[i][0].ToString(); // Turno
                    fila[1] = tabla.Rows[i][1].ToString(); // Lunes
                    fila[2] = tabla.Rows[i][2].ToString(); // Martes
                    fila[3] = tabla.Rows[i][3].ToString(); // Miércoles
                    fila[4] = tabla.Rows[i][4].ToString(); // Jueves
                    fila[5] = tabla.Rows[i][5].ToString(); // Viernes
                    fila[6] = tabla.Rows[i][6].ToString(); // Sábado
                    fila[7] = tabla.Rows[i][7].ToString(); // Domingo
                    dtTemp.Rows.Add(fila);
                }
                // Grid temporal
                DataGrid dg = new DataGrid { DataSource = dtTemp };
                dg.DataBind();
                Boolean resultado = ExportToExcel("Horario Disponibilidad.xls", dg); // Exportar
                if (!resultado)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "No se ha podido exportar el archivo";
                    _lblMensaje.Visible = true;
                    _imgMensaje.Visible = true;
                }
                dg = null;
                dg.Dispose();
            }
            else
            {
                _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                _lblMensaje.Text = "No se ha podido obtener el horario de disponiblidad vigente";
                _lblMensaje.Visible = true;
                _imgMensaje.Visible = true;
            }
        }

        /// <summary>
        /// Método que se encarga de exportar un DataTable a un documento de excel
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="dg"></param>
        /// <returns></returns>

        private Boolean ExportToExcel(string nombreArchivo, DataGrid dg)
        {
            try
            {
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo);
                Response.ContentType = "application/excel";
                var sw = new System.IO.StringWriter();
                var htw = new HtmlTextWriter(sw);
                dg.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.End();
                return true;
            }
            catch (Exception e)
            {
                _controladorHorario = new ControladorHorario();
                _controladorHorario.insertarBitacoraError(e.ToString(), "");
                return false;
            }
        }

        /// <summary>
        /// Método que se encarga de llenar el horario con los turnos que ya se han realizado
        /// </summary>
        /// <param name="horario">DataTable que contendra el horario</param>
        /// <param name="turnos">Lista de listas de objetos que contiene los turnos, la lista contienen (Dia, HoraInicio, Nombre, Login)</param>
        
        public void ordenarTurnos(DataTable horario, List<List<object>> turnos)
        {
            try
            {
                int columna = 0; // Para saber en cual columna (dia) se encuentra el turno
                foreach (List<object> turno in turnos)
                {
                    switch (Convert.ToChar(turno[0])) // Se busca primero el día en el que se encuentra el turno
                    {
                        case 'L':   // Lunes
                            columna = 1;
                            break;
                        case 'K':   // Martes
                            columna = 2;
                            break;
                        case 'M':   // Miercoles
                            columna = 3;
                            break;
                        case 'J':   // Jueves
                            columna = 4;
                            break;
                        case 'V':   // Viernes
                            columna = 5;
                            break;
                        case 'S':   // Sabado
                            columna = 6;
                            break;
                    }
                    if (turno[1].ToString().Equals("07:30:00")) // Se revisa la hora de inicio del turno y se agrega el nombre al horario
                        horario.Rows[0][columna] = horario.Rows[0][columna].ToString() + turno[2].ToString() + "<br/>";
                    else if (turno[1].ToString().Equals("11:30:00"))
                        horario.Rows[1][columna] = horario.Rows[1][columna].ToString() + turno[2].ToString() + "<br/>";
                    else if (turno[1].ToString().Equals("15:30:00"))
                        horario.Rows[2][columna] = horario.Rows[2][columna].ToString() + turno[2].ToString() + "<br/>";
                    else if (turno[1].ToString().Equals("19:30:00"))
                        horario.Rows[3][columna] = horario.Rows[3][columna].ToString() + turno[2].ToString() + "<br/>";
                }
            }
            catch (Exception ex)
            {
                _controladorHorario = new ControladorHorario();
                _controladorHorario.insertarBitacoraError(ex.ToString(), "");
            }
        }

		#endregion
	}
}