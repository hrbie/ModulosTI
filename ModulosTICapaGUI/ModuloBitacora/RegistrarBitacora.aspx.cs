using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTICapaGUI.Compartido;
using ModulosTIControlador.Clases;
using System.Data;

namespace ModulosTICapaGUI.ModuloBitacora
{
    public partial class RegistrarBitacora : System.Web.UI.Page
    {
        #region Atributos

        private static ControladorBitacora _controlador; // Controlador de la Interfaz

        private static List<List<object>> _laboratorios; // Lista de lugares que será cargada en el _ddlLaboratorios
        private HttpCookie _cookieActual;
        private Sesion _sesion;
        private DataTable _eventosSesion = new DataTable();

        readonly DataTable _limpia = new DataTable(); // Tabla en limpio para que el usuario ingrese sus turnos
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) // Solo cargar los datos la primera vez que la página ha sido cargada
            {
                _sesion = new Sesion();
                _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
                if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                {
                    Response.Redirect("../Autentificacion/Login.aspx"); // 
                }
                else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                    Response.SetCookie(_cookieActual);
                _controlador = new ControladorBitacora();
                string grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
                string loginUsuario = _sesion.obtenerLoginUsuario(_cookieActual);
                if ((grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")))
                {
                   _controlador.insertarBitacoraError("Se ha intentado realizar un acceso no permitido por parte del usuario " + loginUsuario + " a la página de GenerarEstadisticaUso.aspx", "");
                    Response.Redirect("../Compartido/AccesoDenegado.aspx");
                }
                _laboratorios = _controlador.obtenerLaboratorios();
                var errores = new int[1];
                errores[0] = 0; //Error al cargar Laboratorios

                if (_laboratorios != null) // Llenar _ddlLaboratorios si se encontraton datos 
                {
                    for (int i = 0; i < _laboratorios.Count; i++)
                    {
                        var item = new ListItem(_laboratorios.ElementAt(i).ElementAt(1).ToString(), Convert.ToString(_laboratorios.ElementAt(i).ElementAt(0)));
                        _ddlLaboratorios.Items.Add(item); // Obtiene los nombres de los laboratorios encontrados
                    }
                }
                else // No se encontraron laboratorios
                    errores[0] = 1;

                if (errores[0] == 1) // Detectar errores 
                {
                    _ddlLaboratorios.Items.Clear();
                    _ddlLaboratorios.Items.Add("No hay laboratorios disponibles");

                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Error al cargar los laboratorios.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;

                }
                iniciarSesion();
                CargarEventosSesion((int)Session["pkSesion"]);

            }
        }

        /// <summary>
        /// Método que inicia la sesion de un usuario para ingresar datos en la bitácora
        /// </summary>
        private void iniciarSesion()
        {
            if (Session["pkSesion"] == null) //Si pk_sesion == 0: Hace falta iniciar una sesion
            {
               _sesion = new Sesion();
               _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
               if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                   Response.Redirect("../Autentificacion/Login.aspx"); // 
               else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
               Response.SetCookie(_cookieActual);
               Session["pkSesion"] = _controlador.insertarSesionBitacora(_sesion.obtenerLoginUsuario(_cookieActual));
                int lolo = (int) Session["pkSesion"];
            }
        }

        protected void _btnRegistrar_Click(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            if ((int)Session["pkSesion"] != 0)
            {
                if (_ddlLaboratorios.SelectedValue != "0")
                {
                    if (_txtComentario.Text != "")
                    {
                        _controlador.insertarEventoBitacora((int)Session["pkSesion"], Convert.ToInt32(_ddlLaboratorios.SelectedValue), _txtComentario.Text, _sesion.obtenerLoginUsuario(_cookieActual));
                        _txtComentario.Text = "";
                        CargarEventosSesion((int)Session["pkSesion"]);
                        _imgMensaje.Visible = false;
                        _lblMensaje.Visible = false;
                    }
                    else
                    {
                        //Error, el comentario no puede estar en blanco
                        _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                        _lblMensaje.Text = "No es ético registrar eventos en blanco";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                    }
                }
                else
                {
                    //Error, debe seleccionar un laboratorio
                    _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                    _lblMensaje.Text = "Seleccione un laboratorio.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }
            else
            {
                //Error, su sesión ya expiró, inicie sesión nuevamente 
                _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                _lblMensaje.Text = "La sesión expiró por inactividad";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }
        }
    
        protected void _gvwEventos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
            {
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            }
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            _gvwEventos.EditIndex = e.NewEditIndex;
            CargarEventosSesion((int)Session["pkSesion"]);
        }

        protected void _gvwEventos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            String pkEntrada = ((Label)_gvwEventos.Rows[e.RowIndex].FindControl("_lblPKEntrada")).Text; // PK del lugar que se está editando puesto en una columna oculta
            String evento = ((TextBox)_gvwEventos.Rows[e.RowIndex].FindControl("_txtEvento")).Text;
            if (!evento.Equals(""))
            {
                int resultado = _controlador.ModificarEvento(_sesion.obtenerLoginUsuario(_cookieActual), (int)Session["pkSesion"], Convert.ToInt32(pkEntrada), evento);
                if (resultado != 0 && resultado != 10 )
                {
                   _imgMensaje.ImageUrl = "~/Imagenes/ok.png";
                    _lblMensaje.Text = "El evento ha sido modificado";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                   
                    _gvwEventos.EditIndex = -1;
                    CargarEventosSesion((int)Session["pkSesion"]);
                }
                else if (resultado == 10)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Ha habido un error al intentar modificar los datos del lugar";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                    CargarEventosSesion((int)Session["pkSesion"]);
                }
                else if(resultado == 0)
                {
                }

            }
        }

        protected void _gvwEventos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
            {
                //Global._usuario = null; HACER CLASE EN CONTROLADOR PARA QUE LO LIMPIE
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            }
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            _gvwEventos.EditIndex = -1;
            CargarEventosSesion((int)Session["pkSesion"]);
           // _upEventos.Update();
        }
        
        private void CargarEventosSesion(int sesionActual)
        {
            if (_ddlLaboratorios.SelectedValue != "0")
            {
                _eventosSesion = _controlador.ConsultarEntradaPorSesion(sesionActual, Convert.ToInt32(_ddlLaboratorios.SelectedValue));
                if (_eventosSesion == null)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Error.png";
                    _lblMensaje.Text = "Problemas al cargar los eventos de la sesión.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;

                    //Crear las columnas de la tabla
                    _limpia.Columns.Add(new DataColumn("PK_Entrada"));
                    _limpia.Columns.Add(new DataColumn("Fecha"));
                    _limpia.Columns.Add(new DataColumn("Operador"));
                    _limpia.Columns.Add(new DataColumn("Evento"));

                    //Llenar la tabla, solo con la columna de turno con valor
                    _limpia.Rows.Add("", "", "", "");
                    _limpia.Rows.Add("", "", "", "");
                    _limpia.Rows.Add("", "", "", "");
                    _limpia.Rows.Add("", "", "", "");
                    _limpia.Rows.Add("", "", "", "");
                    _limpia.Rows.Add("", "", "", "");
                    _limpia.Rows.Add("", "", "", "");

                    _gvwEventos.DataSource = _limpia;
                    _gvwEventos.DataBind();
                    _gvwEventos.Columns[4].Visible = false;

                }
                else if (_eventosSesion.Rows.Count == 0)
                {
                    _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                    _lblMensaje.Text = "No hay eventos registrados en el sistema.";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;

                    //Crear las columnas de la tabla
                    _limpia.Columns.Add(new DataColumn("PK_Entrada"));
                    _limpia.Columns.Add(new DataColumn("Fecha"));
                    _limpia.Columns.Add(new DataColumn("Operador"));
                    _limpia.Columns.Add(new DataColumn("Evento"));

                    //Llenar la tabla, solo con la columna de turno con valor
                    _limpia.Rows.Add("", "", "", "");
                    _limpia.Rows.Add("", "", "", "");
                    _limpia.Rows.Add("", "", "", "");
                    _limpia.Rows.Add("", "", "", "");
                    _limpia.Rows.Add("", "", "", "");
                    _limpia.Rows.Add("", "", "", "");
                    _limpia.Rows.Add("", "", "", "");
                    _gvwEventos.DataSource = _limpia;
                    _gvwEventos.DataBind();
                    _gvwEventos.Columns[4].Visible = false;

                }
                else
                {
                    _gvwEventos.Columns[4].Visible = true;
                    _gvwEventos.DataSource = _eventosSesion;
                    _gvwEventos.DataBind();
                    _imgMensaje.Visible = false;
                    _lblMensaje.Visible = false;
                }
            }
            else 
            {
                _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                _lblMensaje.Text = "Seleccione un laboratorio.";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;

                //Crear las columnas de la tabla
                _limpia.Columns.Add(new DataColumn("PK_Entrada"));
                _limpia.Columns.Add(new DataColumn("Fecha"));
                _limpia.Columns.Add(new DataColumn("Operador"));
                _limpia.Columns.Add(new DataColumn("Evento"));

                //Llenar la tabla, solo con la columna de turno con valor
                _limpia.Rows.Add("", "", "", "");
                _limpia.Rows.Add("", "", "", "");
                _limpia.Rows.Add("", "", "", "");
                _limpia.Rows.Add("", "", "", "");
                _limpia.Rows.Add("", "", "", "");
                _limpia.Rows.Add("", "", "", "");
                _limpia.Rows.Add("", "", "", "");
                _gvwEventos.DataSource = _limpia;
                _gvwEventos.DataBind();
                _gvwEventos.Columns[4].Visible = false;
            }
        }

        protected void _ddlLaboratorios_SelectedIndexChanged(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx"); // 
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            CargarEventosSesion((int)Session["pkSesion"]);
            _upEventos.Update();
            _upMensaje.Update();
        }
    }
    
}