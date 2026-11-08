using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaDTOs;

public class ReservaCrearDTO
{
    public ReservaCrearDTO(
        string clienteId,
        string nombreCliente,
        string apellidosCliente,
        string direccionFacturacion,
        MetodoPago metodoPago,
        IList<LineaReservaCrearDTO> lineasReserva)
    {
        ClienteId = clienteId;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionFacturacion = direccionFacturacion;
        MetodoPago = metodoPago;
        LineasReserva = lineasReserva;
    }

    [Required]
    public string ClienteId { get; set; }

    [Required]
    [StringLength(50)]
    public string NombreCliente { get; set; }

    [Required]
    [StringLength(50)]
    public string ApellidosCliente { get; set; }

    [Required]
    [StringLength(200)]
    public string DireccionFacturacion { get; set; }

    [Required]
    public MetodoPago MetodoPago { get; set; }

    [Required]
    [MinLength(1)]
    public IList<LineaReservaCrearDTO> LineasReserva { get; set; }
}

public class LineaReservaCrearDTO
{
    public LineaReservaCrearDTO(int impresora3DId, TiempoReserva tiempoReserva)
    {
        Impresora3DId = impresora3DId;
        TiempoReserva = tiempoReserva;
    }

    [Range(1, int.MaxValue)]
    public int Impresora3DId { get; set; }

    public TiempoReserva TiempoReserva { get; set; }
}
