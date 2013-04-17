using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTIControlador.Clases
{
	public class Registro
	{
		#region Atributos

		private int      _idLugar;
		private int      _idEstadoLaboratorio;
		private int      _cantidadUsuarios;
		private int      _cantidadPortatiles;
		private String   _comentario;
		private String   _login;
		private DateTime _postDate;

		#endregion

		#region Constructor

		public Registro()
		{
			
		}

		#endregion

		#region Get's y Set's

		public int IdLugar
		{
			get { return _idLugar; }
			set { _idLugar = value; }
		}

		public int IdEstadoLaboratorio
		{
			get { return _idEstadoLaboratorio; }
			set { _idEstadoLaboratorio = value; }
		}

		public int CantidadUsuarios
		{
			get { return _cantidadUsuarios; }
			set { _cantidadUsuarios = value; }
		}

		public int CantidadPortatiles
		{
			get { return _cantidadPortatiles; }
			set { _cantidadPortatiles = value; }
		}
		
		public String Comentario
		{
			get { return _comentario; }
			set { _comentario = value; }
		}
		
		public String Login
		{
			get { return _login; }
			set { _login = value; }
		}
		
		public DateTime PostDate
		{
			get { return _postDate; }
			set { _postDate = value; }
		}

		#endregion
	}
}
