import { Component } from '@angular/core';

@Component({
  selector: 'app-driver-add-form',
  templateUrl: './driver-add-form.component.html',
  styleUrls: ['./driver-add-form.component.css']
})
export class DriverAddFormComponent {
  driver: any = {};

  onSubmit() {
    // Handle the form submission logic here
    // For example, you can send the form data to an API or perform any desired action
    console.log(this.driver); // Log the driver object to the console
  }
}
