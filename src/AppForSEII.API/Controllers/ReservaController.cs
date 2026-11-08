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

    [HttpGet]
    [Route("[action]/{id}")]
    [ProducesResponseType(typeof(ReservaDetalleDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ReservaDetalleDTO>> GetReserva(int id)
    {
        var reserva = await _context.ReservasImpresora
            .Include(r => r.LineasReserva)
                .ThenInclude(l => l.Impresora3D)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reserva == null)
            return NotFound();

        var resultado = new ReservaDetalleDTO(
            reserva.Id,
            reserva.FechaReserva,
            reserva.NombreCliente,
            reserva.ApellidosCliente,
            reserva.DireccionFacturacion,
            reserva.PrecioTotal,
            reserva.MetodoPago.ToString(),
            reserva.LineasReserva.Select(linea => new ReservaItemDTO(
                linea.Id,
                linea.Impresora3DId,
                linea.Impresora3D.Nombre,
                linea.Impresora3D.Modelo,
                linea.TiempoReserva.ToString(),
                linea.PrecioSubtotal)).ToList());

        return Ok(resultado);
    }
}
