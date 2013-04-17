using System;

namespace ModulosTICapaLogica.Compartido
{
    public class Usuario
    {
        #region Atributos

        private String _nombre;
		private String _apellidos;
        private String _carnet;
        private String _telefonoCasa;
        private String _telefonoCelular;
        private String _correo;
		private String _uid;
		private String _contrasena;
        private String _grupo;
        private String _carrera;
		
		#endregion

		#region Propiedades

        public String Nombre
        {
            get { return _nombre;  }
            set { _nombre = value; }
        }

		public String Apellidos
		{
			get { return _apellidos;  }
			set { _apellidos = value; }
		}

		public String Carnet
		{
			get { return _carnet; }
			set { _carnet = value; }
		}

        public String TelefonoCasa
        {
            get { return _telefonoCasa;  }
            set { _telefonoCasa = value; }
        }

        public String TelefonoCelular
        {
            get { return _telefonoCelular;  }
            set { _telefonoCelular = value; }
        }

        public String Correo
        {
            get { return _correo;  }
            set { _correo = value; }
		}

		public String UID
		{
			get { return _uid; }
			set { _uid = value; }
		}

		public String Contrasena
		{
			get { return _contrasena; }
			set { _contrasena = value; }
		}

        public String Grupo
        {
            get { return _grupo; }
            set { _grupo = value; }
        }

        public String Carrera
        {
            get { return _carrera; }
            set { _carrera = value; }
        }

		#endregion
	}
}
