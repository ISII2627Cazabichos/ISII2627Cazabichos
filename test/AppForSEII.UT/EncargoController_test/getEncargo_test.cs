

using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.EncargoDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AppForSEII.UT.EncargoController_test;

public class GetEncargo_test : AppForSEII4SqliteUT
{
    private readonly DateTime _fecha = new DateTime(2026, 11, 8);

    public GetEncargo_test()
    {
        var cliente = new Cliente("cliente1", "Daniel", "Balan", "daniel")
        {
            DireccionFacturacion = "Calle Mayor 1"
        };

        var material = new Material
        {
            Id = 1,
            Nombre = "PLA",
            PrecioPorGramo = 0.05m,
            StockGramos = 1000
        };

        var pieza = new Pieza3D
        {
            Id = 1,
            Nombre = "Adorno",
            Peso = 100,
            Categoria = CategoriaPieza.Decoracion
        };

        var piezaMaterial = new PiezaMaterial
        {
            Pieza = pieza,
            Material = material
        };

        pieza.MaterialesValidos = new List<PiezaMaterial>
        {
            piezaMaterial
        };

        var linea = new LineaEncargo
        {
            Id = 1,
            Pieza = pieza,
            MaterialSeleccionado = material,
            Cantidad = 2,
            PrecioUnidad = 5,
            Subtotal = 10
        };

        var encargo = new EncargoImpresion
        {
            Id = 1,
            FechaEncargo = _fecha,
            NombreCliente = "Daniel",
            ApellidosCliente = "Balan",
            DireccionEnvio = "Calle Mayor 1",
            NumeroTelefono = "600000000",
            Descripcion = "Encargo de prueba",
            PrecioTotal = 10,
            MetodoPago = MetodoPago.TarjetaCredito,
            Cliente = cliente,
            LineasEncargo = new List<LineaEncargo> { linea }
        };

        _context.Add(cliente);
        _context.Add(material);
        _context.Add(pieza);
        _context.Add(piezaMaterial);
        _context.Add(encargo);

        _context.SaveChanges();
    }

    private EncargoController CreateController()
    {
        return new EncargoController(
            _context,
            NullLogger<EncargoController>.Instance
        );
    }

    [Fact]
    public async Task GetEncargo_NotFound_test()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.GetEncargo(0);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetEncargo_Ok_test()
    {
        // Arrange
        var controller = CreateController();

        // Act
        var result = await controller.GetEncargo(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDTO = Assert.IsType<EncargoDetalleDTO>(okResult.Value);

        var expectedDTO = new EncargoDetalleDTO(
            1,
            _fecha,
            "Daniel",
            "Balan",
            "Calle Mayor 1",
            "600000000",
            "Encargo de prueba",
            10m,
            "Tarjeta",
            new List<EncargoItemDTO>
            {
                new EncargoItemDTO(
                    1,
                    1,
                    "Adorno",
                    "Decoracion",
                    100m,
                    1,
                    "PLA",
                    2,
                    5m,
                    10m
                )
            }
        );

        var expectedLinea = expectedDTO.LineasEncargo[0];
        var actualLinea = actualDTO.LineasEncargo[0];

        Assert.Equal(expectedLinea.Id, actualLinea.Id);
        Assert.Equal(expectedLinea.PiezaId, actualLinea.PiezaId);
        Assert.Equal(expectedLinea.NombrePieza, actualLinea.NombrePieza);
        Assert.Equal(expectedLinea.Categoria, actualLinea.Categoria);
        Assert.Equal(expectedLinea.Peso, actualLinea.Peso);
        Assert.Equal(expectedLinea.MaterialId, actualLinea.MaterialId);
        Assert.Equal(expectedLinea.NombreMaterial, actualLinea.NombreMaterial);
        Assert.Equal(expectedLinea.Cantidad, actualLinea.Cantidad);
        Assert.Equal(expectedLinea.PrecioUnidad, actualLinea.PrecioUnidad);
        Assert.Equal(expectedLinea.Subtotal, actualLinea.Subtotal);
    }
}
