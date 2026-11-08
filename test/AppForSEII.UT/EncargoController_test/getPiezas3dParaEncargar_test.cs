
using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AppForSEII.UT.PiezasController_test
{
    public class GetPiezasParaEncargar_test : AppForSEII4SqliteUT
    {
        public GetPiezasParaEncargar_test()
        {
            // Materiales
            var pla = new Material
            {
                Id = 1,
                Nombre = "PLA",
                PrecioPorGramo = 0.05m,
                StockGramos = 1000
            };

            var abs = new Material
            {
                Id = 2,
                Nombre = "ABS",
                PrecioPorGramo = 0.08m,
                StockGramos = 1000
            };

            var petg = new Material
            {
                Id = 3,
                Nombre = "PETG",
                PrecioPorGramo = 0.03m,
                StockGramos = 2
            };

            // Piezas
            var pieza1 = new Pieza3D
            {
                Id = 1,
                Nombre = "Adorno",
                Peso = 100,
                Categoria = CategoriaPieza.Decoracion
            };

            var pieza2 = new Pieza3D
            {
                Id = 2,
                Nombre = "Miniatura",
                Peso = 50,
                Categoria = CategoriaPieza.MiniaturasYMaquetas
            };

            var pieza3 = new Pieza3D
            {
                Id = 3,
                Nombre = "Repuesto",
                Peso = 200,
                Categoria = CategoriaPieza.Repuestos
            };

            var pieza4 = new Pieza3D
            {
                Id = 4,
                Nombre = "SinMaterial",
                Peso = 100,
                Categoria = CategoriaPieza.HerramientasYAccesorios
            };

            var pieza5 = new Pieza3D
            {
                Id = 5,
                Nombre = "SinStock",
                Peso = 100,
                Categoria = CategoriaPieza.Decoracion
            };

            _context.Set<Material>().AddRange(pla, abs, petg);

            _context.Set<Pieza3D>().AddRange(
                pieza1, pieza2, pieza3, pieza4, pieza5);

            // Materiales válidos para cada pieza
            _context.Set<PiezaMaterial>().AddRange(
                new PiezaMaterial
                {
                    PiezaId = 1,
                    MaterialId = 1,
                    Pieza = pieza1,
                    Material = pla
                },
                new PiezaMaterial
                {
                    PiezaId = 1,
                    MaterialId = 2,
                    Pieza = pieza1,
                    Material = abs
                },
                new PiezaMaterial
                {
                    PiezaId = 2,
                    MaterialId = 1,
                    Pieza = pieza2,
                    Material = pla
                },
                new PiezaMaterial
                {
                    PiezaId = 3,
                    MaterialId = 1,
                    Pieza = pieza3,
                    Material = pla
                },
                new PiezaMaterial
                {
                    PiezaId = 5,
                    MaterialId = 3,
                    Pieza = pieza5,
                    Material = petg
                });

            _context.SaveChanges();
        }

        private PiezasController CreateController()
        {
            return new PiezasController(
                _context,
                NullLogger<PiezasController>.Instance);
        }

        [Fact]
        public async Task GetPiezasParaEncargar_Ok_AllAvailablePieces()
        {
            // Arrange
            var controller = CreateController();

            var expected = new List<Pieza3dParaEncargarDTO>
            {
                new Pieza3dParaEncargarDTO(
                    1, "Adorno", "Decoracion",
                    100, "PLA", 5.00m),

                new Pieza3dParaEncargarDTO(
                    2, "Miniatura", "MiniaturasYMaquetas",
                    50, "PLA", 2.50m),

                new Pieza3dParaEncargarDTO(
                    3, "Repuesto", "Repuestos",
                    200, "PLA", 10.00m)
            };

            // Act
            var result = await controller.GetPiezasParaEncargar(
                null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsAssignableFrom<IList<Pieza3dParaEncargarDTO>>(
                okResult.Value);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task GetPiezasParaEncargar_Ok_FilterByName()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.GetPiezasParaEncargar(
                "Adorno", null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsAssignableFrom<IList<Pieza3dParaEncargarDTO>>(
                okResult.Value);

            Assert.Single(actual);

            Assert.Equal(
                new Pieza3dParaEncargarDTO(
                    1, "Adorno", "Decoracion",
                    100, "PLA", 5.00m),
                actual[0]);
        }

        [Fact]
        public async Task GetPiezasParaEncargar_Ok_FilterByCategory()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.GetPiezasParaEncargar(
                null, "Repuestos");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsAssignableFrom<IList<Pieza3dParaEncargarDTO>>(
                okResult.Value);

            Assert.Single(actual);
            Assert.Equal("Repuesto", actual[0].Nombre);
        }

        [Fact]
        public async Task GetPiezasParaEncargar_Ok_FilterByNameAndCategory()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.GetPiezasParaEncargar(
                "Adorno", "Decoracion");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsAssignableFrom<IList<Pieza3dParaEncargarDTO>>(
                okResult.Value);

            Assert.Single(actual);
            Assert.Equal("Adorno", actual[0].Nombre);
        }

        [Fact]
        public async Task GetPiezasParaEncargar_NotFound_NameDoesNotExist()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.GetPiezasParaEncargar(
                "Inexistente", null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(
                "No se detectó material disponible para imprimir piezas o no hay resultados con los filtros proporcionados.",
                notFoundResult.Value);
        }

        [Fact]
        public async Task GetPiezasParaEncargar_NotFound_CategoryDoesNotExist()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.GetPiezasParaEncargar(
                null, "CategoriaInexistente");

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetPiezasParaEncargar_NotFound_NoAvailableStock()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.GetPiezasParaEncargar(
                "SinStock", null);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetPiezasParaEncargar_NotFound_NoValidMaterials()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.GetPiezasParaEncargar(
                "SinMaterial", null);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetPiezasParaEncargar_Ok_SelectCheapestMaterial()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.GetPiezasParaEncargar(
                "Adorno", null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsAssignableFrom<IList<Pieza3dParaEncargarDTO>>(
                okResult.Value);

            Assert.Equal("PLA", actual[0].MaterialPorDefecto);
            Assert.Equal(5.00m, actual[0].PrecioUnitario);
        }

        [Fact]
        public async Task GetPiezasParaEncargar_Ok_OrderByPieceName()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.GetPiezasParaEncargar(
                null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsAssignableFrom<IList<Pieza3dParaEncargarDTO>>(
                okResult.Value);

            Assert.Equal(
                new[] { "Adorno", "Miniatura", "Repuesto" },
                actual.Select(p => p.Nombre).ToArray());
        }
    }
}
