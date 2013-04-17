using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ModulosTIControlador.Clases;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using ModulosTICapaGUI.Compartido;

namespace ModulosTICapaGUI.ModuloPEUL
{
    public partial class RegistroUsoLaboratorio : System.Web.UI.Page
    {
        #region Artributos
        private HttpCookie _cookieActual;
        private Sesion _sesion;
        private ControladorPEUL _controlador;  //Controlador de la Interfaz
        private static List<List<object>> _laboratorios;   //Lista que tendra todos los laboratorios que se encuentran activos
        private static List<List<object>> _estados;    //Lista que contiene todos los estados en los que se puede encontrar un laboratorio
        private List<List<object>> _usoLaboratorios;    //Lista que contiene el uso de cada laboratorio
        private DataTable _dtGrafico;   //Data table utilizado para llenar el grafico
        private DataTable _dtDatosUso;  //Data table utilizado para llenar el grid que contiene los datos del uso de laboratorio
        private int _errorCarga; // Para manejo de errores en el tiempo de carga de la página
                                        // 0: No se cargaron laboratorios
                                        // 1: No se cargaron los estados (Comentarios predefinidos)
                                        // 2: No se han cargado ni estados ni laboratorios
                                        // 3: No se obtuvieron datos del uso de los laboratorios
                                        // 4: No se pudo cargar ninguno de los elementos de la pagina
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack) // Solo cargar los datos la primera vez que la página ha sido cargada
                {
                    _sesion = new Sesion();
                    _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
                    if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                        Response.Redirect("../Autentificacion/Login.aspx"); // 
                    else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                        Response.SetCookie(_cookieActual);
                    _errorCarga = -1;
                    _controlador = new ControladorPEUL();
                    string grupoUsuario = _sesion.obtenerGrupoUsuario(Request.Cookies["PS"]);
                    if ((grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")))
					{
                        _controlador.insertarBitacoraError("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(Request.Cookies["PS"]) + " a la página de RegistroUsoLaboratorio.aspx", "");
						Response.Redirect("../Compartido/AccesoDenegado.aspx");
					}
                    _laboratorios = _controlador.obtenerLaboratorios(); //Obtener los laboratorios
                    if (_laboratorios != null)
                        foreach (List<object> lab in _laboratorios)
                            _ddlLaboratorio.Items.Add(lab[1].ToString());    //Inserta los nombres de los laboratorios en _ddlLaboratorio
                    else
                        _errorCarga = 0;

                    _estados = _controlador.obtenerEstadoLaboratorio(); //Obtener los estados de los laboratorios
                    if (_estados.Count > 0)
                        foreach (List<object> estado in _estados)
                            _ddlEstado.Items.Add(estado[1].ToString()); //Insertar las descripciones de los estados en _ddlEstado
                    else
                    {
                        if (_errorCarga == 0)
                            _errorCarga = 2;
                        else
                            _errorCarga = 1;
                    }

                    _usoLaboratorios = _controlador.calcularUsoActual();    //Obtener los datos del uso de los laboratorios
                    if (_usoLaboratorios != null)
                        cargarDatos(_usoLaboratorios);  //Llama al método que se encarga de cargar los datos en el grid y en el grafico
                    else
                    {
                        if (_errorCarga == 2)
                            _errorCarga = 4;
                        else
                            _errorCarga = 3;
                    }

                    switch (_errorCarga)    //Verificar si hubo algun error
                    {
                        case 0:
                            _ddlLaboratorio.Items.Clear();
                            _ddlLaboratorio.Items.Add("No hay lugares disponibles");
                            _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                            _lblMensaje.Text = "No hay lugares disponibles.";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                            break;
                        case 1:
                            _ddlEstado.Items.Clear();
                            _ddlEstado.Items.Add("No hay estados disponibles");
                            _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                            _lblMensaje.Text = "No hay estados disponibles.";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                            break;
                        case 2:
                            _ddlLaboratorio.Items.Clear();
                            _ddlLaboratorio.Items.Add("No hay lugares disponibles");
                            _ddlEstado.Items.Clear();
                            _ddlEstado.Items.Add("No hay estados disponibles");
                            _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                            _lblMensaje.Text = "No hay lugares y estados disponibles.";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                            break;
                        case 3:
                            _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                            _lblMensaje.Text = "No hay registro del uso de los laboratorios.";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                            break;
                        case 4:
                            _ddlLaboratorio.Items.Clear();
                            _ddlLaboratorio.Items.Add("No hay lugares disponibles");
                            _ddlEstado.Items.Clear();
                            _ddlEstado.Items.Add("No hay estados disponibles");
                            _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                            _lblMensaje.Text = "No hay lugares, estados y registro de uso de los laboratorios disponibles.";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                            break;
                    }
                }
            }
            catch (Exception ex) 
            {
				_controlador = new ControladorPEUL();
                _controlador.insertarBitacoraError(ex.ToString(), "");
            }
        }

        protected void _btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                _sesion = new Sesion();
                _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
                if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                    Response.Redirect("../Autentificacion/Login.aspx"); // 
                else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                    Response.SetCookie(_cookieActual);
                _controlador = new ControladorPEUL();
                List<object> registro = new List<object>(); //Lista que guarda los datos del registro
                if (_ddlLaboratorio.SelectedIndex != 0)     //Revisa que se haya indicado el laboratorio en el cual se va a realizar el registro
                {
                    registro.Add(_laboratorios[_ddlLaboratorio.SelectedIndex -1][0]);
                    
                        registro.Add(_estados[_ddlEstado.SelectedIndex][0]);
                        if ((!_txtCantidadUsuarios.Text.Equals("")) && (!_txtCantidadLaptops.Text.Equals("")))  //Revisa que se hayan indicado la cantidad de usuarios y laptops
                        {
                            registro.Add(_txtCantidadUsuarios.Text);
                            registro.Add(_txtCantidadLaptops.Text);
                            registro.Add(_txtComentario.Text);
                            registro.Add(_sesion.obtenerLoginUsuario(Request.Cookies["PS"]));
                            _usoLaboratorios = _controlador.registrarPeul(registro);                            
                            //Limpia los campos
                            _ddlLaboratorio.SelectedIndex = 0;
                            _ddlEstado.SelectedIndex = 0;
                            _txtCantidadLaptops.Text = "";
                            _txtCantidadUsuarios.Text = "";
                            _txtComentario.Text = "";
                            _imgMensaje.Visible = false;
                            _lblMensaje.Visible = false;
							if (_usoLaboratorios != null)     // Revisa que se hayan obtenido los datos del uso de los laboratorios
								cargarDatos(_usoLaboratorios);
							else
							{
								_imgMensaje.ImageUrl = "~/Imagenes/Error.png";
								_lblMensaje.Text = "Se ha producido un error al obtener los datos del uso de los laboratorios.";
								_imgMensaje.Visible = true;
								_lblMensaje.Visible = true;
							}
                        }
                        else
                        {
                            _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                            _lblMensaje.Text = "Tiene que indicar la cantidad de usuarios en las computadoras y laptops";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                        }
                   
                }
                else
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                    _lblMensaje.Text = "Tiene que especificar primero un laboratorio";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }
            catch (Exception ex)
            {
                _controlador = new ControladorPEUL();
                _controlador.insertarBitacoraError(ex.ToString(), "");
            }
        }

        /// <summary>
        /// Método que se encarga de cargar los datos en el grid y en el grafico
        /// </summary>
        /// <param name="datosUso">Recibe una lista de listas de objetos con todos los datos del uso de los laboratorios</param>
       
        private void cargarDatos (List<List<object>> datosUso)
        {
            try
            {
                // Crear el data table utilizado por el grafico
                _dtGrafico = new DataTable("Datos");
                _dtGrafico.Columns.Add(new DataColumn("Computadoras"));
                _dtGrafico.Columns.Add(new DataColumn("Laptops"));
                _dtGrafico.Columns.Add(new DataColumn("Laboratorio"));

                //Crear el datatable utilizado por el grid
                _dtDatosUso = new DataTable("Datos");
                _dtDatosUso.Columns.Add(new DataColumn("Laboratorio"));
                _dtDatosUso.Columns.Add(new DataColumn("Operador"));
                _dtDatosUso.Columns.Add(new DataColumn("UltimoRegistro"));
                _dtDatosUso.Columns.Add(new DataColumn("Porcentaje"));
                _dtDatosUso.Columns.Add(new DataColumn("Comentario"));

                // Cargar los datatable que utiliza el grid con los datos del uso de los laboratorios y el datatable que utiliza el grafico
                foreach (List<object> datos in datosUso)
                {
                    //Llenar datatable utilizado por el grafico
                    _dtGrafico.Rows.Add(datos[4], datos[5], datos[0]);
                    //Llenar datatable utilizado por el grid
                    _dtDatosUso.Rows.Add(datos[0], datos[1], datos[2], Convert.ToInt32(datos[6]) + "%", datos[3]);
                }            
                //Cargar los datos de la base en el grid
                _gridUsoLaboratorio.DataSource = _dtDatosUso;
                _gridUsoLaboratorio.DataBind();

                //Cargar los datos de la base en el grafico y definir ejes X y Y
                _chtUsoLaboratorio.DataSource = _dtGrafico;
                _chtUsoLaboratorio.Series["Computadoras"].XValueMember = "Laboratorio";
                _chtUsoLaboratorio.Series["Computadoras"].YValueMembers = "Computadoras";
                _chtUsoLaboratorio.Series["Laptops"].XValueMember = "Laboratorio";
                _chtUsoLaboratorio.Series["Laptops"].YValueMembers = "Laptops";
                _chtUsoLaboratorio.DataBind();
            }
            catch (Exception ex)
            {
                _controlador = new ControladorPEUL();
                _controlador.insertarBitacoraError(ex.ToString(), "");
            }
        }

        /// <summary>
        /// Método que se encarga de actualizar los datos del grafico y de la tabla
        /// </summary>
        
        private void actualizarDatos()
        {
            try
            {
                _controlador = new ControladorPEUL();
                _controlador = new ControladorPEUL();
                _usoLaboratorios = _controlador.calcularUsoActual();    //Obtener los datos del uso de los laboratorios
                if (_usoLaboratorios.Count > 0)     //Revisa que se hayan obtenido los datos del uso de los laboratorios
                    cargarDatos(_usoLaboratorios);
                else
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Se ha producido un error al obtener los datos del uso de los laboratorios.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }
            catch (Exception ex)
            {
                _controlador = new ControladorPEUL();
                _controlador.insertarBitacoraError(ex.ToString(), "");
            }
        }


        protected void _tmReloj_Tick(object sender, EventArgs e)
        {
            actualizarDatos();  //Llamar a actualizar el gridview y el grafico
        }
 
    }
}