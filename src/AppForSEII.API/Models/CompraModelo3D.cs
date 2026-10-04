using System.Collections.Generic;
namespace AppForSEII.API.Models

{
   public class CompraModelo3D
    {
        public int Id { get; set; }
        public DateTime FechaCompra { get; set; }
        [StringLength(50)]
        public string NombreCliente { get; set; }= string.Empty;
        [StringLength(50)]
        public string ApellidoCliente { get; set; }= string.Empty;
        [EmailAddress]
        public string CorreoElectronico { get; set; }= string.Empty;
        [StringLength(50)]
        public string DireccionFacturacion { get; set; }= string.Empty;
        [StringLength(50)]
        public string Descripcion { get; set; }= string.Empty;
        [Precision(5, 2)]
        public decimal PrecioTotal { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public ICollection<LineaCompraModelo3D> LineasCompra { get; set; }
           = new List<LineaCompraModelo3D>();   
        public Cliente? Cliente { get; set; }
        
    }
      
}