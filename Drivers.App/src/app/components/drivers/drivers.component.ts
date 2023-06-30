import { Component } from '@angular/core';
import { Driver } from '../../models/driver';
import { DriverService } from '../../services/driver.service'

@Component({
  selector: 'app-drivers',
  templateUrl: './drivers.component.html',
  styleUrls: ['./drivers.component.css']
})
export class DriversComponent {

  drivers: Driver[] = [];

  constructor(private driverService: DriverService) {

  }

  ngOnInit(): void {
    this.driverService.getDrivers().subscribe((result: Driver[]) => (this.drivers = result));
  }

}
