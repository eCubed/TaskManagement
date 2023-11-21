using Microsoft.EntityFrameworkCore;

namespace TaskManagement;
public class TaskDbContext : DbContext
{
  public DbSet<Phase> Phases { get; set; }
  public DbSet<ProjectPhase> ProjectPhases { get; set; }
  public DbSet<Progression> Progressions { get; set; }
  public DbSet<Task> Tasks { get; set; }
  public DbSet<TaskDependency> TaskDependencies { get; set; }
  public DbSet<Project> Projects { get; set; }

  public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
  {
  }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Project>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
    modelBuilder.Entity<Task>().Property(t => t.CreatedDate).HasDefaultValueSql("GETDATE()");

    // Define composite primary key for TaskDependency
    modelBuilder.Entity<TaskDependency>()
        .HasKey(td => new { td.DependentTaskId, td.DependencyTaskId });

    // Configure foreign key relationships
    modelBuilder.Entity<TaskDependency>()
        .HasOne(td => td.DependentTask)
        .WithMany(t => t.DependentTasks)
        .HasForeignKey(td => td.DependentTaskId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<TaskDependency>()
        .HasOne(td => td.DependencyTask)
        .WithMany(t => t.DependencyTasks)
        .HasForeignKey(td => td.DependencyTaskId)
        .OnDelete(DeleteBehavior.NoAction);
  }
}