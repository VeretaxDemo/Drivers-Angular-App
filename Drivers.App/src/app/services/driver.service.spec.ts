import { TestBed } from '@angular/core/testing';
import { HttpClientModule, HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TestingModule } from 'src/testing/testing.module';
import { DriverService } from './driver.service';
import { environment } from 'src/environments/environment';
import { Driver } from '../models/driver';

describe('DriverService',
  () => {
    let service: DriverService;
    let httpMock: HttpTestingController;

    beforeEach(() => {
      TestBed.configureTestingModule({
          imports: [
            TestingModule,
            HttpClientTestingModule
          ], // Import TestingModule and HttpClientTestingModule
          providers: [DriverService],
        })
        .compileComponents();
      service = TestBed.inject(DriverService);
      httpMock = TestBed.inject(HttpTestingController);
    });

    afterEach(() => {
      httpMock.verify();
    });

    describe('getDrivers',
      () => {
        it('should be created',
          () => {
            expect(service).toBeTruthy();
          });

        it('should fetch drivers from API',
          () => {
            const mockDrivers: Driver[] = [
              { id: 1, name: 'John Doe', number: 10, team: 'Team A' },
              { id: 2, name: 'Jane Smith', number: 20, team: 'Team B' },
              // Additional drivers...
            ];


            service.getDrivers().subscribe((drivers) => {
              expect(drivers).toEqual(mockDrivers);
            });

            const request = httpMock.expectOne(`${environment.apiUrl}/Drivers`);
            expect(request.request.method).toBe('GET');
            request.flush(mockDrivers);
          });

        it('should handle empty response when no drivers are available',
          () => {
            const mockDrivers: Driver[] = [];

            service.getDrivers().subscribe((drivers: Driver[]) => {
              expect(drivers).toEqual(mockDrivers);
            });

            const req = httpMock.expectOne(`${environment.apiUrl}/Drivers`);
            expect(req.request.method).toBe('GET');
            req.flush(mockDrivers);
          });

        it('should handle error response when the API returns 404',
          () => {
            const errorResponse = new HttpErrorResponse({
              error: null,
              headers: new HttpHeaders(),
              status: 404,
              statusText: 'Not Found',
              url: `${environment.apiUrl}/Drivers`,
            });
            service.getDrivers().subscribe(
              (drivers: Driver[]) => {
                // This code block should not be executed if an error occurs
                expect(true).toBeFalse();
              },
              (error) => {
                expect(error).toEqual(errorResponse);
              }
            );

            const req = httpMock.expectOne(`${environment.apiUrl}/Drivers`);
            expect(req.request.method).toBe('GET');
            req.flush(null, errorResponse);
          });

        it('should handle error response when the API returns 500',
          () => {
            const errorResponse = new HttpErrorResponse({
              error: null,
              headers: new HttpHeaders({
                'Content-Type': 'application/json'
              }),
              status: 500,
              statusText: 'Internal Server Error',
              url: `${environment.apiUrl}/Drivers`
            });

            service.getDrivers().subscribe(
              (drivers: Driver[]) => {
                // This code block should not be executed if an error occurs
                expect(true).toBeFalse();
              },
              (error) => {
                expect(error).toEqual(errorResponse);
              }
            );

            const req = httpMock.expectOne(`${environment.apiUrl}/Drivers`);
            expect(req.request.method).toBe('GET');
            req.flush(null, errorResponse);
          });
      });

    describe('getDriver',
      () => {
        it('should fetch a driver from API based on driverId',
          () => {
            const driverId = 1;
            const mockDriver: Driver = { id: 1, name: 'John Doe', number: 10, team: 'Team A' };

            service.getDriver(driverId).subscribe((driver) => {
              expect(driver).toEqual(mockDriver);
            });

            const request = httpMock.expectOne(`${environment.apiUrl}/${service.url}/${driverId}`);
            expect(request.request.method).toBe('GET');
            request.flush(mockDriver);
          });

        it('should handle error response when the API returns 404',
          () => {
            const driverId = 1;
            const errorResponse = new HttpErrorResponse({
              error: null,
              headers: new HttpHeaders(),
              status: 404,
              statusText: 'Not Found',
              url: `${environment.apiUrl}/${service.url}/${driverId}`
            });

            service.getDriver(driverId).subscribe(
              (driver: Driver) => {
                // This code block should not be executed if an error occurs
                expect(true).toBeFalse();
              },
              (error) => {
                expect(error).toEqual(errorResponse);
              }
            );

            const req = httpMock.expectOne(`${environment.apiUrl}/${service.url}/${driverId}`);
            expect(req.request.method).toBe('GET');
            req.flush(null, errorResponse);
          });

        it('should handle error response when the API returns 500',
          () => {
            const driverId = 1;
            const errorResponse = new HttpErrorResponse({
              error: null,
              headers: new HttpHeaders(),
              status: 500,
              statusText: 'Internal Server Error',
              url: `${environment.apiUrl}/${service.url}/${driverId}`
            });

            service.getDriver(driverId).subscribe(
              (driver: Driver) => {
                // This code block should not be executed if an error occurs
                expect(true).toBeFalse();
              },
              (error) => {
                expect(error).toEqual(errorResponse);
              }
            );

            const req = httpMock.expectOne(`${environment.apiUrl}/${service.url}/${driverId}`);
            expect(req.request.method).toBe('GET');
            req.flush(null, errorResponse);
          });
      });

    describe('insertDriver',
      () => {
        it('should handle error response when the API returns a duplicate entry error',
          () => {
            const mockDriver: Driver = { id: 1, name: 'John Doe', number: 10, team: 'Team A' };
            const errorResponse = new HttpErrorResponse({
              error: { message: 'Duplicate entry' },
              headers: new HttpHeaders(),
              status: 400,
              statusText: 'Bad Request',
              url: `${environment.apiUrl}/${service.url}`
            });

            service.insertDriver(mockDriver).subscribe(
              () => {
                // This code block should not be executed if an error occurs
                expect(true).toBeFalsy('Expected error to occur');
              },
              (error) => {
                expect(error instanceof HttpErrorResponse).toBeTruthy();
                if (error.error && error.error.message) {
                  expect(error.error.message).toEqual('Duplicate entry');
                } else {
                  expect(error.statusText).toEqual('Bad Request');
                }
              }
            );

            const req = httpMock.expectOne(`${environment.apiUrl}/${service.url}`);
            expect(req.request.method).toBe('POST');
            expect(req.request.body).toEqual(mockDriver);
            req.flush(null, errorResponse);
          });
      });
  });
