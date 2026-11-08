using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class Impresora3D
{
    public Impresora3D()
    {
    }

    public Impresora3D(
        int id,
        string nombre,
        string modelo,
        string tipo,
        string descripcion,
        decimal precioKilovatioHora,
        decimal precioReserva)
    {
        Id = id;
        Nombre = nombre;
        Modelo = modelo;
        Tipo = tipo;
        Descripcion = descripcion;
        PrecioKilovatioHora = precioKilovatioHora;
        PrecioReserva = precioReserva;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Modelo { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Tipo { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string Descripcion { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioKilovatioHora { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioReserva { get; set; }

    public IList<LineaReserva> LineasReserva { get; set; }
        = new List<LineaReserva>();

}
