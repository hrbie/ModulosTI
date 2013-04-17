using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ModulosTIControlador.Clases;
using ModulosTICapaGUI.Compartido;

namespace ModulosTICapaGUI.ModuloPEUL
{
	public partial class UsoLaboratorio : System.Web.UI.Page
	{
        #region Artributos
        private HttpCookie _cookieActual;
        private Sesion _sesion;
        private ControladorPEUL _controlador;  //Controlador de la Interfaz
        private static List<List<object>> _usoLaboratorios;    //Lista que contiene el uso de cada laboratorio
        private DataTable _dtGrafico;   //Data table utilizado para llenar el grafico
        private DataTable _dtDatosUso;  //Data table utilizado para llenar el grid que contiene los datos del uso de laboratorio
        private int _errorCarga; // Para manejo de errores en el tiempo de carga de la página
                                        // 0: No se cargaron laboratorios
                                        // 1: No se cargaron los estados (Comentarios predefinidos)
                                        // 2: No se han cargado ni estados ni laboratorios
                                        // 3: No se obtuvieron datos del uso de los laboratorios
                                        // 4: No se pudo cargar ninguno de los elementos de la pagina
        #endregion

		protected void Page_Load(object sender, EventArgs e)
		{
            try
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
                    _errorCarga = -1;
                    _controlador = new ControladorPEUL();
                    _usoLaboratorios = _controlador.calcularUsoActual();    //Obtener los datos del uso de los laboratorios
                    if (_usoLaboratorios != null)
                        cargarDatos(_usoLaboratorios);  //Llama al método que se encarga de cargar los datos en el grid y en el grafico
                    else
                    {
                        if (_errorCarga == 2)
                            _errorCarga = 4;
                        else
                            _errorCarga = 3;
                    }

                    switch (_errorCarga)    //Verificar si hubo algun error
                    {
                        case 3:
                            _imgMensaje.ImageUrl = "~/Imagenes/Advertencia.png";
                            _lblMensaje.Text = "No hay registro del uso de los laboratorios.";
                            _imgMensaje.Visible = true;
                            _lblMensaje.Visible = true;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                _controlador = new ControladorPEUL();
                _controlador.insertarBitacoraError(ex.ToString(), "");
            }
		}

        /// <summary>
        /// Método que se encarga de cargar los datos en el grid y en el grafico
        /// </summary>
        /// <param name="datosUso">Recibe una lista de listas de objetos con todos los datos del uso de los laboratorios</param>

        private void cargarDatos(List<List<object>> datosUso)
        {
            try
            {
                // Crear el data table utilizado por el grafico
                _dtGrafico = new DataTable("Datos");
                _dtGrafico.Columns.Add(new DataColumn("Computadoras"));
                _dtGrafico.Columns.Add(new DataColumn("Laptops"));
                _dtGrafico.Columns.Add(new DataColumn("Laboratorio"));

                //Crear el datatable utilizado por el grid
                _dtDatosUso = new DataTable("Datos");
                _dtDatosUso.Columns.Add(new DataColumn("Laboratorio"));
                _dtDatosUso.Columns.Add(new DataColumn("Operador"));
                _dtDatosUso.Columns.Add(new DataColumn("UltimoRegistro"));
                _dtDatosUso.Columns.Add(new DataColumn("Porcentaje"));
                _dtDatosUso.Columns.Add(new DataColumn("Comentario"));

                // Cargar los datatable que utiliza el grid con los datos del uso de los laboratorios y el datatable que utiliza el grafico
                foreach (List<object> datos in datosUso)
                {
                    //Llenar datatable utilizado por el grafico
                    _dtGrafico.Rows.Add(datos[4], datos[5], datos[0]);
                    //Llenar datatable utilizado por el grid
                    _dtDatosUso.Rows.Add(datos[0], datos[1], datos[2], Convert.ToInt32(datos[6]) + "%", datos[3]);
                }
                //Cargar los datos de la base en el grid
                _gridUsoLaboratorio.DataSource = _dtDatosUso;
                _gridUsoLaboratorio.DataBind();

                //Cargar los datos de la base en el grafico y definir ejes X y Y
                _chtUsoLaboratorio.DataSource = _dtGrafico;
                _chtUsoLaboratorio.Series["Computadoras"].XValueMember = "Laboratorio";
                _chtUsoLaboratorio.Series["Computadoras"].YValueMembers = "Computadoras";
                _chtUsoLaboratorio.Series["Laptops"].XValueMember = "Laboratorio";
                _chtUsoLaboratorio.Series["Laptops"].YValueMembers = "Laptops";
                _chtUsoLaboratorio.DataBind();
            }
            catch (Exception ex)
            {
                _controlador = new ControladorPEUL();
                _controlador.insertarBitacoraError(ex.ToString(), "");
            }
        }
	}
}