namespace AppForSEII.API.DTOs.ReservaDTOs;

public class ImpresoraParaReservarDTO
{
    public ImpresoraParaReservarDTO(
        int id,
        string nombre,
        string modelo,
        string tipo,
        string descripcion,
        decimal precioReserva)
    {
        Id = id;
        Nombre = nombre;
        Modelo = modelo;
        Tipo = tipo;
        Descripcion = descripcion;
        PrecioReserva = precioReserva;
    }

    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Modelo { get; set; }
    public string Tipo { get; set; }
    public string Descripcion { get; set; }
    public decimal PrecioReserva { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is ImpresoraParaReservarDTO dto &&
               Id == dto.Id &&
               Nombre == dto.Nombre &&
               Modelo == dto.Modelo &&
               Tipo == dto.Tipo &&
               Descripcion == dto.Descripcion &&
               PrecioReserva == dto.PrecioReserva;
    }

    public override int GetHashCode() =>
        HashCode.Combine(Id, Nombre, Modelo, Tipo, Descripcion, PrecioReserva);
}
