using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTIControlador.Clases;
using ModulosTICapaGUI.Compartido;
using ModulosTICapaLogica.Compartido;
using ModulosTICapaLogica.ModuloSGC;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;

namespace ModulosTICapaGUI.ModuloSGC
{
	public partial class CrearCuenta : System.Web.UI.Page
	{
		#region Atributos

		private ControladorSGC _controladorSGC;
		private Sesion _sesion;
		private HttpCookie _cookieActual;
        private static List<List<object>> _listaCarrera;
		

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
				if ((nuevoLogin != null) && (!nuevoLogin.Equals(""))) // Si no hubo inconvenientes se presenta en interfaz el nuevo login
				{
					_txtUsuario.Text = nuevoLogin;
					_imgMensaje.Visible = false;
					_lblMensaje.Visible = false;
				}
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
		/// Evento para crear el usuario
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
            if ((!_txtCarnet.Text.Equals("")) && (!_txtNombre.Text.Equals("")) && (!_txtPApellido.Text.Equals("")) && (!_txtSApellido.Text.Equals("")) &&
                (!_txtTelefono.Text.Equals("")) && (!_txtCelular.Text.Equals("")) && (!_txtCorreo.Text.Equals("")) && (!_txtUsuario.Text.Equals("")) &&
                (_ddlCarrera.SelectedIndex != 0) && (!_txtPassword.Text.Equals("")) && (!_txtCPassword.Text.Equals("")) && ((_rbEstudiante.Checked)
                || (_rbProfesor.Checked)))
            {
                if (Regex.IsMatch(_txtPassword.Text, @"^[a-zA-Z0-9]+$")) // Para verificar que la contraseña solo contenga números y letras
                {
                    if (_txtPassword.Text.Length >= 12) // EL password sea de mínimo 12 caracteres
                    {
                        /*String path = "../Graficos/" + _axAsyncFileUpload.FileName;
                        _axAsyncFileUpload.SaveAs(path); // Donde va a salvar el archivo*/
                        if (_txtPassword.Text.Equals(_txtCPassword.Text)) // Verifica si las contraseñas son iguales
                        {
                            int resultado;
                            _controladorSGC = new ControladorSGC();
                            List<string> datosUsuario = new List<string>();
                            datosUsuario.Add(_txtNombre.Text);
                            datosUsuario.Add(_txtPApellido.Text);
							datosUsuario.Add(_txtSApellido.Text);
                            datosUsuario.Add(_txtCarnet.Text);
                            datosUsuario.Add(_txtTelefono.Text);
                            datosUsuario.Add(_txtCelular.Text); 
                            datosUsuario.Add(_txtCorreo.Text);
							datosUsuario.Add(_ddlCarrera.SelectedValue);	//_listaCarrera.ElementAt(_ddlCarrera.SelectedIndex - 1).ElementAt(0).ToString()
							datosUsuario.Add(_txtUsuario.Text);
							datosUsuario.Add(_txtPassword.Text);
                            byte[] imagen = cargarImagen();
                            //String nombreArchivo = System.IO.Path.GetFileName(_txtCargarFoto.PostedFile.FileName); // Obtener el nombre del archivo
                            if (_rbEstudiante.Checked)
                                resultado = _controladorSGC.crearUsuario(datosUsuario, true, imagen);
                            else
                                resultado = _controladorSGC.crearUsuario(datosUsuario, false, imagen);
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
                                    _lblMensaje.Text = "La cuenta fue creada en el LDAP y Active Directory pero no en la base del Sistema";
                                    _imgMensaje.Visible = true;
                                    _lblMensaje.Visible = true;
                                    break;
                                case 3:
                                    _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                                    _lblMensaje.Text = "La cuenta fue creada en el LDAP pero no en Active Directory";
                                    _imgMensaje.Visible = true;
                                    _lblMensaje.Visible = true;
                                    break;
                                case 4:
                                    _imgMensaje.ImageUrl = "../Imagenes/ok.png";
                                    _lblMensaje.Text = "La cuenta ha sido creada con éxito";
                                    _imgMensaje.Visible = true;
                                    _lblMensaje.Visible = true;
                                    _txtCarnet.Text = "";
                                    _txtNombre.Text = "";
                                    _txtPApellido.Text = "";
                                    _txtSApellido.Text = "";
                                    _txtTelefono.Text = "";
                                    _txtCelular.Text = "";
                                    _txtCorreo.Text = "";
                                    _txtUsuario.Text = "";
                                    _rbEstudiante.Checked = false;
                                    _rbProfesor.Checked = false;
                                    _ddlCarrera.SelectedIndex = 0;
                                    break;
                            }
                        }
                        else
                        {
                            _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                            _lblMensaje.Text = "Las contraseñas no concuerdan";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                        }
                    }
                    else
                    {
                        _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                        _lblMensaje.Text = "La contraseña debe tener un mínimo de 12 caracteres";
                        _imgMensaje.Visible = true;
                        _lblMensaje.Visible = true;
                    }
                }
                else
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensaje.Text = "La contraseña solo puede contener números y letras";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
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

        /// <summary>
        /// Método que se encarga de cargar una imagen en memoria cargada en el componente Input para carga de archivos
        /// </summary>
        /// <returns>Retorna uan arreglo de bytes con la imagen</returns>

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


		protected void _btnGenerar_Click(object sender, EventArgs e)
		{
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx");
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
			_controladorSGC = new ControladorSGC();
			
			String savePath = @"\Excel\";
			String nombreExcel = DateTime.Now.ToString("ddMMyyyy_HHmmss");

			
			string strFileType = System.IO.Path.GetExtension(_fluExcel.FileName).ToString().ToLower();

			if (_fluExcel.HasFile)
			{
				try
				{
					if (strFileType == ".xls" || strFileType == ".xlsx")
					{
						_fluExcel.SaveAs(Server.MapPath(savePath + nombreExcel + strFileType));
					}
					else
					{
						_lblMensajeUpload.Text = "Solo son permitidos archivos de Excel.";
						_lblMensajeUpload.Visible = true;
						return;
					}

					string strNewPath = Server.MapPath(savePath + nombreExcel + strFileType);

					//Se llama al mètodo que procesa el archivo de Excel y se retornan los errores.
					List<ErroresExcel> errores = _controladorSGC.validarDatosExcel(savePath, nombreExcel, strFileType, strNewPath);

					//Si hay errores se muestran al usuario.
					if (errores.Count != 0)
					{
						string _msjErrores = "";
						foreach (ErroresExcel er in errores)
						{
							//Se crea un solo string con los mensajes.
							_msjErrores = _msjErrores + "Fila: " + er.linea + " dato: " + er.dato + " error: " + er.mensaje + "\n";
						}
						_taErrores.InnerText = _msjErrores;
						_taErrores.Visible = true;
					}
					//De lo contrario se procesan los datos del archivo.
					else
					{
						List<string> _estados = new List<string>();
						string _mensaje = "";
						_estados = _controladorSGC.generarCuentas();

						foreach (string cuenta in _estados)
						{
							_mensaje = _mensaje + cuenta;
						}

						_taErrores.InnerText = _mensaje;
						_taErrores.Visible = true;
					}

				}
				catch (Exception ex)
				{
					Response.Write("Error: " + ex.Message.ToString());
				}
			}
			else
			{
				Response.Write("Please select a file to upload.");
			}


			/*Excel.Application xlApp;
			Excel.Workbook xlWorkBook;
			Excel.Worksheet xlWorkSheet;
			object misValores = System.Reflection.Missing.Value;

			xlApp = new Excel.Application();
			xlWorkBook = xlApp.Workbooks.Open(_btnCargar.Value, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
			xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

			String prueba = xlWorkSheet.get_Range("A1","A1").Value2.ToString();

			xlWorkBook.Close(true, misValores, misValores);
			xlApp.Quit();

			liberarObjeto(xlWorkSheet);
			liberarObjeto(xlWorkBook);
			liberarObjeto(xlApp);*/




		}

		private void liberarObjeto(object obj)
		{
			try
			{
				System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
				obj = null;
			}
			catch (Exception )
			{
				obj = null;
			}
			finally
			{
				GC.Collect();
			}
		} 


		#endregion
	}
}