
namespace AppForSEII.API.DTOs.ComprarDTOs
{
    public class ComprarDetalleDTO : ComprarParaCrearDTO
    {
        public ComprarDetalleDTO(int id,DateTime fechaCompra,string nombreCliente, string apellidoCliente,  string correoElectronico, string direccionFacturacion, MetodoPago metodoPago, List<CompraItemDTO> comprarItems, string descripcion = "")
            : base(nombreCliente,
             apellidoCliente,
              correoElectronico,
               direccionFacturacion, 
               metodoPago,
                comprarItems,
            descripcion)
        {
            Id=id;
            FechaCompra=fechaCompra;
           
        }
        public int Id { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal PrecioTotal
        {
            get
            {
                return ComprarItems.Sum(item => item.Subtotal);
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is ComprarDetalleDTO dto &&
                  base.Equals(obj) &&
                   Id == dto.Id &&
                   FechaCompra == dto.FechaCompra &&
                   PrecioTotal == dto.PrecioTotal;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, FechaCompra, PrecioTotal);
        }

       
    }
}