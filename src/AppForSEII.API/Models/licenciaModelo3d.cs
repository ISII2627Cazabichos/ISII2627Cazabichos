namespace AppForSEII.API.Models
{
    public class LicenciaModelo3D
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Nombre { get; set; }= string.Empty;
        public DateTime FechaExpiracion { get; set; }
        public Modelo3D Modelo3D { get; set; }
 
        public CompraModelo3D CompraModelo3D { get; set; }
 
    }
}