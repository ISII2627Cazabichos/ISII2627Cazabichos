
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII.API.DTOs.ApplicationUserDTO;

namespace AppForSEII.API.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Cliente> Cliente { get; set; }

    public DbSet<CompraModelo3D> CompraModelos3D { get; set; }

    public DbSet<LicenciaModelo3D> LicenciasModelo3D { get; set; }

    public DbSet<LineaCompraModelo3D> LineaCompraModelo3D { get; set; }

    public DbSet<Modelo3D> Modelos3D { get; set; }

    public DbSet<EncargoImpresion> EncargoImpresiones { get; set; }

    public DbSet<LineaEncargo> LineasEncargo { get; set; }

    public DbSet<Pieza3D> Piezas3D { get; set; }

    public DbSet<Material> Materiales { get; set; }

    // Nueva entidad intermedia
    public DbSet<PiezaMaterial> PiezasMateriales { get; set; }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    public DbSet<Impresora3D> Impresoras3D { get; set; }

    public DbSet<ReservaImpresora> ReservasImpresora { get; set; }

    public DbSet<LineaReserva> LineasReserva { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Clave primaria compuesta de PiezaMaterial
        builder.Entity<PiezaMaterial>()
            .HasKey(pm => new { pm.PiezaId, pm.MaterialId });

        // Relación Pieza3D (1) -> (N) PiezaMaterial
        builder.Entity<PiezaMaterial>()
            .HasOne(pm => pm.Pieza)
            .WithMany(p => p.MaterialesValidos)
            .HasForeignKey(pm => pm.PiezaId);

        // Relación Material (1) -> (N) PiezaMaterial
        builder.Entity<PiezaMaterial>()
            .HasOne(pm => pm.Material)
            .WithMany(m => m.PiezasValidas)
            .HasForeignKey(pm => pm.MaterialId);
    }
}
