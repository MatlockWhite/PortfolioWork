/**********************************************
 * 
 *      Portfolio Work For Matlock White
 *          User Service Example
 *        Data: UserServiceContext
 *              
 * ********************************************/



using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data;

/// <summary>
/// The UserServiceBaseContext for defining Data types and setup for each of the DB Contexts.
/// </summary>
public class UserServiceContextBase : DbContext
{
    /// <summary>
    /// The Database EF Core Base Context.
    /// </summary>
    /// <param name="options">The UserServiceContextBase</param>
    public UserServiceContextBase(DbContextOptions options)
        : base(options)
    {
    }

    /// <summary>
    /// The DBSet Reference for Users.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// The DBSet Reference for Roles.
    /// </summary>
    public DbSet<Role> Roles { get; set; }


    /*
     * NOTE: Because this is being made for demonstration purposes,
     *       the Fluent API mappings below are intentionally verbose to demonstrate:
     *          - Explicit Table/Column constraints
     *          - Unique Index declarations
     *          - Required and Nullable declarations
     *          - Default value declarations
     *          - MySQL-specific Types
     *          - Foreign-Key relationship and navigation properties
     *          
     *      In a typical implementation, many of these mappings can be inferred
     *      through Entity Framework conventions, data annotations, or generated
     *      from an existing database schema. Explicit configuration would normally
     *      be reserved for mappings where additional control or clarification is needed.
     *
     *      The additional verbosity here is intentional so the database-to-entity
     *      mapping and EF Core configuration are clearly demonstrated.
     *      
     */
    /// <summary>
    /// The Model Creation/Mapping Function.
    /// </summary>
    /// <param name="modelBuilder">Provided Model Builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // The DB Mappings and restrictions for the Role Model.
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("role");

            entity.HasKey(r => r.Id);

            entity.Property(r => r.RoleKey)
                .HasColumnName("RoleKey")
                .HasMaxLength(45)
                .IsRequired();

            entity.HasIndex(r => r.RoleKey)
                .IsUnique();

            entity.Property(r => r.DisplayName)
                .HasColumnName("DisplayName")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(r => r.Description)
                .HasColumnName("Description")
                .HasMaxLength(255);
        });

        // The DB Mappings and restrictions for the User Model.
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Username)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(u => u.Username)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(u => u.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(u => u.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(u => u.Team)
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.PhoneNumber)
                .HasMaxLength(25);

            entity.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(u => u.LastActive)
                .HasColumnType("datetime");

            // Includes the Role Reference based one Role Id.
            entity.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);
        });
    }
}