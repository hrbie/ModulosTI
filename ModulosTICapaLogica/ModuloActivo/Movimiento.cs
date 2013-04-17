using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModulosTICapaLogica.ModuloActivo
{
    public class Movimiento
    {
        #region Atributos

        private DateTime _postDate;
        private string _postBy;
        private string _solicitante;
        private string _comentario;
        private int _idMovimientoActivo;
        private int _idActivo;
        private int _idTipoMovimiento;

        #endregion

        public int IdActivo
        {
            get { return _idActivo; }
            set { _idActivo = value; }
        }

        public int IdTipoMovimiento
        {
            get { return _idTipoMovimiento; }
            set { _idTipoMovimiento = value; }
        }

        public int IdMovimientoActivo
        {
            get { return _idMovimientoActivo; }
            set { _idMovimientoActivo = value; }
        }

        public string PostBy
        {
            get { return _postBy; }
            set { _postBy = value; }
        }

        public string Solicitante
        {
            get { return _solicitante; }
            set { _solicitante = value; }
        }

        public string Comentario
        {
            get { return _comentario; }
            set { _comentario = value; }
        }

        public DateTime PostDate
        {
            get { return _postDate; }
            set { _postDate = value; }
        }

    }
}
