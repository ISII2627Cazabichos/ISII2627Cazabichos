namespace AppForSEII.API.DTOs.ComprarDTOs
{
    public class ComprarParaCrearDTO
    {
        public ComprarParaCrearDTO(string nombreCliente, string apellidoCliente, string correoElectronico, string direccionFacturacion,  MetodoPago metodoPago, List<CompraItemDTO> comprarItems, string descripcion="")
        {
            NombreCliente = nombreCliente;
            ApellidoCliente = apellidoCliente;
            CorreoElectronico = correoElectronico;
            DireccionFacturacion = direccionFacturacion;
            MetodoPago = metodoPago;
            ComprarItems = comprarItems;
            Descripcion = descripcion;
        }
        public ComprarParaCrearDTO()
        {
           ComprarItems = new List<CompraItemDTO>();
        }

        public string NombreCliente { get; set; }= string.Empty;
        public string ApellidoCliente { get; set; }= string.Empty;
        public string CorreoElectronico { get; set; }= string.Empty;
        [System.ComponentModel.DataAnnotations.Display(
         Name = "Dirección de facturación")]
        public string DireccionFacturacion { get; set; }= string.Empty;
        public MetodoPago MetodoPago { get; set; }
        public List<CompraItemDTO> ComprarItems { get; set; }
        public string Descripcion { get; set; }= string.Empty;


        public override bool Equals(object? obj)
        {
            return obj is ComprarParaCrearDTO dto &&
                   NombreCliente == dto.NombreCliente &&
                   ApellidoCliente == dto.ApellidoCliente &&
                   CorreoElectronico == dto.CorreoElectronico &&
                   DireccionFacturacion == dto.DireccionFacturacion &&
                   MetodoPago == dto.MetodoPago &&
                   Descripcion == dto.Descripcion &&
                   ComprarItems.SequenceEqual(dto.ComprarItems);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(NombreCliente, ApellidoCliente, CorreoElectronico, DireccionFacturacion, MetodoPago, Descripcion, ComprarItems);
        }
        
    }
}