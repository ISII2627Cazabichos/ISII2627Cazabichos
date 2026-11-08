
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

        public override bool Equals(object? obj)
        {
            return obj is EncargoItemDTO other &&
                   Id == other.Id &&
                   PiezaId == other.PiezaId &&
                   NombrePieza == other.NombrePieza &&
                   Categoria == other.Categoria &&
                   Peso == other.Peso &&
                   MaterialId == other.MaterialId &&
                   NombreMaterial == other.NombreMaterial &&
                   Cantidad == other.Cantidad &&
                   PrecioUnidad == other.PrecioUnidad &&
                   Subtotal == other.Subtotal;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                HashCode.Combine(
                    Id,
                    PiezaId,
                    NombrePieza,
                    Categoria,
                    Peso
                ),
                HashCode.Combine(
                    MaterialId,
                    NombreMaterial,
                    Cantidad,
                    PrecioUnidad,
                    Subtotal
                )
            );
        }
    }
}
