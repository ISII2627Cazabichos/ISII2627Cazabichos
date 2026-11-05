using AppForSEII.API.Models;

using AppForSEII.API.Shared.Modelos3dDTOs;
namespace AppForSEII.API.Controllers
{// METODO GET DEVULVE TODOS LOS MODELOS 3D
    [Route("api/[controller]")]
    [ApiController]
    public class Modelos3DController : ControllerBase
    {// VARIABLES
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Modelos3DController> _logger;
    // CONSTRUCTOR
        public Modelos3DController(ApplicationDbContext context, ILogger<Modelos3DController> logger)
        {
            _context = context;
            _logger = logger;
        }
        //METODOS
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<Modelo3D>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetModelos3D()
        {
            try
            {
               List<Modelo3D> modelos3D = await _context.Modelos3D.ToListAsync();
                return Ok(modelos3D);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los modelos 3D");
                return BadRequest("Ocurrió un error al obtener los modelos 3D.");
            }
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<Modelos3DParaComprarDTO>), (int)HttpStatusCode.OK)]
         public async Task<IActionResult> GetModelos3DFParaComprar()
       {
         var modelos3D = await _context.Modelos3D
            .Include(m => m.LicenciaModelo3D)
             .Select(m => new Modelos3DParaComprarDTO(m.Id, m.Nombre, m.Categoria, m.Formato.ToString(), m.Precio, m.LicenciaModelo3D != null ? m.LicenciaModelo3D.Nombre
            : "Sin licencia"))
             .ToListAsync();
             return Ok(modelos3D);
        }
        
    }
}