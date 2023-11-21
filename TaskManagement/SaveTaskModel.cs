namespace TaskManagement;
public class SaveTaskModel
{
  public string Name { get; set; } = null!;
  public string? Description { get; set; } // for simplicity, we will not create dated notes.
  public int? ProgressionId { get; set; }
  public int? Priority { get; set; }
  public DateTime? DueDate { get; set; }
}
