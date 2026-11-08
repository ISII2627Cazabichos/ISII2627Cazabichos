
using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.EncargoDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AppForSEII.UT.EncargoController_test
{
    public class CrearEncargo_test : AppForSEII4SqliteUT
    {
        private const string _clienteId = "cliente1";

        public CrearEncargo_test()
        {
            // Cliente
            var cliente = new Cliente(
                _clienteId,
                "Daniel",
                "Balan",
                "daniel");

            cliente.DireccionFacturacion = "Calle Mayor 1";

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

            // Pieza
            var pieza = new Pieza3D
            {
                Id = 1,
                Nombre = "Adorno",
                Peso = 100,
                Categoria = CategoriaPieza.Decoracion
            };

            // Material válido para la pieza
            var piezaMaterial = new PiezaMaterial
            {
                PiezaId = 1,
                MaterialId = 1,
                Pieza = pieza,
                Material = pla
            };

            _context.Add(cliente);
            _context.Add(pla);
            _context.Add(abs);
            _context.Add(pieza);
            _context.Add(piezaMaterial);

            _context.SaveChanges();
        }

        private EncargoController CreateController()
        {
            return new EncargoController(
                _context,
                NullLogger<EncargoController>.Instance);
        }

        private static EncargoCrearDTO CrearDTO(
            string clienteId,
            int piezaId,
            int materialId,
            int cantidad)
        {
            return new EncargoCrearDTO
            {
                ClienteId = clienteId,
                NombreCliente = "Daniel",
                ApellidosCliente = "Balan",
                DireccionEnvio = "Calle Mayor 1",
                NumeroTelefono = "600123456",
                Descripcion = "Encargo de prueba",
                MetodoPago = MetodoPago.TarjetaCredito,
                LineasEncargo = new List<LineaEncargoCrearDTO>
                {
                    new LineaEncargoCrearDTO
                    {
                        PiezaId = piezaId,
                        MaterialId = materialId,
                        Cantidad = cantidad
                    }
                }
            };
        }

        public static IEnumerable<object[]>
            CasosDePruebaPara_CrearEncargo_test()
        {
            var encargoClienteInexistente =
                CrearDTO("clienteFake", 1, 1, 2);

            var encargoPiezaInexistente =
                CrearDTO(_clienteId, 999, 1, 2);

            var encargoMaterialInexistente =
                CrearDTO(_clienteId, 1, 999, 2);

            var encargoMaterialNoValido =
                CrearDTO(_clienteId, 1, 2, 2);

            return new List<object[]>
            {
                new object[]
                {
                    encargoClienteInexistente,
                    "El cliente indicado no existe.",
                    false
                },

                new object[]
                {
                    encargoPiezaInexistente,
                    "No existe la pieza con id 999.",
                    false
                },

                new object[]
                {
                    encargoMaterialInexistente,
                    "No existe el material con id 999.",
                    false
                },

                new object[]
                {
                    encargoMaterialNoValido,
                    "El material ABS no es válido para la pieza Adorno.",
                    true
                }
            };
        }

        [Theory]
        [MemberData(nameof(CasosDePruebaPara_CrearEncargo_test))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CrearEncargo_Error_test(
            EncargoCrearDTO encargoDTO,
            string errorEsperado,
            bool esBadRequest)
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = await controller.CrearEncargo(encargoDTO);

            // Assert
            if (esBadRequest)
            {
                var badRequestResult =
                    Assert.IsType<BadRequestObjectResult>(result);

                Assert.Equal(errorEsperado, badRequestResult.Value);
            }
            else
            {
                var notFoundResult =
                    Assert.IsType<NotFoundObjectResult>(result);

                Assert.Equal(errorEsperado, notFoundResult.Value);
            }
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CrearEncargo_Ok_test()
        {
            // Arrange
            var controller = CreateController();

            var encargoDTO = CrearDTO(
                _clienteId,
                1,
                1,
                2);

            // Act
            var result = await controller.CrearEncargo(encargoDTO);

            // Assert
            var createdResult =
                Assert.IsType<CreatedAtActionResult>(result);

            Assert.Equal(
                nameof(EncargoController.GetEncargo),
                createdResult.ActionName);

            Assert.NotNull(createdResult.RouteValues);
            Assert.Equal(1, createdResult.RouteValues["id"]);

            // Comprobar que se ha guardado correctamente en SQLite
            var encargoCreado = await _context.EncargoImpresiones
                .Include(e => e.LineasEncargo)
                .FirstOrDefaultAsync(e => e.Id == 1);

            Assert.NotNull(encargoCreado);

            Assert.Equal("Daniel", encargoCreado.NombreCliente);
            Assert.Equal("Balan", encargoCreado.ApellidosCliente);
            Assert.Equal("Calle Mayor 1", encargoCreado.DireccionEnvio);
            Assert.Equal("600123456", encargoCreado.NumeroTelefono);
            Assert.Equal("Encargo de prueba", encargoCreado.Descripcion);
            Assert.Equal(MetodoPago.TarjetaCredito, encargoCreado.MetodoPago);

            Assert.Equal(10.00m, encargoCreado.PrecioTotal);

            Assert.Single(encargoCreado.LineasEncargo);

            var linea = encargoCreado.LineasEncargo[0];

            Assert.Equal(1, linea.PiezaId);
            Assert.Equal(1, linea.MaterialSeleccionadoId);
            Assert.Equal(2, linea.Cantidad);
            Assert.Equal(5.00m, linea.PrecioUnidad);
            Assert.Equal(10.00m, linea.Subtotal);
        }
    }
}
