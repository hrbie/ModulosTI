using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using System.Globalization;
using ModulosTICapaGUI.Compartido;
using ModulosTICapaLogica.Compartido;

namespace ModulosTICapaGUI.ModuloSolicitudes
{
    public partial class AdministrarSolicitudes : System.Web.UI.Page
    {
        #region Atributos

        private HttpCookie _cookieActual;
        private Sesion _sesion;
        ControladorSolicitudAS _controlador = new ControladorSolicitudAS();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsPostBack) return;
                _sesion = new Sesion();
                _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
                if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                    Response.Redirect("../Autentificacion/Login.aspx"); // 
                else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                    Response.SetCookie(_cookieActual);

                String grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
                if (grupoUsuario.Equals("jefeti")==false) // En caso de que usuarios que no puedan ingresar a ésta área intenten hacerlo
                {
	
                    Notificacion notificacion = new Notificacion();
                    notificacion.enviarCorreo("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(_cookieActual) + " a la página de CrearReservacion.aspx", "soporte@ic-itcr.ac.cr", "Violación de Seguridad");
                    Response.Redirect("../Compartido/AccesoDenegado.aspx");
					
                }
                
                //Se cargan las solicitudes pendientes en el DropDownList _ddlSolicitudes
                _ddlSolicitudes.DataSource = _controlador.crearDataViewPendientes();
                _ddlSolicitudes.DataTextField = "solicitud";
                _ddlSolicitudes.DataValueField = "id";
                //Se enlazan los datos al control
                _ddlSolicitudes.DataBind();
                _ddlSolicitudes.SelectedIndex = 0;


                //Se cargan los soportistas en el DropDownList _ddlSoporte
                _ddlSoporte.DataSource = _controlador.crearDataViewSoporte();
                _ddlSoporte.DataTextField = "soportista";
                _ddlSoporte.DataValueField = "login";
                _ddlSoporte.DataBind();
                _ddlSoporte.SelectedIndex = 0;

            }
        }

        /// <summary>
        /// Actualiza la información de la página según la solicitud seleccionada en el drop down list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void ddlSolicitudSelection_Change(Object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            if (_ddlSolicitudes.SelectedIndex != 0)
            {
                int id = int.Parse(_ddlSolicitudes.SelectedValue);
                _lblUsuario.Text = _controlador.obtenerSolicitante(id);
                _taDescripcion.InnerText = _controlador.obtenerDescripcion(id);
                _lblPostBy.Text = _controlador.obtenerPostBy(id);
                _lblFechaSolicitud.Text = _controlador.obtenerFechaSolicitud(id);


                //Se actualizan los paneles.
                _upDescripcon.Update();
                _upUsuario.Update();
                _upPosBy.Update();
                _upFechaSolicitud.Update();
            }
        }

        /// <summary>
        /// Método OnClick del botón _btnAsignar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void btnAsignar_Click(Object sender, EventArgs e) 
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            int _idSolicitud = int.Parse(_ddlSolicitudes.SelectedValue);
            String _responsable = _ddlSoporte.SelectedValue;
            String fechaFin = _txtFechaFinal.Text;
            DateTimeFormatInfo _dtfiFechaFin = new DateTimeFormatInfo();
            _dtfiFechaFin.ShortDatePattern = "dd-MM-yyyy";
            _dtfiFechaFin.DateSeparator = "/";
			DateTime _fechaFinalizacion = DateTime.Parse(fechaFin, _dtfiFechaFin);
            //DateTime _fechaFinalizacion = Convert.ToDateTime(_fechaFin, _dtfiFechaFin);
            _controlador.asignar(_idSolicitud, _responsable, _fechaFinalizacion, _sesion.obtenerLoginUsuario(_cookieActual));

            //Se remueve la solicitud de _ddlSolicitudes.
            _ddlSolicitudes.Items.RemoveAt(_ddlSolicitudes.SelectedIndex);

            //Se limpian los campos.
            _ddlSolicitudes.SelectedIndex = 0;
            _ddlSoporte.SelectedIndex = 0;
            _taDescripcion.InnerText = "";
            _txtFechaFinal.Text = "";
            _lblUsuario.Text = "";
            _lblPostBy.Text = "";
            _lblFechaSolicitud.Text = "";

            //Se actualizan los paneles.
            _upSolicitudes.Update();
            _upSoporte.Update();
            _upDescripcon.Update();
            _upUsuario.Update();
            _upPosBy.Update();
            _upFechaSolicitud.Update();
            _upFechaFinal.Update();

        }

        /// <summary>
        /// Método OnClick del botón _btnCancelar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void btnCancelar_Click(Object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            if (_ddlSolicitudes.SelectedIndex != 0)
            {
                int _idSolicitud = int.Parse(_ddlSolicitudes.SelectedValue);
                
                _controlador.cancelar(_idSolicitud, _sesion.obtenerLoginUsuario(_cookieActual));
                
                //Se remueve la solicitud de _ddlSolicitudes.
                _ddlSolicitudes.Items.RemoveAt(_ddlSolicitudes.SelectedIndex);

                //Se limpian los campos.
                _ddlSolicitudes.SelectedIndex = 0;
                _ddlSoporte.SelectedIndex = 0;
                _taDescripcion.InnerText = "";
                _txtFechaFinal.Text = "";
                _lblUsuario.Text = "";
                _lblPostBy.Text = "";
                _lblFechaSolicitud.Text = "";

                //Se actualizan los paneles.
                _upSolicitudes.Update();
                _upSoporte.Update();
                _upDescripcon.Update();
                _upUsuario.Update();
                _upPosBy.Update();
                _upFechaSolicitud.Update();
                _upFechaFinal.Update();
            }
        }


        /// <summary>
        /// Método OnClick del botón _btnAgregarDescripción
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void btnAgregarDescripcion_Click(Object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            if (_ddlSolicitudes.SelectedIndex != 0)
            {
                int idSolicitud = int.Parse(_ddlSolicitudes.SelectedValue);
                _controlador.agregarDescripcion(idSolicitud, _taDescripcion.InnerText, _taAgregarDescripcion.InnerText, _sesion.obtenerLoginUsuario(_cookieActual));

                //Se liempia el campo.
                _taAgregarDescripcion.InnerText = "";

                //Se actualiza el panel
                _upAgregarDescripcion.Update();
            }
            else
            {
                _lblErrorAgregarDescripcion.Visible = true;
                _upErrorAgregarDescripcion.Update();
            }
        }

    }
}