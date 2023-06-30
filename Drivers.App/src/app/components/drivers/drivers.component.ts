import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Driver } from '../../models/driver';
import { DriverService } from '../../services/driver.service'


@Component({
  selector: 'app-drivers',
  templateUrl: './drivers.component.html',
  styleUrls: ['./drivers.component.css']
})

export class DriversComponent implements OnInit {
  drivers: Driver[] = [];
  selectedDriver: Driver | undefined;

  constructor(private driverService: DriverService, private router: Router) { }

  ngOnInit(): void {
    this.loadDrivers();
  }

  loadDrivers(): void {
    this.driverService.getDrivers().subscribe((result: Driver[]) => {
      this.drivers = result;
    });
  }

  showDriverDetails(driver: Driver): void {
    this.selectedDriver = driver;
  }

  goToAddForm(): void {
    this.router.navigate(['/add-driver']);
  }
}
//export class DriversComponent implements OnInit {

//  drivers: Driver[] = [];
//  selectedDriver: Driver | undefined;

//  constructor(private driverService: DriverService, private router: Router) {

//  }

//  ngOnInit(): void {
//    this.driverService.getDrivers().subscribe((result: Driver[]) => (this.drivers = result));
//  }

//  showDriverDetails(driver: Driver): void {
//    this.selectedDriver = driver;
//  }

//  goBack(): void {
//    this.selectedDriver = undefined;
//  }

//  goToAddForm(): void {
//    this.router.navigate(['/add-driver']);
//  }
//}
