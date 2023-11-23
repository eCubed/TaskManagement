using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.WebApp.Server.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProjectsController : ControllerBase
{
  private ITasksService _TasksService;

  public ProjectsController(ITasksService tasksService)
  {
    _TasksService = tasksService;
  }

  [HttpGet]
  public async Task<IActionResult> GetProjects() => Ok(await _TasksService.GetProjects());

  [HttpGet("{projectId}")]
  public async Task<IActionResult> GetProject(int projectId) => Ok(await _TasksService.GetProject(projectId));

  [HttpPost]
  public async Task<IActionResult> CreateProject(SaveProjectModel saveProjectModel)
  {
    var projectModel = await _TasksService.CreateNewProject(saveProjectModel, "");

    return StatusCode(201, projectModel);
  }
}
