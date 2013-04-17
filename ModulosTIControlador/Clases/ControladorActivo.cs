using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaDatos.ModuloActivo;
using ModulosTICapaLogica.Compartido;
using ModulosTICapaLogica.ModuloActivo;


namespace ModulosTIControlador.Clases
{
    public class ControladorActivo
    {
        #region Atributos

        private ManejoBD _conexionBD;
        private ManejoBDActivo _conexionActivoBD;
        private ConexionLDAP _conexionLDAP;

        #endregion

        #region Contructor

        public ControladorActivo()
        {
            _conexionBD = new ManejoBD();
            _conexionActivoBD = new ManejoBDActivo();
            _conexionLDAP = new ConexionLDAP();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método que se encarga de insertar un activo en el sistema
        /// </summary>
        /// <param name="datosActivo">Lista que contiene los datos del activo a crear de la forma:
        /// List(IdTipoActivo:String, Codigo:String, Descripcion:String)</param>
        /// <returns>Un valor int, 1 en caso de éxito, 0 en caso de codigo repetido, -1 error</returns>

        public int insertarActivo(List<string> datosActivo,String login)
        {
            Activo activo = new Activo();
            activo.IdTipoActivo = int.Parse(datosActivo.ElementAt(0));
            activo.Codigo = datosActivo.ElementAt(1);
            activo.Descripcion = datosActivo.ElementAt(2);
            return _conexionActivoBD.insertarActivo(activo,login);
        }

        /// <summary>
        /// Método que se encarga de modificar un activo del sistema
        /// </summary>
        /// <param name="datosSemestre">Lista que contiene los datos del activo a modificar de la forma:
        /// List(IdActivo:String, Descripcion:String, EstadoActivo:String)</param>
        /// <returns>Retorna un int, 1 si fue exitoso, 0 si hubo algun error</returns>

        public int modificarActivo(List<string> datosActivo)
        {
            Activo activo = new Activo();
            activo.IdActivo = Convert.ToInt16(datosActivo.ElementAt(0));
            activo.Descripcion = datosActivo.ElementAt(1);
            activo.EstadoActivo = Convert.ToInt16(datosActivo.ElementAt(2));
            if (_conexionActivoBD.modificarActivo(activo))
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Método que se encarga de obtener todos los tipo de activos del sistema
        /// </summary>
        /// <returns>Retorna una lista de lista de objetos, en donde cada sublista es un Tipo activo de la forma List(List
        /// (IdTipoActivo: String, Descripcion: String)). En caso de fallo retorna null</returns>

        public List<List<object>> obtenerTipoActivo()
        {
            List<List<object>> resultado = _conexionActivoBD.consultarTipoActivo();
            return resultado;
        }

        /// <summary>
        /// Método que se encarga de insertar un movimiento de activo en el sistema
        /// </summary>
        /// <param name="datosMovimientoActivo">Lista que contiene los datos del movimeinto de activo a crear de la forma:
        /// List(IdActivo:String, IdTipoMovimiento:String, Solicitante:String, PostBy:String, Comentario:String)</param>
        /// <returns>Un valor int, 1 en caso de éxito, 0 en caso contrario</returns>

        public int insertarMovimientoActivo(List<string> datosMovimientoActivo)
        {
            Movimiento mov = new Movimiento();
            mov.IdActivo = Convert.ToInt16(datosMovimientoActivo.ElementAt(0));
            mov.IdTipoMovimiento = Convert.ToInt16(datosMovimientoActivo.ElementAt(1));
            mov.Solicitante = datosMovimientoActivo.ElementAt(2);
            mov.PostBy = datosMovimientoActivo.ElementAt(3);
            mov.Comentario = datosMovimientoActivo.ElementAt(4);
            return _conexionActivoBD.insertarMovimientoActivo(mov);
        }

        /// <summary>
        /// Método que se encarga de obtener todos los tipo de movimientos del sistema
        /// </summary>
        /// <returns>Retorna una lista de lista de objetos, en donde cada sublista es un Tipo movimiento de la forma List(List
        /// (IdTipoMovimiento: String, Descripcion: String)). En caso de fallo retorna null</returns>

        public List<List<object>> obtenerTipoMovimiento()
        {
            List<List<object>> resultado = _conexionActivoBD.consultarTipoMovimiento();
            return resultado;
        }

        /// <summary>
        /// Método que se encarga de obtener el id del activo con respecto al codigo
        /// </summary>
        /// <param name="codigo">string del codigo a consultar</param>
        /// <returns>Retorna un int, id del activo si se encuentra, -1 si hubo error, 
        /// -2 si no se encuentra el codigo</returns>

        public int obtenerIdActivo(string codigo)
        {
            int resultado = _conexionActivoBD.consultarIdActivo(codigo);
            return resultado;
        }

        /// <summary>
        /// Método que se encarga de obtener los movimientos de activos con respecto a un estado (prestado o devuelto)
        /// </summary>
        /// <param name="estado">int del estado a consultar</param>
        /// <returns>Retorna una lista de lista de objetos, en donde cada sublista es un movimiento de activo de la forma List(List
        /// ()). En caso de fallo retorna null</returns>

        public DataTable consultarActivoPorEstado(int estado)
        {
            return _conexionActivoBD.consultarActivoPorEstado(estado);
            //return resultado;
        }

        // <summary>
        /// Método que se encarga de obtener los movimientos de activos con respecto a una descripcion
        /// </summary>
        /// <param name="descrip">string de la descripcion a consultar</param>
        /// <returns>Retorna una lista de lista de objetos, en donde cada sublista es un movimiento de activo de la forma List(List
        /// ()). En caso de fallo retorna null</returns>

        public DataTable consultarActivoPorDescripcion(string descrip)
        {
            return _conexionActivoBD.consultarActivoPorDescripcion(descrip);
        }

        // <summary>
        /// Método que se encarga de obtener los movimientos de activos con respecto a una codigo y fecha
        /// </summary>
        /// <param name="codigo">string de la codigo a consultar</param>
        /// <returns>Retorna una lista de lista de objetos, en donde cada sublista es un movimiento de activo de la forma List(List
        /// ()). En caso de fallo retorna null</returns>

        public DataTable consultarActivoPorCodigo(string codigo)
        {
            return _conexionActivoBD.consultarActivoPorCodigo(codigo);
            //return resultado;
        }

        // <summary>
        /// Método que se encarga de obtener los movimientos de activos con respecto a una codigo y fecha
        /// </summary>
        /// <param name="codigo">string de la codigo a consultar</param>
        /// <returns>Retorna una lista de lista de objetos, en donde cada sublista es un movimiento de activo de la forma List(List
        /// ()). En caso de fallo retorna null</returns>

        public List<List<object>> consultarActivoPorCodigo2(string codigo)
        {
            return _conexionActivoBD.consultarActivoPorCodigo2(codigo);
            //return resultado;
        }

        public Boolean verificarProfesor(string login)
        {
            return _conexionLDAP.verificarProfesor(login);
        }

        #endregion
    }
}
