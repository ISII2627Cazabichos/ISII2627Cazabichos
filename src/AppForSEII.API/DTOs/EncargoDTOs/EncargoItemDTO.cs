namespace AppForSEII.API.DTOs.EncargoDTOs
{
    public class EncargoItemDTO
    {
        public EncargoItemDTO() { }

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

        public int Id { get; set; }
        public int PiezaId { get; set; }
        public string NombrePieza { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Peso { get; set; }
        public int MaterialId { get; set; }
        public string NombreMaterial { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public decimal Subtotal { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is EncargoItemDTO dto &&
                   Id == dto.Id &&
                   PiezaId == dto.PiezaId &&
                   NombrePieza == dto.NombrePieza &&
                   Categoria == dto.Categoria &&
                   Peso == dto.Peso &&
                   MaterialId == dto.MaterialId &&
                   NombreMaterial == dto.NombreMaterial &&
                   Cantidad == dto.Cantidad &&
                   PrecioUnidad == dto.PrecioUnidad &&
                   Subtotal == dto.Subtotal;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();

            hash.Add(Id);
            hash.Add(PiezaId);
            hash.Add(NombrePieza);
            hash.Add(Categoria);
            hash.Add(Peso);
            hash.Add(MaterialId);
            hash.Add(NombreMaterial);
            hash.Add(Cantidad);
            hash.Add(PrecioUnidad);
            hash.Add(Subtotal);

            return hash.ToHashCode();
        }
    }
}