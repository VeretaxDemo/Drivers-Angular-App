import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Driver } from '../../models/driver';
import { DriverService } from '../../services/driver.service';

@Component({
  selector: 'app-driver-details',
  templateUrl: './driver-details.component.html',
  styleUrls: ['./driver-details.component.css']
})
export class DriverDetailsComponent implements OnInit {
  driver: Driver | undefined;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private driverService: DriverService
  ) { }

  ngOnInit(): void {
    const driverId = this.route.snapshot.params['id'];
    this.driverService.getDriver(driverId).subscribe((driver: Driver) => {
      this.driver = driver;
    });
  }

  goBack(): void {
    this.router.navigate(['/drivers']);
  }
}
