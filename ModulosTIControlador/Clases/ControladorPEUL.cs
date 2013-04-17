using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaLogica.Compartido;
using ModulosTICapaDatos.ModuloPEUL;
using System.Data;

namespace ModulosTIControlador.Clases
{
    public class ControladorPEUL
    {
        #region Atributos

        private ConexionLDAP _conexionLDAP;
        private ManejoBD _conexionBD;
        private ManejoBDPEUL _conexionPEULBD;

        #endregion

        #region Contructor

        public ControladorPEUL()
        {
            _conexionBD = new ManejoBD();
            _conexionLDAP = new ConexionLDAP();
            _conexionPEULBD = new ManejoBDPEUL();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que se encarga de obtener todos los laboratorios existentes
        /// </summary>
        /// <returns>Retorna una lista de listas de objetos, en donde cada sublista es un lugar de la forma List(List(IdLugar:int, NombreLugar:String
        /// Capacidad:int, Descripcion:String, Encargado:String, Tipo:Boolean)). En caso de fallo retorna null</returns>

        public List<List<object>> obtenerLaboratorios()
        {
            try
            {
                List<Lugar> lugar = _conexionBD.consultarLugares();
                if ((lugar != null) && (lugar.Count != 0))
                {
                    List<object> nodo = new List<object>();
                    List<List<object>> lugares = new List<List<object>>();
                    for (int i = 0; i < lugar.Count; i++)
                    {
                        if (lugar.ElementAt(i).NombreTipoLugar.Equals("Laboratorio"))
                        {
                            nodo.Add(lugar.ElementAt(i).IdLugar);
                            nodo.Add(lugar.ElementAt(i).NombreLugar);
                            nodo.Add(lugar.ElementAt(i).Capacidad);
                            nodo.Add(lugar.ElementAt(i).Descripcion);
                            nodo.Add(lugar.ElementAt(i).Encargado);
                            lugares.Add(nodo);
                            nodo = new List<object>();
                        }
                    }
                    return lugares;
                } // Luego agregar reporte de error sino se encuentran lugares
                else
                    return null;
            }
            catch (Exception ex)
            {
                _conexionBD.insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }

        /// <summary>
        /// Método que se encarga de realizar el registro del uso de un laboratorio, y al mismo tiempo debuelve los datos para actualizar los datos en pantalla
        /// </summary>
        /// <param name="registro">Lista de objetos (datos del registro)</param>
        /// <returns>Retorna una lista de listas de objetos, en donde cada sublista contiene los datos para actualizar la grafica y el grid de la ventana de registro.
        /// Si ocurre algún problema retorna null</returns>

        public List<List<object>> registrarPeul(List<object> registro)
        {
            try
            {
                Registro registrar = new Registro();
                registrar.IdLugar = Convert.ToInt32(registro[0]);
                registrar.IdEstadoLaboratorio = Convert.ToInt32(registro[1]);
                registrar.CantidadUsuarios = Convert.ToInt32(registro[2]);
                registrar.CantidadPortatiles = Convert.ToInt32(registro[3]);
                registrar.Comentario = registro[4].ToString();
                registrar.Login = registro[5].ToString();
                return _conexionPEULBD.registrarPeul(registrar);
            }
            catch (Exception ex)
            {
                _conexionBD.insertarBitacoraError(ex.ToString(), "");
                return null;
            }
        }

        /// <summary>
        /// Método que se encarga de obtener los estados de los laboratorios
        /// </summary>
        /// <returns>Retorna una lista de lista de Objetos, la cual contiene el PK del estado y la descripción de este</returns>
        
        public List<List<object>> obtenerEstadoLaboratorio()
        {
            return _conexionPEULBD.obtenerEstadoLaboratorio();
        }

		/// <summary>
		/// Método que se encarga de obtener los datos de todos los lugares en el sistema
		/// </summary>
		/// <returns>Retorna un DataTable con los datos o null en caso de error</returns>

		public DataTable obtenerDatosLugares()
		{
			return _conexionBD.obtenerDatosLugares();
		}

        /// <summary>
        /// Método que se utiliza para obtener los datos del uso de los laboratorios
        /// </summary>
        /// <returns>Una lista de listas de objetos, la cual contiene (nombre del laboratorio, login del operador, fecha de ultima actualización, comentario,
        /// cantidad de usuarios, cantidad de laptops) </returns>
        
        public List<List<object>> calcularUsoActual()
        {
            return _conexionPEULBD.calcularUsoActual();
        }

		/// <summary>
		/// Método que se encarga de crear un nuevo lugar en el sistema
		/// </summary>
		/// <param name="nombre">Nombre del nuevo lugar</param>
        /// <param name="loginEncargado">Login del encargado del nuevo lugar</param>
		/// <param name="capacidad">Capacidad del nuevo lugar</param>
		/// <param name="tipo">Tipo del nuevo lugar (aula o laboratorio)</param>
		/// <param name="descripcion">Descripción del nuevo lugar</param>
		/// <param name="login">Usuario logueado actualmente</param>
		/// <returns>Retorna un entero. -1 si el login no corresponde a nadie en el sistema, 0 en caso error en la operación, 1 en caso de éxito</returns>

		public int crearLugar(string nombre, string loginEncargado, string capacidad, string descripcion, int idTipoLugar, string login)
		{
			Lugar nuevoLugar = new Lugar();
			nuevoLugar.NombreLugar = nombre;
			nuevoLugar.LoginEncargado = loginEncargado;
            nuevoLugar.Encargado = _conexionLDAP.obtenerNombrePersona(loginEncargado);
            if (nuevoLugar.Encargado == null) // Si el login proporcionado no corresponde a ningún usuario
                return -1;
			nuevoLugar.Capacidad = Convert.ToInt32(capacidad);
			nuevoLugar.IdTipoLugar = idTipoLugar;
			nuevoLugar.Descripcion = descripcion;
			nuevoLugar.Login = login;
            if (_conexionBD.insertarLugar(nuevoLugar))
                return 1; // En caso de éxito
            else
                return 0; // Si hubo un error al crear el lugar
		}

        /// <summary>
        /// Método que se encarga de modificar los datos de un lugar
        /// </summary>
        /// <param name="pkLugar">Identificador del lugar</param>
        /// <param name="nombre">Nombre del lugar</param>
        /// <param name="loginEncargardo">Encargado del lugar</param>
        /// <param name="capacidad">Capacidad del lugar</param>
        /// <param name="descripcion">Descripción del lugar</param>
        /// <param name="tipo">Tipo del lugar: Aula (true) o Laboratorio (false)</param>
        /// <param name="activo">True si el lugar esta activo, false en caso contrario</param>
		/// <param name="login">Login del usuario que se encuentra logueado</param>
        /// <returns>Retorna un entero. -1 si el login no corresponde a nadie en el sistema, 0 en caso error en la operación, 1 en caso de éxito</returns>

        public int modificarLugar(string pkLugar, string nombre, string loginEncargardo, string capacidad, string descripcion, bool activo, int tipo, string login)
		{
			Lugar lugar = new Lugar();
			lugar.IdLugar = Convert.ToInt32(pkLugar);
			lugar.NombreLugar = nombre;
			lugar.Encargado = _conexionLDAP.obtenerNombrePersona(loginEncargardo);
            if (lugar.Encargado == null)
                return -1;
            lugar.LoginEncargado = loginEncargardo;
			lugar.Capacidad = Convert.ToInt32(capacidad);
			lugar.IdTipoLugar = tipo;
			lugar.Descripcion = descripcion;
			lugar.Login = login;
            lugar.Activo = activo;
            if (_conexionBD.modificarLugar(lugar))
                return 1;
            else
                return 0;
		}

        /// <summary>
        /// Método que se encarga de obtener todos los semestres existentes
        /// </summary>
        /// <returns>Retorna una lista de listas de objetos, en donde cada sublista es un lugar de la forma List(List(IdSemestre:int, NombreSemestre:String
        /// En caso de fallo retorna null</returns>

        public List<List<object>> obtenerSemestres()
        {
            List<Semestre> semestre = _conexionBD.consultarSemestres();
            if ((semestre != null) && (semestre.Count != 0))
            {
                List<object> nodo = new List<object>();
                List<List<object>> Semestres = new List<List<object>>();
                for (int i = 0; i < semestre.Count; i++)
                {
                    nodo.Add(semestre.ElementAt(i).IdSemestre);
                    nodo.Add(semestre.ElementAt(i).NombreSemestre);
                    nodo.Add(semestre.ElementAt(i).FechaInicio);
                    nodo.Add(semestre.ElementAt(i).FechaFinal);
                    nodo.Add(semestre.ElementAt(i).Activo);
                    Semestres.Add(nodo);
                    nodo = new List<object>();
                }
            
                return Semestres;
            } // Luego agregar reporte de error sino se encuentran lugares
            else
                return null;
        }

        /// <summary>
        /// Método que se encarga de insertar los errores que se produscan en el sistema en la tabla BitacoraError
        /// </summary>
        /// <param name="descripcionSis">Descripción del error provista por el sistema</param>
        /// <param name="descripcionUser">Descripción del error provista por el usuario</param>

        public void insertarBitacoraError(string descripcionSis, string descripcionUser)
        {
            _conexionBD.insertarBitacoraError(descripcionSis, descripcionUser);
        }

        /// <summary>
        /// Método que se encarga de consultar los reportes de uso de un laboratorio
        /// </summary>
        /// <param name="pklugar">Lugar a consultar</param>
        /// <param name="fechaInicio">Fecha de inicio del rango</param>
        /// <param name="fechaFinal">Fecha final del rango</param>
        /// <returns></returns>

        public DataTable consultarReportesUso(string pklugar, string fechaInicio, string fechaFinal)
        {
            return _conexionPEULBD.consultarRegistroUso(Convert.ToInt32(pklugar), fechaInicio, fechaFinal);
        }


        /// <summary>
        /// Método que se encarga de consultar  el porcentaje de uso de un laboratorio en un rango de fechas
        /// </summary>
        /// <param name="pklugar">Lugar a consultar</param>
        /// <param name="fechaInicio">Fecha de inicio del rango</param>
        /// <param name="fechaFinal">Fecha final del rango</param>
        /// <returns></returns>
        public DataTable consultarPorcentajeUsoAno(int pklugar, string fechaInicio, string fechaFinal)
        {
            return _conexionPEULBD.consultarPorcentajeUsoAnos(Convert.ToInt32(pklugar), fechaInicio, fechaFinal);
        }

        /// <summary>
        /// Método que se encarga de consultar  el porcentaje de uso por dia de un laboratorio en un rango de fechas
        /// </summary>
        /// <param name="pklugar">Lugar a consultar</param>
        /// <param name="fechaInicio">Fecha de inicio del rango</param>
        /// <param name="fechaFinal">Fecha final del rango</param>
        /// <returns></returns>
        public DataTable consultarPorcentajeUsoDias(int pklugar, string fechaInicio, string fechaFinal)
        {
            return _conexionPEULBD.consultarPorcentajeUsoDias(Convert.ToInt32(pklugar), fechaInicio, fechaFinal);
        }

        /// <summary>
        /// Método que se encarga de consultar  el porcentaje de uso de un laboratorio en meses en un rango de fechas
        /// </summary>
        /// <param name="pklugar">Lugar a consultar</param>
        /// <param name="fechaInicio">Fecha de inicio del rango</param>
        /// <param name="fechaFinal">Fecha final del rango</param>
        /// <returns></returns>
        public DataTable consultarPorcentajeUsoMeses(int pklugar, string fechaInicio, string fechaFinal)
        {
            return _conexionPEULBD.consultarPorcentajeUsoMeses(Convert.ToInt32(pklugar), fechaInicio, fechaFinal);
        }

        /// <summary>
        /// Método que se encarga de consultar  el porcentaje de uso de un laboratorio en semestres en un rango de fechas
        /// </summary>
        /// <param name="pklugar">Lugar a consultar</param>
        /// <param name="fechaInicio">Fecha de inicio del rango</param>
        /// <param name="fechaFinal">Fecha final del rango</param>
        /// <returns></returns>
        public DataTable consultarPorcentajeUsoSemestres(int pklugar, string fechaInicio, string fechaFinal)
        {
            return _conexionPEULBD.consultarPorcentajeUsoSemestres(Convert.ToInt32(pklugar), fechaInicio, fechaFinal);
        }

		/// <summary>
		/// Método que se encarga de consultar todos los tipos de lugar que haya en el sistema
		/// </summary>
		/// <returns>Lista con los nombres de tipos de lugares</returns>

		public List<List<Object>> consultarTipoLugar()
		{
			return _conexionBD.consultarTipoLugar();
		}

        #endregion
    }
}
