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

    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(typeof(ComprarDetalleDTO), (int)HttpStatusCode.Created)]    
    [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
     public async Task<ActionResult> crearCompra(ComprarParaCrearDTO comprarParaCrear)
        {
            if (comprarParaCrear.ComprarItems.Count == 0)
            {
                ModelState.AddModelError("comprarItems","debe haber al menos un modelo 3d para comprar");
            }
             var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == comprarParaCrear.NombreCliente);
              if (user.Equals(comprarParaCrear.NombreCliente))
            {
                 ModelState.AddModelError("CompraModelo3DApplicationUser", "Error!{"+ user.Name+"}el nombre no es valido ");

            }
           
            if (ModelState.ErrorCount > 0)
            {
              return BadRequest(new ValidationProblemDetails(ModelState));
            }
            CompraModelo3D compra =new CompraModelo3D
            {
              NombreCliente = comprarParaCrear.NombreCliente,
             ApellidoCliente = comprarParaCrear.ApellidoCliente,
             CorreoElectronico= comprarParaCrear.CorreoElectronico,
             DireccionFacturacion=comprarParaCrear.DireccionFacturacion,
             MetodoPago=comprarParaCrear.MetodoPago,
             FechaCompra=DateTime.Now
             };
            foreach (var item in comprarParaCrear.ComprarItems)
            {
              var modelo = await _context.Modelos3D.Include(m => m.LicenciaModelo3D).FirstOrDefaultAsync(m => m.Id == item.Modelo3DId);

              if (modelo == null)
              {
               ModelState.AddModelError("Modelo3D","No existe el modelo con Id {item.Modelo3DId}");
              }
              else
              {
               compra.LineasCompra.Add(new LineaCompraModelo3D
               {
                CantidadLicencias = item.CantidadLicencias,
                PrecioUnidad = modelo.Precio,
                Modelo3D = modelo
               });
              }
            }
            
            if (ModelState.ErrorCount > 0)
            {
             return BadRequest(new ValidationProblemDetails(ModelState));
            }
            _context.Add(compra);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            _logger.LogError(ex.Message);
            ModelState.AddModelError("Compra","Error al guardar la compra");
             return Conflict(ex.Message);
            }

            var compraDetalle = new ComprarDetalleDTO(
              compra.Id,
              compra.FechaCompra,
              compra.NombreCliente,
              compra.ApellidoCliente,
              compra.CorreoElectronico,
              compra.DireccionFacturacion,
              compra.MetodoPago,
              comprarParaCrear.ComprarItems,
              compra.Descripcion);
             
             return CreatedAtAction("GetCompra",new { id = compra.Id },compraDetalle);
            
        }
    

            
     } 
}
    


           
       

    

