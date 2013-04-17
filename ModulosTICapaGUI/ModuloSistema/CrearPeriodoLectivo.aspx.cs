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
    public partial class CrearPeriodoLectivo : System.Web.UI.Page
    {
        #region Atributos

        private ControladorSistema _controladorSistema;
        private Sesion _sesion;
        private HttpCookie _cookieActual;

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
                _controladorSistema = new ControladorSistema();
                String grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
                if ((grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")) ||
                        (grupoUsuario.Equals("jefes")) || (grupoUsuario.Equals("operadores"))) // Reportar si un usuario autenticado intenta hacer ingreso a una página que no tiene permiso
                {
                    Notificacion notificacion = new Notificacion();
                    notificacion.enviarCorreo("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(_cookieActual) + " a la página de CrearCuenta.aspx", "soporte@ic-itcr.ac.cr", "Violación de Seguridad");
                    Response.Redirect("../Compartido/AccesoDenegado.aspx");
                }
                else
                {
                    
                }
            }
        }

        /// <summary>
        /// Método que controla el evento de crear un periodo lectivo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _btnCrear_Click(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            // Verificar que los campos del formulario estén llenos
            if ((_ddlModalidad.SelectedIndex != 0) && (_ddlPeriodo.SelectedIndex != 0) && (!_txtAnho.Text.Equals("")) && (!_txtFechaInicio.Text.Equals("")) &&
                (!_txtFechaFinal.Text.Equals("")))
            {
                //Verificar que la fecha Inicio es menor que fecha final
                DateTime fechaInicio = Convert.ToDateTime(_txtFechaInicio.Text);
                DateTime fechaFinal = Convert.ToDateTime(_txtFechaFinal.Text);
                if (fechaInicio < fechaFinal)
                {
                    //Verificar que el año ingresado concuerda con el año de la fecha de inicio
                    if (fechaInicio.Year.ToString().Equals(_txtAnho.Text))
                    {
                        //Concatenar nombre del periodo lectivo
                        string nombre = _ddlModalidad.SelectedItem.Text + " " + _ddlPeriodo.SelectedValue + " " + _txtAnho.Text;
                        bool existencia;
                        _controladorSistema = new ControladorSistema();
                        //Verificar si ya existe el nombre del periodo lectivo
                        existencia = _controladorSistema.verificarNombreSemestres(nombre);
                        if (!existencia) //No existe el nombre
                        {
                            int resultado;
                            List<string> datosSemestre = new List<string>();
                            datosSemestre.Add(nombre);
                            datosSemestre.Add(_txtFechaInicio.Text);
                            datosSemestre.Add(_txtFechaFinal.Text);
                            resultado = _controladorSistema.crearSemestre(datosSemestre);
                            if (resultado == 1) //se creo el periodo con exito
                            {
                                _imgMensaje.ImageUrl = "../Imagenes/ok.png";
                                _lblMensaje.Text = "Se ha creado un período lectivo exitosamente";
                                _imgMensaje.Visible = true;
                                _lblMensaje.Visible = true;
                                _txtAnho.Text = "";
                                _txtFechaFinal.Text = "";
                                _txtFechaInicio.Text = "";
                                _ddlModalidad.SelectedIndex = 0;
                                _ddlPeriodo.SelectedIndex = 0;
                            }
                            else //hubo algun tipo de error
                            {
                                _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                                _lblMensaje.Text = "Hubo un error al crear el período lectivo";
                                _imgMensaje.Visible = true;
                                _lblMensaje.Visible = true;
                            }
                        }
                        else //Existe el nombre
                        {
                            _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                            _lblMensaje.Text = "Período lectivo ya existe";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                        }
                    }
                    else //Año no concuerda
                    {
                        _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                        _lblMensaje.Text = "Año no concuerda con la fecha seleccionada";
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
        /// Método que se encarga del manejo de la selección en el _ddlModalidad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void _ddlModalidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            if (_ddlModalidad.SelectedIndex != 0)
            {
                if (_ddlModalidad.SelectedIndex == 1) //si selecciona semestre
                {
                    _ddlPeriodo.Enabled = true;
                    _ddlPeriodo.SelectedIndex = 0;
                    _upPanelPeriodo.Update();
                }
                else //si selecciona verano
                {
                    _ddlPeriodo.Enabled = false;
                    _ddlPeriodo.SelectedIndex = 1;
                    _upPanelPeriodo.Update();
                }
            }
        }
    }
}