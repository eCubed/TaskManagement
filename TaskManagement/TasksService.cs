using Microsoft.EntityFrameworkCore;
using QuickGraph;
using QuickGraph.Algorithms;

namespace TaskManagement;
public class TasksService : ITasksService
{
  private readonly TaskDbContext _dbContext;

  public TasksService(TaskDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async System.Threading.Tasks.Task AddDependency(int dependentTaskId, int dependencyTaskId)
  {
    var graph = await BuildGraphAsync(); // Implement this method

    if (dependentTaskId == dependencyTaskId)
      throw new InvalidOperationException("Task cannot depend on itself");

    if (HasDependencyPath(graph, dependencyTaskId, dependentTaskId))
      throw new InvalidOperationException("Adding this dependency would create a circular dependency path.");

    if (HasDependencyPath(graph, dependentTaskId, dependencyTaskId))
      throw new InvalidOperationException("Adding this dependency would create a circular dependency path.");

    // Now, we can add the dependency
    TaskDependency taskDependency = new TaskDependency
    {
      DependentTaskId = dependentTaskId,
      DependencyTaskId = dependencyTaskId
    };

    _dbContext.TaskDependencies.Add(taskDependency);
    await _dbContext.SaveChangesAsync();
  }

  private bool HasDependencyPath(AdjacencyGraph<int, Edge<int>> graph, int sourceTaskId, int targetTaskId)
  {
    var tryGetPath = graph.ShortestPathsDijkstra((e) => 1.0, sourceTaskId);
    IEnumerable<Edge<int>> path;

    if (tryGetPath(targetTaskId, out path))
    {
      return true; // There is a path
    }
    return false;
  }

  private async Task<AdjacencyGraph<int, Edge<int>>> BuildGraphAsync()
  {
    var graph = new AdjacencyGraph<int, Edge<int>>();

    // Populate the graph with vertices and edges based on TaskDependencies
    var taskDependencies = await _dbContext.TaskDependencies.ToListAsync(); // Assuming you are using Entity Framework Core

    foreach (var taskDependency in taskDependencies)
    {
      graph.AddVerticesAndEdge(new Edge<int>(taskDependency.DependentTaskId, taskDependency.DependencyTaskId));
    }

    return graph;
  }


  public async Task<List<ProjectListItem>> GetProjects()
  {
    return await _dbContext.Projects.OrderBy(p => p.Name).Select(p => new ProjectListItem
    {
      Id = p.Id,
      Name = p.Name
    }).ToListAsync();
  }

  public async Task<List<Progression>> GetProgressions()
  {
    return await _dbContext.Progressions.OrderBy(p => p.Name).ToListAsync();
  }

  public async Task<List<PhaseListItem>> GetPhases()
  {
    return await _dbContext.Phases.OrderBy(p => p.Name).Select(p => new PhaseListItem
    {
      Id = p.Id,
      Name = p.Name
    }).ToListAsync();
  }

  public async Task<ProjectModel> GetProject(int projectId)
  {
    Project? project = await _dbContext.Projects.Include(p => p.ProjectPhases).ThenInclude(pph => pph.Phase).SingleOrDefaultAsync(p => p.Id == projectId) ?? throw new Exception("Project not found");

    ProjectModel projectModel = new()
    {
      Name = project.Name,
      Description = project.Description,
      ProjectPhases = (project.ProjectPhases).ToList().Select(pph =>
      {
        ProjectPhaseModel projectPhaseModel = new ProjectPhaseModel
        {
          Id = pph.Id,
          Name = pph.Phase.Name,
          Tasks =

          [
            .. _dbContext.Tasks.Include(t => t.Progression).Where(t => t.ProjectPhaseId == pph.Id).Select(t => new TaskListItem
                {
                  CreatedBy = t.CreatedBy,
                  Description = t.Description,
                  LastUpdatedBy = t.LastUpdatedBy,
                  Name = t.Name,
                  ProgressionId = t.ProgressionId,
                  ProgressionName = (t.Progression == null) ? "" : t.Progression.Name
                }),
          ]
        };

        return projectPhaseModel;
      }).ToList()
    };

    return projectModel;
  }

  private async System.Threading.Tasks.Task ResolvePhaseToProject(int phaseId, int projectId, int orderInSequence)
  {
    ProjectPhase? projectPhase = await _dbContext.ProjectPhases.SingleOrDefaultAsync(pph => pph.PhaseId == phaseId && pph.ProjectId == projectId);

    if (projectPhase == null)
    {
      projectPhase = new()
      {
        ProjectId = projectId,
        PhaseId = phaseId,
        OrderInSequence = orderInSequence
      };

      _dbContext.ProjectPhases.Add(projectPhase);
    } else
    {
      projectPhase.OrderInSequence = orderInSequence;
    }

    await _dbContext.SaveChangesAsync();
  }

  public async Task<ProjectModel> CreateNewProject(SaveProjectModel saveProjectModel, string username)
  {
    Project? projectThatAlreadyExists = await _dbContext.Projects.SingleOrDefaultAsync(p => p.Name == saveProjectModel.Name);

    if (projectThatAlreadyExists != null)
      throw new Exception("Project by that name already exists");

    Project newProject = new()
    {
      CreatedBy = username,
      Description = saveProjectModel.Description,
      Name = saveProjectModel.Name
    };

    _dbContext.Projects.Add(newProject);
    await _dbContext.SaveChangesAsync();

    // Now, we want to automatically add the default phases!
    List<int> phaseIds = await _dbContext.Phases.OrderBy(p => p.Id).Select(p => p.Id).ToListAsync();

    int orderInSequence = 1;
    foreach(var phaseId in phaseIds)
    {
      await ResolvePhaseToProject(phaseId, newProject.Id, orderInSequence);
      orderInSequence++;
    }

    return await GetProject(newProject.Id);
  }

  public async System.Threading.Tasks.Task UpdateProject(int projectId, SaveProjectModel saveProjectModel, string username)
  {
    Project? project = await _dbContext.Projects.SingleOrDefaultAsync(p => p.Id == projectId) ?? throw new Exception("Project not found");

    project.LastUpdatedBy = username;
    project.Description = saveProjectModel.Description;
    project.Description = saveProjectModel.Name;

    await _dbContext.SaveChangesAsync();
  }

  public async Task<TaskListItem> CreateNewTask(int projectPhaseId, SaveTaskModel saveTaskModel, string username)
  {
    Task newTask = new Task()
    {
      CreatedBy = username,
      Description = saveTaskModel.Description,
      DueDate = saveTaskModel.DueDate,
      Name = saveTaskModel.Name,
      Priority = saveTaskModel.Priority,
      ProgressionId = saveTaskModel.ProgressionId,
      ProjectPhaseId = projectPhaseId
    };

    _dbContext.Tasks.Add(newTask);
    await _dbContext.SaveChangesAsync();

    return new TaskListItem
    {
      Id = newTask.Id,
      CreatedBy = username,
      Description = newTask.Description,
      Name = newTask.Name,
      ProgressionId = newTask.ProgressionId,
      Priority = newTask.Priority,
      ProgressionName = (await _dbContext.Progressions.SingleOrDefaultAsync(p => p.Id == newTask.ProgressionId))?.Name ?? ""
    };
  }

  public async Task<TaskListItem> UpdateTask(int taskId, SaveTaskModel saveTaskModel, string username)
  {
    Task? task = await _dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == taskId) ?? throw new Exception("Task not found");

    task.DueDate = saveTaskModel.DueDate;
    task.Description = saveTaskModel.Description;
    task.LastUpdatedBy = username;
    task.Priority = saveTaskModel.Priority;
    task.ProgressionId = saveTaskModel.ProgressionId;

    await _dbContext.SaveChangesAsync();

    return new TaskListItem
    {
      Id = task.Id,
      CreatedBy = username,
      Description = task.Description,
      Name = task.Name,
      ProgressionId = task.ProgressionId,
      Priority = task.Priority,
      ProgressionName = (await _dbContext.Progressions.SingleOrDefaultAsync(p => p.Id == task.ProgressionId))?.Name ?? ""
    };
  }

  public async System.Threading.Tasks.Task TransferTask(int taskId, int newProjectPhaseId)
  {
    Task? task = await _dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == taskId) ?? throw new Exception("Task not found");
    task.ProjectPhaseId = newProjectPhaseId;
    await _dbContext.SaveChangesAsync();
  }

  public async System.Threading.Tasks.Task DeleteTask(int taskId)
  {
    Task? task = await _dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == taskId) ?? throw new Exception("Task not found");

    _dbContext.Tasks.Remove(task);
    await _dbContext.SaveChangesAsync();
  }

  public async Task<ProjectModel> AppendNewPhaseToProject(int projectId, int phaseId)
  {
    ProjectPhase? projectPhase = await _dbContext.ProjectPhases.SingleOrDefaultAsync(pph => pph.ProjectId == projectId && pph.PhaseId == phaseId);

    if (projectPhase != null)
      throw new Exception("Phase already in project");

    projectPhase = new ProjectPhase
    {
      ProjectId = projectId,
      PhaseId = phaseId,
      OrderInSequence = (await _dbContext.ProjectPhases.Where(pph => pph.ProjectId == projectId).CountAsync()) + 1
    };

    _dbContext.ProjectPhases.Add(projectPhase);
    await _dbContext.SaveChangesAsync();

    return await GetProject(projectId);
  }

  public async Task<ProjectModel> InsertAfterProjectPhase(int projectPhaseId, int phaseId)
  {
    ProjectPhase? projectPhase = await _dbContext.ProjectPhases.SingleOrDefaultAsync(pph => pph.Id == projectPhaseId) ?? throw new Exception("Project Phase not found");

    List<ProjectPhase> projectPhasesToShift = await _dbContext.ProjectPhases.Where(pph => pph.ProjectId == projectPhase.ProjectId && pph.OrderInSequence > projectPhase.OrderInSequence).OrderBy(pph => pph.OrderInSequence).ToListAsync();

    int orderInSequence = (projectPhase.OrderInSequence ?? 1) + 1;

    projectPhase = new()
    {
      ProjectId = projectPhase.ProjectId,
      PhaseId = phaseId,
      OrderInSequence = orderInSequence
    };

    orderInSequence++;

    foreach(var projectPhaseToShift in projectPhasesToShift)
    {
      await ResolvePhaseToProject(phaseId, projectPhase.ProjectId, orderInSequence);
      orderInSequence++;
    }

    return await GetProject(projectPhase.ProjectId);
  }
}
