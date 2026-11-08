using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ReservaDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservaController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReservaController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("[action]")]
    [ProducesResponseType(typeof(IList<ImpresoraParaReservarDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ImpresoraParaReservarDTO>>> GetImpresorasParaReservar()
    {
        var impresoras = await _context.Impresoras3D
            .OrderBy(impresora => impresora.Id)
            .Select(impresora => new ImpresoraParaReservarDTO(
                impresora.Id,
                impresora.Nombre,
                impresora.Modelo,
                impresora.Tipo,
                impresora.Descripcion,
                impresora.PrecioReserva))
            .ToListAsync();

        return Ok(impresoras);
    }
}
