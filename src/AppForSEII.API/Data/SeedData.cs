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
            // ============================================
            // ROLES
            // ============================================

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
                logger.LogError(
                    ex,
                    "An error occurred seeding the roles in the Database.");
            }


            // ============================================
            // USERS
            // ============================================

            var userManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            try
            {
                SeedUsers(userManager, rolesNames);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding the Users in the Database.");
            }


            // ============================================
            // MATERIALS
            // ============================================

            try
            {
                SeedMaterials(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding the Materials in the Database.");
            }


            // ============================================
            // PIEZAS 3D
            // ============================================

            try
            {
                SeedPiezas3D(dbContext);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding the 3D Pieces in the Database.");
            }


            // ============================================
            // ENCARGOS DE IMPRESIÓN
            // ============================================

            try
            {
                var user = dbContext.Cliente
                    .FirstOrDefault(c => c.UserName == "peter@uclm.es");

                if (user == null)
                {
                    logger.LogError(
                        "An error occurred seeding an EncargoImpresion in the Database: customer does not exist.");
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
                    "An error occurred seeding the EncargoImpresion in the Database.");
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
                // Comprueba que el rol no exista en la base de datos
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    IdentityRole role = new IdentityRole();

                    role.Name = roleName;
                    role.NormalizedName = roleName;

                    IdentityResult roleResult =
                        roleManager.CreateAsync(role).Result;
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
            // ============================================
            // ADMINISTRATOR
            // ============================================

            if (userManager.FindByNameAsync("elena@uclm.es").Result == null)
            {
                ApplicationUser user =
                    new ApplicationUser(
                        "1",
                        "Elena",
                        "Navarro Martínez",
                        "elena@uclm.es");

                user.EmailConfirmed = true;

                var result =
                    userManager.CreateAsync(
                        user,
                        "Password1234%");

                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    // Administrator role
                    userManager
                        .AddToRoleAsync(user, roles[0])
                        .Wait();
                }
            }


            // ============================================
            // CUSTOMER
            // ============================================

            if (userManager.FindByNameAsync("peter@uclm.es").Result == null)
            {
                Cliente user =
                    new Cliente
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

                var result =
                    userManager.CreateAsync(
                        user,
                        "OtherPass12$");

                result.Wait();

                if (result.IsCompletedSuccessfully)
                {
                    // Customer role
                    userManager
                        .AddToRoleAsync(user, roles[2])
                        .Wait();
                }
            }
        }


        // =========================================================
        // MATERIALS
        // =========================================================

        public static void SeedMaterials(
            ApplicationDbContext dbContext)
        {
            /*
             * Materiales necesarios para probar el caso de uso:
             *
             * - Un material por defecto para cada pieza.
             * - Otros materiales válidos para poder cambiar
             *   el material de la pieza.
             *
             * También se incluyen diferentes stocks para poder
             * probar los flujos relacionados con disponibilidad.
             */

            if (dbContext.Materiales
                .FirstOrDefault(m => m.Nombre == "PLA") == null)
            {
                dbContext.Materiales.Add(
                    new Material
                    {
                        Nombre = "PLA",
                        PrecioPorGramo = 0.05m,
                        StockGramos = 5000m
                    });
            }

            if (dbContext.Materiales
                .FirstOrDefault(m => m.Nombre == "ABS") == null)
            {
                dbContext.Materiales.Add(
                    new Material
                    {
                        Nombre = "ABS",
                        PrecioPorGramo = 0.07m,
                        StockGramos = 3000m
                    });
            }

            if (dbContext.Materiales
                .FirstOrDefault(m => m.Nombre == "PETG") == null)
            {
                dbContext.Materiales.Add(
                    new Material
                    {
                        Nombre = "PETG",
                        PrecioPorGramo = 0.08m,
                        StockGramos = 2000m
                    });
            }

            if (dbContext.Materiales
                .FirstOrDefault(m => m.Nombre == "TPU") == null)
            {
                dbContext.Materiales.Add(
                    new Material
                    {
                        Nombre = "TPU",
                        PrecioPorGramo = 0.10m,
                        StockGramos = 1500m
                    });
            }

            dbContext.SaveChanges();
        }


        // =========================================================
        // PIEZAS 3D
        // =========================================================

        public static void SeedPiezas3D(
            ApplicationDbContext dbContext)
        {
            // -----------------------------------------------------
            // Obtener materiales existentes
            // -----------------------------------------------------

            var pla = dbContext.Materiales
                .First(m => m.Nombre == "PLA");

            var abs = dbContext.Materiales
                .First(m => m.Nombre == "ABS");

            var petg = dbContext.Materiales
                .First(m => m.Nombre == "PETG");

            var tpu = dbContext.Materiales
                .First(m => m.Nombre == "TPU");


            // -----------------------------------------------------
            // PIEZA 1
            // -----------------------------------------------------

            var pieza1 = dbContext.Piezas3D
                .Include(p => p.MaterialesValidos)
                .FirstOrDefault(
                    p => p.Nombre == "Soporte para móvil");

            if (pieza1 == null)
            {
                pieza1 = new Pieza3D
                {
                    Nombre = "Soporte para móvil",
                    Peso = 80m,
                    Categoria = CategoriaPieza.HerramientasYAccesorios
                };

                pieza1.MaterialesValidos.Add(pla);
                pieza1.MaterialesValidos.Add(petg);

                dbContext.Piezas3D.Add(pieza1);
            }
            else
            {
                pieza1.MaterialesValidos.Add(pla);
                pieza1.MaterialesValidos.Add(petg);
            }


            // -----------------------------------------------------
            // PIEZA 2
            // -----------------------------------------------------

            var pieza2 = dbContext.Piezas3D
                .Include(p => p.MaterialesValidos)
                .FirstOrDefault(
                    p => p.Nombre == "Figura decorativa");

            if (pieza2 == null)
            {
                pieza2 = new Pieza3D
                {
                    Nombre = "Figura decorativa",
                    Peso = 150m,
                    Categoria = CategoriaPieza.Decoracion
                };

                pieza2.MaterialesValidos.Add(pla);
                pieza2.MaterialesValidos.Add(abs);

                dbContext.Piezas3D.Add(pieza2);
            }
            else
            {
                pieza2.MaterialesValidos.Add(pla);
                pieza2.MaterialesValidos.Add(abs);
            }


            // -----------------------------------------------------
            // PIEZA 3
            // -----------------------------------------------------

            var pieza3 = dbContext.Piezas3D
                .Include(p => p.MaterialesValidos)
                .FirstOrDefault(
                    p => p.Nombre == "Engranaje de repuesto");

            if (pieza3 == null)
            {
                pieza3 = new Pieza3D
                {
                    Nombre = "Engranaje de repuesto",
                    Peso = 60m,
                    Categoria = CategoriaPieza.Repuestos
                };

                pieza3.MaterialesValidos.Add(abs);
                pieza3.MaterialesValidos.Add(petg);

                dbContext.Piezas3D.Add(pieza3);
            }
            else
            {
                pieza3.MaterialesValidos.Add(abs);
                pieza3.MaterialesValidos.Add(petg);
            }


            // -----------------------------------------------------
            // PIEZA 4
            // -----------------------------------------------------

            var pieza4 = dbContext.Piezas3D
                .Include(p => p.MaterialesValidos)
                .FirstOrDefault(
                    p => p.Nombre == "Miniatura de personaje");

            if (pieza4 == null)
            {
                pieza4 = new Pieza3D
                {
                    Nombre = "Miniatura de personaje",
                    Peso = 45m,
                    Categoria = CategoriaPieza.MiniaturasYMaquetas
                };

                pieza4.MaterialesValidos.Add(pla);
                pieza4.MaterialesValidos.Add(tpu);

                dbContext.Piezas3D.Add(pieza4);
            }
            else
            {
                pieza4.MaterialesValidos.Add(pla);
                pieza4.MaterialesValidos.Add(tpu);
            }


            // -----------------------------------------------------
            // Guardar cambios
            // -----------------------------------------------------

            dbContext.SaveChanges();
        }


        // =========================================================
        // ENCARGO DE IMPRESIÓN
        // =========================================================

        public static void SeedEncargoImpresion(
            ApplicationDbContext dbContext,
            Cliente cliente)
        {
            /*
             * Se crea un encargo de ejemplo para poder probar
             * el caso de uso "Encargar impresión de piezas 3D".
             *
             * El encargo contiene:
             * - Datos del cliente
             * - Dirección de envío
             * - Teléfono
             * - Método de pago
             * - Piezas encargadas
             * - Material utilizado
             * - Cantidad
             * - Precio por unidad
             * - Subtotal
             * - Precio total
             */

            if (dbContext.EncargoImpresiones
                .FirstOrDefault(e => e.Id == 1) == null)
            {
                var pieza =
                    dbContext.Piezas3D
                        .Include(p => p.MaterialesValidos)
                        .First(p => p.Nombre == "Soporte para móvil");

                var material =
                    dbContext.Materiales
                        .First(m => m.Nombre == "PLA");


                // Precio de impresión:
                // peso × precio por gramo
                decimal precioUnidad =
                    pieza.Peso * material.PrecioPorGramo;

                int cantidad = 2;

                decimal subtotal =
                    precioUnidad * cantidad;


                var encargo =
                    new EncargoImpresion
                    {
                        FechaEncargo = DateTime.Now,

                        NombreCliente = cliente.Name ?? "Peter",

                        ApellidosCliente =
                            cliente.Surname ?? "Jackson",

                        DireccionEnvio =
                            "Calle Mayor 10, Albacete 02001",

                        NumeroTelefono =
                            "600123456",

                        Descripcion =
                            "Impresión de dos unidades del soporte para móvil.",

                        PrecioTotal = subtotal,

                        MetodoPago =
                            MetodoPago.TarjetaCredito,

                        Cliente = cliente
                    };


                // Crear línea del encargo

                var linea =
                    new LineaEncargo
                    {
                        Cantidad = cantidad,

                        PrecioUnidad = precioUnidad,

                        Subtotal = subtotal,

                        Pieza = pieza,

                        MaterialSeleccionado = material
                    };


                // Añadir la línea al encargo

                encargo.LineasEncargo.Add(linea);


                // Añadir el encargo a la base de datos

                dbContext.EncargoImpresiones.Add(encargo);


                // Guardar cambios

                dbContext.SaveChanges();
            }
        }
    }
}