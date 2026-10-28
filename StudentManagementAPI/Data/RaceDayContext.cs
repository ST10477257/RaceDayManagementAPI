using Microsoft.EntityFrameworkCore;
using StudentManagementAPI.Models;

namespace StudentManagementAPI.Data;

public class RaceDayContext : DbContext
{
    public RaceDayContext(DbContextOptions<RaceDayContext> options) : base(options)
    {
    }


public DbSet<RaceDayUser> Users { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;

    public DbSet<EventCategory> EventCategories { get; set; } = null!;

    public DbSet<Enrolment> Enrolments { get; set; } = null!;

    public DbSet<Result> Results { get; set; } = null!;

    public DbSet<StudentManagementAPI.Models.Route> Routes { get; set; } = null!;

}

