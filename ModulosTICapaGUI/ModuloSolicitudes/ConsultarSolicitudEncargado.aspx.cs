using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using ModulosTICapaLogica.Compartido;

namespace ModulosTICapaGUI.Compartido
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        #region Atributos

        private HttpCookie _cookieActual;
        private Sesion _sesion;
        private static ControladorSolicitudCSE _controlador;

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

                String grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
                if ((grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")) || (grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("operadores")) || (grupoUsuario.Equals("soporte")) || (grupoUsuario.Equals("jefes"))) // En caso de que usuarios que no puedan ingresar a ésta área intenten hacerlo
                {
                    Notificacion notificacion = new Notificacion();
                    notificacion.enviarCorreo("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(_cookieActual) + " a la página de CrearReservacion.aspx", "soporte@ic-itcr.ac.cr", "Violación de Seguridad");
                    Response.Redirect("../Compartido/AccesoDenegado.aspx");
                }

                _controlador = new ControladorSolicitudCSE();

                //Se agregan los datos recuperados al DropDownList _ddlUsuario
                _ddlUsuario.DataSource = _controlador.crearDataViewUsuarios();
                _ddlUsuario.DataTextField = "usuario";
                _ddlUsuario.DataValueField = "login";
                _ddlUsuario.DataBind();
                _ddlUsuario.SelectedIndex = 0;

                //Se agregan los datos recuperados al DropDownList _ddlEstado
                _ddlEstado.DataSource = _controlador.crearDataViewEstados();
                _ddlEstado.DataTextField = "estado";
                _ddlEstado.DataValueField = "id";
                _ddlEstado.DataBind();  //Enlazar los datos con el control
                _ddlEstado.SelectedIndex = 0;
                _upEstado.Update();

                //Se agregan los datos recuperados al DropDownList _ddlSoporte
                _ddlSoporte.DataSource = _controlador.crearDataViewSoporte();
                _ddlSoporte.DataTextField = "soportista";
                _ddlSoporte.DataValueField = "login";
                _ddlSoporte.DataBind();
                _ddlSoporte.SelectedIndex = 0;
                _upSoporte.Update();
            }
        }


        public void ddlUsuarioSelection_Change(Object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            if (_ddlUsuario.SelectedIndex != 0)
            {
                _ddlSolicitud.Items.Clear();
                _ddlSolicitud.DataSource = _controlador.crearDataViewSolicitudes(_ddlUsuario.SelectedValue);
                _ddlSolicitud.DataTextField = "asunto";
                _ddlSolicitud.DataValueField = "id";
                _ddlSolicitud.DataBind();
                _ddlSolicitud.SelectedIndex = 0;
                //Se le indica al panel que se actualice
                _upSolicitud.Update();

				//Se limpian los campos
				_taDescripcion.InnerText = "";
				_taAvance.InnerText = "";
				_lblPostBy.Text = "";
				_lblFechaSolicitud.Text = "";
				_ddlFechaAvance.SelectedIndex = 0;
				_ddlEstado.SelectedIndex = 0;
				_ddlSoporte.SelectedIndex = 0;

				//Se actualizan los paneles
				_upDescripcion.Update();
				_upAvance.Update();
				_upPostBy.Update();
				_upFechaSolicitud.Update();
				_upFechaAvance.Update();
				_upEstado.Update();
				_upSoporte.Update();
            }
        }

        public void ddlSolicitudSelection_Change(Object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            if (_ddlSolicitud.SelectedIndex != 0)
            {
                int id = int.Parse(_ddlSolicitud.SelectedValue);
                //Actualiza _taDescripcion con la descripción de la solicitud seleccionada
                _taDescripcion.InnerText = _controlador.obtenerDescripcion(id);
                _upDescripcion.Update();

                //Selecciona el estado de la solicitud en el DropDownList _ddlEstado
                seleccionarEstado(_controlador.obtenerEstado(id));

                //Actualiza _lblPostBy con el postby de la solicitud
                _lblPostBy.Text = _controlador.obtenerPostBy(id);
                _upPostBy.Update();

                //Actualiza _lblFechaSolicitud con la fecha de solicitud
                _lblFechaSolicitud.Text = _controlador.obtenerFechaSolicitud(id);
                _upFechaSolicitud.Update();

                //Selecciona el encarado de la solicitud actual en el DropDownList _ddlSoporte
                seleccionarSoportista(_controlador.obtenerSoportista(id));

                //Se carga el DropDownList de los avances
                _ddlFechaAvance.DataSource = _controlador.crearDataViewAvances(id);
                _ddlFechaAvance.DataTextField = "fecha";
                _ddlFechaAvance.DataValueField = "id";

				//Se limpia el campo de texto de los avances.
				_taAvance.InnerText = "";

                //Se enlazan los datos al control
                _ddlFechaAvance.DataBind();
                _ddlFechaAvance.SelectedIndex = 0;
                _upFechaAvance.Update();
				_upAvance.Update();
            }
        }

        /// <summary>
        /// Carga la descripción del avance cuando se selecciona la fecha del DropDownList
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ddlFechaAvanceSelection_Change(Object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            if (_ddlFechaAvance.SelectedIndex != 0)
            {
                int id = int.Parse(_ddlFechaAvance.SelectedValue);

                _taAvance.InnerText = _controlador.obtenerDescripcionAvance(id);
                _upAvance.Update();
            }
        }

        /// <summary>
        /// Selecciona del DropDownList _ddlEstado el estado de la solicitud actual
        /// </summary>
        /// <param name="estado">Estado de la solicitud actual</param>

        private void seleccionarEstado(String estado)
        {
            int contador = 0;

            foreach (ListItem li in _ddlEstado.Items)
            {
                if (li.Text == estado)
                {
                    _ddlEstado.SelectedIndex = contador;
                    _upEstado.Update();
                }
                else
                    contador++;
            }
        }

        /// <summary>
        /// Selecciona del DropDownList _ddlSoporte el soportista encargado de la solicitud actual
        /// </summary>
        /// <param name="soportista">Encargado de la solicitud actual</param>
        /// 

        private void seleccionarSoportista(String soportista)
        {
            int contador = 0;

            foreach (ListItem li in _ddlSoporte.Items)
            {
                if (li.Value == soportista)
                {
                    _ddlSoporte.SelectedIndex = contador;
                    _upSoporte.Update();
                }
                else
                    contador++;
            }
        }

        /// <summary>
        /// Método OnClick del botón _btnCambiar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void btnCambiar_Click(Object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            int _idSolicitud = int.Parse(_ddlSolicitud.SelectedValue);
            int _idEstado = int.Parse(_ddlEstado.SelectedValue);
            _controlador.cambiarEstado(_idSolicitud, _idEstado, _ddlEstado.SelectedItem.Text, _sesion.obtenerLoginUsuario(_cookieActual));
        }
        
        /// <summary>
        /// Método OnClick del botón _btnCambiar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void btnAsignado_Click(Object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            int idSolicitud = int.Parse(_ddlSolicitud.SelectedValue);
            String encargado = _ddlSoporte.SelectedValue;
            _controlador.cambiarEncargado(idSolicitud, encargado, _sesion.obtenerLoginUsuario(_cookieActual));
        }

        /// <summary>
        /// Método OnClick del botón _btnAgregarDescripción.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void btnAgregarDescripcion_Click(Object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login.
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración.
                Response.SetCookie(_cookieActual);

            if (_ddlSolicitud.SelectedIndex != 0)
            {
                int idSolicitud = int.Parse(_ddlSolicitud.SelectedValue);
                _controlador.agregarDescripcion(idSolicitud, _taDescripcion.InnerText, _taAgregarDescripcion.InnerText, _sesion.obtenerLoginUsuario(_cookieActual));

                //Se limpia el campo.
                _taAgregarDescripcion.InnerText = "";
                //Se actualiza el panel.
                _upAgregarDescripcion.Update();

				//Se obtiene la nueva descripción.
				_taDescripcion.InnerText = _controlador.obtenerDescripcion(idSolicitud);
				//Se actualiza el panel.
				_upDescripcion.Update();

            }
            else
            {
                _lblErrorAgregarDescripcion.Visible = true;
                _upErrorAgregarDescripcion.Update();
            }
        }
    }
}