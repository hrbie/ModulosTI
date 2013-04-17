using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTICapaLogica.ModuloReservacion
{
    public class Reservacion
	{
		#region Atributos

        private int _idLugar; // Id del lugar que se va a rerservar
        private int _idCurso; // Id del curso sobre el cual se desea realizar la reservación
        private List<char> _dia; // Días sobre los cuales se hace la reservación
        private DateTime _horaInicio; // Hora de inicio para la reservación
        private DateTime _horaFinal; // Hora final para la reservación
        private DateTime _fechaInicio; // Fecha de inicio para la reservación
        private DateTime _fechaFinal; // Fecha final para la reservación
        private String _solicitante; // Nombre del solicitante
        private String _loginSolicitante; // Login del usuario solicitante
        private String _descripcion; // Descripción de la reservación

		#endregion

		#region Get's y Set's


		public int IdLugar
        {
            get { return _idLugar; }
            set { _idLugar = value; }
        }

        public int IdCurso
        {
            get { return _idCurso; }
            set { _idCurso = value; }
        }
		public List<char> Dia
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
        public String Solicitante
        {
            get { return _solicitante; }
            set { _solicitante = value; }
        }
        public String LoginSolicitante
        {
            get { return _loginSolicitante; }
            set { _loginSolicitante = value; }
        }
        public String Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
		}

		#endregion
	}
}
