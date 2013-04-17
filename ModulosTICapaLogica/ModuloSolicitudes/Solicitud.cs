using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTICapaLogica.ModuloSolicitudes
{
    public class Solicitud
    {
        private int _id;
        private String _responsable;
        private DateTime _fechaSolicitud;
        private DateTime _fechaFin;
        private String _solicitante;
        private String _asunto;
        private String _descripcion;
        private String _postBy;
        private String _estado;
        private String _login;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public String Responsable
        {
            get { return _responsable; }
            set { _responsable = value; }
        }

        public DateTime FechaSolicitud
        {
            get { return _fechaSolicitud; }
            set { _fechaSolicitud = value; }
        }

        public DateTime FechaFin
        {
            get { return _fechaFin; }
            set { _fechaFin = value; }
        }

        public String Solicitante
        {
            get { return _solicitante; }
            set { _solicitante = value; }
        }

        public String Asunto
        {
            get { return _asunto; }
            set { _asunto = value; }
        }

        public String Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }

        public String PostBy
        {
            get { return _postBy; }
            set { _postBy = value; }
        }

        public String Estado
        {
            get { return _estado; }
            set { _estado = value;}
        }

        public String Login
        {
            get { return _login; }
            set { _login = value; }
        }
    }
}
