namespace AppForSEII.API.Models
{
    public class LicenciaModelo3D
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Nombre { get; set; }= string.Empty;
        public DateTime FechaExpiracion { get; set; }
 
    }
}