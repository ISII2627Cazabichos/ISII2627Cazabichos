
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

            try
            {
                SeedImpresoras3D(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred seeding the 3D printers.");
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

        public static void SeedMaterials(ApplicationDbContext dbContext)
        {
            var materiales = new[]
            {
                new Material
                {
                    Nombre = "PLA",
                    PrecioPorGramo = 0.05m,
                    StockGramos = 5000m
                },
                new Material
                {
                    Nombre = "ABS",
                    PrecioPorGramo = 0.07m,
                    StockGramos = 3000m
                },
                new Material
                {
                    Nombre = "PETG",
                    PrecioPorGramo = 0.08m,
                    StockGramos = 2000m
                },
                new Material
                {
                    Nombre = "TPU",
                    PrecioPorGramo = 0.10m,
                    StockGramos = 1500m
                },
                new Material
                {
                    Nombre = "Nylon",
                    PrecioPorGramo = 0.12m,
                    StockGramos = 2500m
                },
                new Material
                {
                    Nombre = "Resina",
                    PrecioPorGramo = 0.15m,
                    StockGramos = 2000m
                },
                new Material
                {
                    Nombre = "PLA+",
                    PrecioPorGramo = 0.06m,
                    StockGramos = 3500m
                },
                new Material
                {
                    Nombre = "Fibra de carbono",
                    PrecioPorGramo = 0.20m,
                    StockGramos = 1000m
                }
            };

            foreach (var material in materiales)
            {
                if (!dbContext.Materiales.Any(m => m.Nombre == material.Nombre))
                {
                    dbContext.Materiales.Add(material);
                }
            }

            dbContext.SaveChanges();
        }

        public static void SeedImpresoras3D(ApplicationDbContext dbContext)
        {
            var impresoras = new[]
            {
                new Impresora3D(
                    1,
                    "Prusa",
                    "MK4",
                    "Filamento",
                    "Impresora FDM de alta precisión para prototipos y piezas funcionales.",
                    0.20m,
                    4m),
                new Impresora3D(
                    2,
                    "Elegoo",
                    "Mars 4",
                    "Resina",
                    "Impresora de resina para modelos detallados y miniaturas.",
                    0.25m,
                    5m),
                new Impresora3D(
                    3,
                    "Bambu Lab",
                    "X1 Carbon",
                    "Filamento",
                    "Impresora rápida con calibración automática y múltiples materiales.",
                    0.30m,
                    6m)
            };

            foreach (var impresora in impresoras)
            {
                if (!dbContext.Impresoras3D.Any(i => i.Id == impresora.Id))
                {
                    dbContext.Impresoras3D.Add(impresora);
                }
            }

            dbContext.SaveChanges();
        }

        // =========================================================
        // PIEZAS 3D Y RELACIONES PIEZAMATERIAL
        // =========================================================

        public static void SeedPiezas3D(ApplicationDbContext dbContext)
        {
            var piezas = new[]
            {
                // DECORACIÓN
                ("Figura decorativa", 150m, CategoriaPieza.Decoracion, new[] { "PLA", "ABS" }),
                ("Jarrón decorativo", 180m, CategoriaPieza.Decoracion, new[] { "PLA", "PETG" }),
                ("Maceta geométrica", 120m, CategoriaPieza.Decoracion, new[] { "PLA+", "PETG" }),
                ("Lámpara decorativa", 250m, CategoriaPieza.Decoracion, new[] { "ABS", "PLA" }),
                ("Soporte para velas", 90m, CategoriaPieza.Decoracion, new[] { "PLA", "PLA+" }),

                // MINIATURAS Y MAQUETAS
                ("Miniatura de personaje", 45m, CategoriaPieza.MiniaturasYMaquetas, new[] { "PLA", "Resina" }),
                ("Dragón fantástico", 80m, CategoriaPieza.MiniaturasYMaquetas, new[] { "PLA", "Resina" }),
                ("Casa en miniatura", 200m, CategoriaPieza.MiniaturasYMaquetas, new[] { "PLA", "ABS" }),
                ("Figura de colección", 100m, CategoriaPieza.MiniaturasYMaquetas, new[] { "Resina", "PLA" }),
                ("Vehículo en miniatura", 75m, CategoriaPieza.MiniaturasYMaquetas, new[] { "PLA", "ABS" }),

                // REPUESTOS
                ("Engranaje de repuesto", 60m, CategoriaPieza.Repuestos, new[] { "ABS", "PETG" }),
                ("Pieza de unión", 35m, CategoriaPieza.Repuestos, new[] { "Nylon", "ABS" }),
                ("Bisagra mecánica", 55m, CategoriaPieza.Repuestos, new[] { "PETG", "Nylon" }),
                ("Tapa de protección", 40m, CategoriaPieza.Repuestos, new[] { "ABS", "PLA+" }),
                ("Soporte estructural", 130m, CategoriaPieza.Repuestos, new[] { "Nylon", "Fibra de carbono" }),

                // HERRAMIENTAS Y ACCESORIOS
                ("Soporte para móvil", 80m, CategoriaPieza.HerramientasYAccesorios, new[] { "PLA", "PETG" }),
                ("Organizador de escritorio", 160m, CategoriaPieza.HerramientasYAccesorios, new[] { "PLA", "PLA+" }),
                ("Soporte para auriculares", 110m, CategoriaPieza.HerramientasYAccesorios, new[] { "PLA", "ABS" }),
                ("Carcasa electrónica", 95m, CategoriaPieza.HerramientasYAccesorios, new[] { "ABS", "PETG" }),
                ("Adaptador universal", 50m, CategoriaPieza.HerramientasYAccesorios, new[] { "TPU", "Nylon" })
            };

            foreach (var dato in piezas)
            {
                var pieza = ObtenerOCrearPieza(
                    dbContext,
                    dato.Item1,
                    dato.Item2,
                    dato.Item3
                );

                dbContext.SaveChanges();

                foreach (var nombreMaterial in dato.Item4)
                {
                    var material = dbContext.Materiales
                        .First(m => m.Nombre == nombreMaterial);

                    AgregarMaterialValido(dbContext, pieza, material);
                }
            }

            dbContext.SaveChanges();
        }

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
            var encargos = new[]
            {
                ("Soporte para móvil", "PLA", 2),
                ("Figura decorativa", "ABS", 1),
                ("Engranaje de repuesto", "PETG", 5),
                ("Miniatura de personaje", "PLA", 3),
                ("Jarrón decorativo", "PETG", 2),
                ("Maceta geométrica", "PLA+", 4),
                ("Dragón fantástico", "Resina", 1),
                ("Pieza de unión", "Nylon", 6),
                ("Bisagra mecánica", "PETG", 3),
                ("Organizador de escritorio", "PLA", 2),
                ("Soporte para auriculares", "ABS", 1),
                ("Carcasa electrónica", "PETG", 2),
                ("Soporte estructural", "Nylon", 3),
                ("Adaptador universal", "TPU", 5),
                ("Lámpara decorativa", "ABS", 1)
            };

            for (int i = 0; i < encargos.Length; i++)
            {
                string descripcion = $"SEED-ENCARGO-{i + 1:D2}";

                bool existe = dbContext.EncargoImpresiones
                    .Any(e => e.Descripcion == descripcion);

                if (existe)
                    continue;

                var pieza = dbContext.Piezas3D
                    .First(p => p.Nombre == encargos[i].Item1);

                var material = dbContext.Materiales
                    .First(m => m.Nombre == encargos[i].Item2);

                bool materialValido = dbContext.PiezasMateriales.Any(pm =>
                    pm.PiezaId == pieza.Id &&
                    pm.MaterialId == material.Id);

                if (!materialValido)
                    continue;

                int cantidad = encargos[i].Item3;

                decimal precioUnidad =
                    pieza.Peso * material.PrecioPorGramo;

                decimal subtotal = precioUnidad * cantidad;

                var encargo = new EncargoImpresion
                {
                    FechaEncargo = DateTime.Now.AddDays(-i),

                    NombreCliente = cliente.Name ?? "Peter",

                    ApellidosCliente = cliente.Surname ?? "Jackson",

                    DireccionEnvio = "Calle Mayor 10, Albacete 02001",

                    NumeroTelefono = "600123456",

                    Descripcion = descripcion,

                    PrecioTotal = subtotal,

                    MetodoPago = MetodoPago.TarjetaCredito,

                    Cliente = cliente
                };

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
            }

            dbContext.SaveChanges();
        }

    }
}
