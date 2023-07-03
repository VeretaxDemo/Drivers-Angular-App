import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable, throwError} from 'rxjs';
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

  public getDriver(driverId: string): Observable<Driver> {
    return this.http.get<Driver>(`${environment.apiUrl}/${this.url}/${driverId}`);
  }

  public insertDriver(driver: Driver): Observable<any> {
    return this.http.post<any>(`${environment.apiUrl}/${this.url}`, driver).pipe(
      catchError((error: any) => {
        if (error.error && error.error.message) {
          throw new Error(error.error.message);
        } else {
          throw new Error('Unknown Error');
        }
      })
    );
  }

  public updateDriver(driver: Driver): Observable<any> {
    const driverId = driver.id;
    return this.http.put<any>(`${environment.apiUrl}/${this.url}/${driverId}`, driver).pipe(
      catchError((error: any) => {
        if (error.error && error.error.message) {
          throw new Error(error.error.message);
        } else {
          throw new Error('Unknown Error');
        }
      })
    );
  }

}
