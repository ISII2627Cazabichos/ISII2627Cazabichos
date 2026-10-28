using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs
{
    public class Pieza3dParaEncargarDTO
    {
        public Pieza3dParaEncargarDTO() { }

        public Pieza3dParaEncargarDTO(int id, string nombre, string categoria, decimal peso, string materialPorDefecto, decimal precioUnitario)
        {
            Id = id;
            Nombre = nombre;
            Categoria = categoria;
            Peso = peso;
            MaterialPorDefecto = materialPorDefecto;
            PrecioUnitario = precioUnitario;
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Peso { get; set; }
        public string MaterialPorDefecto { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Pieza3dParaEncargarDTO dto &&
                   Id == dto.Id &&
                   Nombre == dto.Nombre &&
                   Categoria == dto.Categoria &&
                   Peso == dto.Peso &&
                   MaterialPorDefecto == dto.MaterialPorDefecto &&
                   PrecioUnitario == dto.PrecioUnitario;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Nombre, Categoria, Peso, MaterialPorDefecto, PrecioUnitario);
        }
    }
}