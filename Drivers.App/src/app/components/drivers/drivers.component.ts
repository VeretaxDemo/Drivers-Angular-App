import { Component } from '@angular/core';
import { Driver } from '../../models/driver';
import { DriverService } from '../../services/driver.service'
import { Router } from '@angular/router';

@Component({
  selector: 'app-drivers',
  templateUrl: './drivers.component.html',
  styleUrls: ['./drivers.component.css']
})
export class DriversComponent {

  drivers: Driver[] = [];
  selectedDriver: Driver | undefined;

  constructor(private driverService: DriverService, private router: Router) {

  }

  ngOnInit(): void {
    this.driverService.getDrivers().subscribe((result: Driver[]) => (this.drivers = result));
  }

  showDriverDetails(driver: Driver): void {
    this.selectedDriver = driver;
  }

  goBack(): void {
    this.selectedDriver = undefined;
  }
}
