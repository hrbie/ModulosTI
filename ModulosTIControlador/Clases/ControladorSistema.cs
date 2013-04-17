using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaLogica.Compartido;
using ModulosTICapaDatos.ModuloSistema;

namespace ModulosTIControlador.Clases
{
    public class ControladorSistema
    {
        #region Atributos

		private ManejoBD _conexionBD;
		private ManejoBDSistema _sistemaBD;

		#endregion

		#region Contructor

		public ControladorSistema()
		{
			_conexionBD = new ManejoBD();
            _sistemaBD = new ManejoBDSistema();
        }


        #endregion

        #region Métodos

        /// <summary>
        /// Método que se encarga de obtener las entradas de errores que se han dado en la plataforma, pueden o no usarse filtros
        /// </summary>
        /// <param name="fechaInicio">Fecha inicio para buscar eventos</param>
        /// <param name="fechaFinal">Fecha Final para buscar eventos</param>
        /// <param name="estado">Estados de los filtros a buscar</param>
        /// <returns>Retorna una lista de listas de objetos de la forma (PK_Error:int, Fecha:DateTime, DescripciónSis:string, DescripciónUs:string, 
        ///          Estado:int)</returns>

        public List<List<object>> obtenerEntradasBitError(string fechaInicio, string fechaFinal, int estado)
        {
            List<List<object>> resultado = _sistemaBD.obtenerEntradasBitError(fechaInicio, fechaFinal, estado);
            return resultado;
        }

        /// <summary>
        /// Método que solicita modificar una entrada la bitácora de error
        /// </summary>
        /// <param name="idBitError">PK de la entrada a modificar</param>
        /// <param name="estado">Estado de la entrada</param>
        /// <returns>Retorna una valor booleano indicando si tuvo éxito (true) o no (false)</returns>

        public Boolean modificarEntradaBitError(int idBitError, int estado)
        {
            return _sistemaBD.modificarBitacoraError(idBitError, estado);
        }

        /// <summary>
        /// Método que solicita borrar una entrada de la bitácora de error
        /// </summary>
        /// <param name="idBitError">PK de la entrada a eliminar</param>
        /// <returns>Retorna una valor booleano indicando si tuvo éxito (true) o no (false)</returns>

        public Boolean eliminarBitError(int idBitError)
        {
            return _sistemaBD.eliminarBitacoraError(idBitError);
        }

        /// <summary>
        /// Método que se encarga de crear un periodo lectivo en el sistema
        /// </summary>
        /// <param name="datosSemestre">Lista que contiene los datos del periodo lectivo a crear de la forma:
        /// List(NombreSemestre:String, FechaInicio:String, FechaFinal:String)</param>
        /// <returns>Un valor int, 1 en caso de éxito, 0 en caso contrario</returns>

        public int crearSemestre(List<string> datosSemestre)
        {
            Semestre semestre = new Semestre();
            semestre.NombreSemestre = datosSemestre.ElementAt(0);
            semestre.FechaInicio = Convert.ToDateTime(datosSemestre.ElementAt(1));
            semestre.FechaFinal = Convert.ToDateTime(datosSemestre.ElementAt(2));
            return _sistemaBD.crearSemestre(semestre);
        }

        /// <summary>
        /// Método que se encarga de obtener todos los periodos lectivos del sistema
        /// </summary>
        /// <returns>Retorna una lista de tipo Semestre, en donde cada elemento es un Perido Lectivo de la forma List(IdSemstre: int,
        /// NombreSemestre: String, FechaInicio: DateTime, FechaFinal: DateTime, Activo: int)). En caso de fallo retorna null</returns>

        public List<Semestre> obtenerSemestres()
        {
            List<Semestre> resultado = _conexionBD.consultarSemestres();
            return resultado;
        }

        /// <summary>
        /// Método que se encarga de verificar la existencia de un periodo lectivo del sistema
        /// </summary>
        /// <param name="nombre">string que contiene el nombre del periodo lectivo a verificar</param>
        /// <returns>Retorna un booleano, true si existe, false si no existe</returns>

        public bool verificarNombreSemestres(string nombre)
        {
            List<Semestre> resultado = _sistemaBD.verificarNombreSemestres(nombre);
            if (resultado.Count == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Método que se encarga de modificar un periodo lectivo del sistema
        /// </summary>
        /// <param name="datosSemestre">Lista que contiene los datos del periodo lectivo a modificar de la forma:
        /// List(NombreSemestre:String, FechaInicio:String, FechaFinal:String)</param>
        /// <returns>Retorna un int, 1 si fue exitoso, 0 si hubo algun error</returns>

        public int modificarSemestre(List<string> datosSemestre)
        {
            Semestre semestre = new Semestre();
            semestre.IdSemestre = Convert.ToInt16(datosSemestre.ElementAt(0));
            semestre.NombreSemestre = datosSemestre.ElementAt(1);
            semestre.FechaInicio = Convert.ToDateTime(datosSemestre.ElementAt(2));
            semestre.FechaFinal = Convert.ToDateTime(datosSemestre.ElementAt(3));
            semestre.Activo = Convert.ToInt16(datosSemestre.ElementAt(4));
            if (_sistemaBD.modificarSemestre(semestre))
                return 1;
            else
                return 0;
        }

        #endregion
    }
}
