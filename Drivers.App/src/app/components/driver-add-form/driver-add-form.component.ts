import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Driver } from '../../models/driver';
import { DriverService } from '../../services/driver.service';

@Component({
  selector: 'app-driver-add-form',
  templateUrl: './driver-add-form.component.html',
  styleUrls: ['./driver-add-form.component.css']
})
export class DriverAddFormComponent {
  driver: Driver = new Driver();

  constructor(private driverService: DriverService, private router: Router) {}

  onSubmit() {
    // Handle the form submission logic here
    // For example, you can send the form data to an API or perform any desired action
    console.log(this.driver); // Log the driver object to the console

    this.driverService.insertDriver(this.driver).subscribe(
      response => {
        // Handle successful response here
        console.log('Driver inserted:', response);
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
