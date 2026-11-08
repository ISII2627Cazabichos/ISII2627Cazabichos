using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ReservaDTOs;
using AppForSEII.API.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII.UT;

public class ReservaControllerTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public ReservaControllerTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetImpresorasParaReservarDevuelveLasImpresorasOrdenadas()
    {
        await using var context = CreateContext();
        var impresora = new Impresora3D(1, "Prusa", "MK4", "Filamento", "FDM", 0.20m, 4m);
        context.Impresoras3D.Add(impresora);
        await context.SaveChangesAsync();

        var controller = new ReservaController(context);
        var action = await controller.GetImpresorasParaReservar();

        var expected = new List<ImpresoraParaReservarDTO>
        {
            new(1, "Prusa", "MK4", "Filamento", "FDM", 4m)
        };
        var actual = Assert.IsType<List<ImpresoraParaReservarDTO>>(
            Assert.IsType<OkObjectResult>(action.Result).Value);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetReservaDevuelveElDetalleConSusLineas()
    {
        await using var context = CreateContext();
        var cliente = new Cliente("client-1", "Ana", "Lopez", "ana@example.com")
        {
            DireccionFacturacion = "Calle 1"
        };
        var impresora = new Impresora3D(1, "Prusa", "MK4", "Filamento", "FDM", 0.20m, 4m);
        var reserva = new ReservaImpresora(
            new DateTime(2026, 11, 8, 10, 0, 0),
            "Ana",
            "Lopez",
            "Calle 1",
            MetodoPago.TarjetaCredito,
            cliente)
        {
            Id = 1,
            PrecioTotal = 8m
        };
        reserva.LineasReserva.Add(new LineaReserva(TiempoReserva.DosHoras, 8m, impresora));
        context.AddRange(cliente, impresora, reserva);
        await context.SaveChangesAsync();

        var controller = new ReservaController(context);
        var action = await controller.GetReserva(1);

        var expected = new ReservaDetalleDTO(
            1,
            new DateTime(2026, 11, 8, 10, 0, 0),
            "Ana",
            "Lopez",
            "Calle 1",
            8m,
            "TarjetaCredito",
            new List<ReservaItemDTO>
            {
                new(1, 1, "Prusa", "MK4", "DosHoras", 8m)
            });
        var actual = Assert.IsType<ReservaDetalleDTO>(
            Assert.IsType<OkObjectResult>(action.Result).Value);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetReservaDevuelveNotFoundCuandoNoExiste()
    {
        await using var context = CreateContext();
        var controller = new ReservaController(context);

        var action = await controller.GetReserva(999);

        Assert.IsType<NotFoundResult>(action.Result);
    }

    [Fact]
    public async Task CrearReservaPersisteLaReservaYDevuelveSuDetalle()
    {
        await using var context = CreateContext();
        var cliente = new Cliente("client-1", "Ana", "Lopez", "ana@example.com")
        {
            DireccionFacturacion = "Calle 1"
        };
        var impresora = new Impresora3D(1, "Prusa", "MK4", "Filamento", "FDM", 0.20m, 4m);
        context.AddRange(cliente, impresora);
        await context.SaveChangesAsync();

        var dto = new ReservaCrearDTO(
            "client-1",
            "Ana",
            "Lopez",
            "Calle 1",
            MetodoPago.PayPal,
            new List<LineaReservaCrearDTO>
            {
                new(1, TiempoReserva.TresHoras)
            });
        var controller = new ReservaController(context);

        var action = await controller.CrearReserva(dto);

        var created = Assert.IsType<CreatedAtActionResult>(action.Result);
        var actual = Assert.IsType<ReservaDetalleDTO>(created.Value);
        var expected = new ReservaDetalleDTO(
            actual.Id,
            actual.FechaReserva,
            "Ana",
            "Lopez",
            "Calle 1",
            12m,
            "PayPal",
            new List<ReservaItemDTO>
            {
                new(actual.LineasReserva[0].Id, 1, "Prusa", "MK4", "TresHoras", 12m)
            });

        Assert.Equal(expected, actual);
        Assert.Equal(1, await context.ReservasImpresora.CountAsync());
    }

    [Fact]
    public async Task CrearReservaDevuelveNotFoundSiNoExisteElCliente()
    {
        await using var context = CreateContext();
        var dto = new ReservaCrearDTO(
            "missing",
            "Ana",
            "Lopez",
            "Calle 1",
            MetodoPago.Bizum,
            new List<LineaReservaCrearDTO>
            {
                new(1, TiempoReserva.UnaHora)
            });
        var controller = new ReservaController(context);

        var action = await controller.CrearReserva(dto);

        Assert.IsType<NotFoundObjectResult>(action.Result);
    }

    [Fact]
    public async Task CrearReservaDevuelveNotFoundSiNoExisteLaImpresora()
    {
        await using var context = CreateContext();
        var cliente = new Cliente("client-1", "Ana", "Lopez", "ana@example.com");
        context.Cliente.Add(cliente);
        await context.SaveChangesAsync();
        var dto = new ReservaCrearDTO(
            "client-1",
            "Ana",
            "Lopez",
            "Calle 1",
            MetodoPago.Bizum,
            new List<LineaReservaCrearDTO>
            {
                new(999, TiempoReserva.UnaHora)
            });
        var controller = new ReservaController(context);

        var action = await controller.CrearReserva(dto);

        Assert.IsType<NotFoundObjectResult>(action.Result);
    }

    [Fact]
    public async Task CrearReservaDevuelveBadRequestSiElModelStateEsInvalido()
    {
        await using var context = CreateContext();
        var controller = new ReservaController(context);
        controller.ModelState.AddModelError("LineasReserva", "Debe incluir una línea.");
        var dto = new ReservaCrearDTO(
            "client-1",
            "Ana",
            "Lopez",
            "Calle 1",
            MetodoPago.Bizum,
            new List<LineaReservaCrearDTO>());

        var action = await controller.CrearReserva(dto);

        Assert.IsType<BadRequestObjectResult>(action.Result);
    }

    private ApplicationDbContext CreateContext() => new(_options);

    public void Dispose()
    {
        _connection.Dispose();
    }
}
