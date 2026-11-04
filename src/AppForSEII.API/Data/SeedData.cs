
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Data
{
    public class SeedData
    {
        public static void Initialize(
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider,
            ILogger logger)
        {
            // ROLES
            List<string> rolesNames = new List<string>
            {
                "Administrator",
                "Employee",
                "Customer"
            };

            var roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                SeedRoles(roleManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the roles.");
            }

            // USERS
            var userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            try
            {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Users.");
            }

            // MATERIALS
            try
            {
                SeedMaterials(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the Materials.");
            }

            // PIEZAS 3D Y SUS MATERIALES VÁLIDOS
            try
            {
                SeedPiezas3D(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the 3D Pieces.");
            }

            // ENCARGOS DE IMPRESIÓN
            try
            {
                var user = dbContext.Cliente
                    .FirstOrDefault(c => c.UserName == "peter@uclm.es");

                if (user == null)
                {
                    logger.LogError(
                        "Customer does not exist. Cannot seed EncargoImpresion.");
                }
                else
                {
                    SeedEncargoImpresion(dbContext, user);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding the EncargoImpresion.");
            }
        }

        // =========================================================
        // ROLES
        // =========================================================

        public static void SeedRoles(
            RoleManager<IdentityRole> roleManager,
            List<string> roles)
        {
            foreach (string roleName in roles)
            {
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();

                    role.Name = roleName;
                    role.NormalizedName = roleName;

                    roleManager.CreateAsync(role).Wait();
                }
            }
        }

        // =========================================================
        // USERS
        // =========================================================

        public static void SeedUsers(
            UserManager<ApplicationUser> userManager,
            List<string> roles)
        {
            // ADMINISTRATOR
            if (userManager.FindByNameAsync("elena@uclm.es").Result == null)
            {
                ApplicationUser user = new ApplicationUser(
                    "1",
                    "Elena",
                    "Navarro Martínez",
                    "elena@uclm.es");

                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(
                    user,
                    "Password1234%").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, roles[0]).Wait();
                }
            }

            // CUSTOMER
            if (userManager.FindByNameAsync("peter@uclm.es").Result == null)
            {
                Cliente user = new Cliente
                {
                    Id = "3",
                    Name = "Peter",
                    Surname = "Jackson",
                    UserName = "peter@uclm.es",
                    Email = "peter@uclm.es",
                    EmailConfirmed = true,
                    DireccionFacturacion =
                        "Calle Mayor 10, Albacete 02001"
                };

                var result = userManager.CreateAsync(
                    user,
                    "OtherPass12$").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, roles[2]).Wait();
                }
            }
        }

        // =========================================================
        // MATERIALS
        // =========================================================

        public static void SeedMaterials(
            ApplicationDbContext dbContext)
        {
            if (!dbContext.Materiales.Any(m => m.Nombre == "PLA"))
            {
                dbContext.Materiales.Add(new Material
                {
                    Nombre = "PLA",
                    PrecioPorGramo = 0.05m,
                    StockGramos = 5000m
                });
            }

            if (!dbContext.Materiales.Any(m => m.Nombre == "ABS"))
            {
                dbContext.Materiales.Add(new Material
                {
                    Nombre = "ABS",
                    PrecioPorGramo = 0.07m,
                    StockGramos = 3000m
                });
            }

            if (!dbContext.Materiales.Any(m => m.Nombre == "PETG"))
            {
                dbContext.Materiales.Add(new Material
                {
                    Nombre = "PETG",
                    PrecioPorGramo = 0.08m,
                    StockGramos = 2000m
                });
            }

            if (!dbContext.Materiales.Any(m => m.Nombre == "TPU"))
            {
                dbContext.Materiales.Add(new Material
                {
                    Nombre = "TPU",
                    PrecioPorGramo = 0.10m,
                    StockGramos = 1500m
                });
            }

            dbContext.SaveChanges();
        }

        // =========================================================
        // PIEZAS 3D Y RELACIONES PIEZAMATERIAL
        // =========================================================

        public static void SeedPiezas3D(
            ApplicationDbContext dbContext)
        {
            // Obtener o crear las piezas

            var pieza1 = ObtenerOCrearPieza(
                dbContext,
                "Soporte para móvil",
                80m,
                CategoriaPieza.HerramientasYAccesorios);

            var pieza2 = ObtenerOCrearPieza(
                dbContext,
                "Figura decorativa",
                150m,
                CategoriaPieza.Decoracion);

            var pieza3 = ObtenerOCrearPieza(
                dbContext,
                "Engranaje de repuesto",
                60m,
                CategoriaPieza.Repuestos);

            var pieza4 = ObtenerOCrearPieza(
                dbContext,
                "Miniatura de personaje",
                45m,
                CategoriaPieza.MiniaturasYMaquetas);

            // Guardar las piezas antes de crear las relaciones
            dbContext.SaveChanges();

            // Obtener materiales
            var pla = dbContext.Materiales.First(m => m.Nombre == "PLA");
            var abs = dbContext.Materiales.First(m => m.Nombre == "ABS");
            var petg = dbContext.Materiales.First(m => m.Nombre == "PETG");
            var tpu = dbContext.Materiales.First(m => m.Nombre == "TPU");

            // Crear relaciones mediante PiezaMaterial

            // Pieza 1: Soporte para móvil
            AgregarMaterialValido(dbContext, pieza1, pla);
            AgregarMaterialValido(dbContext, pieza1, petg);

            // Pieza 2: Figura decorativa
            AgregarMaterialValido(dbContext, pieza2, pla);
            AgregarMaterialValido(dbContext, pieza2, abs);

            // Pieza 3: Engranaje de repuesto
            AgregarMaterialValido(dbContext, pieza3, abs);
            AgregarMaterialValido(dbContext, pieza3, petg);

            // Pieza 4: Miniatura de personaje
            AgregarMaterialValido(dbContext, pieza4, pla);
            AgregarMaterialValido(dbContext, pieza4, tpu);

            dbContext.SaveChanges();
        }

        // Obtener una pieza existente o crearla
        private static Pieza3D ObtenerOCrearPieza(
            ApplicationDbContext dbContext,
            string nombre,
            decimal peso,
            CategoriaPieza categoria)
        {
            var pieza = dbContext.Piezas3D
                .FirstOrDefault(p => p.Nombre == nombre);

            if (pieza == null)
            {
                pieza = new Pieza3D
                {
                    Nombre = nombre,
                    Peso = peso,
                    Categoria = categoria
                };

                dbContext.Piezas3D.Add(pieza);
            }

            return pieza;
        }

        // Crear la relación si todavía no existe
        private static void AgregarMaterialValido(
            ApplicationDbContext dbContext,
            Pieza3D pieza,
            Material material)
        {
            bool existe = dbContext.PiezasMateriales.Any(pm =>
                pm.PiezaId == pieza.Id &&
                pm.MaterialId == material.Id);

            if (!existe)
            {
                dbContext.PiezasMateriales.Add(new PiezaMaterial
                {
                    PiezaId = pieza.Id,
                    MaterialId = material.Id
                });
            }
        }

        // =========================================================
        // ENCARGO DE IMPRESIÓN
        // =========================================================

        public static void SeedEncargoImpresion(
            ApplicationDbContext dbContext,
            Cliente cliente)
        {
            if (dbContext.EncargoImpresiones.Any(e => e.Id == 1))
            {
                return;
            }

            var pieza = dbContext.Piezas3D
                .First(p => p.Nombre == "Soporte para móvil");

            var material = dbContext.Materiales
                .First(m => m.Nombre == "PLA");

            // Precio: peso x precio por gramo
            decimal precioUnidad =
                pieza.Peso * material.PrecioPorGramo;

            int cantidad = 2;

            decimal subtotal = precioUnidad * cantidad;

            var encargo = new EncargoImpresion
            {
                FechaEncargo = DateTime.Now,

                NombreCliente = cliente.Name ?? "Peter",

                ApellidosCliente = cliente.Surname ?? "Jackson",

                DireccionEnvio = "Calle Mayor 10, Albacete 02001",

                NumeroTelefono = "600123456",

                Descripcion =
                    "Impresión de dos unidades del soporte para móvil.",

                PrecioTotal = subtotal,

                MetodoPago = MetodoPago.TarjetaCredito,

                Cliente = cliente
            };

            // Crear línea del encargo
            var linea = new LineaEncargo
            {
                Cantidad = cantidad,

                PrecioUnidad = precioUnidad,

                Subtotal = subtotal,

                PiezaId = pieza.Id,

                Pieza = pieza,

                MaterialSeleccionadoId = material.Id,

                MaterialSeleccionado = material
            };

            encargo.LineasEncargo.Add(linea);

            dbContext.EncargoImpresiones.Add(encargo);

            dbContext.SaveChanges();
        }
    }
}
