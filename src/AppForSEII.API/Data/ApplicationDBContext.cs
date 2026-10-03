using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.ApplicationUserDTO;

namespace AppForSEII.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<CompraModelo3D> CompraModelos3D { get; set; }
    public DbSet<LicenciaModelo3D> LicenciasModelo3D { get; set; }
    public DbSet<lineaCompraModelo3D> LineaCompraModelo3D { get; set; }

    public DbSet<Modelo3D> Modelos3D { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);


    }


    public DbSet<ApplicationUser> ApplicationUsers { get; set; }




}