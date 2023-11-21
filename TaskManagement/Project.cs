using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement;
public class Project
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public string? Description { get; set; }
  [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
  public DateTime? CreatedDate { get; set; }
  public string? CreatedBy { get; set; }
  public string? LastUpdatedBy { get; set; }

  public virtual ICollection<ProjectPhase> ProjectPhases { get; set; } = [];
}
