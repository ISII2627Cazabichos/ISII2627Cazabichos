namespace AppForSEII.API.Models
{
   public class CompraModelo3D
    {
        public int Id { get; set; }
        public DateTime FechaCompra { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidoCliente { get; set; }
        public string CorreoElectronico { get; set; }
        public string DireccionFacturacion { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioTotal { get; set; }
        public MetodoPago MetodoPago { get; set; }
    }
}