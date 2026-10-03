namespace AppForSEII.API.Models
{
  
    public class Cliente : ApplicationUser
    {
        [StringLength(50)]
        public string DireccionFacturacion  { get; set; }= string.Empty;

    }
}