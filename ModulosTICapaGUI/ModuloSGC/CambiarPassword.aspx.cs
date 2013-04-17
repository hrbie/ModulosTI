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

namespace ModulosTICapaGUI.ModuloSGC
{
	public partial class CambiarPassword : System.Web.UI.Page
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
				try
				{
					_controladorSGC = new ControladorSGC();
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
				catch (Exception ex)
				{
					_controladorSGC.insertarBitacoraError(ex.ToString(), "");
				}

			}


			//Cargar los datos del usuario.
			cargarUsuario();
			_txtPassword.Enabled = true;
			_txtCPassword.Enabled = true;

		}

		/// <summary>
		/// Método que carga los dados de un usuario en el formulario.
		/// </summary>

		protected void cargarUsuario()
		{
			Usuario user = null;
			int contador = 0;
			_controladorSGC = new ControladorSGC();

			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx");
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);


			user = _controladorSGC.buscarUsuario(_sesion.obtenerLoginUsuario(_cookieActual), true); // Login

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


				_lblMensaje.Visible = false;
				_imgMensaje.Visible = false;



			}
			else
			{
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

				if ((_txtContraActual.Text != String.Empty) && (_txtPassword.Text != String.Empty) && (_txtCPassword.Text != String.Empty))
				{
					if (_controladorSGC.validarUsuario(_sesion.obtenerLoginUsuario(_cookieActual), _txtContraActual.Text))
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
					else
					{
						_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
						_lblMensaje.Text = "La contraseña actual es incorrecta";
						_imgMensaje.Visible = true;
						_lblMensaje.Visible = true;
						return;
					}

				}

				int resultado;
				_controladorSGC = new ControladorSGC();
				List<string> datosUsuario = new List<string>();
				datosUsuario.Add(_txtNombre.Text);
				datosUsuario.Add(_txtPApellido.Text + " " + _txtSApellido.Text);
				datosUsuario.Add(_txtCarnet.Text);
				datosUsuario.Add(_txtTelefono.Text);
				datosUsuario.Add(_txtCelular.Text);
				datosUsuario.Add(_sesion.obtenerLoginUsuario(_cookieActual));
				datosUsuario.Add(_txtPassword.Text);
				datosUsuario.Add(_txtCorreo.Text);
				datosUsuario.Add(_ddlCarrera.SelectedValue);
				datosUsuario.Add(_rblUsarios.SelectedValue);

				resultado = _controladorSGC.modificarPassword(datosUsuario, true, changePass);

				switch (resultado)
				{
					case 1:
						_imgMensaje.ImageUrl = "../Imagenes/Error.png";
						_lblMensaje.Text = "Se ha producido un error al modificar la cuenta en el LDAP";
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

		#endregion
	}
}