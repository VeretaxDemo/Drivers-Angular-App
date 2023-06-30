import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { Driver } from '../../models/driver';
import { DriverService } from '../../services/driver.service';

@Component({
  selector: 'app-driver-add-form',
  templateUrl: './driver-add-form.component.html',
  styleUrls: ['./driver-add-form.component.css']
})
export class DriverAddFormComponent {
  driver: Driver = new Driver();
  driverForm: FormGroup;
  isSubmitted: boolean = false;

  constructor(private driverService: DriverService, private router: Router) {
    this.driverForm = new FormGroup({
      name: new FormControl('', Validators.required),
      number: new FormControl('', Validators.required),
      team: new FormControl('', Validators.required)
    });
  }

  onSubmit() {
    this.isSubmitted = true;
    // Check if the form is valid
    if (this.driverForm.invalid) {
      // Mark all fields as touched to display validation errors
      this.driverForm.markAllAsTouched();
      return;
    }

    // Update the driver object with the form field values
    this.driver.name = this.driverForm.value.name;
    this.driver.number = this.driverForm.value.number;
    this.driver.team = this.driverForm.value.team;

    // Handle the form submission logic here
    // For example, you can send the form data to an API or perform any desired action
    console.log(this.driver); // Log the driver object to the console

    this.driverService.insertDriver(this.driver).subscribe(
      response => {
        // Handle successful response here
        console.log('Driver inserted:', response);

        // Route to the driver-detail component with the inserted driver's ID
        this.router.navigate(['/drivers', response.id]);
      },
      error => {
        // Handle error response here
        console.error('Failed to insert driver:', error);
      }
    );
  }

  onCancel() {
    // Handle the cancel button click here
    // For example, navigate back to the drivers page
    this.router.navigate(['/drivers']); // Replace '/drivers' with the actual route of the drivers page
  }
}
