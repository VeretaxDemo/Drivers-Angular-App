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
  id: string | null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private driverService: DriverService
  ) {
    this.id = this.route.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    if (this.id) {
      this.driverService.getDriver(this.id).subscribe((driver: Driver) => {
        this.driver = driver;
      });
    }
    //const driverId = this.route.snapshot.params['id'];
    //this.driverService.getDriver(this.id).subscribe((driver: Driver) => {
    //  this.driver = driver;
    //});
  }

  goBack(): void {
    this.router.navigate(['/drivers']);
  }
}
