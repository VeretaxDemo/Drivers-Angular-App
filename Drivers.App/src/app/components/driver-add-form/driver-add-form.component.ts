import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
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
  driverId: string | undefined;
  driverForm: FormGroup;
  isSubmitted: boolean = false;
  isUpdateMode: boolean = false;
  errorMessage: string = '';

  constructor(
    private driverService: DriverService,
    private router: Router,
    private route: ActivatedRoute) {
    this.driverForm = new FormGroup({
      name: new FormControl('', Validators.required),
      number: new FormControl('', Validators.required),
      team: new FormControl('', Validators.required)
    });
  }

  ngOnInit() {
    // Check if the component is in update mode
    this.route.paramMap.subscribe((params) => {
      const driverId = params.get('id');
      if (driverId) {
        this.isUpdateMode = true;
        this.getDriver(driverId);
      }
    });
  }

  getDriver(driverId: string) {
    this.driverService.getDriver(driverId).subscribe(
      (driver) => {
        this.driver = driver;
        // Update the form fields with the driver's data
        this.driverForm.patchValue({
          name: driver.name,
          number: driver.number,
          team: driver.team
        });
      },
      (error) => {
        console.error('Failed to fetch driver:', error);
      }
    );
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

    if (this.isUpdateMode) {
      // Handle the driver update logic here
      this.updateDriver();
    } else {
      this.driverService.insertDriver(this.driver).subscribe(
        (response) => {
          // Handle successful response here
          console.log('Driver inserted:', response);

          // Route to the driver-detail component with the inserted driver's ID
          this.router.navigate(['/drivers', response.id]);
        },
        (error) => {
          // Handle error response here
          console.error('Failed to insert driver:', error);

          // Display error message to the user
          if (error === 'Duplicate entry') {
            this.errorMessage = 'Duplicate driver found. Please enter a different driver.';
          } else {
            this.errorMessage = 'An error occurred while inserting the driver. Please try again.';
          }
        }
      );
    }
  }

  updateDriver() {
      this.driverService.updateDriver(this.driver).subscribe(
        (response) => {
          // Handle successful response here
          console.log('Driver updated:', response);

          // Route to the driver-detail component with the updated driver's ID
          this.router.navigate(['/drivers', response.id]);
        },
        (error) => {
          // Handle error response here
          console.error('Failed to update driver:', error);

          // Display error message to the user
          if (error === 'Duplicate entry') {
            this.errorMessage = 'Duplicate driver found. Please enter a different driver.';
          } else {
            this.errorMessage = 'An error occurred while updating the driver. Please try again.';
          }
        }
      );
    }

  onCancel() {
    // Handle the cancel button click here
    // For example, navigate back to the drivers page
    this.router.navigate(['/drivers']); // Replace '/drivers' with the actual route of the drivers page
  }
}
