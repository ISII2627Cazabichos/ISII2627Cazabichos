
using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.EncargoDTOs
{
    public class EncargoCrearDTO
    {
        [Required]
        public string ClienteId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ApellidosCliente { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string DireccionEnvio { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string NumeroTelefono { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        [Required]
        [MinLength(1)]
        public IList<LineaEncargoCrearDTO> LineasEncargo { get; set; }
            = new List<LineaEncargoCrearDTO>();
    }

    public class LineaEncargoCrearDTO
    {
        [Range(1, int.MaxValue)]
        public int PiezaId { get; set; }

        [Range(1, int.MaxValue)]
        public int MaterialId { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }
    }
}
