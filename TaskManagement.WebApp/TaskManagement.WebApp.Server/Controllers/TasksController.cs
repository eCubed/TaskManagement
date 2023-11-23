using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.WebApp.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
  private ITasksService _TasksService;
  public TasksController(ITasksService tasksService)
  {
    _TasksService = tasksService;
  }
    
  [HttpPost("{projectPhaseId}")]
  public async Task<IActionResult> CreateNewTask(int projectPhaseId, SaveTaskModel saveTaskModel)
  {
    var projectModel = await _TasksService.CreateNewTask(projectPhaseId, saveTaskModel, "");

    return StatusCode(201, projectModel);
  }

  [HttpPut("{taskId}")]
  public async Task<IActionResult> UpdateTask(int taskId, SaveTaskModel saveTaskModel)
  {
    var projectModel = await _TasksService.UpdateTask(taskId, saveTaskModel, "");

    return StatusCode(201, projectModel);
  }
}
