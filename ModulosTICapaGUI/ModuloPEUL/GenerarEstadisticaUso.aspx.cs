using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using ModulosTICapaGUI.Compartido;
using ModulosTIControlador.Clases;

namespace ModulosTICapaGUI.ModuloPEUL
{


    public partial class GenerarEstadisticaUso : System.Web.UI.Page
    {
        #region Atributos

            private static ControladorPEUL _controlador; // Controlador de la Interfaz
            private static List<List<object>> _laboratorios; // Lista de lugares que será cargada en el _ddlLaboratorios
            private static List<List<object>> _Semestres; // Lista de lugares que será cargada en el _ddlSemestreInicio y _ddlSmestreFinal
            private HttpCookie _cookieActual;
            private Sesion _sesion;

        #endregion

        #region Métodos

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
                _controlador = new ControladorPEUL();
                string grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
                string loginUsuario = _sesion.obtenerLoginUsuario(_cookieActual);
				if ((grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")) ||
						(grupoUsuario.Equals("operadores")))
				{
					_controlador.insertarBitacoraError("Se ha intentado realizar un acceso no permitido por parte del usuario " + loginUsuario + " a la página de GenerarEstadisticaUso.aspx", "");
					Response.Redirect("../Compartido/AccesoDenegado.aspx");
				}
                _laboratorios = _controlador.obtenerLaboratorios();
                _Semestres = _controlador.obtenerSemestres();
                var _errores = new int[2];
                _errores[0] = 0; //Error al cargar Laboratorios
                _errores[1] = 0; //Error al cargar Semestres

                if (_laboratorios != null) // Llenar _ddlLaboratorios si se encontraton datos 
                {
                    for (int i = 0; i < _laboratorios.Count; i++)
                    {
                        var item = new ListItem(_laboratorios.ElementAt(i).ElementAt(1).ToString(), Convert.ToString(_laboratorios.ElementAt(i).ElementAt(0)));
                        _ddlLaboratorios.Items.Add(item); // Obtiene los nombres de los laboratorios encontrados
                    }
                              
                }
                else // No se encontraron laboratorios
                    _errores [0]= 1;

                if (_Semestres != null) // Llenar _ddlSemestreInicio y final si se encontraton datos 
                {
                    for (int i = 0; i < _Semestres.Count; i++)
                    {

                        var item = new ListItem(  _Semestres.ElementAt(i).ElementAt(1).ToString(),_Semestres.ElementAt(i).ElementAt(2).ToString());
                        _ddlSemestreInicio.Items.Add(item); 
                            // Obtiene los nombres de los Semestres encontrados
                        item = new ListItem( _Semestres.ElementAt(i).ElementAt(1).ToString(),_Semestres.ElementAt(i).ElementAt(3).ToString());
                        _ddlSemestreFinal.Items.Add(item);
                    }
                }
                else // No se encontraron semestres
                    _errores[1] = 1;

                if (_errores[0] == 1) // Detectar errores 
                {
                    if(_errores[1] == 1)
                    {
                        _ddlLaboratorios.Items.Clear();
                        _ddlSemestreFinal.Items.Clear();
                        _ddlSemestreInicio.Items.Clear();
                        _ddlLaboratorios.Items.Add("No hay laboratorios disponibles");
                        _ddlSemestreInicio.Items.Add("No hay semestres disponibles");         
                        _ddlSemestreFinal.Items.Add("No hay semestres disponibles");  

                        _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                        _lblMensaje.Text = "Error al cargar los de semestres y laboratorios";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                    }
                    else
                    {
                        _ddlLaboratorios.Items.Clear();
                        _ddlLaboratorios.Items.Add("No hay laboratorios disponibles");

                        _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                        _lblMensaje.Text = "Error al cargar los laboratorios.";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                    }
                }
                if (_errores[1] == 1) // Detectar errores 
                {
                    if(_errores[0] == 1)
                    {
                        _ddlLaboratorios.Items.Clear();
                        _ddlSemestreFinal.Items.Clear();
                        _ddlSemestreInicio.Items.Clear();
                        _ddlLaboratorios.Items.Add("No hay laboratorios disponibles");
                        _ddlSemestreInicio.Items.Add("No hay semestres disponibles");         
                        _ddlSemestreFinal.Items.Add("No hay semestres disponibles");  

                        _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                        _lblMensaje.Text = "Error al cargar los de semestres y laboratorios";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                    }
                    else
                    {
                        _ddlLaboratorios.Items.Clear();
                        _ddlLaboratorios.Items.Add("No hay semestres disponibles");

                        _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                        _lblMensaje.Text = "Erro al cargar los semestres";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                    }
                }

                var dt = new DataTable("Datos");
                dt.Columns.Add(new DataColumn("Uso"));
                dt.Columns.Add(new DataColumn("Periodos"));
               
                dt.Rows.Add(15, "Enero-2010");
                dt.Rows.Add(10, "Febrero-2010");
                dt.Rows.Add(5,  "Marzo-2010");
                dt.Rows.Add(35, "Abril-2010");
                dt.Rows.Add(90, "Mayo-2010");
                dt.Rows.Add(50, "Junio-2010");
              
                //Cargar los datos de la base y define ejes X y Y
                _chtEstadisticas.DataSource = dt;
                _chtEstadisticas.Series["_serieUso"].XValueMember = "Periodos";
                _chtEstadisticas.Series["_serieUso"].YValueMembers = "Uso";
                _chtEstadisticas.DataBind();

                _chtEstadisticas.Series["_serieUso"]["ShowMarkerLines"] = "True";
                _chtEstadisticas.ChartAreas["_chartAreaEstadisticas"].AxisX.Title = "Fechas";
                _chtEstadisticas.ChartAreas["_chartAreaEstadisticas"].AxisY.Title = "Porcentaje de uso";

                 dt = new DataTable("Datos");
                dt.Columns.Add(new DataColumn("Uso"));
                dt.Columns.Add(new DataColumn("Periodos"));

                dt.Rows.Add(8, "Enero-2010");
                dt.Rows.Add(2, "Febrero-2010");
                dt.Rows.Add(6, "Marzo-2010");
                dt.Rows.Add(5, "Abril-2010");
                dt.Rows.Add(15, "Mayo-2010");
                dt.Rows.Add(10, "Junio-2010");

                //Cargar los datos de la base y define ejes X y Y
                _chtEstadisticasLaptops.DataSource = dt;
                _chtEstadisticasLaptops.Series["_serieUsoLaptop"].XValueMember = "Periodos";
                _chtEstadisticasLaptops.Series["_serieUsoLaptop"].YValueMembers = "Uso";
                _chtEstadisticasLaptops.DataBind();

                _chtEstadisticasLaptops.Series["_serieUsoLaptop"]["ShowMarkerLines"] = "True";
                _chtEstadisticasLaptops.ChartAreas["_chartAreaEstadisticasLaptops"].AxisX.Title = "Fechas";
                _chtEstadisticasLaptops.ChartAreas["_chartAreaEstadisticasLaptops"].AxisY.Title = "Cantidad de laptops";

                UpdatePanel.Update();
                
            }
        }

        protected void _btnGraficar_Click(object sender, EventArgs e)
        {
            int mostrar;
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            if (_ddlLaboratorios.SelectedValue != "Seleccionar" )
            {
                var dt = new DataTable();
                if (_ddlMostrar.SelectedIndex != 0)
                { 
                    mostrar = Convert.ToInt32(_ddlMostrar.SelectedValue);
                    if (mostrar != 3)
                    {
                        if (_txtFechaInicio.Text != "" && _txtFechaFinal.Text != "")
                        {
                            String tempStringIncio = String.Format("{0:u}", _txtFechaInicio.Text);  // "2008-03-09 16:05:07Z"
                            String tempStringFinal = String.Format("{0:u}", _txtFechaFinal.Text);  // "2008-03-09 16:05:07Z"
                            //Valida si las fechas de inicio y final son correctas
                            if (Convert.ToDateTime(tempStringIncio) <= Convert.ToDateTime(tempStringFinal))
                            {
                                //La fecha es valida
                                switch (mostrar)
                                {
                                    case 0:     //Seleccionar
                                        _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                                        _lblMensaje.Text = "Debe seleccionar una forma para mostrar";
                                        _imgMensaje.Visible = true;
                                        _lblMensaje.Visible = true;
                                        break;

                                    case 1:     //Diario
                                        dt = _controlador.consultarPorcentajeUsoDias(Convert.ToInt32(_ddlLaboratorios.SelectedValue), tempStringIncio, tempStringFinal);
                                        //Cargar los datos de la base y define ejes X y Y
                                        _chtEstadisticas.DataSource = dt;
                                        
                                        _chtEstadisticas.Series["_serieUso"].XValueMember = "Periodos";
                                        _chtEstadisticas.Series["_serieUso"].YValueMembers = "Usos";
                                        _chtEstadisticas.DataBind();
                                        _chtEstadisticas.ChartAreas["_chartAreaEstadisticas"].AxisX.Title = "Fechas";
                                        _chtEstadisticas.ChartAreas["_chartAreaEstadisticas"].AxisY.Title = "Porcentaje de uso";
                                        
                                        _chtEstadisticasLaptops.DataSource = dt;
                                        _chtEstadisticasLaptops.Series["_serieUsoLaptop"].XValueMember = "Periodos";
                                        _chtEstadisticasLaptops.Series["_serieUsoLaptop"].YValueMembers = "Laptops";
                                        _chtEstadisticasLaptops.DataBind();
                                        _chtEstadisticasLaptops.ChartAreas["_chartAreaEstadisticasLaptops"].AxisX.Title = "Fechas";
                                        _chtEstadisticasLaptops.ChartAreas["_chartAreaEstadisticasLaptops"].AxisY.Title = "Catidad de Laptops";
                                        break;

                                    case 2:     //Mensual
                                        dt = _controlador.consultarPorcentajeUsoMeses(Convert.ToInt32(_ddlLaboratorios.SelectedValue), tempStringIncio, tempStringFinal);
                                        //Cargar los datos de la base y define ejes X y Y
                                        _chtEstadisticas.DataSource = dt;
                                        _chtEstadisticas.Series["_serieUso"].XValueMember = "Periodos";
                                        _chtEstadisticas.Series["_serieUso"].YValueMembers = "Usos";
                                        _chtEstadisticas.DataBind();
                                        _chtEstadisticas.ChartAreas["_chartAreaEstadisticas"].AxisX.Title = "Fechas";
                                        _chtEstadisticas.ChartAreas["_chartAreaEstadisticas"].AxisY.Title = "Porcentaje de uso";

                                        _chtEstadisticasLaptops.DataSource = dt;
                                        _chtEstadisticasLaptops.Series["_serieUsoLaptop"].XValueMember = "Periodos";
                                        _chtEstadisticasLaptops.Series["_serieUsoLaptop"].YValueMembers = "Laptops";
                                        _chtEstadisticasLaptops.DataBind();
                                        _chtEstadisticasLaptops.ChartAreas["_chartAreaEstadisticasLaptops"].AxisX.Title = "Fechas";
                                        _chtEstadisticasLaptops.ChartAreas["_chartAreaEstadisticasLaptops"].AxisY.Title = "Catidad de Laptops";
                                        break;

                                    case 4:     //Anual
                                        dt = _controlador.consultarPorcentajeUsoAno(Convert.ToInt32(_ddlLaboratorios.SelectedValue), tempStringIncio, tempStringFinal);
                                        //Cargar los datos de la base y define ejes X y Y
                                        _chtEstadisticas.DataSource = dt;

                                        _chtEstadisticas.Series["_serieUso"].XValueMember = "Periodos";
                                        _chtEstadisticas.Series["_serieUso"].YValueMembers = "Usos";
                                        _chtEstadisticas.DataBind();
                                        _chtEstadisticas.ChartAreas["_chartAreaEstadisticas"].AxisX.Title = "Fechas";
                                        _chtEstadisticas.ChartAreas["_chartAreaEstadisticas"].AxisY.Title = "Porcentaje de uso";

                                         _chtEstadisticasLaptops.DataSource = dt;
                                        _chtEstadisticasLaptops.Series["_serieUsoLaptop"].XValueMember = "Periodos";
                                        _chtEstadisticasLaptops.Series["_serieUsoLaptop"].YValueMembers = "Laptops";
                                        _chtEstadisticasLaptops.DataBind();
                                        _chtEstadisticasLaptops.ChartAreas["_chartAreaEstadisticasLaptops"].AxisX.Title = "Fechas";
                                        _chtEstadisticasLaptops.ChartAreas["_chartAreaEstadisticasLaptops"].AxisY.Title = "Catidad de Laptops";
                                        break;
                                }
                                _imgMensaje.Visible = false;
                                _lblMensaje.Visible = false;
                                UpdatePanel.Update();
                            }
                            else
                            {
                                //Error fecha inicio mayor a la de final
                                _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                                _lblMensaje.Text = "La fecha de incio debe ser anterior a la de final";
                                _imgMensaje.Visible = true;
                                _lblMensaje.Visible = true;
                            }
                        }
                        else
                        {
                            //Error, falta seleccionar fechas de inicio y fin
                            _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                            _lblMensaje.Text = "Debe seleccionar una fecha de incio y una de fin para graficar";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                        }
                    }
                    else
                    {
                        if (_ddlSemestreInicio.SelectedValue != "Semestre" && _ddlSemestreFinal.SelectedValue != "Semestre")
                        {
                            if (Convert.ToDateTime(_ddlSemestreInicio.SelectedValue) <= Convert.ToDateTime(_ddlSemestreFinal.SelectedValue))
                            {
                                dt = _controlador.consultarPorcentajeUsoSemestres(Convert.ToInt32(_ddlLaboratorios.SelectedValue),
                                     _ddlSemestreInicio.SelectedValue, _ddlSemestreFinal.SelectedValue);
                                //Cargar los datos de la base y define ejes X y Y
                                _chtEstadisticas.DataSource = dt;
                                _chtEstadisticas.Series["_serieUso"].XValueMember = "Periodos";
                                _chtEstadisticas.Series["_serieUso"].YValueMembers = "Usos";
                                _chtEstadisticas.DataBind();
                                _chtEstadisticas.ChartAreas["_chartAreaEstadisticas"].AxisX.Title = "Fechas";
                                _chtEstadisticas.ChartAreas["_chartAreaEstadisticas"].AxisY.Title = "Porcentaje de uso";

                                _chtEstadisticasLaptops.DataSource = dt;
                                _chtEstadisticasLaptops.Series["_serieUsoLaptop"].XValueMember = "Periodos";
                                _chtEstadisticasLaptops.Series["_serieUsoLaptop"].YValueMembers = "Laptops";
                                _chtEstadisticasLaptops.DataBind();
                                _chtEstadisticasLaptops.ChartAreas["_chartAreaEstadisticasLaptops"].AxisX.Title = "Fechas";
                                _chtEstadisticasLaptops.ChartAreas["_chartAreaEstadisticasLaptops"].AxisY.Title = "Catidad de Laptops";
                                _imgMensaje.Visible = false;
                                _lblMensaje.Visible = false;
                                UpdatePanel.Update();
                            }
                            else
                            {
                                //Error rango de fechas inapropiado 
                                _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                                _lblMensaje.Text = "El semestre inicial debe ser anterior al semestre final";
                                _imgMensaje.Visible = true;
                                _lblMensaje.Visible = true;
                            }
                        }
                        else
                        {
                            //Error, falta seleccionar rango de semestres
                            _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                            _lblMensaje.Text = "Debe seleccionar un semestre de inicio y uno de fin";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                        }
                    }
                }
                else
                {
                    //Error al seleccionar la forma de mostrar
                    _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                    _lblMensaje.Text = "Debe seleccionar una opción para mostrar";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }
            else
            {
                //Error falta seleccionar Laboratrorio
                _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                _lblMensaje.Text = "Debe seleccionar un laboratorio";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }
            _upMensaje.Update(); 
        }


        protected void _ddlMostrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ddlMostrar.SelectedIndex == 3)
            {
                _ddlSemestreInicio.Enabled = true;
                _ddlSemestreFinal.Enabled = true;
                _axCalendarioInicio.Enabled = false;
                _axCalendarioFinal.Enabled = false;
                _upSemestreInicial.Update();
                _upSemestreFinal.Update();
                _upFechaIncio.Update();
                _upFechaFinal.Update();
            }
            if (_ddlMostrar.SelectedIndex != 3)
            {
                _ddlSemestreInicio.Enabled = false;
                _ddlSemestreFinal.Enabled = false;
                _axCalendarioInicio.Enabled = true;
                _axCalendarioFinal.Enabled = true;
                _upSemestreInicial.Update();
                _upSemestreFinal.Update();
                _upFechaIncio.Update();
                _upFechaFinal.Update();
            }
        }
            
        #endregion
       
    }
    
}