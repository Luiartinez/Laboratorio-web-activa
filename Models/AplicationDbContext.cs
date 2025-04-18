using Microsoft.EntityFrameworkCore;

namespace Coreproject.Models
{
    public class AplicationDbContext:DbContext
    {
        private readonly AplicationDbContext _context;
        internal object productos;

        public AplicationDbContext(DbContextOptions<AplicationDbContext>options)
            : base(options) { }


        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación: Categoría 1 - M Productos
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Producto)
                .HasForeignKey(p => p.CategoriaId);

            // Relación: Proveedor 1 - M Productos
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Proveedor)
                .WithMany(prov => prov.Productos)
                .HasForeignKey(p => p.ProveedorId);

            // Relación: Inventario 1 - 1 Producto
            modelBuilder.Entity<Inventario>()
                .HasOne(i => i.Producto)
                .WithOne(p => p.Inventario)
                .HasForeignKey<Inventario>(i => i.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

        }


    }
}
