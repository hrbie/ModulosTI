using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTICapaLogica.Compartido
{
	public class Lugar
	{
		#region Atributos

		private int _idLugar; // id del lugar que se referencia
		private int _capacidad; // Capacidad del mismo
		private int _idTipoLugar; // Id del TipoLugar al que pertenece el Lugar
		private String _nombreLugar; // Nombre del laboratorio o aula
		private String _login; // Usuario que esta creando el lugar
		private String _descripcion; // Descripción del uso del lugar
		private String _encargado; // Encargado de laboratorio o aula
        private String _loginEncargado; // Login del encargado del Lugar
		private String _nombreTipoLugar; // Nombre del TipoLugar
        private Boolean _activo; // False en caso de que el lugar no este activo, true en caso contrario

		#endregion

		#region Get's y Set's

		public int IdLugar
		{
			get { return _idLugar; }
			set { _idLugar = value; }
		}
		public int Capacidad
		{
			get { return _capacidad; }
			set { _capacidad = value; }
		}
		public int IdTipoLugar
		{
			get { return _idTipoLugar; }
			set { _idTipoLugar = value; }
		}
		public String NombreLugar
		{
			get { return _nombreLugar; }
			set { _nombreLugar = value; }
		}
		public String Login
		{
			get { return _login; }
			set { _login = value; }
		}
		public String Descripcion
		{
			get { return _descripcion; }
			set { _descripcion = value; }
		}
		public String NombreTipoLugar
		{
			get { return _nombreTipoLugar; }
			set { _nombreTipoLugar = value; }
		}
		public String Encargado
		{
			get { return _encargado; }
			set { _encargado = value; }
		}
        public String LoginEncargado
        {
            get { return _loginEncargado; }
            set { _loginEncargado = value; }
        }
        public Boolean Activo
        {
            get { return _activo; }
            set { _activo = value; }
        }

		#endregion
	}
}
