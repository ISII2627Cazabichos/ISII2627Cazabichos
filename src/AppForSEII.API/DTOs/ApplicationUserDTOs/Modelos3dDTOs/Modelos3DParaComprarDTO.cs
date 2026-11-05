
// Objetos de Transferencia de Datos
namespace AppForSEII.API.Shared.Modelos3dDTOs
{
    public class Modelos3DParaComprarDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Formato { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Licencia { get; set; } = string.Empty;

        public Modelos3DParaComprarDTO(int id, string nombre, string categoria, string formato, decimal precio , string licencia)
        {
            Id = id;
            Nombre = nombre;
            Categoria = categoria;
            Formato = formato;
            Precio = precio;
            Licencia = licencia;
        }
        public Modelos3DParaComprarDTO()
        {
            
        }
    }
    

}