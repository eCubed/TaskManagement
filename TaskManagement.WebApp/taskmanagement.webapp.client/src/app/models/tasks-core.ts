export interface SaveProjectModel {
  name: string
  description: string
}

export interface ProjectListItem {
  id: number
  name: string
}

export interface TaskListItem {
  id: number
  name: string
  description?: string
  progressionId: number
  progressionName?: string
  createdBy?: string
  priority?: number
  lastUpdatedBy?: string
}

export interface ProjectPhaseModel {
  id: number
  name: string
  tasks: Array<TaskListItem>
}

export interface ProjectModel extends SaveProjectModel {
  projectPhases: Array<ProjectPhaseModel>
}




