using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.ReservaDTOs;

namespace AppForSEII.UT.ReservaController_test;

public class GetImpresorasParaReservar_test : AppForSEII4SqliteUT
{
    public GetImpresorasParaReservar_test()
    {
        var impresoras = new List<Impresora3D>
        {
            new(
                7,
                "Bambu Lab",
                "X1 Carbon",
                "Filamento",
                "Calibración automática",
                0.30m,
                6m),
            new(
                3,
                "Prusa",
                "MK4",
                "Filamento",
                "Alta precisión",
                0.20m,
                4m)
        };

        _context.AddRange(impresoras);
        _context.SaveChanges();
    }

    // Comprueba que el catálogo se devuelve ordenado por identificador.
    [Fact]
    public async Task GetImpresorasParaReservar_DevuelveLasImpresorasOrdenadas()
    {
        // Arrange
        var controller = new ReservaController(_context);

        // Act
        var result = await controller.GetImpresorasParaReservar();

        // Assert
        var actual = Assert.IsType<List<ImpresoraParaReservarDTO>>(
            Assert.IsType<OkObjectResult>(result.Result).Value);
        var expected = new List<ImpresoraParaReservarDTO>
        {
            new(3, "Prusa", "MK4", "Filamento", "Alta precisión", 4m),
            new(7, "Bambu Lab", "X1 Carbon", "Filamento", "Calibración automática", 6m)
        };
        Assert.Equal(expected, actual);
    }
}
