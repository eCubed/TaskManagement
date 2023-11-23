import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaveProjectUiComponent } from './save-project-ui.component';

describe('SaveProjectUiComponent', () => {
  let component: SaveProjectUiComponent;
  let fixture: ComponentFixture<SaveProjectUiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SaveProjectUiComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SaveProjectUiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
