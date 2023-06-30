import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DriversComponent } from './components/drivers/drivers.component';
import { DriverDetailsComponent } from './components/driver-details/driver-details.component';
import { DriverAddFormComponent } from './components/driver-add-form/driver-add-form.component';

const routes: Routes = [
  { path: '', redirectTo: '/drivers', pathMatch: 'full' },
  { path: 'drivers', component: DriversComponent },
  { path: 'drivers/:id', component: DriverDetailsComponent },
  { path: 'add-driver', component: DriverAddFormComponent },
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
