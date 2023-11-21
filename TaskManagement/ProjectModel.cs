namespace TaskManagement;
public class ProjectModel : SaveProjectModel
{
  // We'll have a list of Models that represent the phases
  public List<ProjectPhaseModel> ProjectPhases { get; set; } = [];
}
