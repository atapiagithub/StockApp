using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockApp.Models
{
    public class ProductoModel
    {
        private int idProducto;
        private string _Codigo;
        private string _Descripcion;
        private int _Cantidad;

        public string Codigo { get => _Codigo; set => _Codigo = value; }
        public string Descripcion { get => _Descripcion; set => _Descripcion = value; }
        public int Cantidad { get => _Cantidad; set => _Cantidad = value; }
        public int IdProducto { get => idProducto; set => idProducto = value; }

        public ProductoModel()
        {
            
        }

        public ProductoModel(int pCantidad, string pCodigo, string pDescripcion)
        {
            Cantidad = pCantidad;
            Descripcion = pDescripcion;
            Codigo = pCodigo;
        }

    }
}
