import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Driver } from '../models/driver';
//import { DRIVERS } from '../data/drivers.data';

@Injectable({
  providedIn: 'root'
})
export class DriverService {

  url = "Drivers";

  constructor(private http: HttpClient) { }

  public getDrivers(): Observable<Driver[]> {
    return this.http.get<Driver[]>(`${environment.apiUrl}/${this.url}`);
  }

  public getDriver(driverId: number): Observable<Driver> {
    return this.http.get<Driver>(`${environment.apiUrl}/${this.url}/${driverId}`);
  }
  //getDrivers(): Observable<Driver[]> {
  //  return of(DRIVERS);
  //}

  //getDriver(driverId: number): Observable<Driver | undefined> {
  //  return of(DRIVERS.find(driver => driver.id === driverId));
  //}

  //url = "Drivers";

  //constructor(private http: HttpClient) { }

  //public getDrivers() : Observable<Driver[]> {

  //  return this.http.get<Driver[]>(`${environment.apiUrl}/${this.url}`);
  //}
}
