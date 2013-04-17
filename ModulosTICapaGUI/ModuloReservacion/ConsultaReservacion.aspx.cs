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
using ModulosTIControlador;

namespace ModulosTICapaGUI.ModuloReservacion
{
	public partial class ConsultaReservacion : System.Web.UI.Page
	{


        #region Atributos

        private Sesion _sesion;
        private HttpCookie _cookieActual;
        private ControladorReservacionCR _controlador; // Controlador de la Interfaz
        private static List<List<object>> _lugares; // Lista de lugares que será cargada en el _ddlLugar
        private DataTable _tablaReportes;

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
                _lugares = _controlador.obtenerLugares();
                if (_lugares != null) // Llenar _ddlLugar si se encontraton datos 
                {
                   // _ddlLugar.Items.Clear();
                    _ddlLugar.Items.Add("Seleccionar");
                    for (int i = 0; i < _lugares.Count; i++)
                        _ddlLugar.Items.Add(_lugares.ElementAt(i).ElementAt(1).ToString()); // Obtiene los nombres de los lugares encontrados
                }
                else // No se encontraron lugares
                {
                    _ddlLugar.Items.Clear();
                    _ddlLugar.Items.Add("No hay lugares disponibles");
                    _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                    _lblMensaje.Text = "Se ha producido un error al obtener los lugares.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            
            }
        }

        protected void _gridHorario_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Método que controla el evento de consultar una reservación
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _btnConsultar_Click(object sender, EventArgs e)
        {
            _controlador = new ControladorReservacionCR();
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            //// Verificar que todos los campos del formulario han sido especificados por el usuario
            if ((_txtFechaInicio.Text.Equals("")) || (_txtFechaFinal.Text.Equals("")) ||  (_ddlLugar.SelectedIndex == 0))
            {
                _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensaje.Text = "Debe especificar todos los campos del formulario";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
                //reservar = false;
            } // La fechaFinal no sea menor a la fechaInicio
            else if ((Convert.ToDateTime(_txtFechaInicio.Text) > Convert.ToDateTime(_txtFechaFinal.Text)))
            {
                _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensaje.Text = "La fecha final no puede ser menor a la inicial";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }
            else  // Enviar al controlador de Reservación la información para que pueda ser creada
            {
                _tablaReportes = _controlador.consultarReservacion(_lugares.ElementAt(_ddlLugar.SelectedIndex - 1).ElementAt(0).ToString(), _txtFechaInicio.Text, _txtFechaFinal.Text);
                if (_tablaReportes == null)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Ha habido un error al encontrar las reservaciones del período.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
                else if (_tablaReportes.Rows.Count == 0)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                    _lblMensaje.Text = "No hay reservaciones en ese período.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    _gridHorario.Visible = false;
                }
                else // Llenar la tabla
                {
                    _imgMensaje.Visible = false;
                    _lblMensaje.Visible = false;
                    _gridHorario.Visible = true;
                    _gridHorario.DataSource = _tablaReportes;
                    _gridHorario.DataBind();
                }

            }
        }
    #endregion Métodos
	}
}