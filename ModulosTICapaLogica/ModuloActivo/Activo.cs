namespace ModulosTICapaLogica.ModuloActivo
{
    public class Activo
    {
        #region Atributos

        private string _descripcion;
        private string _codigo;
        private int _idActivo;
        private int _idTipoActivo;
        private int _estadoActivo;

        #endregion

        public int IdActivo
        {
            get { return _idActivo; }
            set { _idActivo = value; }
        }

        public int IdTipoActivo
        {
            get { return _idTipoActivo; }
            set { _idTipoActivo = value; }
        }

        public int EstadoActivo
        {
            get { return _estadoActivo; }
            set { _estadoActivo = value; }
        }

        public string Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        public string Descripcion
        {
            get { return _descripcion; }
            set { _descripcion = value; }
        }
    }
}
