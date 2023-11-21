namespace TaskManagement;
public class TaskListItem
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Description { get; set; } // for simplicity, we will not create dated notes.
  public int? ProgressionId { get; set; }
  public string? ProgressionName { get; set; }
  public string? CreatedBy { get; set; }
  public int? Priority { get; set; }
  public string? LastUpdatedBy { get; set; }

}
