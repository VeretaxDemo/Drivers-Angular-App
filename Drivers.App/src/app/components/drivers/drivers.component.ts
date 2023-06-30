import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Driver } from '../../models/driver';
import { DriverService } from '../../services/driver.service';



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

    // Note that while testing Cancel on add-driver I discovered
    // that it wasn't loading, so I thought by calling .then
    // after it was done with the navigate could do this.
    // However, it turns out, at the moment to not be necessary,
    // because getDrivers is called in ngOnInit
    //this.router.navigate(['/add-driver']).then(() => {
    //  this.loadDrivers(); // Refresh the drivers after navigating back
    //});
  }
}
