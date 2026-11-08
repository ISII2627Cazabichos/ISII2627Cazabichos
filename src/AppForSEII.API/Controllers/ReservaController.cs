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

    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(typeof(ReservaDetalleDTO), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ReservaDetalleDTO>> CrearReserva(
        [FromBody] ReservaCrearDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cliente = await _context.Cliente
            .FirstOrDefaultAsync(c => c.Id == dto.ClienteId);

        if (cliente == null)
            return NotFound($"El cliente indicado no existe.");

        var reserva = new ReservaImpresora
        {
            FechaReserva = DateTime.Now,
            NombreCliente = dto.NombreCliente,
            ApellidosCliente = dto.ApellidosCliente,
            DireccionFacturacion = dto.DireccionFacturacion,
            MetodoPago = dto.MetodoPago,
            Cliente = cliente
        };

        foreach (var lineaDTO in dto.LineasReserva)
        {
            var impresora = await _context.Impresoras3D
                .FirstOrDefaultAsync(i => i.Id == lineaDTO.Impresora3DId);

            if (impresora == null)
                return NotFound($"No existe la impresora con id {lineaDTO.Impresora3DId}.");

            var precioSubtotal = impresora.PrecioReserva * ObtenerHoras(lineaDTO.TiempoReserva);
            var linea = new LineaReserva
            {
                TiempoReserva = lineaDTO.TiempoReserva,
                PrecioSubtotal = precioSubtotal,
                Impresora3DId = impresora.Id,
                Impresora3D = impresora,
                ReservaImpresora = reserva
            };

            reserva.LineasReserva.Add(linea);
            reserva.PrecioTotal += precioSubtotal;
        }

        _context.ReservasImpresora.Add(reserva);
        await _context.SaveChangesAsync();

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

        return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, resultado);
    }

    private static int ObtenerHoras(TiempoReserva tiempoReserva) =>
        tiempoReserva switch
        {
            TiempoReserva.UnaHora => 1,
            TiempoReserva.DosHoras => 2,
            TiempoReserva.TresHoras => 3,
            TiempoReserva.CuatroHoras => 4,
            TiempoReserva.CincoHoras => 5,
            TiempoReserva.UnDia => 24,
            TiempoReserva.DosDias => 48,
            _ => throw new ArgumentOutOfRangeException(nameof(tiempoReserva))
        };
}
