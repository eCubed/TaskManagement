import { Component, OnInit } from '@angular/core';
import { TasksService } from '../../services/tasks.service';
import { ProjectListItem, ProjectModel, SaveProjectModel } from '../../models/tasks-core';
import { SaveProjectUiComponent } from '../../components/save-project-ui/save-project-ui.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  standalone: true,
  imports: [CommonModule, SaveProjectUiComponent]
})
export class HomeComponent implements OnInit {



  projects?: Array<ProjectListItem>

  newProject?: SaveProjectModel | null
  projectBeingEdited?: ProjectModel | null
  projectBeingEditedId?: number | null

  constructor(private tasksService: TasksService) {

  }

  async ngOnInit() {
    try {
      this.projects = await this.tasksService.getProjects()
    } catch {
      // later
    }
  }

  async loadProject(projectId: number) {
    try {
      this.projectBeingEdited = await this.tasksService.getProject(projectId)
      this.projectBeingEditedId = projectId
    } catch {
      // later
    }
  }

  createProject() {
    this.newProject = { name: '', description: ''}
  }

  async onProjectSaved(project: ProjectModel) {
    try {
      this.projects = await this.tasksService.getProjects()
      this.projectBeingEdited = project
      this.newProject = null
    } catch {
      // later
    }
  }

  onCancelled() {
    this.newProject = null
  }

}
