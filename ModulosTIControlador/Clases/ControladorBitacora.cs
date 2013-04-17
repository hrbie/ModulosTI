using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaDatos.ModuloBitacora;
using ModulosTICapaLogica.Compartido;
using ModulosTICapaLogica.ModuloBitacora;

namespace ModulosTIControlador.Clases
{
    public class ControladorBitacora
    {
        #region Atributos

        private ManejoBD _conexionBD;
        private ManejoBDBitacora _conexionBitacoraBD;

        #endregion

        #region Contructor

        public ControladorBitacora()
        {
            _conexionBD = new ManejoBD();
            _conexionBitacoraBD = new ManejoBDBitacora();
        }

        #endregion

        #region Métodos

            /// <summary>
            /// Método que se encarga de consultar  los eventos registrados en la bitacora en una fecha
            /// </summary>
            /// <param name="pklugar">Lugar a consultar</param>
            /// <param name="fechaInicio">Fecha que se desea consultar</param>
            /// <returns>DataTable con los eventos obtenidos</returns>
            public DataTable consultarEntradaPorDia(int pklugar, string fecha)
            {
                return _conexionBitacoraBD.ConsultarEntradaPorDia(Convert.ToInt32(pklugar), (Convert.ToDateTime(fecha)));
            }

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
            /// Método que se encarga de insertar los errores que se produscan en el sistema en la tabla BitacoraError
            /// </summary>
            /// <param name="descripcionSis">Descripción del error provista por el sistema</param>
            /// <param name="descripcionUser">Descripción del error provista por el usuario</param>

            public void insertarBitacoraError(string descripcionSis, string descripcionUser)
            {
                _conexionBD.insertarBitacoraError(descripcionSis, descripcionUser);
            }

            /// <summary>
            /// Método que ingresa una nueva sesion en la base de datos
            /// </summary>
            /// <param name="login">Usuario que inicia la sesion</param>

            public int insertarSesionBitacora(string login)
            {
                int idSesion = _conexionBitacoraBD.insertarSesionBitacora(login);
                return idSesion;
            }

            /// <summary>
            /// Método que se encarga de insertar los eventos en la bitacora
            /// </summary>
            /// <param name="idSesion">Sesion actual de bitacora</param>
            /// <param name="idLugar">Lugar donde se dese registrar el evento</param>
            /// <param name="evento">Descripción del evento</param>
            /// <param name="login">Usuario que registra el evento</param>

            public int insertarEventoBitacora(int idSesion, int idLugar, string descripcion, string login)
            {
                Evento evento = new Evento();
                evento.Descripcion = descripcion;
                evento.Operador = login;
                evento.IdSesion = idSesion;
                evento.IdLugar = idLugar;
                return _conexionBitacoraBD.insertarEventoBitacora(evento);
            }

            /// <summary>
            /// Método que se encarga de cargar las entradas registradas en una sesión
            /// </summary>
            /// <param name="idSesion">Sesión actual de bitacora</param>
            /// <returns> DataTable con las entradas registradas en la sesión</returns>

            public DataTable ConsultarEntradaPorSesion(int idSesion, int idLugar)
            {
                return _conexionBitacoraBD.ConsultarEntradaPorSesion(idSesion, idLugar);
            }

            /// <summary>
            /// Método que se encarga de modificar un evento durante una sesión
            /// </summary>
            /// <returns>Lista con los nombres de tipos de lugares</returns>

            public int ModificarEvento(string login, int idSesion, int idEntrada, string evento)
            {
                return _conexionBitacoraBD.ModificarEvento(login, idSesion, idEntrada, evento);
            }

            /// <summary>
            /// Método que se encarga de cerrar una sesión
            /// </summary>
            /// <returns>Bool true si cerra coractemente la sesion o false si no logra cerrar la sesion</returns>
             public bool cerrarSesion(int idSesion, string login)
             {
                    return _conexionBitacoraBD.cerrarSesion(idSesion, login);
             }

            
        #endregion
    }
}
