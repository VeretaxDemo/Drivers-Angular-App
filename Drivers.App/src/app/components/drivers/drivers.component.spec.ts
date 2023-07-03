import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DriversComponent } from './drivers.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DriverService } from '../../services/driver.service';


describe('DriversComponent', () => {
  let component: DriversComponent;
  let fixture: ComponentFixture<DriversComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [DriversComponent],
      imports: [HttpClientTestingModule], // Add HttpClientModule here
      providers: [DriverService] // Include any other required providers
    });
    fixture = TestBed.createComponent(DriversComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
