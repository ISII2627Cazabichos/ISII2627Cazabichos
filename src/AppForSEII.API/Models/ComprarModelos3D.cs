public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
: IdentityDbContext<ApplicationUser>(options)
{
public DbSet<ComprarModelos3D> ComprasModelo3D { get; set; }
}