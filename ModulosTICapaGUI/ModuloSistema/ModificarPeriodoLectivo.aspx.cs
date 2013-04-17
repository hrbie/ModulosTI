using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using ModulosTICapaGUI.Compartido;
using ModulosTICapaLogica.Compartido;
using System.IO;
using System.Text.RegularExpressions;
using ModulosTIControlador;


namespace ModulosTICapaGUI.ModuloSistema
{
    public partial class ModificarPeriodoLectivo : System.Web.UI.Page
    {
        private ControladorSistema _controladorSistema;
        private Sesion _sesion;
        private HttpCookie _cookieActual;
        private static List<Semestre> _listaSemestre;

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
                _controladorSistema = new ControladorSistema();
                String grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
                if ((grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")) ||
                        (grupoUsuario.Equals("jefes")) || (grupoUsuario.Equals("operadores"))) // Reportar si un usuario autenticado intenta hacer ingreso a una página que no tiene permiso
                {
                    Notificacion notificacion = new Notificacion();
                    notificacion.enviarCorreo("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(_cookieActual) + " a la página de CrearCuenta.aspx", "soporte@ic-itcr.ac.cr", "Violación de Seguridad");
                    Response.Redirect("../Compartido/AccesoDenegado.aspx");
                }
                _ObtenerDatos();
            }
        }

        /// <summary>
        /// Método que obtiene los datos de los periodos lectivos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void _ObtenerDatos()
        {
            _listaSemestre = _controladorSistema.obtenerSemestres();
            if ((_listaSemestre != null) && (_listaSemestre.Count != 0))
            {
                _ddlNombre.Items.Add("Seleccionar");
                for (int i = 0; i < _listaSemestre.Count; i++) //guardar
                {
                    _ddlNombre.Items.Add(_listaSemestre.ElementAt(i).NombreSemestre.ToString());
                    _ddlId.Items.Add(_listaSemestre.ElementAt(i).IdSemestre.ToString());
                    _ddlFechaIni.Items.Add(_listaSemestre.ElementAt(i).FechaInicio.ToString());
                    _ddlFechaFin.Items.Add(_listaSemestre.ElementAt(i).FechaFinal.ToString());
                    _ddlActivo.Items.Add(_listaSemestre.ElementAt(i).Activo.ToString());
                }
            }
            else if (_listaSemestre == null) //hubo algun error
            {
                _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                _lblMensaje.Text = "Hubo un error al obtener los datos de los períodos lectivos en el sistema";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }
            else // no hay datos
            {
                _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensaje.Text = "No hay períodos lectivos registrados en el sistema";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }
        }

        /// <summary>
        /// Método que controla el evento de modificar un periodo lectivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _btnModificar_Click(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            // Verificar que los campos del formulario estén llenos
            if ((_ddlNombre.SelectedIndex != 0) && (!_txtFechaInicio.Text.Equals("")) && (!_txtFechaFinal.Text.Equals("")))
            {
                //Verificar que la fecha Inicio es menor que fecha final
                DateTime fechaInicio = Convert.ToDateTime(_txtFechaInicio.Text);
                DateTime fechaFinal = Convert.ToDateTime(_txtFechaFinal.Text);
                if (fechaInicio < fechaFinal)
                {
                        int resultado;
                        _controladorSistema = new ControladorSistema();
                        List<string> datosSemestre = new List<string>();
                        datosSemestre.Add(_ddlId.SelectedItem.Text);
                        datosSemestre.Add(_ddlNombre.SelectedItem.Text);
                        datosSemestre.Add(_txtFechaInicio.Text);
                        datosSemestre.Add(_txtFechaFinal.Text);
                        if (_cbActivo.Checked == true)
                            datosSemestre.Add("1");
                        else
                            datosSemestre.Add("0");
                        resultado = _controladorSistema.modificarSemestre(datosSemestre);
                        if (resultado == 1) //se modifico exitosamente
                        {
                            _imgMensaje.ImageUrl = "../Imagenes/ok.png";
                            _lblMensaje.Text = "Se ha modificado un período lectivo exitosamente";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                            _ddlNombre.Enabled = true;
                            _btnBuscar.Enabled = true;
                            _txtFechaInicio.Text = "";
                            _txtFechaFinal.Text = "";
                            _cbActivo.Checked = false;
                            _imgFechaInicio.Visible = false;
                            _imgFechaFinal.Visible = false;
                            _cbActivo.Enabled = false;
                            _btnCancelar.Enabled = false;
                            _btnModificar.Enabled = false;
                            _ddlNombre.Items.Clear();
                            _ddlId.Items.Clear();
                            _ddlFechaIni.Items.Clear();
                            _ddlFechaFin.Items.Clear();
                            _ddlActivo.Items.Clear();
                            _ObtenerDatos(); 
                        }
                        else //hubo algun error
                        {
                            _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                            _lblMensaje.Text = "Hubo un error al modificar el período lectivo";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                        }
                }
                else //fecha inicio mayor que fecha final
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensaje.Text = "La fecha inicio debe ser menor que la fecha final";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }
            else //hay espacios nulos
            {
                _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensaje.Text = "Debe completar todos los campos del formulario";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }
        }

        /// <summary>
        /// Método que controla el evento de buscar un periodo lectivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _btnBuscar_Click(object sender, EventArgs e)
        {
            if (_ddlNombre.SelectedIndex != 0)
            {
                _ddlNombre.Enabled = false;
                _btnBuscar.Enabled = false;
                _ddlFechaIni.SelectedIndex = _ddlNombre.SelectedIndex - 1;
                _ddlFechaFin.SelectedIndex = _ddlNombre.SelectedIndex - 1;
                _ddlActivo.SelectedIndex = _ddlNombre.SelectedIndex - 1;
                _ddlId.SelectedIndex = _ddlNombre.SelectedIndex - 1;
                _txtFechaInicio.Text = _ddlFechaIni.SelectedItem.Text;
                _txtFechaFinal.Text = _ddlFechaFin.SelectedItem.Text;
                if (_ddlActivo.SelectedItem.Text.Equals("1"))
                {
                    _cbActivo.Checked = true;
                }
                _imgFechaInicio.Visible = true;
                _imgFechaFinal.Visible = true;
                _cbActivo.Enabled = true;
                _btnCancelar.Enabled = true;
                _btnModificar.Enabled = true;
                _imgMensaje.Visible = false;
                _lblMensaje.Visible = false;
                _imgMensaje1.Visible = false;
                _lblMensaje1.Visible = false;
            }
            else
            {
                _imgMensaje1.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensaje1.Text = "Seleccione el período lectivo a buscar";
                _imgMensaje1.Visible = true;
                _lblMensaje1.Visible = true;
            }
        }

        /// <summary>
        /// Método que controla el evento de cancelar una modificacion de un periodo lectivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {
            _ddlNombre.Enabled = true;
            _btnBuscar.Enabled = true;
            _ddlNombre.SelectedIndex = 0;
            _ddlFechaIni.SelectedIndex = 0;
            _ddlFechaFin.SelectedIndex = 0;
            _ddlActivo.SelectedIndex = 0;
            _ddlId.SelectedIndex = 0;
            _txtFechaInicio.Text = "";
            _txtFechaFinal.Text = "";
            _cbActivo.Checked = false;
            _imgFechaInicio.Visible = false;
            _imgFechaFinal.Visible = false;
            _cbActivo.Enabled = false;
            _btnCancelar.Enabled = false;
            _btnModificar.Enabled = false;
        }
    }
}