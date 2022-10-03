using Microsoft.EntityFrameworkCore;

namespace DemoManipulation;

internal class ProductContext : DbContext
{
    public ProductContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>(); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Core");
        modelBuilder.Entity<Brand>()
            .HasMany(b => b.Products)
            .WithOne(b => b.Brand)
            .OnDelete(DeleteBehavior.ClientCascade);
        modelBuilder.Entity<Brand>()
            .Property(b => b.Name)
            .IsConcurrencyToken();

        //  DeleteBehavior.ClientSetNull
        ///         For entities being tracked by the DbContext, the values of foreign key properties in
        ///         dependent entities are set to null when the related principal is deleted.
        ///         This helps keep the graph of entities in a consistent state while they are being tracked, such that a
        ///         fully consistent graph can then be written to the database. If a property cannot be set to null because
        ///         it is not a nullable type, then an exception will be thrown when DbContext.SaveChanges() is called.
        ///         If the database has been created from the model using Entity Framework Migrations or the
        ///         DatabaseFacade.EnsureCreated method, then the behavior in the database
        ///         is to generate an error if a foreign key constraint is violated.
        ///         This is the default for optional relationships. That is, for relationships that have
        ///         nullable foreign keys.

        //  DeleteBehavior.Restrict
        ///         For entities being tracked by the DbContext, the values of foreign key properties in
        ///         dependent entities are set to null when the related principal is deleted.
        ///         This helps keep the graph of entities in a consistent state while they are being tracked, such that a
        ///         fully consistent graph can then be written to the database. If a property cannot be set to null because
        ///         it is not a nullable type, then an exception will be thrown when DbContext.SaveChanges() is called.
        ///         If the database has been created from the model using Entity Framework Migrations or the
        ///         DatabaseFacade.EnsureCreated method, then the behavior in the database
        ///         is to generate an error if a foreign key constraint is violated.

        //  DeleteBehavior.SetNull
        ///         For entities being tracked by the DbContext, the values of foreign key properties in
        ///         dependent entities are set to null when the related principal is deleted.
        ///         This helps keep the graph of entities in a consistent state while they are being tracked, such that a
        ///         fully consistent graph can then be written to the database. If a property cannot be set to null because
        ///         it is not a nullable type, then an exception will be thrown when DbContext.SaveChanges() is called.
        ///         If the database has been created from the model using Entity Framework Migrations or the
        ///         DatabaseFacade.EnsureCreated method, then the behavior in the database is
        ///         the same as is described above for tracked entities. Keep in mind that some databases cannot easily
        ///         support this behavior, especially if there are cycles in relationships, in which case it may
        ///         be better to use ClientSetNull which will allow EF to cascade null values
        ///         on loaded entities even if the database does not support this.

        //  DeleteBehavior.Cascade
        ///         For entities being tracked by the DbContext, dependent entities
        ///         will be deleted when the related principal is deleted.
        ///         If the database has been created from the model using Entity Framework Migrations or the
        ///         DatabaseFacade.EnsureCreated method, then the behavior in the database is
        ///         the same as is described above for tracked entities. Keep in mind that some databases cannot easily
        ///         support this behavior, especially if there are cycles in relationships, in which case it may
        ///         be better to use ClientCascade which will allow EF to perform cascade deletes
        ///         on loaded entities even if the database does not support this.
        ///         This is the default for required relationships. That is, for relationships that have
        ///         non-nullable foreign keys.

        //  DeleteBehavior.ClientCascade
        ///         For entities being tracked by the DbContext, dependent entities
        ///         will be deleted when the related principal is deleted.
        ///         If the database has been created from the model using Entity Framework Migrations or the
        ///         DatabaseFacade.EnsureCreated method, then the behavior in the database
        ///         is to generate an error if a foreign key constraint is violated.

        //  DeleteBehavior.NoAction
        ///         For entities being tracked by the DbContext, the values of foreign key properties in
        ///         dependent entities are set to null when the related principal is deleted.
        ///         This helps keep the graph of entities in a consistent state while they are being tracked, such that a
        ///         fully consistent graph can then be written to the database. If a property cannot be set to null because
        ///         it is not a nullable type, then an exception will be thrown when DbContext.SaveChanges() is called.
        ///         If the database has been created from the model using Entity Framework Migrations or the
        ///         DatabaseFacade.EnsureCreated method, then the behavior in the database
        ///         is to generate an error if a foreign key constraint is violated.

        //  DeleteBehavior.ClientNoAction
        ///         Note: it is unusual to use this value. Consider using ClientSetNull instead to match
        ///         the behavior of EF6 with cascading deletes disabled.
        ///         For entities being tracked by the DbContext, the values of foreign key properties in
        ///         dependent entities are not changed when the related principal entity is deleted.
        ///         This can result in an inconsistent graph of entities where the values of foreign key properties do
        ///         not match the relationships in the graph.
        ///         If the database has been created from the model using Entity Framework Migrations or the
        ///         DatabaseFacade.EnsureCreated method, then the behavior in the database
        ///         is to generate an error if a foreign key constraint is violated.
    }
}
