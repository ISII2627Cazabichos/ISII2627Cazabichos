
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

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> CrearEncargo(
            [FromBody] EncargoCrearDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Comprobar que existe el cliente
            var cliente = await _context.Set<Cliente>()
                .FirstOrDefaultAsync(c => c.Id == dto.ClienteId);

            if (cliente == null)
            {
                _logger.LogInformation(
                    $"Cliente {dto.ClienteId} no encontrado.");

                return NotFound("El cliente indicado no existe.");
            }

            // Crear el encargo
            var encargo = new EncargoImpresion
            {
                FechaEncargo = DateTime.Now,
                NombreCliente = dto.NombreCliente,
                ApellidosCliente = dto.ApellidosCliente,
                DireccionEnvio = dto.DireccionEnvio,
                NumeroTelefono = dto.NumeroTelefono,
                Descripcion = dto.Descripcion,
                MetodoPago = dto.MetodoPago,
                Cliente = cliente,
                PrecioTotal = 0
            };

            // Procesar las líneas del encargo
            foreach (var lineaDTO in dto.LineasEncargo)
            {
                var pieza = await _context.Set<Pieza3D>()
                    .FirstOrDefaultAsync(p => p.Id == lineaDTO.PiezaId);

                if (pieza == null)
                {
                    return NotFound(
                        $"No existe la pieza con id {lineaDTO.PiezaId}.");
                }

                var material = await _context.Set<Material>()
                    .FirstOrDefaultAsync(m => m.Id == lineaDTO.MaterialId);

                if (material == null)
                {
                    return NotFound(
                        $"No existe el material con id {lineaDTO.MaterialId}.");
                }

                // Comprobar que el material es válido para la pieza
                var materialValido = await _context.Set<PiezaMaterial>()
                    .AnyAsync(pm =>
                        pm.PiezaId == pieza.Id &&
                        pm.MaterialId == material.Id);

                if (!materialValido)
                {
                    return BadRequest(
                        $"El material {material.Nombre} no es válido para la pieza {pieza.Nombre}.");
                }

                // Calcular precios
                decimal precioUnidad =
                    pieza.Peso * material.PrecioPorGramo;

                decimal subtotal =
                    precioUnidad * lineaDTO.Cantidad;

                var linea = new LineaEncargo
                {
                    Pieza = pieza,
                    PiezaId = pieza.Id,
                    MaterialSeleccionado = material,
                    MaterialSeleccionadoId = material.Id,
                    Cantidad = lineaDTO.Cantidad,
                    PrecioUnidad = precioUnidad,
                    Subtotal = subtotal,
                    EncargoImpresion = encargo
                };

                encargo.LineasEncargo.Add(linea);
                encargo.PrecioTotal += subtotal;
            }

            // Guardar el encargo
            _context.EncargoImpresiones.Add(encargo);

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                $"Encargo {encargo.Id} creado correctamente.");

            return CreatedAtAction(
                nameof(GetEncargo),
                new { id = encargo.Id },
                new { id = encargo.Id }
            );
        }
    }
}
  