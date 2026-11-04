
using AppForSEII.API.Data;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PiezasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PiezasController> _logger;

        public PiezasController(
            ApplicationDbContext context,
            ILogger<PiezasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<Pieza3dParaEncargarDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPiezasParaEncargar(
            string? nombrePieza,
            string? categoriaPieza)
        {
            var piezasQuery = await _context.Set<Pieza3D>()
                .Include(p => p.MaterialesValidos)
                    .ThenInclude(pm => pm.Material)
                .Where(p =>
                    (nombrePieza == null || p.Nombre.Contains(nombrePieza)) &&
                    (categoriaPieza == null || p.Categoria.ToString().Equals(categoriaPieza)))
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            var resultadoLista = new List<Pieza3dParaEncargarDTO>();

            foreach (var pieza in piezasQuery)
            {
                var materialSeleccionado = pieza.MaterialesValidos
                    .Where(pm => pm.Material.StockGramos >= pieza.Peso)
                    .OrderBy(pm => pm.Material.PrecioPorGramo)
                    .FirstOrDefault();

                if (materialSeleccionado == null)
                {
                    _logger.LogWarning(
                        $"No valid material with sufficient stock for piece: {pieza.Nombre}");
                    continue;
                }

                decimal precioUnitario =
                    pieza.Peso * materialSeleccionado.Material.PrecioPorGramo;

                resultadoLista.Add(new Pieza3dParaEncargarDTO(
                    pieza.Id,
                    pieza.Nombre,
                    pieza.Categoria.ToString(),
                    pieza.Peso,
                    materialSeleccionado.Material.Nombre,
                    precioUnitario
                ));
            }

            if (resultadoLista.Count == 0)
            {
                _logger.LogInformation(
                    "No 3D pieces available with the specified criteria or stock.");

                return NotFound(
                    "No se detectó material disponible para imprimir piezas o no hay resultados con los filtros proporcionados.");
            }

            return Ok(resultadoLista);
        }
    }
}
