
namespace AppForSEII.API.DTOs.EncargoDTOs
{
    public class EncargoItemDTO
    {
        public int Id { get; set; }

        public int PiezaId { get; set; }

        public string NombrePieza { get; set; }

        public string Categoria { get; set; }

        public decimal Peso { get; set; }

        public int MaterialId { get; set; }

        public string NombreMaterial { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnidad { get; set; }

        public decimal Subtotal { get; set; }

        public EncargoItemDTO(
            int id,
            int piezaId,
            string nombrePieza,
            string categoria,
            decimal peso,
            int materialId,
            string nombreMaterial,
            int cantidad,
            decimal precioUnidad,
            decimal subtotal)
        {
            Id = id;
            PiezaId = piezaId;
            NombrePieza = nombrePieza;
            Categoria = categoria;
            Peso = peso;
            MaterialId = materialId;
            NombreMaterial = nombreMaterial;
            Cantidad = cantidad;
            PrecioUnidad = precioUnidad;
            Subtotal = subtotal;
        }
    }
}
