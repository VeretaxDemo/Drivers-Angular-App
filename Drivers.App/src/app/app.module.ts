import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { Router } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms'; // Import FormsModule
import { DriversComponent } from './components/drivers/drivers.component';
import { DriverDetailsComponent } from './components/driver-details/driver-details.component';
import { DriverAddFormComponent } from './components/driver-add-form/driver-add-form.component';
import { DriverService } from './services/driver.service';
import { DriverEditFormComponent } from './components/driver-edit-form/driver-edit-form.component';

@NgModule({
  declarations: [
    AppComponent,
    DriversComponent,
    DriverDetailsComponent,
    DriverAddFormComponent,
    DriverEditFormComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule
  ],
  providers: [DriverService],
  bootstrap: [AppComponent]
})
export class AppModule { }
