using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTICapaLogica.ModuloHorario
{
    public class Turno
    {
        #region Atributos

        private int _idHorario; // Id del horario al que se le agrega el turno
        private int _idTurno; // Id del turno que se esta manejando
        private char _dia; // Día que se esta especificando el turno
        private DateTime _horaInicio; // Hora de inicio del turno
        private DateTime _horaFinal; // Hora final del turno
        private String _nombrePersona; // Nombre de la persona que está asociando a ese turno

        #endregion

        #region Get's y Set's

        public int IdHorario
        {
            get { return _idHorario; }
            set { _idHorario = value; }
        }

        public int IdTurno
        {
            get { return _idTurno; }
            set { _idTurno = value; }
        }

        public char Dia
        {
            get { return _dia; }
            set { _dia = value; }
        }

        public DateTime HoraInicio
        {
            get { return _horaInicio; }
            set { _horaInicio = value; }
        }

        public DateTime HoraFinal
        {
            get { return _horaFinal; }
            set { _horaFinal = value; }
        }

        public String NombrePersona
        {
            get { return _nombrePersona; }
            set { _nombrePersona = value; }
        }

        #endregion
    }
}
