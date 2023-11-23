using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.WebApp.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProgressionsController : ControllerBase
{
  private ITasksService _TasksService;

  public ProgressionsController(ITasksService tasksService)
  {
    _TasksService = tasksService;
  }

  [HttpGet]
  public async Task<IActionResult> GetProgressions() => Ok(await _TasksService.GetProgressions());
}
