import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DriverAddFormComponent } from './driver-add-form.component';

describe('DriverAddFormComponent', () => {
  let component: DriverAddFormComponent;
  let fixture: ComponentFixture<DriverAddFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DriverAddFormComponent]
    });
    fixture = TestBed.createComponent(DriverAddFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
