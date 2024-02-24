using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using System;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("Users");

        modelBuilder.Entity<User>().HasData(new[]
        {
            new User { Id = 1,Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth= DateTime.Parse("10/10/1991") },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true,  DateOfBirth= DateTime.Parse("7/15/1972") },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false,  DateOfBirth= DateTime.Parse("7/17/1989") },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true,  DateOfBirth= DateTime.Parse("3/23/1985") },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true,  DateOfBirth= DateTime.Parse("10/20/1970") },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true,  DateOfBirth= DateTime.Parse("2/22/1982") },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false,  DateOfBirth= DateTime.Parse("5/29/1986") },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false,  DateOfBirth= DateTime.Parse("10/15/1997") },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false,  DateOfBirth= DateTime.Parse("7/20/1997") },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true,  DateOfBirth= DateTime.Parse("6/11/1980") },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true,  DateOfBirth= DateTime.Parse("6/11/1981") },
        });
    }

    public virtual DbSet<User>? Users { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public void Create<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
        SaveChanges();
    }

    public new void Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        SaveChanges();
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        SaveChanges();
    }
}
