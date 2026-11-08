namespace AppForSEII.API.DTOs.ReservaDTOs;

public class ReservaItemDTO
{
    public ReservaItemDTO(
        int id,
        int impresoraId,
        string nombreImpresora,
        string modeloImpresora,
        string tiempoReserva,
        decimal precioSubtotal)
    {
        Id = id;
        ImpresoraId = impresoraId;
        NombreImpresora = nombreImpresora;
        ModeloImpresora = modeloImpresora;
        TiempoReserva = tiempoReserva;
        PrecioSubtotal = precioSubtotal;
    }

    public int Id { get; set; }
    public int ImpresoraId { get; set; }
    public string NombreImpresora { get; set; }
    public string ModeloImpresora { get; set; }
    public string TiempoReserva { get; set; }
    public decimal PrecioSubtotal { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is ReservaItemDTO dto &&
               Id == dto.Id &&
               ImpresoraId == dto.ImpresoraId &&
               NombreImpresora == dto.NombreImpresora &&
               ModeloImpresora == dto.ModeloImpresora &&
               TiempoReserva == dto.TiempoReserva &&
               PrecioSubtotal == dto.PrecioSubtotal;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Id, ImpresoraId, NombreImpresora, ModeloImpresora, TiempoReserva, PrecioSubtotal);
}
