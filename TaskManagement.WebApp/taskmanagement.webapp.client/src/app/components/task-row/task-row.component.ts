import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { Component, HostBinding, Input, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Progression, TaskListItem } from '../../models/tasks-core';
import { TasksService } from '../../services/tasks.service';

@Component({
  selector: '[task-row]',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task-row.component.html',
  styleUrl: './task-row.component.scss'
})
export class TaskRowComponent implements OnChanges {

  @Input('task-list-item') taskListItem!: TaskListItem
  @Input('project-phase-id') projectPhaseId!: number
  @Input() progressions!: Array<Progression>

  isUpdateMode = false
  saveTaskForm!: FormGroup

  constructor(private tasksService: TasksService) {

  }

  @HostBinding('style.background-color')
  get backgroundColor() {
    return (this.taskListItem.id == 0) ? '#eeffee' : 'transparent'
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(changes['taskListItem'] && changes['taskListItem'].currentValue != null &&
       changes['progressions'] && changes['progressions'].currentValue != null) {
      this.saveTaskForm = new FormGroup({
        name: new FormControl(this.taskListItem.name, [Validators.required]),
        progressionId: new FormControl(this.taskListItem.progressionId),
        description: new FormControl(this.taskListItem.description),
        priority: new FormControl(this.taskListItem.priority),
        dueDate: new FormControl(null) // later - requires date picker!
      })
    }
  }

  getNgClassFromProgression(): string|string[]|Set<string>|{ [klass: string]: any; }|null|undefined {
    return {
      'started': this.taskListItem.progressionName == 'Started',
      'in-progress': this.taskListItem.progressionName == 'In Progress',
      'completed': this.taskListItem.progressionName == 'Completed'
    }
  }

  goIntoUpdateMode() {
    this.isUpdateMode = true
  }

  cancel() {
    this.isUpdateMode = false
  }

  async save() {
    try {
      if (this.taskListItem.id == 0) {
        this.taskListItem = await this.tasksService.createNewTask(this.projectPhaseId, this.saveTaskForm.value)
      } else {
        this.taskListItem = await this.tasksService.updateTask(this.taskListItem.id, this.saveTaskForm.value)
      }

    } catch {

    } finally {
      this.isUpdateMode = false
    }


  }
}
