using TaskManagement;

public interface ITasksService
{
  Task<List<ProjectListItem>> GetProjects();
  Task<List<Progression>> GetProgressions();
  Task<List<PhaseListItem>> GetPhases();
  System.Threading.Tasks.Task AddDependency(int dependentTaskId, int dependencyTaskId);

  Task<ProjectModel> GetProject(int projectId);
  Task<ProjectModel> CreateNewProject(SaveProjectModel saveProjectModel, string username);
  System.Threading.Tasks.Task UpdateProject(int projectId, SaveProjectModel saveProjectModel, string username);
  Task<TaskListItem> CreateNewTask(int projectPhaseId, SaveTaskModel saveTaskModel, string username);
  Task<TaskListItem> UpdateTask(int taskId, SaveTaskModel saveTaskModel, string username);
  System.Threading.Tasks.Task TransferTask(int taskId, int newProjectPhaseId);
  System.Threading.Tasks.Task DeleteTask(int taskId);
  Task<ProjectModel> AppendNewPhaseToProject(int projectId, int phaseId);
  Task<ProjectModel> InsertAfterProjectPhase(int projectPhaseId, int phaseId);
  Task<ProjectModel> InsertBeforeProjectPhase(int projectPhaseId, int phaseId);
  Task<ProjectModel> DeleteProjectPhase(int projectPhaseId, string username);

}
