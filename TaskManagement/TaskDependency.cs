namespace TaskManagement;
public class TaskDependency
{
  public int DependentTaskId { get; set; }
  public int DependencyTaskId { get; set; }

  // Navigation property for the dependent task
  public Task DependentTask { get; set; } = null!;

  // Navigation property for the dependency task
  public Task DependencyTask { get; set; } = null!;
}