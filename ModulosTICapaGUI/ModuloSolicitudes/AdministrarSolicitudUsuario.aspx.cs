using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;

namespace ModulosTICapaGUI.Compartido
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        #region Atributos

        private static ControladorSolicitudASU _controlador;
        private HttpCookie _cookieActual;
        private Sesion _sesion;

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

                _controlador = new ControladorSolicitudASU(_sesion.obtenerLoginUsuario(_cookieActual));
                                
                _ddlSolicitud.DataSource = _controlador.crearDataViewSolicitudes();
                _ddlSolicitud.DataTextField = "solicitud";
                _ddlSolicitud.DataValueField = "id";
                //Se enlazan los datos al control
                _ddlSolicitud.DataBind();
                _ddlSolicitud.SelectedIndex = 0;

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
                _lblResponsable.Text = _controlador.obtenerResponsable(id);
                _lblEstado.Text = _controlador.obtenerEstado(id);
                _lblFechaSolicitud.Text = _controlador.obtenerFechaSolicitud(id);
                _lblFechaFin.Text = _controlador.obtenerFechaFin(id);

                //Se carga el DropDownList de los avances
                _ddlFechaAvance.DataSource = _controlador.crearDataViewAvances(id);
                _ddlFechaAvance.DataTextField = "fecha";
                _ddlFechaAvance.DataValueField = "id";
                //Se enlazan los datos al control
                _ddlFechaAvance.DataBind();
                _ddlFechaAvance.SelectedIndex = 0;
				_taAvance.InnerText = "";
				

                //Se actualizan los paneles
                _upDescripcion.Update();
                _upResponsable.Update();
                _upEstado.Update();
                _upFechaSolicitud.Update();
                _upFechaFin.Update();
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
    }
}