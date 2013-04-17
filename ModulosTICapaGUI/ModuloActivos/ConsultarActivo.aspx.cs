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

namespace ModulosTICapaGUI.ModuloActivos
{
    public partial class ConsultarActivo : System.Web.UI.Page
    {

        #region Atributos

        private Sesion _sesion;
        private HttpCookie _cookieActual;
        private ControladorActivo _controlador; // Controlador de la Interfaz
        //private static List<List<object>> _tipos; // Lista de tipos de activos
        private static List<List<object>> _estados; // Lista de estado de los activos
        private DataTable _tablaReportes;
        private int _errorCarga; // Para manejo de errores en el tiempo de carga de la página
                                // 0: No se cargaron los tipos
                                // 1: No se cargaron los estados
                                // 2: No se cargaron tanto tipos como estados

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
                _controlador = new ControladorActivo();
                String grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
                if ((grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests"))) // En caso de que usuarios que no puedan ingresar a ésta área intenten hacerlo
                {
                    Notificacion notificacion = new Notificacion();
                    notificacion.enviarCorreo("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(_cookieActual) + " a la página de CrearReservacion.aspx", "soporte@ic-itcr.ac.cr", "Violación de Seguridad");
                    Response.Redirect("../Compartido/AccesoDenegado.aspx");
                }
                //_tipos = _controlador.obtenerTipoActivo();
                _errorCarga = -1;
                //if (_tipos != null) // Llenar _ddlTipo si se encontraton datos 
                //{
                //    // _ddlLugar.Items.Clear();
                //    _ddlTipo.Items.Add("Seleccionar");
                //    for (int i = 0; i < _tipos.Count; i++)
                //        _ddlTipo.Items.Add(_tipos.ElementAt(i).ElementAt(1).ToString()); // Obtiene los nombres de los tipos encontrados
                //}
                //else // No se encontraron lugares
                    //_errorCarga = 0;

                _estados = _controlador.obtenerTipoMovimiento();
                if ((_estados != null) && (_estados.Count != 0)) // Llenar _ddlCarrera si se encontraton datos 
                {
                    _ddlEstado.Items.Add("Seleccionar");
                    for (int i = 0; i < _estados.Count; i++)
                        _ddlEstado.Items.Add(_estados.ElementAt(i).ElementAt(1).ToString()); // Obtiene los nombres de las carreras encontradas
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
                        //_ddlTipo.Items.Clear();
                        //_ddlTipo.Items.Add("No hay tipos de activo");
                        _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                        _lblMensaje.Text = "Se ha producido un error al obtener los tipos.";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        break;
                    case 1:
                        _ddlEstado.Items.Clear();
                        _ddlEstado.Items.Add("No hay estados de activos");
                        _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                        _lblMensaje.Text = "Se ha producido un error al obtener las carreras.";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        break;
                    case 2:
                        _ddlEstado.Items.Add("No hay estados de activos");
                        //_ddlTipo.Items.Clear();
                        //_ddlTipo.Items.Add("No hay tipos de activo");
                        _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                        _lblMensaje.Text = "Ha habido un problema al cargar información en la página.";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        break;
                }


            }

        }


        /// <summary>
        /// Método que controla el evento de consultar una reservación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _btnConsultar_Click(object sender, EventArgs e)
        {
            _controlador = new ControladorActivo();
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            _errorCarga = -1;
            bool bandera = true;
            ////// Verificar que todos los campos del formulario han sido especificados por el usuario
            if (f1.Checked == true)
            {
                if (_txtDescrip.Text != "")
                    _tablaReportes = _controlador.consultarActivoPorDescripcion(_txtDescrip.Text);
                else
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensaje.Text = "Debe especificar la descripcion";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _gridActivos.Visible = false;
                    bandera = false;
                }
            }
            else if (f2.Checked == true)
            {
                if (_txtCodigo.Text != "")
                    _tablaReportes = _controlador.consultarActivoPorCodigo(_txtCodigo.Text);
                else
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensaje.Text = "Debe especificar un código a consultar";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _gridActivos.Visible = false;
                    bandera = false;
                }
            }
            else if (f3.Checked == true)
            {
                if (_ddlEstado.SelectedIndex != 0)
                    _tablaReportes = _controlador.consultarActivoPorEstado(int.Parse(_estados.ElementAt(_ddlEstado.SelectedIndex - 1).ElementAt(0).ToString()));
                else
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensaje.Text = "Debe especificar alguno de los estados de activos de la lista";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _gridActivos.Visible = false;
                    bandera = false;
                }
            }
            else if (f4.Checked == true)
            {
                if (_txtDescripcion.Text != "")
                {
                    //_tablaReportes = _controlador.consultarActivo(_lugares.ElementAt(_ddlLugar.SelectedIndex - 1).ElementAt(0).ToString(), _txtFechaInicio.Text, _txtFechaFinal.Text);
                }
                else
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensaje.Text = "Debe especificar una descripción para la búsqueda";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _gridActivos.Visible = false;
                    bandera = false;
                }
            }
            else
            {
                _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                _lblMensaje.Text = "Seleccione una forma de búsqueda";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
                _gridActivos.Visible = false;
                bandera = false;
            }
            //    _tablaReportes = _controlador.consultarReservacion(_lugares.ElementAt(_ddlLugar.SelectedIndex - 1).ElementAt(0).ToString(), _txtFechaInicio.Text, _txtFechaFinal.Text);
            if(bandera)
                if (_tablaReportes == null)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Ha habido un error al encontrar los activos";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _gridActivos.Visible = false;
                }
                else if (_tablaReportes.Rows.Count == 0)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                    _lblMensaje.Text = "No hay activos que cumplan con el filtro.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _gridActivos.Visible = false;
                }
                else // Llenar la tabla
                {
                    _imgMensaje.Visible = false;
                    _lblMensaje.Visible = false;
                    _gridActivos.Visible = true;
                    _gridActivos.DataSource = _tablaReportes;
                    _gridActivos.DataBind();
                }

            //}
        }


         /// <summary>
        /// Método que controla el evento de check del radio button f1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _rbfiltro1_OnChecked(object sender, EventArgs e)
        {
                _txtDescrip.Enabled = true;
                _ddlEstado.Enabled = false;
                _txtCodigo.Enabled = false;
                _txtDescripcion.Enabled = false;
                _txtDescrip.Text = "";
                _ddlEstado.SelectedIndex = 0;
                _txtCodigo.Text = "";
                _txtDescripcion.Text = "";
                _upPanelBusqueda.Update();
        }
        
        /// <summary>
        /// Método que controla el evento de check del radio button f2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _rbfiltro2_OnChecked(object sender, EventArgs e)
        {
            _txtDescrip.Enabled = false;
            _ddlEstado.Enabled = false;
            _txtCodigo.Enabled = true;
            _txtDescripcion.Enabled = false;
            _txtDescrip.Text = "";
            _ddlEstado.SelectedIndex = 0;
            _txtCodigo.Text = "";
            _txtDescripcion.Text = "";
            _upPanelBusqueda.Update();
            
        }

        /// <summary>
        /// Método que controla el evento de check del radio button f3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _rbfiltro3_OnChecked(object sender, EventArgs e)
        {
            _txtDescrip.Enabled = false;
            _ddlEstado.Enabled = true;
            _txtCodigo.Enabled = false;
            _txtDescripcion.Enabled = false;
            _txtDescrip.Text = "";
            _ddlEstado.SelectedIndex = 0;
            _txtCodigo.Text = "";
            _txtDescripcion.Text = "";
            _upPanelBusqueda.Update();
        }

        /// <summary>
        /// Método que controla el evento de check del radio button f4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _rbfiltro4_OnChecked(object sender, EventArgs e)
        {
            //_ddlTipo.Enabled = false;
            _ddlEstado.Enabled = false;
            _txtCodigo.Enabled = false;
            _txtDescripcion.Enabled = true;
            
        }
        #endregion
    }
}