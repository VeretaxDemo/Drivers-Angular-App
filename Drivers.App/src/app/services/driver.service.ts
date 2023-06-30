import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Driver } from '../models/driver';


@Injectable({
  providedIn: 'root'
})
export class DriverService {

  url = "Drivers";
  driverId: string | undefined;

  constructor(private http: HttpClient) {}

  public getDrivers(): Observable<Driver[]> {
    return this.http.get<Driver[]>(`${environment.apiUrl}/${this.url}`);
  }

  public getDriver(driverId: number): Observable<Driver> {
    return this.http.get<Driver>(`${environment.apiUrl}/${this.url}/${driverId}`);
  }

  public insertDriver(driver: Driver): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/${this.url}`, driver);

  }
}
