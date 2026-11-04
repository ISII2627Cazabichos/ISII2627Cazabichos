using AppForSEII.API.Data;
using AppForSEII.API.DTOs.EncargoDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncargoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EncargoController> _logger;

        public EncargoController(
            ApplicationDbContext context,
            ILogger<EncargoController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [Route("[action]/{id}")]
        [ProducesResponseType(typeof(EncargoDetalleDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetEncargo(int id)
        {
            var encargo = await _context.EncargoImpresiones
                .Include(e => e.LineasEncargo)
                    .ThenInclude(l => l.Pieza)
                .Include(e => e.LineasEncargo)
                    .ThenInclude(l => l.MaterialSeleccionado)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (encargo == null)
            {
                _logger.LogInformation($"Encargo {id} no encontrado.");
                return NotFound();
            }

            var resultado = new EncargoDetalleDTO(
                encargo.Id,
                encargo.FechaEncargo,
                encargo.NombreCliente,
                encargo.ApellidosCliente,
                encargo.DireccionEnvio,
                encargo.NumeroTelefono,
                encargo.Descripcion,
                encargo.PrecioTotal,
                encargo.MetodoPago.ToString(),
                encargo.LineasEncargo.Select(l => new EncargoItemDTO(
                    l.Id,
                    l.Pieza.Id,
                    l.Pieza.Nombre,
                    l.Pieza.Categoria.ToString(),
                    l.Pieza.Peso,
                    l.MaterialSeleccionado.Id,
                    l.MaterialSeleccionado.Nombre,
                    l.Cantidad,
                    l.PrecioUnidad,
                    l.Subtotal
                )).ToList()
            );

            return Ok(resultado);
        }
    }
}
        