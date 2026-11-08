using System.ComponentModel.DataAnnotations;
 
namespace AppForSEII.API.Models;
 
public class Cliente : ApplicationUser
{
public Cliente()
{
}

public Cliente(string id, string name, string surname, string userName)
    : base(id, name, surname, userName)
{
}

[Required]
[StringLength(200)]
public string DireccionFacturacion { get; set; } = string.Empty;
 
public ICollection<CompraModelo3D> Compras { get; set; }
= new List<CompraModelo3D>();
 
public IList<EncargoImpresion> EncargosRealizados { get; set; }
= new List<EncargoImpresion>();

public IList<ReservaImpresora> ReservasRealizadas { get; set; }
= new List<ReservaImpresora>();
}