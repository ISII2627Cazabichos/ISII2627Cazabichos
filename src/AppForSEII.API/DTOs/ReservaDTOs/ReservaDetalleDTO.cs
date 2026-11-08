namespace AppForSEII.API.DTOs.ReservaDTOs;

public class ReservaDetalleDTO
{
    public ReservaDetalleDTO(
        int id,
        DateTime fechaReserva,
        string nombreCliente,
        string apellidosCliente,
        string direccionFacturacion,
        decimal precioTotal,
        string metodoPago,
        IList<ReservaItemDTO> lineasReserva)
    {
        Id = id;
        FechaReserva = fechaReserva;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionFacturacion = direccionFacturacion;
        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;
        LineasReserva = lineasReserva;
    }

    public int Id { get; set; }
    public DateTime FechaReserva { get; set; }
    public string NombreCliente { get; set; }
    public string ApellidosCliente { get; set; }
    public string DireccionFacturacion { get; set; }
    public decimal PrecioTotal { get; set; }
    public string MetodoPago { get; set; }
    public IList<ReservaItemDTO> LineasReserva { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is ReservaDetalleDTO dto &&
               Id == dto.Id &&
               FechaReserva == dto.FechaReserva &&
               NombreCliente == dto.NombreCliente &&
               ApellidosCliente == dto.ApellidosCliente &&
               DireccionFacturacion == dto.DireccionFacturacion &&
               PrecioTotal == dto.PrecioTotal &&
               MetodoPago == dto.MetodoPago &&
               LineasReserva.SequenceEqual(dto.LineasReserva);
    }

    public override int GetHashCode() =>
        HashCode.Combine(Id, FechaReserva, NombreCliente, ApellidosCliente, DireccionFacturacion, PrecioTotal, MetodoPago);
}
