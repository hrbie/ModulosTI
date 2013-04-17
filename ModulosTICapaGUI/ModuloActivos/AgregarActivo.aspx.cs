using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using ModulosTICapaGUI.Compartido;
using ModulosTICapaLogica.Compartido;

namespace ModulosTICapaGUI.ModuloActivos
{
    public partial class AgregarActivo : System.Web.UI.Page
    {

        #region 

        private Sesion _sesion;
        private HttpCookie _cookieActual;
        private ControladorActivo _controlador; // Controlador de la Interfaz
        private static List<List<object>> _tipos; // Lista de tipos de activos
        private int _errorDatos; // Para manejo de errores en el llenado de la informacion por parte del usuario
        // 0: No lleno el codigo
        // 1: No lleno la descripcion
        // 2: No escogio un tipo
        // 3: No se lleno codigo ni descripcion
        // 4: No hay nada en el formulario

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
                _tipos = _controlador.obtenerTipoActivo();
                if (_tipos != null) // Llenar _ddlTipo si se encontraton datos 
                {
                    // _ddlLugar.Items.Clear();
                    _ddlTipo.Items.Add("Seleccionar");
                    for (int i = 0; i < _tipos.Count; i++)
                        _ddlTipo.Items.Add(_tipos.ElementAt(i).ElementAt(1).ToString()); // Obtiene los nombres de los tipos encontrados
                }
                else // No se encontraron lugares
                {
                    _ddlTipo.Items.Clear();
                    _ddlTipo.Items.Add("No hay tipos de activo");
                    _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                    _lblMensaje.Text = "Se ha producido un error al obtener los tipos.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }

        }

        /// <summary>
        /// Método que controla el evento de insertar un activo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _btnGuardar_Click(object sender, EventArgs e)
        {
            bool ingreso = false;
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            // Verificar que todos los campos del formulario han sido especificados por el usuario
            _errorDatos = -1;
            if (_txtCodigo.Text.Equals(""))
                _errorDatos = 0;
            else if ((_txtDescripcion.Text.Equals("")))
                _errorDatos = 1;
            else if ((_ddlTipo.SelectedIndex == 0))
                _errorDatos = 2;
            else
                ingreso = true;

            switch (_errorDatos)
            {
                case 0:
                    {
                        _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                        _lblMensaje.Text = "Debe especificar el codigo del activo";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        ingreso = false;
                        break;
                    }
                case 1:
                    {
                        _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                        _lblMensaje.Text = "Debe especificar la descripción del activo";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        ingreso = false;
                        break;
                    }
                case 2:
                    {
                        _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                        _lblMensaje.Text = "Debe seleccionar el tipo del activo";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        ingreso = false;
                        break;
                    }
                case 3:
                    {
                        _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                        _lblMensaje.Text = "Debe especificar el código y la descripción del activo";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        ingreso = false;
                        break;
                    }
                case 4:
                    {
                        _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                        _lblMensaje.Text = "Debe ingresar toda la información solicitada";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        ingreso = false;
                        break;
                    }
            }
            
            if (ingreso)
            { 
                List<string> datos = new List<string>();
                datos.Add(_tipos.ElementAt(_ddlTipo.SelectedIndex - 1).ElementAt(0).ToString());
                datos.Add(_txtCodigo.Text);
                datos.Add(_txtDescripcion.Text);
                String login =_sesion.obtenerLoginUsuario(_cookieActual);
                _controlador = new ControladorActivo();
                int resultado = _controlador.insertarActivo(datos,login);
                if (resultado == 1) // Si tuvo éxito
                {
                    _imgMensaje.ImageUrl = "../Imagenes/ok.png";
                    _lblMensaje.Text = "La insersion del activo se realizo con exito";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true; 
                    _ddlTipo.SelectedIndex = 0;
                    _txtDescripcion.Text = "";
                    _txtCodigo.Text = "";
                }
                else if (resultado == 0) // Si el codigo ya existe
                {
                    _lblMensaje.Text = "El código ingresado ya existe en el sistema";
                    _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
                else // Cualquier otro casoSi hay error al insertar el activo
                {
                    _lblMensaje.Text = "Ha habido un error al insertar el activo";
                    _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }
            
            
        }

    #endregion
    }
}