using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using ModulosTICapaGUI.Compartido;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaLogica.Compartido;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace ModulosTICapaGUI.ModuloSGC
{
    public partial class ConsultarCuenta : System.Web.UI.Page
    {
        #region Atributos

        private ControladorSGC _controladorSGC;
        private Sesion _sesion;
        private HttpCookie _cookieActual;
        private static List<List<object>> _listaCarrera;
        //private string _valorBusqueda;

        #endregion

        #region Métodos

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
                _controladorSGC = new ControladorSGC();
                String grupoUsuario = _sesion.obtenerGrupoUsuario(_cookieActual);
                if ((grupoUsuario.Equals("prof")) || (grupoUsuario.Equals("users")) || (grupoUsuario.Equals("ests")) ||
                        (grupoUsuario.Equals("jefes")) || (grupoUsuario.Equals("operadores"))) // Reportar si un usuario autenticado intenta hacer ingreso a una página que no tiene permiso
                {
                    Notificacion notificacion = new Notificacion();
                    notificacion.enviarCorreo("Se ha intentado realizar un acceso no permitido por parte del usuario " + _sesion.obtenerLoginUsuario(_cookieActual) + " a la página de CrearCuenta.aspx", "soporte@ic-itcr.ac.cr", "Violación de Seguridad");
                    Response.Redirect("../Compartido/AccesoDenegado.aspx");
                }
                _listaCarrera = _controladorSGC.obtenerCarreras();
                if ((_listaCarrera != null) && (_listaCarrera.Count != 0))
                {
                    _ddlCarrera.Items.Add("Seleccionar");
                    for (int i = 0; i < _listaCarrera.Count; i++)
                        _ddlCarrera.Items.Add(_listaCarrera.ElementAt(i).ElementAt(1).ToString());
                }
                else if (_listaCarrera == null)
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                    _lblMensaje.Text = "Hubo un error al obtener los datos de las carreras en el sistema";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
                else
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensaje.Text = "No hay carreras registradas en el sistema";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }
        }

        /*protected void _axAsyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('La imagen se ha subido con éxito')</SCRIPT>");
        }

        protected void _axAsyncFileUpload_UploadedFileError(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            HttpContext.Current.Response.Write("<SCRIPT LANGUAGE='JavaScript'>alert('Ha habido un error al subir la imagen')</SCRIPT>");
        }*/

        protected void _btnSugerir_Click(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            if ((!_txtNombre.Text.Equals("")) && (!_txtPApellido.Text.Equals(""))) // Revisar que se haya ingresado los datos del nombre de la persona
            {
                _controladorSGC = new ControladorSGC();
                String nuevoLogin = _controladorSGC.buscarNombreUsuario(_txtNombre.Text, _txtPApellido.Text); // Buscar el login del nuevo usuario
                if ((nuevoLogin != null) && (!nuevoLogin.Equals(""))) // Sino hubo inconvenientes se presenta en interfaz el nuevo login
                    _txtUsuario.Text = nuevoLogin;
                else if (nuevoLogin.Equals(""))
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensaje.Text = "No hay ninguna combinación posible para el nombre de usuario";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
                else
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                    _lblMensaje.Text = "Se ha producido un error al tratar de obtener un nombre de usuario";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }
            else
            {
                _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensaje.Text = "Antes de sugerir un usuario se debe especificar el nombre y apellidos de la persona";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }
        }

        /// <summary>
        /// Evento para crear el  usuario
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

            _txtCarnet.Enabled = true;
            _txtNombre.Enabled = true;
            _txtPApellido.Enabled = true;
            _txtSApellido.Enabled = true;
            _txtTelefono.Enabled = true;
            _txtCelular.Enabled = true;
            _txtCorreo.Enabled = true;
            _txtPassword.Enabled = true;
            _txtCPassword.Enabled = true;
            _rblUsarios.Enabled = true;
            _ddlCarrera.Enabled = true;
            _txtCargarFoto.Visible = true;
            _btnCancelar.Visible = true;
            _btnGuardar.Visible = true;
            _txtValor.Enabled = false;
            _upConsultaUsuario.Update();
            UpdatePanel2.Update();
        }

        /// <summary> 
        /// Método que se encarga de cargar una imagen en memoria cargada en el componente Input para carga de archivos
        /// </summary>   
        /// <returns>Retorna uan arreglo de bytes con la imagen</returns>

        protected void _btnConsultar_Click(object sender, EventArgs e)
        {
            Usuario user = null;
            int contador = 0;
            _controladorSGC = new ControladorSGC();
            _imgMensajeBusqueda.Visible = false;
            _lblMensajeBusqueda.Visible = false;

            _sesion = new Sesion();
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);

            // Verificar que se haya selecionado un criterio de búsqueda
            if (_ddlCriterio.SelectedIndex == 0)
            { // 
                _imgMensajeBusqueda.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensajeBusqueda.Text = "Debe seleccionar un criterio de búsqueda";
                _imgMensajeBusqueda.Visible = true;
                _lblMensajeBusqueda.Visible = true;
                return;
            }
            if (_ddlCriterio.SelectedIndex == 3) //si el criterio seleccionado es por rango de fechas
            {
                // Verificar que los campos de la busqueda estén llenos
                if ((_txtValor2.Text.Equals("")) || (_txtValor3.Text.Equals("")))
                {
                    _imgMensajeBusqueda.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensajeBusqueda.Text = "Debe ingresar fechas para la busqueda";
                    _imgMensajeBusqueda.Visible = true;
                    _lblMensajeBusqueda.Visible = true;
                    return;
                }
                else
                {
                    DateTime fechaInicial = DateTime.ParseExact(_txtValor2.Text, "dd/MM/yyyy", null);
                    DateTime fechaFinal = DateTime.ParseExact(_txtValor3.Text, "dd/MM/yyyy", null);
                    try
                    {
                        List<List<string>> contenido = _controladorSGC.crearArchivoUsuariosPorFecha(fechaInicial, fechaFinal);
                        
                        string nombreArchivo = "Cuentas entre " + fechaInicial.Date.Day.ToString() + "-" + fechaInicial.Month.ToString() + "-" + fechaInicial.Year.ToString() + " y " + fechaFinal.Day.ToString() + "-" + fechaFinal.Month.ToString() + "-" + fechaFinal.Year.ToString();

                        ReporteExcel report = new ReporteExcel();

                        SpreadsheetGear.IWorkbook workbook = report.generarReporte(nombreArchivo, new List<string> { "Login", "Carrera", "Fecha de Creación" }, contenido);

                        Thread STAThread = new Thread(() =>
                        {
                            // Stream the Excel spreadsheet to the client in a format
                            // compatible with Excel 97/2000/XP/2003/2007/2010.
                            Response.Clear();
                            Response.ContentType = "application/vnd.ms-excel";
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + nombreArchivo + ".xls");
                            workbook.SaveToStream(Response.OutputStream, SpreadsheetGear.FileFormat.Excel8);
                            Response.End();
                        });

                        STAThread.SetApartmentState(ApartmentState.STA);
                        STAThread.Start();
                        STAThread.Join();
                    }
                    catch (Exception ex)
                    {
                        _imgMensajeBusqueda.ImageUrl = "../Imagenes/Advertencia.png";
                        _lblMensajeBusqueda.Text = ex.Message;
                        _imgMensajeBusqueda.Visible = true;
                        _lblMensajeBusqueda.Visible = true;
                    }
                }
            }
            else
            {
                // Verificar que los campos de la busqueda estén llenos
                if (_txtValor.Text.Equals(""))
                {
                    _imgMensajeBusqueda.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensajeBusqueda.Text = "Debe ingresar algún valor para la busqueda";
                    _imgMensajeBusqueda.Visible = true;
                    _lblMensajeBusqueda.Visible = true;
                    return;
                }
                // El criterio seleccionado es "Login"
                if (_ddlCriterio.SelectedIndex == 1)
                    user = _controladorSGC.buscarUsuario(_txtValor.Text, true); // Login

                if (_ddlCriterio.SelectedIndex == 2)
                {
                    user = _controladorSGC.buscarUsuario(_txtValor.Text, false); //Carnet
                    _lblUsuario.Visible = true;
                    _txtUsuario.Visible = true;
                }


                // Se actualiza el contenido de la interfaz
                if (user != null)
                {
                    _txtCarnet.Text = user.Carnet;
                    _txtNombre.Text = user.Nombre;
                    _txtPApellido.Text = user.Apellidos.Split(' ')[0];
                    _txtSApellido.Text = user.Apellidos.Split(' ')[1];
                    _txtTelefono.Text = user.TelefonoCasa;
                    _txtCelular.Text = user.TelefonoCelular;
                    _txtCorreo.Text = user.Correo;
                    _txtUsuario.Text = user.UID;

                    foreach (ListItem carrera in _ddlCarrera.Items)
                    {
                        if (carrera.Text == user.Carrera)
                            _ddlCarrera.SelectedIndex = contador;
                        else
                            contador++;
                    }

                    if (user.Grupo == "Estudiante")
                        _rblUsarios.SelectedIndex = 0;
                    else
                        _rblUsarios.SelectedIndex = 1;


                    _imgMensajeBusqueda.Visible = false;
                    _lblMensajeBusqueda.Visible = false;
                    _bntModificar.Enabled = true;
                    _lblMensaje.Visible = false;
                    _imgMensaje.Visible = false;

                    _upConsultaUsuario.Update();

                }
                else
                {
                    _imgMensajeBusqueda.ImageUrl = "../Imagenes/Error.png";
                    _lblMensajeBusqueda.Text = "Usuario no encontrado";
                    _imgMensajeBusqueda.Visible = true;
                    _lblMensajeBusqueda.Visible = true;

                    _txtCarnet.Text = "";
                    _txtNombre.Text = "";
                    _txtPApellido.Text = "";
                    _txtSApellido.Text = "";
                    _txtTelefono.Text = "";
                    _txtCelular.Text = "";
                    _txtCorreo.Text = "";
                    _txtPassword.Text = "";
                    _txtCPassword.Text = "";
                    _ddlCarrera.SelectedIndex = 0;
                    //if(_rblUsarios.SelectedItem.Selected)
                    //      _rblUsarios.SelectedItem.Selected = false;
                    _bntModificar.Enabled = false;
                }
            }
        }




        protected void _btnGuardar_Click(object sender, EventArgs e)
        {
            _sesion = new Sesion();
            Boolean changePass = false;
            _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
            if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                Response.Redirect("../Autentificacion/Login.aspx");
            else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
                Response.SetCookie(_cookieActual);
            // Verificar que los campos del formulario estén llenos
            if ((!_txtCarnet.Text.Equals("")) && (!_txtNombre.Text.Equals("")) && (!_txtPApellido.Text.Equals("")) && (!_txtSApellido.Text.Equals("")) &&
                (!_txtTelefono.Text.Equals("")) && (!_txtCelular.Text.Equals("")) && (!_txtCorreo.Text.Equals("")) && (_ddlCarrera.SelectedIndex != 0))
            {

                if (_txtPassword.Text != String.Empty)
                {
                    if (!Regex.IsMatch(_txtPassword.Text, @"^[a-zA-Z0-9]+$")) // Para verificar que la contraseña solo contenga números y letras
                    {
                        _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                        _lblMensaje.Text = "La contraseña solo puede contener números y letras";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        return;
                    }
                    if (_txtPassword.Text.Length < 12) // EL password sea de mínimo 12 caracteres
                    {
                        _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                        _lblMensaje.Text = "La contraseña debe tener un mínimo de 12 caracteres";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        return;
                    }
                    //String path = "../Graficos/" + _axAsyncFileUpload.FileName;
                    //_axAsyncFileUpload.SaveAs(path); // Donde va a salvar el archivo
                    if (!_txtPassword.Text.Equals(_txtCPassword.Text))  // Verifica si las contraseñas son iguales
                    {
                        _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                        _lblMensaje.Text = "Las contraseñas no concuerdan";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        return;
                    }
                    changePass = true;

                }

                int resultado;
                _controladorSGC = new ControladorSGC();
                List<string> datosUsuario = new List<string>();
                datosUsuario.Add(_txtNombre.Text);
                datosUsuario.Add(_txtPApellido.Text);
                datosUsuario.Add(_txtSApellido.Text);
                datosUsuario.Add(_txtCarnet.Text);
                datosUsuario.Add(_txtTelefono.Text);
                datosUsuario.Add(_txtCelular.Text);
                datosUsuario.Add(_txtValor.Text);
                datosUsuario.Add(_txtPassword.Text);
                datosUsuario.Add(_txtCorreo.Text);
                datosUsuario.Add(_ddlCarrera.SelectedValue);
                datosUsuario.Add(_rblUsarios.SelectedValue);
                byte[] imagen = cargarImagen();
                //String nombreArchivo = System.IO.Path.GetFileName(_txtCargarFoto.PostedFile.FileName); // Obtener el nombre del archivo
                if (_ddlCriterio.SelectedIndex == 1)
                    resultado = _controladorSGC.modificarUsuario(datosUsuario, true, imagen, changePass);
                else
                    resultado = _controladorSGC.modificarUsuario(datosUsuario, false, imagen, changePass);
                switch (resultado)
                {
                    case 1:
                        _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                        _lblMensaje.Text = "Se ha producido un error al crear la cuenta en el LDAP";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        break;
                    case 2:
                        _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                        _lblMensaje.Text = "La cuenta fue modifica en el LDAP y Active Directory pero no en la base del Sistema";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        break;
                    case 3:
                        _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                        _lblMensaje.Text = "La cuenta fue modificada en el LDAP pero no en Active Directory";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        break;
                    case 4:
                        //_btnConsultar_Click(sender, e);
                        _btnCancelar_Click(sender, e);
                        _ddlCarrera.SelectedIndex = 1;
                        _imgMensaje.ImageUrl = "../Imagenes/ok.png";
                        _lblMensaje.Text = "La cuenta ha sido modificada con éxito";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                        break;
                }
            }

            else
            {
                _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                _lblMensaje.Text = "Debe completar todos los campos del formulario";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }


        }

        protected void _btnCancelar_Click(object sender, EventArgs e)
        {

            _txtCarnet.Enabled = false;
            _txtNombre.Enabled = false;
            _txtPApellido.Enabled = false;
            _txtSApellido.Enabled = false;
            _txtTelefono.Enabled = false;
            _txtCelular.Enabled = false;
            _txtCorreo.Enabled = false;
            _txtPassword.Enabled = false;
            _txtCPassword.Enabled = false;
            _rblUsarios.Enabled = false;
            _ddlCarrera.Enabled = false;
            _txtCargarFoto.Visible = false;
            _btnCancelar.Visible = false;
            _btnGuardar.Visible = false;
            _txtValor.Enabled = true;
            _bntModificar.Enabled = false;

            _txtCarnet.Text = String.Empty;
            _txtNombre.Text = String.Empty;
            _txtPApellido.Text = String.Empty;
            _txtSApellido.Text = String.Empty;
            _txtTelefono.Text = String.Empty;
            _txtCelular.Text = String.Empty;
            _txtCorreo.Text = String.Empty;
            _txtPassword.Text = String.Empty;
            _txtCPassword.Text = String.Empty;
            _ddlCarrera.SelectedIndex = 0;
            _btnGuardar.Visible = false;
            _txtValor.Text = String.Empty;
            _ddlCriterio.SelectedIndex = 0;


        }
        private byte[] cargarImagen()
        {
            byte[] imagen = null;

            _lbImagen.Visible = true;
            try
            {
                if (_txtCargarFoto != null) // Sino se seleccionó una imagen para el usuario
                {
                    if ((_txtCargarFoto.PostedFile != null) && (_txtCargarFoto.PostedFile.ContentLength > 0)) // Si el archivo tiene algún contenido
                    {
                        Stream stream = _txtCargarFoto.PostedFile.InputStream;
                        long iLength = stream.Length;
                        imagen = new byte[(int)stream.Length]; // Almacenar el archivo en memoria (un arreglo de bytes)
                        stream.Read(imagen, 0, (int)stream.Length);
                        stream.Close();
                        _lbImagen.Visible = false;
                        return imagen;
                    }
                    else
                    {
                        _lbImagen.Visible = false;
                        return null;
                    }
                }
                _lbImagen.Visible = false;
                return null;
            }
            catch (Exception e) // Si existiera un error
            {
                _controladorSGC.insertarBitacoraError(e.ToString(), "");
                _lbImagen.Visible = false;
                return null;
            }
        }

        protected void _ddlCriterioChanged(object sender, EventArgs e)
        {
            if (_ddlCriterio.SelectedIndex == 3)
            {
                _txtValor.Visible = false;
                _txtValor2.Visible = true;
                _imgFechaInicio.Visible = true;
                _txtValor3.Visible = true;
                _imgFechaFinal.Visible = true;
                _bntConsultar.Text = "Crear Archivo";
                UpdatePanel2.Update();
                return;

            }
            else
            {
                _txtValor.Visible = true;
                _txtValor2.Visible = false;
                _imgFechaInicio.Visible = false;
                _txtValor3.Visible = false;
                _imgFechaFinal.Visible = false;
                _bntConsultar.Text = "Buscar";
                UpdatePanel2.Update();
                return;
            }
        }
        #endregion
    }
}