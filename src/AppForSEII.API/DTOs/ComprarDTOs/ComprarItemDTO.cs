
namespace AppForSEII.API.DTOs.ComprarDTOs 
{
     public class CompraItemDTO 
     { 
        public CompraItemDTO(int id, int modelo3DId, int cantidadLicencias,string licenciaModelo3D, decimal precioUnidad) 
        { 
            Id = id; 
            Modelo3DId = modelo3DId; 
            CantidadLicencias = cantidadLicencias; 
            LicenciaModelo3D= licenciaModelo3D;
            PrecioUnidad = precioUnidad; 
            
        }
          public int Id { get; set; }
          public int Modelo3DId { get; set; }
          public int CantidadLicencias { get; set; }

          public decimal PrecioUnidad { get;}
          public decimal Subtotal
          {
               get
               {
                    return CantidadLicencias * PrecioUnidad;
               }
          }
          public string LicenciaModelo3D { get; set; }

          public override bool Equals(object? obj)
        {
            return obj is CompraItemDTO dto &&
                   Id == dto.Id &&
                   Modelo3DId == dto.Modelo3DId &&
                   CantidadLicencias == dto.CantidadLicencias &&
                   PrecioUnidad == dto.PrecioUnidad &&
                   Subtotal == dto.Subtotal &&
                   LicenciaModelo3D == dto.LicenciaModelo3D;   

        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Modelo3DId, CantidadLicencias, PrecioUnidad, Subtotal, LicenciaModelo3D);
        }



     } 

 } 