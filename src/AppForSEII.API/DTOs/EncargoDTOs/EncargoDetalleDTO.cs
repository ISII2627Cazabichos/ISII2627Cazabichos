namespace AppForSEII.API.DTOs.EncargoDTOs
{
    public class EncargoDetalleDTO
    {
        public EncargoDetalleDTO() { }

        public EncargoDetalleDTO(
            int id,
            DateTime fechaEncargo,
            string nombreCliente,
            string apellidosCliente,
            string direccionEnvio,
            string numeroTelefono,
            string? descripcion,
            decimal precioTotal,
            string metodoPago,
            IList<EncargoItemDTO> lineasEncargo)
        {
            Id = id;
            FechaEncargo = fechaEncargo;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionEnvio = direccionEnvio;
            NumeroTelefono = numeroTelefono;
            Descripcion = descripcion;
            PrecioTotal = precioTotal;
            MetodoPago = metodoPago;
            LineasEncargo = lineasEncargo;
        }

        public int Id { get; set; }
        public DateTime FechaEncargo { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string ApellidosCliente { get; set; } = string.Empty;
        public string DireccionEnvio { get; set; } = string.Empty;
        public string NumeroTelefono { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioTotal { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public IList<EncargoItemDTO> LineasEncargo { get; set; }
            = new List<EncargoItemDTO>();

        public override bool Equals(object? obj)
        {
            return obj is EncargoDetalleDTO dto &&
                   Id == dto.Id &&
                   FechaEncargo == dto.FechaEncargo &&
                   NombreCliente == dto.NombreCliente &&
                   ApellidosCliente == dto.ApellidosCliente &&
                   DireccionEnvio == dto.DireccionEnvio &&
                   NumeroTelefono == dto.NumeroTelefono &&
                   Descripcion == dto.Descripcion &&
                   PrecioTotal == dto.PrecioTotal &&
                   MetodoPago == dto.MetodoPago &&
                   LineasEncargo.SequenceEqual(dto.LineasEncargo);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                Id, FechaEncargo, NombreCliente, ApellidosCliente,
                DireccionEnvio, NumeroTelefono, Descripcion, PrecioTotal);
        }
    }
}