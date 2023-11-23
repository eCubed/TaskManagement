import { Component, Input, Output, OnChanges, SimpleChanges, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectModel, SaveProjectModel } from '../../models/tasks-core';
import { TasksService } from '../../services/tasks.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-save-project-ui',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './save-project-ui.component.html',
  styleUrl: './save-project-ui.component.scss'
})
export class SaveProjectUiComponent implements OnChanges {
  @Input('project-id') projectId?: number
  @Input() project?: SaveProjectModel
  @Output() projectSaved: EventEmitter<ProjectModel> = new EventEmitter<ProjectModel>()
  @Output() cancelled: EventEmitter<null> = new EventEmitter<null>();

  projectForm!: FormGroup

  constructor(private tasksService: TasksService) {

  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['project'] && changes['project'].currentValue != null &&
        changes['projectId'] && changes['projectId'].currentValue != null) {
      this.projectForm = new FormGroup({
        id: new FormControl(this.projectId),
        name: new FormControl(this.project!.name, [Validators.required]),
        description: new FormControl(this.project!.description)
      })
    }
  }

  async save() {
    try {
      if (this.projectForm.valid) {
        const projectModel: ProjectModel = await this.tasksService.createNewProject(this.projectForm.value as SaveProjectModel)
        this.projectSaved.emit(projectModel)
      }

    } catch {

    }
  }

  cancel() {
    this.cancelled.emit()
  }
}
