using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTICapaLogica.Compartido
{
    public class Semestre
    {
        #region Atributos

        private int _idSemestre; // id del semestre que se referencia
        private int _activo; // Capacidad del mismo
        private String _nombreSemestre; // Nombre del laboratorio o aula
        private DateTime _fechaInicio; // Usuario que esta creando el lugar
        private DateTime _fechaFinal; // Descripción del uso del lugar

        #endregion

        #region Get's y Set's

        public int IdSemestre
        {
            get { return _idSemestre; }
            set { _idSemestre = value; }
        }
        public int Activo
        {
            get { return _activo; }
            set { _activo = value; }
        }
        public String NombreSemestre
        {
            get { return _nombreSemestre; }
            set { _nombreSemestre = value; }
        }
        public DateTime FechaInicio
        {
            get { return _fechaInicio; }
            set { _fechaInicio = value; }
        }
        public DateTime FechaFinal
        {
            get { return _fechaFinal; }
            set { _fechaFinal = value; }
        }
       
        #endregion
    }
}
