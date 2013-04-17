using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using System.Data;
using System.Web;
using ModulosTICapaGUI.Compartido;
using System.Web.UI;

namespace ModulosTICapaGUI.ModuloHorario
{
    public partial class ConsultarHorario : System.Web.UI.Page
    {

        #region Atributos

        private HttpCookie _cookieActual;
        private Sesion _sesion;
        private ControladorHorario _controladorHorario; // Controlador para el manejo del horario
        private static List<List<String>> _listaLugares; // Lista de lugares que se cargarán
        private static List<List<Object>> _listaTurnos; // Lista de los turnos del horario       

        #endregion
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            _controladorHorario = new ControladorHorario();
            ViewState["SA"] = _controladorHorario.consultarSemestreActivo();    //Semestre que se encuentra activo, se guarda en un ViewState para mayor facilidad
            _listaLugares = _controladorHorario.consultarLugares(); // Obtener los lugares
            if (_listaLugares != null)
            {
                if (_listaLugares.Count == 0)
                {
                    _lblMensaje.Text = "No hay lugares registrados en el sistema";
                    _lblMensaje.Visible = true;
                    _imgMensaje.Visible = true;
                }
                else
                {
                    _ddlLugar.Items.Add("Seleccionar");
                    for (int t = 0; t < _listaLugares.Count; t++ )
                    {
                        var temp = new ListItem(_listaLugares[t][1].ToString(), _listaLugares[t][1].ToString());
                        _ddlLugar.Items.Add(temp);
                    }
                }
            }
            else
            {
                _lblMensaje.Text = "Se ha presentado un error al cargar la información de los lugares";
                _lblMensaje.Visible = true;
                _imgMensaje.Visible = true;
            }
        }

        protected void _btnConsultarHorario_Click(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            if (_ddlLugar.SelectedIndex > 0)
            {
                if ((int)ViewState["SA"] > 0)
                {
                    _controladorHorario = new ControladorHorario();
                    _listaTurnos = _controladorHorario.consultarTurnosHorario(Convert.ToInt32(_listaLugares[_ddlLugar.SelectedIndex - 1][0]), (int)ViewState["SA"]);
                    if ((_listaTurnos != null) && (_listaTurnos.Count > 0))
                    {
                        llenarTabla(1); // Llenar las tablas
                        _lblMensaje.Visible = false;
                        _imgMensaje.Visible = false;
                        _gridHorario.Visible = true;
                        _btnExportar.Enabled = true;
                    }
                    else if (_listaTurnos == null)
                    {
                        _lblMensaje.Text = "Ha habido un error al obtener el horario";
                        _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                        _lblMensaje.Visible = true;
                        _imgMensaje.Visible = true;
                        _gridHorario.Visible = false;
                        _btnExportar.Enabled = false;
                    }
                    else
                    {
                        _lblMensaje.Text = "No existe un horario para el lugar indicado";
                        _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                        _lblMensaje.Visible = true;
                        _imgMensaje.Visible = true;
                        _gridHorario.Visible = false;
                        _btnExportar.Enabled = false;
                    }
                }
                else
                {
                    _lblMensaje.Text = "No hay semestres habilitados para consulta";
                    _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                    _lblMensaje.Visible = true;
                    _imgMensaje.Visible = true;
                    _gridHorario.Visible = false;
                    _btnExportar.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Método que se encarga de llenar el gridview del horario
        /// </summary>
        /// <param name="tipo">Indica si al horario hay que cargarle los turnos o no. 0:NO 1:SI</param>

        public void llenarTabla(int tipo)
        {
            DataTable tablaTurnos = new DataTable();
            String nombreTipoLugar = _listaLugares[_ddlLugar.SelectedIndex - 1][2]; // Obtener el nombre del tipo lugar
            // Crear las columnas de la tabla
            tablaTurnos.Columns.Add(new DataColumn("Turno"));
            tablaTurnos.Columns.Add(new DataColumn("Lunes"));
            tablaTurnos.Columns.Add(new DataColumn("Martes"));
            tablaTurnos.Columns.Add(new DataColumn("Miércoles"));
            tablaTurnos.Columns.Add(new DataColumn("Jueves"));
            tablaTurnos.Columns.Add(new DataColumn("Viernes"));
            tablaTurnos.Columns.Add(new DataColumn("Sábado"));
            tablaTurnos.Columns.Add(new DataColumn("Domingo"));
            if (nombreTipoLugar.Equals("Oficina"))
            {
                // Llenar la tabla, solo con la columna de turno con valor
                tablaTurnos.Rows.Add("7:30 a.m - 8:30 a.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("8:30 a.m - 9:30 a.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("9:30 a.m - 10:30 a.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("10:30 a.m - 11:30 a.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("11:30 a.m - 12:00 p.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("1:00 p.m - 2:00 p.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("2:00 p.m - 3:00 p.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("3:00 p.m - 4:00 p.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("4:00 p.m - 5:00 p.m", "", "", "", "", "", "");
                if (tipo == 1)
                    llenarTurnos(0, tablaTurnos);
                _gridHorario.DataSource = tablaTurnos;
                _gridHorario.DataBind();
            }
            else if (nombreTipoLugar.Equals("Aula"))
            {
                // Llenar la tabla, solo con la columna de turno con valor
                tablaTurnos.Rows.Add("7:30 a.m - 9:20 a.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("9:30 a.m - 11:20 a.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("11:30 a.m - 12:20 p.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("1:00 p.m - 2:50 a.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("3:00 p.m - 4:50 p.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("5:00 p.m - 6:50 p.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("7:00 p.m - 9:00 p.m", "", "", "", "", "", "");
                if (tipo == 1)
                    llenarTurnos(1, tablaTurnos);
                _gridHorario.DataSource = tablaTurnos;
                _gridHorario.DataBind();
            }
            else if (nombreTipoLugar.Equals("Laboratorio"))
            {
                // Llenar la tabla, solo con la columna de turno con valor
                tablaTurnos.Rows.Add("7:30 a.m - 11:30 a.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("11:30 a.m - 3:30 p.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("3:30 p.m - 7:30 p.m", "", "", "", "", "", "");
                tablaTurnos.Rows.Add("7:30 p.m - 10:30 p.m", "", "", "", "", "", "");
                if (tipo == 1)
                    llenarTurnos(2, tablaTurnos);
                _gridHorario.DataSource = tablaTurnos;
                _gridHorario.DataBind();
            }
            Session["tablaTurnos"] = tablaTurnos;       //Guardar el grid que se esta viendo, 
        }

        /// <summary>
        /// Método que se encarga de llenar los turnos del horario
        /// </summary>
        /// <param name="tipo">Indica el tipo de horario (de acuerdo al lugar) que se va a llenar</param>

        public void llenarTurnos(int tipo, DataTable tablaTurnos)
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
                            tablaTurnos.Rows[0][columna] = tablaTurnos.Rows[0][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "08:30:00":
                            tablaTurnos.Rows[1][columna] = tablaTurnos.Rows[1][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "09:30:00":
                            tablaTurnos.Rows[2][columna] = tablaTurnos.Rows[2][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "10:30:00":
                            tablaTurnos.Rows[3][columna] = tablaTurnos.Rows[3][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "11:30:00":
                            tablaTurnos.Rows[4][columna] = tablaTurnos.Rows[4][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "13:00:00":
                            tablaTurnos.Rows[5][columna] = tablaTurnos.Rows[5][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "14:00:00":
                            tablaTurnos.Rows[6][columna] = tablaTurnos.Rows[6][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "15:00:00":
                            tablaTurnos.Rows[7][columna] = tablaTurnos.Rows[7][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "16:00:00":
                            tablaTurnos.Rows[8][columna] = tablaTurnos.Rows[8][columna].ToString() + _listaTurnos[i][4].ToString();
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
                            tablaTurnos.Rows[0][columna] = tablaTurnos.Rows[0][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "09:30:00":
                            tablaTurnos.Rows[1][columna] = tablaTurnos.Rows[1][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "11:30:00":
                            tablaTurnos.Rows[2][columna] = tablaTurnos.Rows[2][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "13:00:00":
                            tablaTurnos.Rows[3][columna] = tablaTurnos.Rows[3][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "15:00:00":
                            tablaTurnos.Rows[4][columna] = tablaTurnos.Rows[4][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "17:00:00":
                            tablaTurnos.Rows[5][columna] = tablaTurnos.Rows[5][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "19:00:00":
                            tablaTurnos.Rows[6][columna] = tablaTurnos.Rows[6][columna].ToString() + _listaTurnos[i][4].ToString();
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
                            tablaTurnos.Rows[0][columna] = tablaTurnos.Rows[0][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "11:30:00":
                            tablaTurnos.Rows[1][columna] = tablaTurnos.Rows[1][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "15:30:00":
                            tablaTurnos.Rows[2][columna] = tablaTurnos.Rows[2][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                        case "19:30:00":
                            tablaTurnos.Rows[3][columna] = tablaTurnos.Rows[3][columna].ToString() + _listaTurnos[i][4].ToString();
                            break;
                    }
                }
            }
        }

        protected void _btnExportar_Click(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            // Crear una tabla temporal
            DataTable dtTemp = new DataTable();
            DataTable tablaTurnos = (DataTable)Session["tablaTurnos"];

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
            for (int i = 0; i < tablaTurnos.Rows.Count; i++)
            {
                fila = dtTemp.NewRow();
                fila[0] = tablaTurnos.Rows[i][0].ToString(); // Turno
                fila[1] = tablaTurnos.Rows[i][1].ToString(); // Lunes
                fila[2] = tablaTurnos.Rows[i][2].ToString(); // Martes
                fila[3] = tablaTurnos.Rows[i][3].ToString(); // Miércoles
                fila[4] = tablaTurnos.Rows[i][4].ToString(); // Jueves
                fila[5] = tablaTurnos.Rows[i][5].ToString(); // Viernes
                fila[6] = tablaTurnos.Rows[i][6].ToString(); // Sábado
                fila[7] = tablaTurnos.Rows[i][7].ToString(); // Domingo
                dtTemp.Rows.Add(fila);
            }

            // Grid temporal
            var dg = new DataGrid {DataSource = dtTemp};
            dg.DataBind();
            ExportToExcel("Horario" + _ddlLugar.SelectedItem.Text + ".xls", dg); // Exportar
            dg = null;
            dg.Dispose();
        }

        private void ExportToExcel(string strFileName, DataGrid dg)
        {
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=" + strFileName);
            Response.ContentType = "application/excel";
            var sw = new System.IO.StringWriter();
            var htw = new HtmlTextWriter(sw);
            dg.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}