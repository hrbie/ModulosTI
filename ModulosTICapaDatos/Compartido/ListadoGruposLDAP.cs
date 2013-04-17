using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTICapaDatos.Compartido
{
    public class ListadoGruposLDAP
	{
		#region Atributos

		private GrupoLDAP _profesor;
		private GrupoLDAP _people;
		private GrupoLDAP _estudiante;
		private GrupoLDAP _operador;
		private GrupoLDAP _soporte;
		private GrupoLDAP _jefe;
		private GrupoLDAP _jefeti;

		#endregion

		#region Get's y Set's

		public GrupoLDAP Profesor
		{
			get { return _profesor; }
			set { _profesor = value; }
		}

		public GrupoLDAP People
		{
			get { return _people; }
			set { _people = value; }
		}

		public GrupoLDAP Estudiante
		{
			get { return _estudiante; }
			set { _estudiante = value; }
		}

		public GrupoLDAP Operador
		{
			get { return _operador; }
			set { _operador = value; }
		}

		public GrupoLDAP Soporte
		{
			get { return _soporte; }
			set { _soporte = value; }
		}

		public GrupoLDAP Jefe
		{
			get { return _jefe; }
			set { _jefe = value; }
		}

		public GrupoLDAP Jefeti
		{
			get { return _jefeti; }
			set { _jefeti = value; }
		}

		#endregion

		#region Constructor

		public ListadoGruposLDAP()
		{
			_profesor = new GrupoLDAP();
			_people = new GrupoLDAP();
			_estudiante = new GrupoLDAP();
			_operador = new GrupoLDAP();
			_soporte = new GrupoLDAP();
			_jefe = new GrupoLDAP();
			_jefeti = new GrupoLDAP();
			_profesor.NombreGrupo = "prof";
			_people.NombreGrupo = "users"; 
			_estudiante.NombreGrupo = "ests"; // Este se pone porque los estudiantes del 2005 para arriba no están en este grupo sino en users
			_operador.NombreGrupo = "operadores";
			_soporte.NombreGrupo = "soporte";
			_jefe.NombreGrupo = "jefes";
			_jefeti.NombreGrupo = "jefeti"; // Profesor encargado de la Oficina de TI
		}

		#endregion

		#region Métodos

		/// <summary>
		/// Método que se encarga de retornar todos los grupos existen en el sistema
		/// </summary>
		/// <returns>Lista que contiene tipos GrupoLDAP con todos los grupos del sistema</returns>

        public List<GrupoLDAP> obtenerGruposLDAP()
        {
            List<GrupoLDAP> result = new List<GrupoLDAP>();
			result.Add(Jefeti);
			result.Add(Soporte);
			result.Add(Jefe);
            result.Add(Operador);
            result.Add(Profesor);           
			result.Add(People);
            //result.Add(Estudiante);
			
            return result;
		}

		#endregion
	}
}
