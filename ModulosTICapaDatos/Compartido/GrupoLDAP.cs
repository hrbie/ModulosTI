using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTICapaDatos.Compartido
{
	public class GrupoLDAP
	{
		// Atributos

		private String _nombreGrupo;

		// GET's y SET's

		public String NombreGrupo
		{
			get { return _nombreGrupo; }
			set { _nombreGrupo = value; }
		}

		// Constructor

        public GrupoLDAP()
        {
        }
    }
}
