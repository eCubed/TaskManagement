import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Progression, ProjectPhaseModel } from '../../models/tasks-core';
import { TaskRowComponent } from '../task-row/task-row.component';

@Component({
  selector: 'app-project-phase',
  standalone: true,
  imports: [CommonModule, TaskRowComponent],
  templateUrl: './project-phase.component.html',
  styleUrl: './project-phase.component.scss'
})
export class ProjectPhaseComponent {
  @Input('project-phase') projectPhase?: ProjectPhaseModel
  @Input() progressions!: Array<Progression>

  addNewTask() {
    this.projectPhase?.tasks.push({
      id: 0,
      name: 'New Task',
      progressionId: undefined,
      description: ''
    })
  }
}
