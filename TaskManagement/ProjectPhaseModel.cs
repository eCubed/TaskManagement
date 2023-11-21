using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement;
public class ProjectPhaseModel
{
  /// <summary>
  /// The Id of the ProjectPhase
  /// </summary>
  public int Id { get; set; }
  /// <summary>
  /// The name of the phase
  /// </summary>
  public string Name { get; set; } = null!;

  /// <summary>
  /// The tasks that would show under each phase in a project
  /// </summary>
  public List<TaskListItem> Tasks { get; set; } = [];
}
