import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';


@NgModule({
  imports: [
    HttpClientModule,
    HttpClientTestingModule
  ], // Import any necessary testing modules
})
export class TestingModule { }
