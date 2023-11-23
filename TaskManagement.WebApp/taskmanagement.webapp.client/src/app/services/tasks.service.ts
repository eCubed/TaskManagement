import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Progression, ProjectListItem, ProjectModel, SaveProjectModel, SaveTaskModel, TaskListItem } from '../models/tasks-core';
import { firstValueFrom } from 'rxjs'

@Injectable({
  providedIn: 'root',
})
export class TasksService {

  constructor(private http: HttpClient) { }

  async getProgressions(): Promise<Array<Progression>> {
    return firstValueFrom(this.http.get<Array<Progression>>('/api/progressions'))
  }

  async getProjects(): Promise<Array<ProjectListItem>> {
    return firstValueFrom(this.http.get<Array<ProjectListItem>>('/api/projects'))
  }

  async getProject(projectId: number): Promise<ProjectModel> {
    return firstValueFrom(this.http.get<ProjectModel>(`/api/projects/${projectId}`))
  }

  async createNewProject(newProject: SaveProjectModel): Promise<ProjectModel> {
    return firstValueFrom(this.http.post<ProjectModel>('/api/projects', newProject))
  }

  async createNewTask(projectPhaseId: number, saveTaskModel: SaveTaskModel): Promise<TaskListItem> {
    return firstValueFrom(this.http.post<TaskListItem>(`/api/tasks/${projectPhaseId}`, saveTaskModel))
  }

  async updateTask(taskId: number, saveTaskModel: SaveTaskModel): Promise<TaskListItem> {
    return firstValueFrom(this.http.put<TaskListItem>(`/api/tasks/${taskId}`, saveTaskModel))
  }

}
