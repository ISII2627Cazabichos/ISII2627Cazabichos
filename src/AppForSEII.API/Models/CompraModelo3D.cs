public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
: IdentityDbContext<ApplicationUser>(options)
{
public DbSet<CompraModelo3D> ComprasModelo3D { get; set; }
}