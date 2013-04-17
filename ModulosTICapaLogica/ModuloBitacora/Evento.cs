namespace ModulosTICapaLogica.ModuloBitacora
{
    public class Evento
    {
        #region Atributos

        private string _operador;
        private string _descripcion;
        private int _idSesion;
        private int _idLugar;

        #endregion

        public int IdSesion
        {
            get { return _idSesion; }
            set { _idSesion = value; }
        }

        public int IdLugar
        {
            get { return _idLugar; }
            set { _idLugar = value; }
        }

        public string Operador
        {
            get { return _operador; }
            set { _operador = value; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }
    }
}