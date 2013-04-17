using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTICapaLogica.ModuloSGC
{
	public class ErroresExcel
	{
		int _linea;
		String _dato;
		String _mensaje;

		public int linea
		{
			get { return _linea; }
			set { _linea = value; }
		}

		public String dato
		{
			get { return _dato; }
			set { _dato = value; }
		}

		public String mensaje
		{
			get { return _mensaje; }
			set { _mensaje = value; }
		}
	}
}
