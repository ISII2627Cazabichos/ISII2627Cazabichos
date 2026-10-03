namespace AppForSEII.API.Models
{
    public class lineaCompraModelo3D
    {
        public int Id { get; set; }
        public int CantidadLicencias { get; set; }
        [Precision(5, 2)]
        public decimal PrecioUnidad { get; set; }
        [Precision(5, 2)]
        public decimal Subtotal { get; set; }
    } 
}