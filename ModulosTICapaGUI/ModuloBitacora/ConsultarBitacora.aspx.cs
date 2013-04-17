﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTICapaGUI.Compartido;
using ModulosTIControlador.Clases;
using System.Data;
using System.Threading;

namespace ModulosTICapaGUI.ModuloBitacora
{
    public partial class ConsultarBitacora : System.Web.UI.Page
    {
        #region Atributos

        private static ControladorBitacora _controlador; // Controlador de la Interfaz

        private static List<List<object>> _laboratorios; // Lista de lugares que será cargada en el _ddlLaboratorios
        private HttpCookie _cookieActual;
        private Sesion _sesion;

        #endregion

        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // Solo cargar los datos la primera vez que la página ha sido cargada
            {
                _sesion = new Sesion();
                _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
                if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                {
                    Response.Redirect("../Autentificacion/Login.aspx"); // 
                }
                else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                    Response.SetCookie(_cookieActual);
                _controlador = new ControladorBitacora();
                string grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
                string loginUsuario = _sesion.obtenerLoginUsuario(_cookieActual);
                if ((grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")))
                {
                    _controlador.insertarBitacoraError("Se ha intentado realizar un acceso no permitido por parte del usuario " + loginUsuario + " a la página de GenerarEstadisticaUso.aspx", "");
                    Response.Redirect("../Compartido/AccesoDenegado.aspx");
                }
                _laboratorios = _controlador.obtenerLaboratorios();
                var errores = new int[2];
                errores[0] = 0; //Error al cargar Laboratorios

                if (_laboratorios != null) // Llenar _ddlLaboratorios si se encontraton datos 
                {
                    for (int i = 0; i < _laboratorios.Count; i++)
                    {
                        var item = new ListItem(_laboratorios.ElementAt(i).ElementAt(1).ToString(), Convert.ToString(_laboratorios.ElementAt(i).ElementAt(0)));
                        _ddlLaboratorios.Items.Add(item); // Obtiene los nombres de los laboratorios encontrados
                    }

                }
                else // No se encontraron laboratorios
                    errores[0] = 1;

                if (errores[0] == 1) // Detectar errores 
                {
                    _ddlLaboratorios.Items.Clear();
                    _ddlLaboratorios.Items.Add("No hay laboratorios disponibles");

                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Error al cargar los laboratorios.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;

                }
                else
                {
                    var dt = new DataTable();
                    dt.Columns.Add(new DataColumn("PK_Entrada"));
                    dt.Columns.Add(new DataColumn("Fecha"));
                    dt.Columns.Add(new DataColumn("Operador"));
                    dt.Columns.Add(new DataColumn("Evento"));

                    _btnExportarExcel.Enabled = false;

                    _imgMensaje.Visible = false;
                    _lblMensaje.Visible = false;
                    _gvwEventos.DataSource = dt;
                    _gvwEventos.DataBind();
                    _upMensaje.Update();
                    _upEvento.Update();

                }
            }
        }

        //
        public void generarReporte()
        {
            Thread STAThread = new Thread( ()=>
            {
                // Create a new workbook.
                SpreadsheetGear.IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook();
                SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets["Sheet1"];
                SpreadsheetGear.IRange cells = worksheet.Cells;

                // Set the worksheet name.
                worksheet.Name = "2005 Sales";

                // Load column titles and center.
                cells["B1"].Formula = "North";
                cells["C1"].Formula = "South";
                cells["D1"].Formula = "East";
                cells["E1"].Formula = "West";
                cells["B1:E1"].HorizontalAlignment = SpreadsheetGear.HAlign.Center;

                // Load row titles using multiple cell text reference and iteration.
                int quarter = 1;
                foreach (SpreadsheetGear.IRange cell in cells["A2:A5"])
                    cell.Formula = "Q" + quarter++;

                // Load random data and format as $ using a multiple cell range.
                SpreadsheetGear.IRange body = cells[1, 1, 4, 4];
                body.Formula = "=RAND() * 10000";
                body.NumberFormat = "$#,##0_);($#,##0)";

                // Stream the Excel spreadsheet to the client in a format
                // compatible with Excel 97/2000/XP/2003/2007/2010.
                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment; filename=report.xls");
                workbook.SaveToStream(Response.OutputStream, SpreadsheetGear.FileFormat.Excel8);
                Response.End();
            });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();
        }

        protected void _btnConsultar_Click(object sender, EventArgs e)
        {
            //generarReporte();
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            if (_ddlLaboratorios.SelectedValue != "0")
            {
                var dt = new DataTable();
                if (_txtFechaConsulta.Text != "")
                {
                    String tempStringFechaConsulta = String.Format("{0:u}", _txtFechaConsulta.Text);  // "2008-03-09 16:05:07Z"

                    dt = _controlador.consultarEntradaPorDia(Convert.ToInt32(_ddlLaboratorios.SelectedValue), tempStringFechaConsulta);
                    _imgMensaje.Visible = false;
                    _lblMensaje.Visible = false;
                    if (dt.Rows.Count == 0)
                    {
                        _lblMensaje.Text = "No hay registros en la bitácora para la fecha seleccionada";
                        _imgMensaje.ImageUrl = "~/Imagenes/Ok.png";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        _btnExportarExcel.Enabled = false;
                    }
                    else {
                        _btnExportarExcel.Enabled = true;
                        //UpdatePanel2.Update();
                    }
                    _gvwEventos.DataSource = dt;
                    _gvwEventos.DataBind();
                    _upMensaje.Update();
                    _upEvento.Update();
                }
                else
                {
                    //error, debe seleccionar una fecha
                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Error, debe seleccionar una fecha.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _upMensaje.Update();
                }
            }
            else
            {
                //error, debe seleccionar un laboratorio
                _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                _lblMensaje.Text = "Error, debe seleccionar un laboratorio.";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
                _upMensaje.Update();
            }
        }

        protected void _btnExportarExcel_Click(object sender, EventArgs e)
        {
            generarReporte();            
        }
    }
}