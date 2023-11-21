using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement;
public class Task
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Description { get; set; }

  [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
  public DateTime? CreatedDate { get; set; }
  public string? CreatedBy { get; set; }
  public string? LastUpdatedBy { get; set; }
  public DateTime? DueDate { get; set; }
  public int? Priority { get; set; }

  public int ProjectPhaseId { get; set; }
  public ProjectPhase ProjectPhase { get; set; } = null!;

  public int? ProgressionId { get; set; }
  public Progression? Progression { get; set; }

  // Navigation property for dependent tasks
  public virtual ICollection<TaskDependency> DependentTasks { get; set; } = new List<TaskDependency>();

  // Navigation property for dependency tasks
  public virtual ICollection<TaskDependency> DependencyTasks { get; set; } = new List<TaskDependency>();
}