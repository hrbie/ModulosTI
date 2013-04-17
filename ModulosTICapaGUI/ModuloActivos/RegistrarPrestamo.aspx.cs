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
    public partial class RegistrarPrestamo : System.Web.UI.Page
    {
        #region Atributos

        private Sesion _sesion;
        private HttpCookie _cookieActual;
        private List<List<object>> activo; //guarda los datos del activo con el que se desea realizar un movimiento
        private static List<List<object>> _estados; // Lista de estado de los activos
        private ControladorActivo _controlador; // Controlador de la Interfaz
        #endregion

        #region Metodos

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
                _estados = _controlador.obtenerTipoMovimiento();
                if (_estados != null) // Llenar _ddlEstado si se encontraton datos 
                {
                    _ddlEstado.Items.Add("");
                    for (int i = 0; i < _estados.Count; i++)
                        _ddlEstado.Items.Add(_estados.ElementAt(i).ElementAt(1).ToString()); // Obtiene los nombres de los tipos encontrados
                }
                else // No se encontraron estados
                {
                    _ddlEstado.Items.Clear();
                    _ddlEstado.Items.Add("No hay tipos de movimiento");
                    _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                    _lblMensaje.Text = "Se ha producido un error al obtener los tipos.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
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
            if (_txtCodigo.Text.Equals(""))
            {
                _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensaje.Text = "Debe especificar el código del activo";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
                _lblCMensaje.Visible = false;
                _imgCMensaje.Visible = false;
                   
            }
            else
            {
                activo = _controlador.consultarActivoPorCodigo2(_txtCodigo.Text);
                if (activo == null)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Ha habido un error al encontrar los activos";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _lblCMensaje.Visible = false;
                    _imgCMensaje.Visible = false;
                   
                }
                else if (activo.Count == 0)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                    _lblMensaje.Text = "No existe el activo buscado";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _lblCMensaje.Visible = false;
                    _imgCMensaje.Visible = false;
                   
                }
                else
                {
                    _txtCdescripcion.Text = activo.ElementAt(0).ElementAt(1).ToString();
                    _txtCcodigo.Text = activo.ElementAt(0).ElementAt(0).ToString();
                    _ddlEstado.SelectedValue = activo.ElementAt(0).ElementAt(2).ToString();
                    _txtId.Text = activo.ElementAt(0).ElementAt(7).ToString();
                    if (_ddlEstado.SelectedIndex == 1)
                        _ddlEstado.SelectedIndex = 2;
                    else
                        _ddlEstado.SelectedIndex = 1;
                    _imgMensaje.Visible = false;
                    _lblMensaje.Visible = false;
                    //agregarEstados();
                    _txtClogin.Enabled = true;
                    _txtComentario.Enabled = true;
                    _btnMovimiento.Enabled = true;
                    _btnCancelar.Enabled = true;
                    _btnConsultar.Enabled = false;
                    _txtCodigo.Enabled = false;
                    _lblCMensaje.Visible = false;
                    _imgCMensaje.Visible = false;
                   
                }
            }
        }

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            _btnCancelar.Enabled = false;
            _btnConsultar.Enabled = true;
            _btnMovimiento.Enabled = false;
            _txtCodigo.Enabled = true;
            _txtClogin.Enabled = false;
            _txtComentario.Enabled = false;
            _txtCodigo.Text = "";
            _txtCcodigo.Text = "";
            _txtCdescripcion.Text = "";
            _txtClogin.Text = "";
            _txtComentario.Text = "";
            _ddlEstado.SelectedIndex = 0;
            _imgCMensaje.Visible = false;
            _lblCMensaje.Visible = false;
            _imgMensaje.Visible = false;
            _lblMensaje.Visible = false;
            
        }

        /// <summary>
        /// Método que controla el evento de modificar movimiento de un activo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _btnMovimiento_Click(object sender, EventArgs e)
        {
            _controlador = new ControladorActivo();
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            // Verificar que todos los campos del formulario han sido especificados por el usuario
            if (_txtClogin.Text.Equals(""))
            {
                _imgCMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                _lblCMensaje.Text = "Debe ingresar el login del solicitante del movimiento del activo.";
                _imgCMensaje.Visible = true;
                _lblCMensaje.Visible = true;
            }
            else
            {
                Boolean isProfesor = _controlador.verificarProfesor(_txtClogin.Text);

                if (isProfesor)
                {
                    List<string> activoDatos = new List<string>();
                    //codigo, movimiento, login,postby,comentario
                    activoDatos.Add(_txtId.Text);
                    activoDatos.Add(_estados.ElementAt(_ddlEstado.SelectedIndex - 1).ElementAt(0).ToString());
                    activoDatos.Add(_txtClogin.Text);
                    activoDatos.Add(_sesion.obtenerLoginUsuario(_cookieActual));
                    activoDatos.Add(_txtComentario.Text);
                    int resultado = _controlador.insertarMovimientoActivo(activoDatos);
                    if (resultado == 1) // Si tuvo éxito
                    {
                        _imgCMensaje.ImageUrl = "../Imagenes/ok.png";
                        _lblCMensaje.Text = "El movimiento del activo se realizo con exito";
                        _imgCMensaje.Visible = true;
                        _lblCMensaje.Visible = true;
                        _lblMensaje.Visible = false;
                        _imgMensaje.Visible = false;
                        _ddlEstado.Enabled = false;
                        _txtClogin.Enabled = false;
                        _txtComentario.Enabled = false;
                        _txtCcodigo.Text = "";
                        _txtComentario.Text = "";
                        _txtCdescripcion.Text = "";
                        _ddlEstado.SelectedIndex = 0;
                        _txtCodigo.Text = "";
                        _txtCodigo.Enabled = true;
                        _txtClogin.Text = "";
                        _btnCancelar.Enabled = false;
                        _btnConsultar.Enabled = true;
                        _btnMovimiento.Enabled = false;
                    }
                    else // Si hubo una excepción retorna el error
                    {
                        _lblCMensaje.Text = "Ha habido un error al realizar el movimiento del activo";
                        _imgCMensaje.ImageUrl = "../Imagenes/Error.png";
                        _imgCMensaje.Visible = true;
                        _lblCMensaje.Visible = true;
                    }
                }
                else
                {
                    _lblCMensaje.Text = "El login no corresponde a un profesor";
                        _imgCMensaje.ImageUrl = "../Imagenes/Error.png";
                        _imgCMensaje.Visible = true;
                        _lblCMensaje.Visible = true;
                }
            }

        }



        /// <summary>
        /// Método que llena el ddl de estados
        /// </summary>
        protected void agregarEstados()
        {
            //_estados = _controlador.obtenerEstados();
            if ((_estados != null) && (_estados.Count != 0)) // Llenar _ddlCarrera si se encontraton datos 
            {
                for (int i = 0; i < _estados.Count; i++)
                    _ddlEstado.Items.Add(_estados.ElementAt(i).ElementAt(1).ToString()); // Obtiene los nombres de las carreras encontradas
            }
            else // No se encontraron carreras
            {
                _ddlEstado.Items.Clear();
                _ddlEstado.Items.Add("No hay estados de activos");
                _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                _lblMensaje.Text = "Se ha producido un error al obtener las carreras.";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }

        }

        #endregion

    }
}