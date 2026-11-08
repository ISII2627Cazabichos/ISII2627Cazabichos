using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models;

public class ReservaImpresora
{
    public ReservaImpresora()
    {
    }

    public ReservaImpresora(
        DateTime fechaReserva,
        string nombreCliente,
        string apellidosCliente,
        string direccionFacturacion,
        MetodoPago metodoPago,
        Cliente cliente)
    {
        FechaReserva = fechaReserva;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionFacturacion = direccionFacturacion;
        MetodoPago = metodoPago;
        ClienteId = cliente.Id;
        Cliente = cliente;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime FechaReserva { get; set; }

    [Required]
    [StringLength(50)]
    public string NombreCliente { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string ApellidosCliente { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string DireccionFacturacion { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal PrecioTotal { get; set; }

    [Required]
    [StringLength(50)]
    public MetodoPago MetodoPago { get; set; }

    [Required]
    public string ClienteId { get; set; } = string.Empty;

    [Required]
    public Cliente Cliente { get; set; } = null!;

    public IList<LineaReserva> LineasReserva { get; set; }
        = new List<LineaReserva>();
}
