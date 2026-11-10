using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ReservaDTOs;

namespace AppForSEII.UT.ReservaController_test;

public class CrearReserva_test : AppForSEII4SqliteUT
{
    public CrearReserva_test()
    {
        var cliente = new Cliente(
            "cliente-creacion",
            "Marta",
            "Gil",
            "marta@example.com")
        {
            DireccionFacturacion = "Avenida Europa 2"
        };
        var impresoras = new List<Impresora3D>
        {
            new(
                5,
                "Prusa",
                "Mini+",
                "Filamento",
                "Formato compacto",
                0.18m,
                4m),
            new(
                8,
                "Bambu Lab",
                "A1",
                "Filamento",
                "Uso doméstico",
                0.22m,
                7m)
        };

        _context.Add(cliente);
        _context.AddRange(impresoras);
        _context.SaveChanges();
    }

    public static IEnumerable<object[]> TestCasesForCrearReserva()
    {
        var lineasValidas = new List<LineaReservaCrearDTO>
        {
            new(5, TiempoReserva.UnaHora)
        };
        var clienteInexistente = new ReservaCrearDTO(
            "cliente-inexistente",
            "Marta",
            "Gil",
            "Avenida Europa 2",
            MetodoPago.Bizum,
            lineasValidas);
        var impresoraInexistente = new ReservaCrearDTO(
            "cliente-creacion",
            "Marta",
            "Gil",
            "Avenida Europa 2",
            MetodoPago.Bizum,
            new List<LineaReservaCrearDTO>
            {
                new(404, TiempoReserva.UnaHora)
            });

        return new List<object[]>
        {
            new object[] { clienteInexistente, "El cliente indicado no existe." },
            new object[] { impresoraInexistente, "No existe la impresora con id 404." }
        };
    }

    // Agrupa los errores de cliente o impresora inexistentes.
    [Theory]
    [MemberData(nameof(TestCasesForCrearReserva))]
    public async Task CrearReserva_Error_test(
        ReservaCrearDTO dto,
        string errorEsperado)
    {
        // Arrange
        var controller = new ReservaController(_context);

        // Act
        var result = await controller.CrearReserva(dto);

        // Assert
        var notFound = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(errorEsperado, notFound.Value);
    }

    // Comprueba el rechazo de una petición con un modelo inválido.
    [Fact]
    public async Task CrearReserva_ModelStateInvalido_DevuelveBadRequest()
    {
        // Arrange
        var controller = new ReservaController(_context);
        controller.ModelState.AddModelError("LineasReserva", "Debe incluir una línea.");
        var dto = new ReservaCrearDTO(
            "cliente-creacion",
            "Marta",
            "Gil",
            "Avenida Europa 2",
            MetodoPago.TarjetaCredito,
            new List<LineaReservaCrearDTO>());

        // Act
        var result = await controller.CrearReserva(dto);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.NotNull(badRequest.Value);
    }

    // Comprueba la creación de una reserva con dos líneas.
    [Fact]
    public async Task CrearReserva_Success_test()
    {
        // Arrange
        var dto = new ReservaCrearDTO(
            "cliente-creacion",
            "Marta",
            "Gil",
            "Avenida Europa 2",
            MetodoPago.TarjetaCredito,
            new List<LineaReservaCrearDTO>
            {
                new(5, TiempoReserva.UnaHora),
                new(8, TiempoReserva.DosHoras)
            });
        var controller = new ReservaController(_context);

        // Act
        var result = await controller.CrearReserva(dto);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var actual = Assert.IsType<ReservaDetalleDTO>(created.Value);
        var expected = new ReservaDetalleDTO(
            actual.Id,
            actual.FechaReserva,
            "Marta",
            "Gil",
            "Avenida Europa 2",
            18m,
            "TarjetaCredito",
            new List<ReservaItemDTO>
            {
                new(actual.LineasReserva[0].Id, 5, "Prusa", "Mini+", "UnaHora", 4m),
                new(actual.LineasReserva[1].Id, 8, "Bambu Lab", "A1", "DosHoras", 14m)
            });
        Assert.Equal(expected, actual);
        Assert.Equal(1, await _context.ReservasImpresora.CountAsync());
        Assert.Equal(2, await _context.LineasReserva.CountAsync());
    }
}
