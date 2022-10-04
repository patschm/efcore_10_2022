using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace BasicEF;

public class DatabseContext : DbContext
{
    //public DatabseContext()
    //{

    //}
    public DatabseContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Person> People => Set<Person>();
    public DbSet<Hobby> Hobbies => Set<Hobby>();

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer(Program.conStr);
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(bld => {
            bld.HasIndex(p => p.Name);
            bld.Property(p => p.Name)
                .HasMaxLength(255)
        .IsRequired();
            bld.HasData(new Person { Id = 1, Name = "Jan de Vries" }, new Person { Id = 2, Name = "Marieke Klaasen"});
            //bld.ToTable("Mensen", "dbo");
            //bld.Property(p => p.Id).UseIdentityColumn();
        });
        modelBuilder.Entity<Hobby>(bld => {
            bld.HasIndex(p => p.Description);
            bld.Property(p => p.Description)
                .HasMaxLength(1024)
                .IsRequired();
            bld.HasData(new Hobby { Id = 1, Description = "Sigarenbandjes" }, new Hobby { Id = 2, Description = "Kanteklossen" });
        });
    }
}
