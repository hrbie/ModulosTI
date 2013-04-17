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
    public partial class WebForm3 : System.Web.UI.Page
    {
        #region Atributos

        private HttpCookie _cookieActual;
        private Sesion _sesion;
        private static ControladorSolicitudSS _controlador;

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
                if ((grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")) || (grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("operadores")) || (grupoUsuario.Equals("jefeti")) || (grupoUsuario.Equals("jefes"))) // En caso de que usuarios que no puedan ingresar a ésta área intenten hacerlo
                {
                    Notificacion notificacion = new Notificacion();
                    notificacion.enviarCorreo("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(_cookieActual) + " a la página de CrearReservacion.aspx", "soporte@ic-itcr.ac.cr", "Violación de Seguridad");
                    Response.Redirect("../Compartido/AccesoDenegado.aspx");
                }

                _controlador = new ControladorSolicitudSS(_sesion.obtenerLoginUsuario(_cookieActual));

                //Agregar las solicitudes al DropDownList
                _ddlSolicitud.DataSource = _controlador.crearDataViewSolicitudes();
                _ddlSolicitud.DataTextField = "asunto";
                _ddlSolicitud.DataValueField = "id";
                //Se enlazan los datos al control
                _ddlSolicitud.DataBind();
                _ddlSolicitud.SelectedIndex = 0;
                _upSolicitud.Update();


                //Agregar los estados de la solicitud al DropDownList
                _ddlEstado.DataSource = _controlador.crearDataViewEstados();
                _ddlEstado.DataTextField = "estado";
                _ddlEstado.DataValueField = "id";
                _ddlEstado.DataBind();
                _ddlEstado.SelectedIndex = 0;
                _upEstado.Update();

            }
        }

        /// <summary>
        /// Carga los datos de la solicitud cuando es seleccionada en el DropDownList _ddlSolicitud
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

            if (_ddlSolicitud.SelectedIndex != 0)
            {
                int id = int.Parse(_ddlSolicitud.SelectedValue);

                //Se obtienen y asignan los valores de la solicitud
                _taDescripcion.InnerText = _controlador.obtenerDescripcion(id);   
                _lblFechaSolicitud.Text = _controlador.obtenerFechaSolicitud(id);
                _lblFechaSolicitud.Visible = true;
                _lblFechaFin.Text = _controlador.obtenerFechaFin(id);
                _lblFechaFin.Visible = true;
                _lblPostBy.Text = _controlador.obtenerPostBy(id);
                _lblPostBy.Visible = true;
                _lblSolicitante.Text = _controlador.obtenerSolicitante(id);
                _lblSolicitante.Visible = true;
                seleccionarEstado(_controlador.obtenerEstado(id));

                //Se carga el DropDownList de los avances
                _ddlFechaAvance.DataSource = _controlador.crearDataViewAvances(id);
                _ddlFechaAvance.DataTextField = "fecha";
                _ddlFechaAvance.DataValueField = "id";
                //Se enlazan los datos al control
                _ddlFechaAvance.DataBind();
                _ddlFechaAvance.SelectedIndex = 0;
                
                //Se actualizan los paneles
                _upDescripcion.Update();    
                _upFechaSolicitud.Update();
                _upFechaFin.Update();
                _upPostBy.Update();
                _upSolicitante.Update();
                _upFechaAvance.Update();
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
        /// Método OnClick del botón _btnAvance
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void btnAvance_Click(Object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            int idSolicitud = int.Parse(_ddlSolicitud.SelectedValue);
            String descripcion = _taNuevoAvance.InnerText;

            _controlador.agregarAvance(idSolicitud, descripcion, _sesion.obtenerLoginUsuario(_cookieActual));

            //Se agrega a _ddlFechaAvance el nuevo avance.
            _ddlFechaAvance.Items.Add(new ListItem(Convert.ToString(DateTime.Now), "0"));

            //Se limpia el campo.
            _taNuevoAvance.InnerText = "";

            //Se actualizan los paneles.
            _upFechaAvance.Update();
            _upNuevoAvance.Update();
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

            int idSolicitud = int.Parse(_ddlSolicitud.SelectedValue);
            int idEstado = int.Parse(_ddlEstado.SelectedValue);
            _controlador.cambiarEstado(idSolicitud, idEstado, _ddlEstado.SelectedItem.Text, _sesion.obtenerLoginUsuario(_cookieActual));
        }
    }
}