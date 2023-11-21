namespace TaskManagement;
public class ProjectPhase
{
  public int Id { get; set; }
  public int ProjectId { get; set; }
  public Project Project { get; set; } = null!;
  public int PhaseId { get; set; }
  public Phase Phase { get; set; } = null!;
  public int? OrderInSequence { get; set; }
}
