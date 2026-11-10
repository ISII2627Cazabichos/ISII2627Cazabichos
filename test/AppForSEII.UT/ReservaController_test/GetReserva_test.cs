using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ReservaDTOs;

namespace AppForSEII.UT.ReservaController_test;

public class GetReserva_test : AppForSEII4SqliteUT
{
    private readonly DateTime _fechaReserva = new(2026, 11, 10, 18, 30, 0);

    public GetReserva_test()
    {
        var cliente = new Cliente(
            "cliente-detalle",
            "Lucía",
            "Santos",
            "lucia@example.com")
        {
            DireccionFacturacion = "Calle Real 18"
        };
        var impresora = new Impresora3D(
            4,
            "Elegoo",
            "Mars 4",
            "Resina",
            "Modelos detallados",
            0.25m,
            5m);
        var reserva = new ReservaImpresora(
            _fechaReserva,
            "Lucía",
            "Santos",
            "Calle Real 18",
            MetodoPago.PayPal,
            cliente)
        {
            Id = 9,
            PrecioTotal = 10m
        };
        reserva.LineasReserva.Add(new LineaReserva(TiempoReserva.DosHoras, 10m, impresora));

        _context.AddRange(cliente, impresora, reserva);
        _context.SaveChanges();
    }

    // Comprueba la respuesta cuando la reserva solicitada no existe.
    [Fact]
    public async Task GetReserva_NotFound_test()
    {
        // Arrange
        var controller = new ReservaController(_context);

        // Act
        var result = await controller.GetReserva(42);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    // Comprueba que se devuelve el detalle junto con sus líneas de reserva.
    [Fact]
    public async Task GetReserva_Found_test()
    {
        // Arrange
        var controller = new ReservaController(_context);

        // Act
        var result = await controller.GetReserva(9);

        // Assert
        var actual = Assert.IsType<ReservaDetalleDTO>(
            Assert.IsType<OkObjectResult>(result.Result).Value);
        var expected = new ReservaDetalleDTO(
            9,
            _fechaReserva,
            "Lucía",
            "Santos",
            "Calle Real 18",
            10m,
            "PayPal",
            new List<ReservaItemDTO>
            {
                new(1, 4, "Elegoo", "Mars 4", "DosHoras", 10m)
            });
        Assert.Equal(expected, actual);
    }
}
