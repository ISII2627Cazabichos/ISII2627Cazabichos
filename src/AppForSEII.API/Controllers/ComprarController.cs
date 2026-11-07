using AppForSEII.API.Models;

using AppForSEII.API.DTOs.ComprarDTOs;
using SQLitePCL;

namespace AppForSEII.API.Controllers
{
 [Route("api/[controller]")]
    [ApiController]
    public class ComprarController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ComprarController> _logger;
        public ComprarController(ApplicationDbContext context, ILogger<ComprarController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(ComprarDetalleDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCompra(int id)
        {
            if(_context.CompraModelos3D == null)
            {
                return NotFound();
            }
            var compra= await _context.CompraModelos3D
            .Where(r => r.Id == id)
                .Include(r => r.LineasCompra)
                 .ThenInclude(ri  => ri.Modelo3D)
                   .ThenInclude(modelo3d => modelo3d.LicenciaModelo3D) 
                .Select(r => new ComprarDetalleDTO(
                    r.Id,
                    r.FechaCompra,
                    r.NombreCliente,
                    r.ApellidoCliente,
                    r.CorreoElectronico,
                    r.DireccionFacturacion,
                    r.MetodoPago,
                    r.LineasCompra
                      .Select(ri => new CompraItemDTO(
                        ri.Id,
                        ri.Modelo3D.Id,
                        ri.CantidadLicencias,
                        ri.Modelo3D.LicenciaModelo3D != null ? ri.Modelo3D.LicenciaModelo3D.Nombre : "Sin licencia",
                        ri.PrecioUnidad
                    )).ToList(),
                    r.Descripcion
                )).FirstOrDefaultAsync();
                if(compra == null)
                {
                    _logger.LogError($"Error: Compra with id {id} does not exist");
                    return NotFound();
                }
                return Ok(compra);
        }
            
    }

           
       
}
    

