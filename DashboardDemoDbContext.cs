using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DashboardDemo.Entities.Models;
using DashboardDemo.Entities.Identity.Roles;
using DashboardDemo.Entities.Identity.Users;
using DashboardDemo.Entities.DTOs.Turnos;

namespace DashboardDemo
{
    public class DashboardDemoDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
                    ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
                    ApplicationRoleClaim, ApplicationUserToken>
    {
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Asignacion> Asignaciones { get; set; }

        public DashboardDemoDbContext(DbContextOptions<DashboardDemoDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Clave alternativa compuesta en Asignaciones
            modelBuilder.Entity<Asignacion>()
            .HasAlternateKey(e => new { e.TurnoId, e.MesaId });

            modelBuilder.Entity<ApplicationUser>(b =>
            {

                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });
        }
    }
}
