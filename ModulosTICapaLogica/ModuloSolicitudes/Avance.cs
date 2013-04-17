using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTICapaLogica.ModuloSolicitudes
{
    public class Avance
    {
        private int _idSolicitud;
        private String _descripcion;
        private String _postBy;
        private DateTime _fecha;

        public int ID
        {
            get { return _idSolicitud; }
            set { _idSolicitud = value; }
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

        public DateTime fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }
    }
}
