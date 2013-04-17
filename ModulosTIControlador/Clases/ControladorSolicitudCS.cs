using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModulosTICapaDatos.Compartido;
using ModulosTICapaDatos.ModuloSolicitudes;
using ModulosTICapaLogica.ModuloSolicitudes;

namespace ModulosTIControlador.Clases
{
    
    public class ControladorSolicitudCS
    {
        private ManejoBDSolicitudes _manejoBD;
        private ConexionLDAP _ldap;

        public ControladorSolicitudCS()
        {
            _manejoBD = new ManejoBDSolicitudes();
            _ldap = new ConexionLDAP();
        }

        /// <summary>
        /// Crea un objeto solicitud y se lo envía el método insertarSolicitud de la capa de Datos
        /// </summary>
        /// <param name="asunto"></param>
        /// <param name="descripcion"></param>
        /// <param name="login"></param>

        public bool crearSolicitud(String asunto, String descripcion, String login, String usuario)
        {
            Solicitud solicitud = new Solicitud();
            solicitud.Asunto = asunto;
            solicitud.Descripcion = descripcion;
            solicitud.PostBy = usuario;
            solicitud.Login = login;
            solicitud.Solicitante = _ldap.obtenerNombrePersona(login);

            return _manejoBD.insertarSolicitud(solicitud);
        }
        
    }
}
