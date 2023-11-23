import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectPhaseComponent } from './project-phase.component';

describe('ProjectPhaseComponent', () => {
  let component: ProjectPhaseComponent;
  let fixture: ComponentFixture<ProjectPhaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProjectPhaseComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ProjectPhaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
