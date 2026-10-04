using System.Collections.Generic;
namespace AppForSEII.API.Models
{
    public class LineaCompraModelo3D
    {
        public int Id { get; set; }
        public int CantidadLicencias { get; set; }
        [Precision(5, 2)]
        public decimal PrecioUnidad { get; set; }
        [Precision(5, 2)]
        public decimal Subtotal { get; set; }
        public CompraModelo3D? CompraModelo3D { get; set; }
        public Modelo3D? Modelo3D { get; set; }


    } 
       
      
}