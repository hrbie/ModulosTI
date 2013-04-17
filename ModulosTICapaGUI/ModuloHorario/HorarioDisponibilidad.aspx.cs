using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using ModulosTICapaGUI.Compartido;
using System.Data;

namespace ModulosTICapaGUI.ModuloHorario
{
    public partial class HorarioDisponibilidad : System.Web.UI.Page
    {

        #region Artributos
        private HttpCookie _cookieActual;
        private Sesion _sesion;
        private ControladorHorario _controlador;  //Controlador de la Interfaz

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
                _controlador = new ControladorHorario();
                string grupoUsuario = _sesion.obtenerGrupoUsuario(Request.Cookies["PS"]);
                if ((grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")) || (grupoUsuario.Equals("soporte"))
                    || (grupoUsuario.Equals("jefeti")))
                {
                    _controlador.insertarBitacoraError("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(Request.Cookies["PS"]) + " a la página de RegistroUsoLaboratorio.aspx", "");
                    Response.Redirect("../Compartido/AccesoDenegado.aspx");
                }
                ViewState["FE"] = -1;   //ultima fila editada en el grid
                ViewState["CE"] = -1;   //ultima columna editada en el grid
                ViewState["IH"] = _controlador.consultarHorarioDisponibilidad(); //Id del horario de disponibilidad, guardado en el viewState
                _gridHorario.DataSource = cargarHorarioDisponibilidad((int)ViewState["IH"]);
                _gridHorario.DataBind();
            }
        }

        #region Métodos
        /// <summary>
        /// Método que se encarga de cargar el horario de disponibiliadad
        /// </summary>
        /// <returns>Un datatable con el horario</returns>
        public DataTable cargarHorarioDisponibilidad(int idHorario)
        {
            DataTable resultado = new DataTable();
            try
            {
                if (idHorario == 0)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                    _lblMensaje.Text = "Por el momento no hay horarios de disponibilidad habilitados.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _btnHorario.Visible = false;
                    _btnGuardar.Visible = false;
                }
                else
                {
                    _controlador = new ControladorHorario();
                    //Crear las columnas de la tabla
                    resultado.Columns.Add(new DataColumn("Turno"));
                    resultado.Columns.Add(new DataColumn("Lunes"));
                    resultado.Columns.Add(new DataColumn("Martes"));
                    resultado.Columns.Add(new DataColumn("Miércoles"));
                    resultado.Columns.Add(new DataColumn("Jueves"));
                    resultado.Columns.Add(new DataColumn("Viernes"));
                    resultado.Columns.Add(new DataColumn("Sábado"));

                    //Llenar la tabla, solo con la columna de turno con valor
                    resultado.Rows.Add("7:30 a.m - 11:30 a.m", "", "", "", "", "", "");
                    resultado.Rows.Add("11:30 a.m - 3:30 p.m", "", "", "", "", "", "");
                    resultado.Rows.Add("3:30 p.m - 7:30 p.m", "", "", "", "", "", "");
                    resultado.Rows.Add("7:30 p.m - 10:30 p.m", "", "", "", "", "", "");

                    List<List<object>> turnos = _controlador.obtenerTurnosDisponibilidad(idHorario);
                    if (turnos.Count > 0)
                    {
                        ordenarTurnos(resultado, turnos);
                    }
                }
            }
            catch (Exception ex) 
            {
                _controlador = new ControladorHorario();
                _controlador.insertarBitacoraError(ex.ToString(), "");
            }
            return resultado;
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
                _sesion = new Sesion();
                string loginUsuario = _sesion.obtenerLoginUsuario(Request.Cookies["PS"]);
                List<List<object>> turnoUsuario = new List<List<object>>();
                int columna = 0;    //Para saber en cual columna (dia) se encuentra el turno
                foreach (List<object> turno in turnos)
                {
                    //Se busca primero el día en el que se encuentra el turno
                    switch (Convert.ToChar(turno[0]))
                    {
                        case 'L':   //Lunes
                            columna = 1;
                            break;
                        case 'K':   //Martes
                            columna = 2;
                            break;
                        case 'M':   //Miercoles
                            columna = 3;
                            break;
                        case 'J':   //Jueves
                            columna = 4;
                            break;
                        case 'V':   //Viernes
                            columna = 5;
                            break;
                        case 'S':   //Sabado
                            columna = 6;
                            break;
                    }
                    //Se revisa la hora de inicio del turno y se agrega el nombre al horario
                    if (turno[1].ToString().Equals("07:30:00"))
                    {
                        horario.Rows[0][columna] = horario.Rows[0][columna].ToString() + turno[2].ToString() + "<br/>";
                        // Verificar si el turno pertence al usuario
                        if (turno[3].ToString().Equals(loginUsuario))
                        {
                            // Si pertenece lo agrega a la lista que contiene los turnos que el usuario posee
                            turnoUsuario.Add(turno);
                        }
                    }
                    else if (turno[1].ToString().Equals("11:30:00"))
                    {
                        horario.Rows[1][columna] = horario.Rows[1][columna].ToString() + turno[2].ToString() + "<br/>";
                        // Verificar si el turno pertence al usuario
                        if (turno[3].ToString().Equals(loginUsuario))
                        {
                            // Si pertenece lo agrega a la lista que contiene los turnos que el usuario posee
                            turnoUsuario.Add(turno);
                        }
                    }
                    else if (turno[1].ToString().Equals("15:30:00"))
                    {
                        horario.Rows[2][columna] = horario.Rows[2][columna].ToString() + turno[2].ToString() + "<br/>";
                        // Verificar si el turno pertence al usuario
                        if (turno[3].ToString().Equals(loginUsuario))
                        {
                            // Si pertenece lo agrega a la lista que contiene los turnos que el usuario posee
                            turnoUsuario.Add(turno);
                        }
                    }
                    else if (turno[1].ToString().Equals("19:30:00"))
                    {
                        horario.Rows[3][columna] = horario.Rows[3][columna].ToString() + turno[2].ToString() + "<br/>";
                        // Verificar si el turno pertence al usuario
                        if (turno[3].ToString().Equals(loginUsuario))
                        {
                            // Si pertenece lo agrega a la lista que contiene los turnos que el usuario posee
                            turnoUsuario.Add(turno);
                        }
                    }
                }
                Session["turnosUsuari"] = turnoUsuario;     //Turnos que el usuario haya ingresado previamente, se guardan en un session
            }
            catch (Exception ex)
            {
                _controlador = new ControladorHorario();
                _controlador.insertarBitacoraError(ex.ToString(), "");
            }
        }

        protected void _gridHorario_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                _sesion = new Sesion();
                _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
                if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                    Response.Redirect("../Autentificacion/Login.aspx"); // 
                else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                    Response.SetCookie(_cookieActual);
                Control controlDesplegable; //Label que se encuentra en cada celda del grid, se utiliza para lo que se va a desplegar en la celda
                Control controlEditable;    //Textbox que se enceuntra en cada celda del grid, se utiliza para poder editar la celda            

                //Verificar si hay alguna celda editada para guardar lo que hay en esta
                if ((int)ViewState["FE"] > -1)
                {
                    controlDesplegable = _gridHorario.Rows[(int)ViewState["FE"]].Cells[(int)ViewState["CE"]].Controls[1];
                    controlEditable = _gridHorario.Rows[(int)ViewState["FE"]].Cells[(int)ViewState["CE"]].Controls[3];
                    ((Label)controlDesplegable).Text = ((TextBox)controlEditable).Text; //Copiar lo que hay en el controlEditable
                    controlDesplegable.Visible = true;  //Mostrar el controlDesplegable
                    controlEditable.Visible = false;    //Ocultar el controlEditable
                }

                // Obtener la fila y columna de la celda
                int fila = int.Parse(e.CommandArgument.ToString());
                int columna = int.Parse(Request.Form["__EVENTARGUMENT"]);

                // Obtener el label de la celda (control desplegable) y ocultarlo
                controlDesplegable = _gridHorario.Rows[fila].Cells[columna].Controls[1];
                controlDesplegable.Visible = false;
                // Obtener el textbox de la celda (control editable) y hacelo visible, para que se pueda utilizar
                controlEditable = _gridHorario.Rows[fila].Cells[columna].Controls[3];
                ((TextBox)controlEditable).Text = ((TextBox)controlEditable).Text.Replace("<br/>", " ");
                controlEditable.Visible = true;

                // Setear el foco al control editable
                ClientScript.RegisterStartupScript(GetType(), "SetFocus", "<script>document.getElementById('" + controlEditable.ClientID + "').focus();</script>");
                // Seleccionar el texto del control desplegable, esto para que este en foco
                ((TextBox)controlEditable).Attributes.Add("onfocus", "this.select()");
                ViewState["FE"] = fila;    // Guardas la fila de la celda que se edito
                ViewState["CE"] = columna;  //Guardar la columna de la celda que se edito
            }
            catch (Exception ex)
            {
                _controlador = new ControladorHorario();
                _controlador.insertarBitacoraError(ex.ToString(), "");
            }
        }

        // Se utiliza para que se habilite lo que se hiso en el metodo RowDataBound
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

        protected void _btnHorario_Click(object sender, EventArgs e)
        {
            try
            {
                _sesion = new Sesion();
                _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
                if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                    Response.Redirect("../Autentificacion/Login.aspx"); // 
                else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                    Response.SetCookie(_cookieActual);
                _gridHorario.Enabled = true;
                _imgMensaje.Visible = false;
                _lblMensaje.Visible = false;
                DataTable limpia = new DataTable(); // Tabla en limpio para que el usuario ingrese sus turnos

                //Crear las columnas de la tabla
                limpia.Columns.Add(new DataColumn("Turno"));
                limpia.Columns.Add(new DataColumn("Lunes"));
                limpia.Columns.Add(new DataColumn("Martes"));
                limpia.Columns.Add(new DataColumn("Miércoles"));
                limpia.Columns.Add(new DataColumn("Jueves"));
                limpia.Columns.Add(new DataColumn("Viernes"));
                limpia.Columns.Add(new DataColumn("Sábado"));

                //Llenar la tabla, solo con la columna de turno con valor
                limpia.Rows.Add("7:30 a.m - 11:30 a.m", "", "", "", "", "", "");
                limpia.Rows.Add("11:30 a.m - 3:30 p.m", "", "", "", "", "", "");
                limpia.Rows.Add("3:30 p.m - 7:30 p.m", "", "", "", "", "", "");
                limpia.Rows.Add("7:30 p.m - 10:30 p.m", "", "", "", "", "", "");
                ordenarTurnos(limpia, ((List<List<object>>)Session["turnosUsuari"]));
                _gridHorario.DataSource = limpia;
                _gridHorario.DataBind();

                _btnGuardar.Enabled = true; //Habilitar boton guardar
                _btnHorario.Enabled = false;//Deshabilitar boton ingresar horario disponibilidad

                // Habilitar el click en los campos del gridview
                foreach (GridViewRow fila in _gridHorario.Rows)
                {
                    // Obtener el LinkButton de la primera celda
                    LinkButton boton = (LinkButton)fila.Cells[0].Controls[0];
                    // Obtner el javascript que utiliza el LinkButton
                    string jsBoton = ClientScript.GetPostBackClientHyperlink(boton, "");

                    // Añadir los eventos a todas las celdas que se pueden editar
                    int primeraCeldaEditar = 2; //desde cual celda se va a empesar a añadir los eventos
                    for (int i = primeraCeldaEditar; i < fila.Cells.Count; i++)
                    {
                        // Verificar que no se pueda poner un turno el sabado en la noche
                        if ((((Label)fila.Cells[1].Controls[1]).Text.Equals("7:30 p.m - 10:30 p.m")) && (i == 7))
                            continue;
                        else
                        {
                            // Añadir el indice de la columna como parametro para el evento
                            string js = jsBoton.Insert(jsBoton.Length - 2, i.ToString());
                            // Añadir el script anterior al atributo onclick de la celda
                            fila.Cells[i].Attributes["onclick"] = js;
                            // Añadir el estilo del cursor a la celda
                            fila.Cells[i].Attributes["style"] += "cursor:pointer;cursor:hand;";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _controlador = new ControladorHorario();
                _controlador.insertarBitacoraError(ex.ToString(), "");
            }
        }

        protected void _btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                _sesion = new Sesion();
                _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
                if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                    Response.Redirect("../Autentificacion/Login.aspx"); // 
                else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                    Response.SetCookie(_cookieActual);
                Control controlEditable;    //Textbox que se enceuntra en cada celda del grid, se utiliza para poder editar la celda
                List<List<object>> turnos = new List<List<object>>();
                List<object> turno;
                for (int k = 0; k < _gridHorario.Rows.Count; k++)
                {
                    for (int i = 2; i < _gridHorario.Rows[k].Cells.Count; i++)
                    {
                        controlEditable = _gridHorario.Rows[k].Cells[i].Controls[3];
                        if (!((TextBox)controlEditable).Text.Equals(""))
                        {
                            turno = new List<object>();
                            turno.Add((int)ViewState["IH"]);  //Obtener el PK del horario
                            // Obtener el día
                            switch (i)
                            {
                                case 2:
                                    turno.Add('L');
                                    break;
                                case 3:
                                    turno.Add('K');
                                    break;
                                case 4:
                                    turno.Add('M');
                                    break;
                                case 5:
                                    turno.Add('J');
                                    break;
                                case 6:
                                    turno.Add('V');
                                    break;
                                case 7:
                                    turno.Add('S');
                                    break;
                            }
                            //Obtener las hora
                            switch (k)
                            {
                                case 0:
                                    turno.Add("7:30");
                                    turno.Add("11:30");
                                    break;
                                case 1:
                                    turno.Add("11:30");
                                    turno.Add("15:30");
                                    break;
                                case 2:
                                    turno.Add("15:30");
                                    turno.Add("19:30");
                                    break;
                                case 3:
                                    turno.Add("19:30");
                                    turno.Add("22:30");
                                    break;
                            }
                            turno.Add(((TextBox)controlEditable).Text); //Obtener el nombre
                            turno.Add(_sesion.obtenerLoginUsuario(Request.Cookies["PS"]));   //Obtener el login
                            turnos.Add(turno);  //Agregar el turno a la lista de turnos
                        }
                    }
                }
                // Se revisa que haya al menos un turno en el horario
                if (turnos.Count > 0)
                {
                    _controlador = new ControladorHorario();
                    bool completado = _controlador.insertarTurnosDisponibilidad(turnos);
                    if (completado)
                    {
                        _imgMensaje.ImageUrl = "~/Imagenes/ok.png";
                        _lblMensaje.Text = "Se ha guardado su horario con exito.";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                    }
                    else
                    {
                        _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                        _lblMensaje.Text = "No se pudo guardar su horario. Por favor buelva a intentar más tarde.";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                    }
                    Session["turnosUsuari"] = new List<List<object>>();  //Limpiar los turnos para que no los buelva a cargar
                    _gridHorario.DataSource = cargarHorarioDisponibilidad((int)ViewState["IH"]);
                    _gridHorario.DataBind();
                    _btnHorario.Enabled = true;
                    _btnGuardar.Enabled = false;
                }
            }
            catch (Exception ex)    // Si algo falla se reporta el error en la Bitácora de Errores del sistema
            {
                _controlador = new ControladorHorario();
                _controlador.insertarBitacoraError(ex.ToString(), "");
            }
        }
    }
    #endregion
}