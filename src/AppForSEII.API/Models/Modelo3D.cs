namespace AppForSEII.API.Models
{
  public class Modelo3D
  {
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Categoria  { get; set; }
    public FormatoModelo3D Formato { get; set; }
    public decimal Precio { get; set; }
    
  }
}