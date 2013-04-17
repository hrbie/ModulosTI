using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModulosTICapaGUI.Compartido;
using ModulosTIControlador.Clases;
using ModulosTICapaLogica.Compartido;

namespace ModulosTICapaGUI.ModuloSistema
{
    public partial class GestionBitError : System.Web.UI.Page
    {
        #region Atributos

        private ControladorSistema _controladorSistema;
        private Sesion _sesion;
        private HttpCookie _cookieActual;
        private static List<List<object>> _listaErrores;

        #endregion

        #region Métodos

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _sesion = new Sesion();
                _cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
                if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
                    Response.Redirect("../Autentificacion/Login.aspx");
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
                _listaErrores = _controladorSistema.obtenerEntradasBitError("", "", -1);
                if ((_listaErrores != null) && (_listaErrores.Count != 0))
                {
                    for (int i = 0; i < _listaErrores.Count; i++)
                    {
                        String texto = _listaErrores.ElementAt(i).ElementAt(1).ToString();
                        switch (Convert.ToInt32(_listaErrores.ElementAt(i).ElementAt(4)))
                        {
                            case 0:
                                texto = texto + " - Pendiente";
                                break;
                            case 1:
                                texto = texto + " - En proceso";
                                break;
                            case 2:
                                texto = texto + " - Resuelto";
                                break;
                        }
                        _ltbErrores.Items.Add(texto);
                    }
                }
                else if (_listaErrores == null)
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                    _lblMensaje.Text = "Hubo un error al obtener los errores reportados";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
                else
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensaje.Text = "No hay errores reportados";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }
        }

        protected void _btnNotificar_Click(object sender, EventArgs e)
        {
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx");
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
			Notificacion notificacion = new Notificacion();
            Boolean resultado = notificacion.enviarCorreo("Descripción del sistema:" + '\n' + _txtDetalle.Text + '\n' + _txtDetalleUs.Text + '\n' + "Notificación enviada por: " + _sesion.obtenerLoginUsuario(_cookieActual), "soporte@ic-itcr.ac.cr", "Reporte de Error Plataforma de Servicio");
            if (resultado)
            {
                _imgMensaje.ImageUrl = "../Imagenes/ok.png";
                _lblMensaje.Text = "Se ha enviado con éxito la notificación del error";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }
            else
            {
                _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                _lblMensaje.Text = "Se ha dado un problema al intentar enviar la notificación del error";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }
        }

        protected void _ltbErrores_SelectedIndexChanged(object sender, EventArgs e)
        {
            _txtDetalle.Text = _listaErrores.ElementAt(_ltbErrores.SelectedIndex).ElementAt(2).ToString();
            _txtDetalleUs.Text = _listaErrores.ElementAt(_ltbErrores.SelectedIndex).ElementAt(3).ToString();
            _upDetalle.Update();
            _upDetalleUs.Update();
			_ddlEstadoDetalle.SelectedIndex = 0;
			_imgMensaje.Visible = false;
			_lblMensaje.Visible = false;
        }

        protected void _btnActualizar_Click(object sender, EventArgs e)
        {
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx");
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
			if ((!_txtDetalle.Text.Equals("")) || (!_txtDetalle.Text.Equals("")))
			{
				if (_ddlEstadoDetalle.SelectedIndex != 0)
				{
					String nuevo = _ddlEstadoDetalle.SelectedValue;
					int nuevoEstado = -1;
					switch (nuevo)
					{
						case "Pendiente":
							nuevoEstado = 0;
							break;
						case "En proceso":
							nuevoEstado = 1;
							break;
						case "Resuelto":
							nuevoEstado = 2;
							break;
					}
					_controladorSistema = new ControladorSistema();
					Boolean resultado = _controladorSistema.modificarEntradaBitError(Convert.ToInt32(_listaErrores.ElementAt(_ltbErrores.SelectedIndex).ElementAt(0)), nuevoEstado);
					if (resultado)
					{
						_imgMensaje.ImageUrl = "../Imagenes/ok.png";
						_lblMensaje.Text = "Se modificó el estado del registro con éxito";
						_imgMensaje.Visible = true;
						_lblMensaje.Visible = true;
						_listaErrores = _controladorSistema.obtenerEntradasBitError("", "", -1);
						if ((_listaErrores != null) && (_listaErrores.Count != 0))
						{
							_ltbErrores.Items.Clear();
							for (int i = 0; i < _listaErrores.Count; i++)
							{
								String texto = _listaErrores.ElementAt(i).ElementAt(1).ToString();
								switch (Convert.ToInt32(_listaErrores.ElementAt(i).ElementAt(4)))
								{
									case 0:
										texto = texto + " - Pendiente";
										break;
									case 1:
										texto = texto + " - En proceso";
										break;
									case 2:
										texto = texto + " - Resuelto";
										break;
								}
								_ltbErrores.Items.Add(texto);
							}
						}
						else if (_listaErrores == null)
						{
							_imgMensaje.ImageUrl = "../Imagenes/Error.png";
							_lblMensaje.Text = "Hubo un error al volver a obtener los errores reportados";
							_imgMensaje.Visible = true;
							_lblMensaje.Visible = true;
						}
						else
						{
							_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
							_lblMensaje.Text = "No hay errores reportados";
							_imgMensaje.Visible = true;
							_lblMensaje.Visible = true;
						}
					}
					else
					{
						_imgMensaje.ImageUrl = "../Imagenes/Error.png";
						_lblMensaje.Text = "Hubo un error al tratar de actualizar el estado del registro";
						_imgMensaje.Visible = true;
						_lblMensaje.Visible = true;
					}
				}
				else
				{
					_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
					_lblMensaje.Text = "Debe seleccionar el nuevo estado del registro";
					_imgMensaje.Visible = true;
					_lblMensaje.Visible = true;
				}
			}
			else
			{
				_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
				_lblMensaje.Text = "Debe seleccionar un registro de error";
				_imgMensaje.Visible = true;
				_lblMensaje.Visible = true;
			}
        }

        protected void _btnFiltrar_Click(object sender, EventArgs e)
        {
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx");
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
			_controladorSistema = new ControladorSistema();
			if (((!_txtFechaInicio.Text.Equals("")) && (!_txtFechaFinal.Text.Equals(""))) || (_ddlEstado.SelectedIndex != 0))
			{
				if (_ddlEstado.SelectedIndex != 0)
				{
					String nuevo = _ddlEstado.SelectedValue;
					int estado = -1;
					switch (nuevo)
					{
						case "Pendiente":
							estado = 0;
							break;
						case "En proceso":
							estado = 1;
							break;
						case "Resuelto":
							estado = 2;
							break;
					}
					if (_txtFechaInicio.Text.Equals("")) // Solo buscar por estado
						_listaErrores = _controladorSistema.obtenerEntradasBitError("", "", estado);
					else // Buscar por estado y fechas
						_listaErrores = _controladorSistema.obtenerEntradasBitError(_txtFechaInicio.Text, _txtFechaFinal.Text, estado);
				}
				else // Solo buscar por fechas
					_listaErrores = _controladorSistema.obtenerEntradasBitError(_txtFechaInicio.Text, _txtFechaFinal.Text, -1);
				if ((_listaErrores != null) && (_listaErrores.Count != 0)) // Cargar los registros de error
				{
					_ltbErrores.Items.Clear();
					for (int i = 0; i < _listaErrores.Count; i++)
					{
						String texto = _listaErrores.ElementAt(i).ElementAt(1).ToString();
						switch (Convert.ToInt32(_listaErrores.ElementAt(i).ElementAt(4)))
						{
							case 0:
								texto = texto + " - Pendiente";
								break;
							case 1:
								texto = texto + " - En proceso";
								break;
							case 2:
								texto = texto + " - Resuelto";
								break;
						}
						_ltbErrores.Items.Add(texto);
					}
				}
				else if (_listaErrores == null)
				{
					_imgMensaje.ImageUrl = "../Imagenes/Error.png";
					_lblMensaje.Text = "Hubo un error al obtener los errores reportados";
					_imgMensaje.Visible = true;
					_lblMensaje.Visible = true;
				}
				else
				{
					_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
					_lblMensaje.Text = "No hay errores reportados";
					_imgMensaje.Visible = true;
					_lblMensaje.Visible = true;
					_ltbErrores.Items.Clear();
					_txtDetalle.Text = "";
					_txtDetalleUs.Text = "";
				}
			}
			else
			{
				_imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
				_lblMensaje.Text = "Debe seleccionar algún criterio para poder filtrar";
				_imgMensaje.Visible = true;
				_lblMensaje.Visible = true;
			}
        }

        protected void _btnEliminar_Click(object sender, EventArgs e)
        {
			_sesion = new Sesion();
			_cookieActual = _sesion.verificarValidez(Request.Cookies["PS"]);
			if (_cookieActual == null) // Si la cookie expira redirecciona a la pantalla de Login
				Response.Redirect("../Autentificacion/Login.aspx");
			else // Volver a crear la cookie en el cliente, con el nuevo tiempo de expiración
				Response.SetCookie(_cookieActual);
			_controladorSistema = new ControladorSistema();
            Boolean resultado = _controladorSistema.eliminarBitError(Convert.ToInt32(_listaErrores.ElementAt(_ltbErrores.SelectedIndex).ElementAt(0)));
            if (resultado)
            {
				_txtDetalle.Text = "";
				_txtDetalleUs.Text = "";
				_upDetalle.Update();
				_upDetalleUs.Update();
				_ddlEstadoDetalle.SelectedIndex = 0;
				_imgMensaje.Visible = false;
				_lblMensaje.Visible = false;
                _imgMensaje.ImageUrl = "../Imagenes/ok.png";
                _lblMensaje.Text = "Se eliminó el registro con éxito";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
                _txtDetalle.Text = "";
                _txtDetalleUs.Text = "";
                _upDetalle.Update();
                _upDetalleUs.Update();
                _ddlEstadoDetalle.SelectedIndex = 0;
                _listaErrores = _controladorSistema.obtenerEntradasBitError("", "", -1);
                if ((_listaErrores != null) && (_listaErrores.Count != 0))
                {
					_ltbErrores.Items.Clear();
                    for (int i = 0; i < _listaErrores.Count; i++)
                    {
                        String texto = _listaErrores.ElementAt(i).ElementAt(1).ToString();
                        switch (Convert.ToInt32(_listaErrores.ElementAt(i).ElementAt(4)))
                        {
                            case 0:
                                texto = texto + " - Pendiente";
                                break;
                            case 1:
                                texto = texto + " - En proceso";
                                break;
                            case 2:
                                texto = texto + " - Resuelto";
                                break;
                        }
                        _ltbErrores.Items.Add(texto);
                    }
                }
                else if (_listaErrores == null)
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                    _lblMensaje.Text = "Hubo un error al volver a obtener los errores reportados";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
                else
                {
                    _imgMensaje.ImageUrl = "../Imagenes/Advertencia.png";
                    _lblMensaje.Text = "No hay errores reportados";
                    _imgMensaje.Visible = true;
                    _lblMensaje.Visible = true;
                }
            }
            else
            {
                _imgMensaje.ImageUrl = "../Imagenes/Error.png";
                _lblMensaje.Text = "Hubo un error al tratar de eliminar el registro";
                _imgMensaje.Visible = true;
                _lblMensaje.Visible = true;
            }
        }

        #endregion
    }
}